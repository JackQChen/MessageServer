using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageServer.Extension;

namespace MessageServer.Controls
{
    public class ViewerHost : Panel
    {
        public struct UpdateInfo
        {
            public DateTime CollectedTime;
            public int ConnectionCount;
            public long SendingSpeed;
            public long ReceivingSpeed;
        }

        public event Action OnEmbed;
        BlockingCollection<UpdateInfo> collection = new BlockingCollection<UpdateInfo>();
        ServiceInfo serviceInfo;

        public ViewerHost()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Update(collection.Take());
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Start()
        {
            Start(string.Empty);
        }

        public void Start(string args)
        {
            if (m_AppProcess != null)
                Stop();
            try
            {
                serviceInfo = Activator.GetObject(typeof(ServiceInfo), string.Format("ipc://FlowViewer{0}/ServiceInfo", args)) as ServiceInfo;
                ProcessStartInfo info = new ProcessStartInfo(this.m_AppFileName);
                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.Arguments = args;
                m_AppProcess = Process.Start(info);
                Application.Idle += Application_Idle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Stop();
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (this.m_AppProcess == null || this.m_AppProcess.HasExited)
            {
                this.m_AppProcess = null;
                Application.Idle -= Application_Idle;
                return;
            }
            if (m_AppProcess.MainWindowHandle == IntPtr.Zero)
                return;
            Application.Idle -= Application_Idle;
            EmbedProcess(m_AppProcess, this);
            if (this.OnEmbed != null)
                this.OnEmbed();
        }

        public void Stop()
        {
            if (m_AppProcess != null)
            {
                try
                {
                    if (!m_AppProcess.HasExited)
                        m_AppProcess.Kill();
                }
                catch (Exception)
                {
                }
                m_AppProcess = null;
            }
        }

        public void Update(UpdateInfo info)
        {
            try
            {
                if (this.m_AppProcess != null)
                    serviceInfo.Update(info.CollectedTime, info.ConnectionCount, info.SendingSpeed, info.ReceivingSpeed);
            }
            catch
            {
            }
        }

        public void UpdateAsync(UpdateInfo info)
        {
            this.collection.Add(info);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            Stop();
            base.OnHandleDestroyed(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            if (m_AppProcess != null)
                MoveWindow(m_AppProcess.MainWindowHandle, 0, 0, this.Width, this.Height, true);
            base.OnResize(eventargs);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        #region 属性

        Process m_AppProcess = null;
        public Process AppProcess
        {
            get { return this.m_AppProcess; }
            set { this.m_AppProcess = value; }
        }

        private string m_AppFileName = "";
        public string AppFileName
        {
            get { return m_AppFileName; }
            set { m_AppFileName = value; }
        }

        public bool IsStarted { get { return (this.m_AppProcess != null); } }

        #endregion 属性

        #region Win32 API

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;

        #endregion

        private void EmbedProcess(Process app, Control control)
        {
            if (app == null || app.MainWindowHandle == IntPtr.Zero || control == null)
                return;
            try
            {
                SetParent(app.MainWindowHandle, control.Handle);
                SetWindowLong(new HandleRef(this, app.MainWindowHandle), GWL_STYLE, WS_VISIBLE);
                MoveWindow(app.MainWindowHandle, 0, 0, control.Width, control.Height, true);
            }
            catch
            {
            }
        }
    }
}
