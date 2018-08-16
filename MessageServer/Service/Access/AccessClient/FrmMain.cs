using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using MessageLib;

namespace AccessClient
{
    public partial class FrmMain : Form
    {
        TcpClient client;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            var caption = this.Text;
            this.Text = caption + "(正在授权...)";
            client = new TcpClient();
            client.OnReceive += new TcpClientEvent.OnReceiveEventHandler(client_OnReceive);
            client.OnClose += new TcpClientEvent.OnCloseEventHandler(client_OnClose);
            this.Access();
            this.Text = caption;
            this.Close();
            this.notifyIcon1.ShowBalloonTip(3000);
        }

        void WriteLog(string log)
        {
            if (this.txtMsg.IsHandleCreated)
                this.txtMsg.Invoke(new Action(() =>
                {
                    this.txtMsg.AppendText(string.Format("{0}\r\n{1}\r\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));
                }));
        }

        void Access()
        {
            if (client.IsStarted)
                client.Stop();
            if (!client.Connect(
                  ConfigurationManager.AppSettings["IP"],
                  ushort.Parse(ConfigurationManager.AppSettings["Port"]),
                  async: false))
            {
                WriteLog("授权服务器连接失败!");
            }
        }

        HandleResult client_OnClose(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            WriteLog("授权信息已失效，请重新授权!");
            this.isAccess = false;
            return HandleResult.Ignore;
        }

        bool isAccess = false;
        string strResult = "";
        HandleResult client_OnReceive(TcpClient sender, byte[] bytes)
        {
            strResult += Encoding.Default.GetString(bytes);
            if (strResult.IndexOf('\n') > 0)
            {
                if (isAccess)
                {
                    WriteLog(strResult.Trim());
                }
                else
                {
                    isAccess = true;
                    var key = ConfigurationManager.AppSettings["Key"];
                    try
                    {
                        var data = Encoding.Default.GetBytes(Encrypt.AESDecrypt(strResult.Trim(), key) + "\r\n");
                        this.client.Send(data, data.Length);
                    }
                    catch
                    {
                        WriteLog("授权信息不正确，请核对!");
                    }
                }
                strResult = "";
            }
            return HandleResult.Ignore;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void btnAccess_Click(object sender, EventArgs e)
        {
            this.Access();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.FormClosing -= this.FrmMain_FormClosing;
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
        }

    }
}
