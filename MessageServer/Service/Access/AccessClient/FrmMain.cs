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
            var data = Encoding.Default.GetBytes(ConfigurationManager.AppSettings["Key"] + "\r\n");
            client.Send(data, data.Length);
        }

        HandleResult client_OnClose(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            if (!this.txtMsg.IsHandleCreated)
                return HandleResult.Ignore;
            this.txtMsg.Invoke(new Action(() =>
            {
                this.txtMsg.AppendText("授权信息已失效，请重新授权\r\n");
            }));
            return HandleResult.Ignore;
        }

        string strResult = "";
        HandleResult client_OnReceive(TcpClient sender, byte[] bytes)
        {
            if (!this.txtMsg.IsHandleCreated)
                return HandleResult.Ignore;
            var str = Encoding.Default.GetString(bytes);
            strResult += str;
            if (strResult.IndexOf('\n') > 0)
            {
                this.txtMsg.Invoke(new Action(() =>
                {
                    this.txtMsg.AppendText(strResult);
                }));
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
