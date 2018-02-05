using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace CustomSkin
{
    public class Res
    {
        private Assembly myAssembly;
        private Res(Assembly assembly)
        {
            myAssembly = assembly;
        }
        private Dictionary<string, Image> dicRes;
        public List<Image> ImageList { get; set; }

        private static Res _res;

        public static Res Current
        {
            get
            {
                if (_res == null)
                {
                    //设置资源所在程序集
                    //_res = new Res(Assembly.GetCallingAssembly());
                    //目前使用自身程序集资源
                    _res = new Res(typeof(Res).Assembly);
                    _res.dicRes = new Dictionary<string, Image>();
                    _res.ImageList = new List<Image>();
                }
                return _res;
            }
        }
        public static Res Created(Assembly assembly)
        {
            return new Res(assembly);
        }
        public static Res Created(Type type)
        {
            return new Res(type.Assembly);
        }
        public Image GetImage(string name)
        {
            if (dicRes.ContainsKey(name))
                return dicRes[name];
            else
            {
                Image image = null;
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name is null");
                }
                StringBuilder strBuilder = new StringBuilder();
                string fullName = string.Format("{0}.{1}", myAssembly.GetName().Name, name);
                using (var stream = myAssembly.GetManifestResourceStream(fullName))
                {
                    image = Image.FromStream(stream);
                }
                dicRes.Add(name, image);
                return image;
            }
        }

    }
}
