using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoUpdate
{
    public class UpdateInfo
    {
        public UpdateInfo()
        {
            this.FileList = new List<FileItem>();
        }

        public string UpdatePath { get; set; }

        public List<FileItem> FileList { get; set; }
    }

    public class FileItem
    {
        public string ID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string MD5 { get; set; }
    }
}
