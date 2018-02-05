using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace UpdateConfig
{
    public partial class FrmMain : Form
    {
        JavaScriptSerializer convert = new JavaScriptSerializer();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.txtLocalPath.Text = ConfigurationManager.AppSettings["LocalPath"];
            this.folderBrowserDialog1.SelectedPath = this.txtLocalPath.Text;
            this.txtWebPath.Text = ConfigurationManager.AppSettings["WebPath"];
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.txtLocalPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtLocalPath.Text))
            {
                MessageBox.Show("更新路径不能为空", "提示");
                return;
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["LocalPath"].Value = this.txtLocalPath.Text;
            config.AppSettings.Settings["WebPath"].Value = this.txtWebPath.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            UpdateInfo info = new UpdateInfo();
            info.UpdatePath = this.txtWebPath.Text;
            foreach (var fileFullPath in Directory.GetFiles(this.txtLocalPath.Text, "*.*", SearchOption.AllDirectories))
            {
                info.FileList.Add(new FileItem()
                {
                    ID = info.FileList.Count.ToString(),
                    Name = Path.GetFileName(fileFullPath),
                    Path = Path.GetDirectoryName(fileFullPath).Replace(this.txtLocalPath.Text, ""),
                    Size = new FileInfo(fileFullPath).Length,
                    MD5 = GetMD5HashFromFile(fileFullPath)
                });
            }
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "UpdateConfig.dat", convert.Serialize(info));
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "UpdateTime.dat", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            MessageBox.Show("生成成功!", "提示");
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
    }
}
