using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MessageLib;
using System.Text;

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
            client.Connect(
                ConfigurationManager.AppSettings["IP"],
                ushort.Parse(ConfigurationManager.AppSettings["Port"]),
                async: false);
            var data = Encoding.Default.GetBytes(ConfigurationManager.AppSettings["Key"] + "\r\n");
            client.Send(data, data.Length);
            this.Close();
            this.notifyIcon1.ShowBalloonTip(3000);
        }

        string strResult = "";
        HandleResult client_OnReceive(TcpClient sender, byte[] bytes)
        {
            var str = Encoding.Default.GetString(bytes);
            strResult += str;
            if (strResult.IndexOf('\n') > 0)
                this.txtMsg.AppendText(strResult);
            return HandleResult.Ignore;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIcon1.Visible = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            //托盘区图标隐藏
            notifyIcon1.Visible = false;
            this.Show();
        }

    }
}
