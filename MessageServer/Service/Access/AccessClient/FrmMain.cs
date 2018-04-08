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

        private void FrmMain_Load(object sender, EventArgs e)
        {
            client = new TcpClient();
            client.OnReceive += new TcpClientEvent.OnReceiveEventHandler(client_OnReceive);
            client.OnClose += new TcpClientEvent.OnCloseEventHandler(client_OnClose);
            this.Access();
            this.Close();
            this.notifyIcon1.ShowBalloonTip(3000);
        }

        void Access()
        {
            if (client.IsStarted)
                return;
            client.Connect(
                  ConfigurationManager.AppSettings["IP"],
                  ushort.Parse(ConfigurationManager.AppSettings["Port"]),
                  async: false);
        }

        HandleResult client_OnClose(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            if (this.txtMsg.IsHandleCreated)
                this.txtMsg.Invoke(new Action(() =>
                {
                    this.txtMsg.AppendText("授权信息已失效，请重新授权\r\n");
                }));
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
                    if (this.txtMsg.IsHandleCreated)
                        this.txtMsg.Invoke(new Action(() =>
                        {
                            this.txtMsg.AppendText(strResult);
                        }));
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
                        if (this.txtMsg.IsHandleCreated)
                            this.txtMsg.Invoke(new Action(() =>
                            {
                                this.txtMsg.AppendText("授权信息不正确，请核对\r\n");
                            }));
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
            Application.ExitThread();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
        }

    }
}
