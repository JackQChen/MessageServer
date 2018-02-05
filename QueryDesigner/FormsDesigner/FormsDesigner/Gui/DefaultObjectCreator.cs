namespace FormsDesigner.Gui
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;

    public class DefaultObjectCreator : IObjectCreator
    {
        public virtual object CreateObject(string name, XmlElement el, object obj)
        {
            try
            {
                object obj2 = typeof(Control).Assembly.CreateInstance(name);
                if (obj2 == null)
                {
                    obj2 = typeof(Point).Assembly.CreateInstance(name);
                }
                if (obj2 == null)
                {
                    obj2 = typeof(string).Assembly.CreateInstance(name);
                }
                if (obj2 == null)
                {
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly assembly in assemblies)
                    {
                        obj2 = assembly.CreateInstance(name);
                        if (obj2 != null)
                        {
                            break;
                        }
                    }
                }
                if (obj2 is Control)
                {
                    ((Control)obj2).SuspendLayout();
                }
                return obj2;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual System.Type GetType(string name)
        {
            System.Type type = typeof(Control).Assembly.GetType(name);
            if (type == null)
            {
                type = typeof(Point).Assembly.GetType(name);
            }
            if (type == null)
            {
                type = typeof(string).Assembly.GetType(name);
            }
            if (type == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    type = assembly.GetType(name);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return type;
        }
    }
}

