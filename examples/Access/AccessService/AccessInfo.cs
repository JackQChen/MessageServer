using System;
using System.Collections.Generic;
using System.Linq;

namespace AccessService
{
    public class AccessInfo : MarshalByRefObject
    {
        internal static AccessInfo instance = new AccessInfo();

        public List<string> IPList { get; set; }

        public string AccessKey { get; set; }

        public AccessInfo()
        {
            IPList = new List<string>();
        }

        public bool IsVerified(string ip)
        {
            return IPList.Select(s => s.Split(':')[0]).Contains(ip.Split(':')[0]);
        }

    }
}
