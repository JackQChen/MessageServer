using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AccessService
{
    public class INIOperation
    {
        private static readonly string filePath = AppDomain.CurrentDomain.BaseDirectory + "Access.ini";
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        public static string ReadString(string section, string key)
        {
            return ReadString(section, key, "");
        }

        public static string ReadString(string section, string key, string defaultString)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, defaultString, temp, 1024, filePath);
            return temp.ToString();
        }

        public static void WriteString(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }
    }
}
