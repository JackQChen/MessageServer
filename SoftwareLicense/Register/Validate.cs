
using System;
using System.IO;
using System.Management;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Register
{
    public sealed class Validate
    {

        private RSACryption rsa = new RSACryption();
        private JavaScriptSerializer convert = new JavaScriptSerializer();
        private string machineCode;

        private readonly string publicKey = @"
此处公钥需要通过rsa.RSAKey()来进行生成，与Generator项目中私钥是一对
";

        public Validate()
        {
            machineCode = MD5Crytion.Encrypt(GetCpuID() + GetMacAddr()).ToUpper();
        }

        #region 如果为单用户通过机器码获取注册码

        /// <summary>
        /// 获取CPUID
        /// </summary>
        /// <returns></returns>
        private string GetCpuID()
        {
            try
            {
                string cpuInfo = "";//cpu序列号   
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
        }

        #region 未处理移动盘

        ///// <summary>
        ///// 获取硬盘ID
        ///// </summary>
        ///// <returns></returns>
        //private string GetDiskID()
        //{
        //    try
        //    {
        //        String HDid = "";
        //        ManagementClass mc = new ManagementClass("Win32_DiskDrive");
        //        ManagementObjectCollection moc = mc.GetInstances();
        //        foreach (ManagementObject mo in moc)
        //        {
        //            HDid = (string)mo.Properties["Model"].Value;
        //        }
        //        moc = null;
        //        mc = null;
        //        return HDid;
        //    }
        //    catch
        //    {
        //        return "unknow";
        //    }
        //}

        #endregion

        /// <summary> 
        /// 获取网卡物理地址 
        /// </summary> 
        /// <returns></returns> 
        private string GetMacAddr()
        {
            try
            {
                string madAddr = null;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                    {
                        madAddr = mo["MacAddress"].ToString();
                    }
                    mo.Dispose();
                }
                return madAddr;
            }
            catch
            {
                return null;
            }
        }

        internal string MachineCode
        {
            get
            {
                return this.machineCode;
            }
        }

        #endregion

        #region 验证信息

        /// <summary>
        /// 验证是否为当前机器
        /// </summary>
        /// <param name="RegString"></param>
        /// <returns></returns>
        private bool IsCurrentMachine(string machineCode)
        {
            return machineCode == this.MachineCode ? true : false;
        }

        /// <summary>
        /// 效期是否有效
        /// </summary>
        private bool IsValidity(string validity)
        {
            return DateTime.Now < Convert.ToDateTime(validity);
        }

        internal bool CheckReg(string regString, bool showMsg)
        {
            try
            {
                var infoString = Encoding.Default.GetString(Convert.FromBase64String(regString));
                var reg = convert.Deserialize<RegInfo>(infoString);
                if (rsa.SignatureDeformatter(publicKey, rsa.GetHash(convert.Serialize(reg.RegBase)), reg.Signature))
                {
                    if (!IsCurrentMachine(reg.RegBase.MachineCode))
                    {
                        if (showMsg)
                            MessageBox.Show("非当前机器授权信息，请核对！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (!IsValidity(reg.RegBase.ExpiryDate))
                    {
                        if (showMsg)
                            MessageBox.Show("授权时间已过期，请重新注册！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    if (showMsg)
                        MessageBox.Show("授权信息验证失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch
            {
                if (showMsg)
                    MessageBox.Show("授权信息不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        static Validate validate = new Validate();

        public static bool Check()
        {
            return Check(true);
        }

        public static bool Check(bool showRegister)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "key.lic", strReg = "";
            if (File.Exists(strPath))
                strReg = File.ReadAllText(strPath, Encoding.Default);
            if (validate.CheckReg(strReg, showRegister))
            {
                return true;
            }
            else
            {
                if (showRegister)
                {
                    using (var frm = new FrmRegister())
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                            return true;
                    }
                }
                return false;
            }
        }

        #endregion

    }
}
