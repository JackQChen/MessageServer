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
            ////从FTP取更新时间
            //return this.GetUpdateTime(this.configPath, updateTimeName);
            ////////////////////////////使用FTP获取则可删除////////////////////////////
            //从GitHub取更新时间
            DateTime dtUpdate = DateTime.MinValue;
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create("https://raw.githubusercontent.com/chen365409389/QueueSystem/master/Update/UpdateTime.dat");
                Stream stream = request.GetResponse().GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                dtUpdate = Convert.ToDateTime(streamReader.ReadToEnd());
                streamReader.Close();
                stream.Close();
            }
            catch
            {
                //GitHub获取失败则从FTP取更新时间
                dtUpdate = this.GetUpdateTime(this.configPath, updateTimeName);
            }
            return dtUpdate;
            ///////////////////////////////////////////////////////////////////////////
        }

        public DateTime GetUpdateTime(string remoteFilePath, string remoteFileName)
        {
            FtpWebRequest reqFTP;
            DateTime dtUpdate = DateTime.MinValue;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(string.Format("{0}//{1}", remoteFilePath, remoteFileName)));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(ftpStream, Encoding.UTF8);
                dtUpdate = Convert.ToDateTime(streamReader.ReadToEnd());
                streamReader.Close();
                ftpStream.Close();
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
