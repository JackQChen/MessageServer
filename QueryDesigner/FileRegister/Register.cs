using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace FileRegister
{
    public class Register
    {
        /// <summary>
        /// 使文件类型与对应的图标及应用程序关联起来。
        /// </summary>        
        public void RegisterFile(FileInfo regInfo)
        {
            if (FileRegistered(regInfo.ExtendName))
            {
                return;
            }

            string relationName = regInfo.ExtendName.Substring(1, regInfo.ExtendName.Length - 1).ToUpper() + "_FileType";

            RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
            fileTypeKey.SetValue("", relationName);
            fileTypeKey.Close();

            RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            relationKey.SetValue("", regInfo.Description);

            RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue("", regInfo.IcoPath);

            RegistryKey shellKey = relationKey.CreateSubKey("Shell");
            RegistryKey openKey = shellKey.CreateSubKey("Open");
            RegistryKey commandKey = openKey.CreateSubKey("Command");
            commandKey.SetValue("", regInfo.ExePath + " %1");

            relationKey.Close();
        }

        /// <summary>
        /// 得到指定文件类型关联信息
        /// </summary>        
        public FileInfo GetFileReg(string extendName)
        {
            if (!FileRegistered(extendName))
            {
                return null;
            }

            FileInfo regInfo = new FileInfo(extendName);

            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName);
            regInfo.Description = relationKey.GetValue("").ToString();

            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");
            regInfo.IcoPath = iconKey.GetValue("").ToString();

            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command");
            string temp = commandKey.GetValue("").ToString();
            regInfo.ExePath = temp.Substring(0, temp.Length - 3);

            return regInfo;
        }

        /// <summary>
        /// 更新指定文件类型关联信息
        /// </summary>    
        public bool UpdateFileReg(FileInfo regInfo)
        {
            if (!FileRegistered(regInfo.ExtendName))
            {
                return false;
            }


            string extendName = regInfo.ExtendName;
            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";

            try
            {
                RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName, true);
                relationKey.SetValue("", regInfo.Description);

                RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon", true);
                iconKey.SetValue("", regInfo.IcoPath);

                RegistryKey shellKey = relationKey.OpenSubKey("Shell");
                RegistryKey openKey = shellKey.OpenSubKey("Open");
                RegistryKey commandKey = openKey.OpenSubKey("Command", true);
                commandKey.SetValue("", regInfo.ExePath + " %1");

                relationKey.Close();

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未将对象") >= 0)
                {
                    RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
                    fileTypeKey.SetValue("", relationName);
                    fileTypeKey.Close();

                    RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
                    relationKey.SetValue("", regInfo.Description);

                    RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
                    iconKey.SetValue("", regInfo.IcoPath);

                    RegistryKey shellKey = relationKey.CreateSubKey("Shell");
                    RegistryKey openKey = shellKey.CreateSubKey("Open");
                    RegistryKey commandKey = openKey.CreateSubKey("Command");
                    commandKey.SetValue("", regInfo.ExePath + " %1");

                    relationKey.Close();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 指定文件类型是否已经注册
        /// </summary>        
        public bool FileRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                return true;
            }

            return false;
        }
    }
}
