using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonService;
using MessageLib;

namespace CommonClient
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        CommonClient client;
        Action<string> actlog;
        Action<int> actProgress;
        BlockingCollection<string> logList = new BlockingCollection<string>();

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var strResult = new StringBuilder();
                    var strTemp = "";
                    while (true)
                    {
                        if (logList.TryTake(out strTemp))
                            strResult.Append(strTemp);
                        else
                            break;
                    }
                    if (this.textBox1.IsHandleCreated && !this.textBox1.IsDisposed)
                        this.textBox1.Invoke(actlog, strResult.ToString());
                    Thread.Sleep(10);
                }
            });
            actlog = log =>
            {
                if (this.textBox1.Lines.Length > 100000)
                    this.textBox1.Clear();
                this.textBox1.AppendText(log);
            };
            actProgress = value =>
            {
                if (value == 0)
                    this.statusStrip.Visible = true;
                else if (value == 100)
                    this.statusStrip.Visible = false;
                else
                    this.progressBar.Value = value;
            };
            client = new CommonClient();
            client.Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3347);
            client.Error += Client_Error;
            client.MessageReceived += Client_MessageReceived;
            client.RawDataReceived += Client_RawDataReceived;
            client.Disconnected += Client_Disconnected;
        }

        void Log(string log)
        {
            this.logList.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\r\n" + log + "\r\n");
        }

        void SetProgress(int value)
        {
            this.Invoke(actProgress, value);
        }

        private void Client_Error(System.Net.Sockets.SocketError error)
        {
            this.Log("Error:" + error.ToString());
        }

        FileStream recvFileStream;
        int progress = 0;
        long recvFilePos = 0, recvFileLength = 0;
        string recvFileMD5;

        private void Client_MessageReceived(CommonService.Message message)
        {
            switch (message.Type)
            {
                case MessageType.Text:
                case MessageType.Data:
                    this.Log((message as TextMessage).Text);
                    break;
                case MessageType.RawData:
                    {
                        var msg = message as FileInfoMessage;
                        recvFileLength = msg.FileSize;
                        recvFileMD5 = msg.FileMD5;
                        this.SetProgress(0);
                        this.Log(string.Format(@"Receiving start:
FileName={0}
FileSize={1}
MD5={2}", msg.FileName, msg.FileSize.ToFileSize(), msg.FileMD5));
                        var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReceivedFiles");
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        var filePath = Path.Combine(dir, msg.FileName);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        recvFileStream = File.Create(filePath);
                    }
                    break;
                default:
                    throw new Exception("Unexpected message type!");
            }
        }

        private void Client_RawDataReceived(byte[] buffer)
        {
            recvFileStream.Write(buffer, 0, buffer.Length);
            recvFilePos += buffer.Length;
            var value = Convert.ToInt32(recvFilePos * 100 / recvFileLength);
            if (progress != value)
            {
                this.SetProgress(value);
                this.progress = value;
            }
            if (recvFilePos == recvFileLength)
            {
                this.SetProgress(100);
                recvFilePos = 0;
                recvFileStream.Flush();
                recvFileStream.Seek(0, SeekOrigin.Begin);
                var md5 = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(recvFileStream)).Replace("-", "");
                recvFileStream.Close();
                recvFileStream.Dispose();
                Log("Receiving end:MD5 check " + (md5 == recvFileMD5 ? "passed" : "failed"));
            }
        }

        private void Client_Disconnected()
        {
            this.Log("Disconnected");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Log(client.Connect() ? "Connected" : "Connect failed");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                var message = new TextMessage();
                message.Text = "Data:" + i;
                this.client.SendMessage(message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Log("SendBigObject");
            Task.Factory.StartNew(() =>
            {
                var message = new DataMessage();
                message.Data = new byte[int.MaxValue / 3];
                var bytes = this.client.handler.FormatterMessageBytes(message);
                message.Text = "Length:" + Convert.ToInt64(bytes.Length).ToFileSize();
                this.client.SendMessage(message);
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath;
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                filePath = dialog.FileName;
            }
            this.Log("SendBigFile");
            Task.Factory.StartNew(() =>
            {
                var fileInfo = new FileInfo(filePath);
                var message = new FileInfoMessage();
                using (var fileStream = File.OpenRead(filePath))
                {
                    var fileMD5 = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(fileStream)).Replace("-", "");
                    fileStream.Seek(0, SeekOrigin.Begin);
                    message.FileName = fileInfo.Name;
                    message.FileSize = fileInfo.Length;
                    message.FileMD5 = fileMD5;
                    if (!this.client.SendMessage(message))
                        return;
                    while (true)
                    {
                        var buffer = new byte[this.client.OptionSendBufferSize];
                        var count = fileStream.Read(buffer, 0, buffer.Length);
                        if (count == 0)
                            break;
                        this.client.SendAsync(buffer, 0, count);
                    }
                }
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.client.Disconnect();
        }

        ConcurrentBag<TcpClient> clientList = new ConcurrentBag<TcpClient>();

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        var client = new TcpClient();
                        client.Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3347);
                        client.ConnectAsync();
                        clientList.Add(client);
                    }
                }) { IsBackground = true }.Start();
            }
        }
    }
}
