using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MessageLib;
using CommonService;
using System.Threading;

namespace CommonClient
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        TcpClient client = new TcpClient();
        Process process = new Process();
        ExtraData data = new ExtraData();
        Action<string> actlog;

        private void FrmMain_Load(object sender, EventArgs e)
        {
            actlog = log =>
            {
                this.textBox1.AppendText(log + "\r\n");
            };
            client.OnReceive += new TcpClientEvent.OnReceiveEventHandler(client_OnReceive);
            process.ReceiveMessage += new Action<IntPtr, CommonService.Message>(process_ReceiveMessage);
        }

        HandleResult client_OnReceive(TcpClient sender, byte[] bytes)
        {
            process.RecvData(client.ConnectionId, data, bytes);
            return HandleResult.Ok;
        }

        void process_ReceiveMessage(IntPtr connId, CommonService.Message obj)
        {
            this.textBox1.Invoke(actlog, obj.Content);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = client.Connect("127.0.0.1", 3347, false).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                var message = new CommonService.Message();
                message.Content = "Data:" + i;
                var bytes = this.process.FormatterMessageBytes(message);
                this.client.Send(bytes, bytes.Length);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.client.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        var client = new TcpClient();
                        client.Connect("127.0.0.1", 3347);
                        //client.Stop();
                    }
                }) { IsBackground = true }.Start();
            }
        }
    }
}
