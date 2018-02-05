using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileRegister
{
    public class FileInfo
    {
        /// <summary>
        /// 目标类型文件的扩展名
        /// </summary>
        public string ExtendName { get; set; }
        
        /// <summary>
        /// 目标文件类型说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 目标类型文件关联的图标

        /// </summary>
        public string IcoPath { get; set; }

        /// <summary>
        /// 打开目标类型文件的应用程序

        /// </summary>
        public string ExePath { get; set; }

        public FileInfo()
        {

        }

        public FileInfo(string extendName)
        {
            ExtendName = extendName ;
        }
    }
}
