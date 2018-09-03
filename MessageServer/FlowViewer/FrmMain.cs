using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowViewer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        Process serverProc;

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            this.wbPerformance.Navigate(AppDomain.CurrentDomain.BaseDirectory + "index.html");
            Task.Factory.StartNew(() =>
            {
                int count = 0;
                while (true)
                {
                    if (serverProc != null)
                        if (serverProc.HasExited)
                            Application.Exit();
                    Thread.Sleep(10000);
                    count++;
                    if (count > 5)
                    {
                        this.ClearMemory();
                        count = 0;
                    }
                }
            });
        }

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        private void wbPerformance_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.wbPerformance.Document.InvokeScript("initRate", new object[] { this.wbPerformance.Height });
        }

        private void wbPerformance_SizeChanged(object sender, System.EventArgs e)
        {
            if (this.wbPerformance.Document != null)
                this.wbPerformance.Document.InvokeScript("initRate", new object[] { this.wbPerformance.Height });
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == 0x0C)
            {
                var strText = Marshal.PtrToStringUni(m.LParam);
                switch ((int)m.WParam)
                {
                    case 1:
                        {
                            this.serverProc = Process.GetProcessById(Convert.ToInt32(strText));
                        }
                        break;
                    case 2:
                        {
                            if (this.wbPerformance.Document != null)
                            {
                                var rate = strText.Split(',');
                                this.wbPerformance.Document.InvokeScript("setRate", new object[] { Convert.ToInt32(rate[0]), Convert.ToInt32(rate[1]) });
                            }
                        }
                        break;
                }
            }
            else
                base.DefWndProc(ref m);
        }
    }
}
