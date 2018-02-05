using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QueryDesigner
{
    public static class Compare
    {
        internal static bool CompareInitialize()
        {
            string p = global::QueryDesigner.Properties.Resources.String2;
            FileStream f;

            if (!File.Exists(p))
            {
                f = File.Create(p);

                StreamWriter w = new StreamWriter(f);

                w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));

                w.Close();
                f.Close();
            }
            else
            {
                f = File.Open(p, FileMode.Open);
                StreamReader r = new StreamReader(f);
                DateTime d = DateTime.Parse(r.ReadLine());
                if (DateTime.Now > d.AddDays(360))
                {
                    return false;
                }

                r.Close();
                f.Close();
            }

            return true;
        }
    }
}
