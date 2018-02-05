namespace FormsDesigner.Services
{
    using System;
    using System.ComponentModel.Design;
    using System.Reflection;

    public class TypeResolutionService : ITypeResolutionService
    {
        public Assembly GetAssembly(AssemblyName name)
        {
            return this.GetAssembly(name, false);
        }

        public Assembly GetAssembly(AssemblyName name, bool throwOnError)
        {
            return Assembly.Load(name);
        }

        public string GetPathOfAssembly(AssemblyName name)
        {
            Assembly assembly = this.GetAssembly(name);
            if (assembly != null)
            {
                return assembly.Location;
            }
            return null;
        }

        public Type GetType(string name)
        {
            return this.GetType(name, false);
        }

        public Type GetType(string name, bool throwOnError)
        {
            return this.GetType(name, throwOnError, false);
        }

        public Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            if ((name == null) || (name.Length == 0))
            {
                return null;
            }
            Assembly assembly = null;
            foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly2.GetType(name, throwOnError) != null)
                {
                    assembly = assembly2;
                }
            }
            if (assembly != null)
            {
                return assembly.GetType(name, throwOnError, ignoreCase);
            }
            Type type2 = Type.GetType(name, throwOnError, ignoreCase);
            if ((type2 == null) && (name.IndexOf(",") > 0))
            {
                string[] strArray = name.Split(new char[] { ',' });
                string str = strArray[0];
                string assemblyString = strArray[1].Substring(1);
                Assembly assembly3 = null;
                try
                {
                    assembly3 = Assembly.Load(assemblyString);
                }
                catch (Exception)
                {
                }
                if (assembly3 != null)
                {
                    type2 = assembly3.GetType(str, throwOnError, ignoreCase);
                }
                else
                {
                    type2 = Type.GetType(str, throwOnError, ignoreCase);
                }
            }
            return type2;
        }

        public void ReferenceAssembly(AssemblyName name)
        {
            Console.WriteLine("TODO!!! : Add Assembly reference : " + name);
        }
    }
}

