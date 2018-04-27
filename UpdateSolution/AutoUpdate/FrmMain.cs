using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class FrmMain : Form
    {

        string configPath, configName = "UpdateConfig.dat";
        string ftpUserName, ftpPassword;
        JavaScriptSerializer convert = new JavaScriptSerializer();
        Action<string, bool> actUpdate;
        Action<string> actFocus;
        string currentFileID;
        long currentCount;
        public DateTime dtLastUpdateTime;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.actUpdate = (key, result) =>
            {
                var item = this.lvUpdate.Items[key];
                item.SubItems[3].Text = result ? "100" : "0";
                item.SubItems[4].Tag = null;
                var index = item.Index + 1;
                if (index >= this.lvUpdate.Items.Count - 1)
                    index = this.lvUpdate.Items.Count - 1;
                this.lvUpdate.Items[index].EnsureVisible();
            };
            this.actFocus = key =>
            {
                var item = this.lvUpdate.Items[key];
                if (!item.Focused)
                    item.Focused = true;
            };
            this.ftpUserName = ConfigurationManager.AppSettings["UserName"];
            this.ftpPassword = ConfigurationManager.AppSettings["Password"];
            this.configPath = ConfigurationManager.AppSettings["UpdateUrl"];
            this.Download(this.configPath, configName, "", configName);
            this.UpdateFileList();
        }

        public bool Download(string remoteFilePath, string remoteFileName, string localFilePath, string localFileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                var localDir = AppDomain.CurrentDomain.BaseDirectory + localFilePath;
                if (!Directory.Exists(localDir))
                    Directory.CreateDirectory(localDir);
                FileStream outputStream = new FileStream(string.Format("{0}\\{1}", localDir, localFileName), FileMode.Create);
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
                    currentCount += readCount;
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateFileList()
        {
            var configPath = AppDomain.CurrentDomain.BaseDirectory + configName;
            if (!File.Exists(configPath))
            {
                MessageBox.Show("获取更新配置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var info = convert.Deserialize<UpdateInfo>(File.ReadAllText(configPath));
            File.Delete(configPath);
            var localPath = Application.StartupPath;
            var q = from file in Directory.GetFiles(localPath, "*.*", SearchOption.AllDirectories)
                    join fileNew in info.FileList
                    on new
                    {
                        Path = Path.GetDirectoryName(file).Replace(localPath, ""),
                        Name = Path.GetFileName(file),
                        MD5 = GetMD5HashFromFile(file)
                    } equals new
                    {
                        Path = fileNew.Path,
                        Name = fileNew.Name,
                        MD5 = fileNew.MD5
                    }
                    select fileNew.ID;
            info.FileList.RemoveAll(m => q.Contains(m.ID));
            foreach (var fileItem in info.FileList)
            {
                var item = this.lvUpdate.Items.Add(fileItem.ID, fileItem.Name, -1);
                item.SubItems.Add(FormatFileSize(fileItem.Size));
                item.SubItems.Add(fileItem.MD5);
                item.SubItems.Add("0");
                item.SubItems.Add("").Tag = fileItem.Size;
            }
            new Thread(obj =>
            {
                UpdateInfo tInfo = obj as UpdateInfo;
                var result = true;
                foreach (var fileItem in info.FileList)
                {
                    this.lvUpdate.Invoke(this.actFocus, fileItem.ID);
                    currentFileID = fileItem.ID;
                    currentCount = 0;
                    var fileResult = false;
                    if (fileItem.Name == "AutoUpdate.exe")
                        fileResult = this.Download(tInfo.UpdatePath + fileItem.Path, fileItem.Name, fileItem.Path, "AutoUpdate.exe.tmp");
                    else
                        fileResult = this.Download(tInfo.UpdatePath + fileItem.Path, fileItem.Name, fileItem.Path, fileItem.Name);
                    this.lvUpdate.Invoke(this.actUpdate, fileItem.ID, fileResult);
                    result = result && fileResult;
                }
                if (result)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["UpdateTime"].Value = this.dtLastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    config.Save(ConfigurationSaveMode.Modified);
                }
                this.Invoke(new Action(() => { this.Close(); }));
            }) { IsBackground = true }.Start(info);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var item = this.lvUpdate.Items[this.currentFileID];
            if (item == null)
                return;
            var tag = item.SubItems[4].Tag;
            if (tag == null)
                return;
            var totalCount = Convert.ToDecimal(tag);
            if (totalCount == 0)
                return;
            item.SubItems[3].Text = (this.currentCount / totalCount * 100).ToString();
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file;
            if (File.Exists(fileName))
                file = File.OpenRead(fileName);
            else
                return null;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                str.Append(retVal[i].ToString("x2"));
            }
            return str.ToString().Trim().ToUpper();
        }

        internal static string FormatFileSize(long fileSize)
        {
            if (fileSize < 0)
                return "ErrorSize";
            else if (fileSize >= 1024 * 1024 * 1024)
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            else if (fileSize >= 1024 * 1024)
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            else if (fileSize >= 1024)
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            else
                return string.Format("{0} Bytes", fileSize);
        }
    }
}
