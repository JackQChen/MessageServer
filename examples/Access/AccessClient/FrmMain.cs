using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace AccessClient
{
    public partial class FrmMain : Form
    {
        AccessTcpClient client;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            var caption = this.Text;
            this.Text = caption + "(正在授权...)";
            client = new AccessTcpClient();
            client.Received += Client_Received;
            client.Disconnected += Client_Disconnected;
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
            if (client.IsConnected)
                client.Disconnect();
            client.Endpoint.Address = Dns.GetHostAddresses(ConfigurationManager.AppSettings["IP"])[0];
            client.Endpoint.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            if (!client.ConnectAsync())
                WriteLog("授权服务器连接失败!");
        }

        bool isAccess = false;
        string strResult = "";

        private void Client_Disconnected()
        {
            WriteLog("授权信息已失效，请重新授权!");
            this.isAccess = false;
        }

        private void Client_Received(byte[] buffer, int offset, int size)
        {
            strResult += Encoding.UTF8.GetString(buffer, offset, size);
            if (strResult.IndexOf('\n') > 0)
            {
                if (isAccess)
                    WriteLog(strResult.Trim());
                else
                {
                    isAccess = true;
                    var key = ConfigurationManager.AppSettings["Key"];
                    try
                    {
                        var data = Encoding.UTF8.GetBytes(Encrypt.AESDecrypt(strResult.Trim(), key) + "\r\n");
                        this.client.SendAsync(data, 0, data.Length);
                    }
                    catch
                    {
                        WriteLog("授权信息不正确，请核对!");
                    }
                }
                strResult = "";
            }
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

        private void txtMsg_DoubleClick(object sender, EventArgs e)
        {
            this.txtMsg.Clear();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
        }

    }
}
