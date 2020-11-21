using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonService
{
    [Serializable]
    public class Message
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public byte[] Data { get; set; }
    }
}
