using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AutoUpdate
{
    public class UpdateCheck
    {
        string ftpUserName, ftpPassword;
        string configPath, updateTimeName = "UpdateTime.dat";

        [DllImport("user32.dll")]
        static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string Caption, int type, int DWORD, int time);

        public DialogResult MessageBoxTimeout(IntPtr handle, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, int time)
        {
            return (System.Windows.Forms.DialogResult)MessageBoxTimeoutA(handle, text, caption, buttons.GetHashCode() | icon.GetHashCode() | defaultButton.GetHashCode(), 0, time);
        }

        public UpdateCheck()
        {
            this.ftpUserName = ConfigurationManager.AppSettings["UserName"];
            this.ftpPassword = ConfigurationManager.AppSettings["Password"];
            this.configPath = ConfigurationManager.AppSettings["UpdateUrl"];
        }

        public DateTime GetUpdateTime()
        {
            return this.GetUpdateTime(this.configPath, updateTimeName);
        }

        public DateTime GetUpdateTime(string remoteFilePath, string remoteFileName)
        {
            FtpWebRequest reqFTP;
            DateTime dtUpdate = DateTime.MinValue;
            try
            {
                MemoryStream outputStream = new MemoryStream();
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(string.Format("{0}//{1}", remoteFilePath, remoteFileName)));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                dtUpdate = Convert.ToDateTime(Encoding.Default.GetString(outputStream.ToArray()));
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBoxTimeout(IntPtr.Zero, ex.Message, "自动更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 5000);
            }
            return dtUpdate;
        }
    }
}
