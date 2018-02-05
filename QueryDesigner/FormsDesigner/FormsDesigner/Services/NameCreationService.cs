namespace FormsDesigner.Services
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics;

    public class NameCreationService : INameCreationService
    {
        string INameCreationService.CreateName(IContainer container, Type type)
        {
            int num6;
            ComponentCollection components = container.Components;
            int num = 0x7fffffff;
            int num2 = -2147483648;
            int num3 = 0;
            for (int i = 0; i < components.Count; i++)
            {
                Component component = components[i] as Component;
                if (component.GetType() == type)
                {
                    num3++;
                    string name = component.Site.Name;
                    if (name.StartsWith(type.Name))
                    {
                        try
                        {
                            int num5 = int.Parse(name.Substring(type.Name.Length));
                            if (num5 < num)
                            {
                                num = num5;
                            }
                            if (num5 > num2)
                            {
                                num2 = num5;
                            }
                        }
                        catch (Exception exception)
                        {
                            Trace.WriteLine(exception.ToString());
                        }
                    }
                }
            }
            if (num3 == 0)
            {
                return (type.Name + "1");
            }
            if (num > 1)
            {
                num6 = num - 1;
                return (type.Name + num6.ToString());
            }
            num6 = num2 + 1;
            return (type.Name + num6.ToString());
        }

        bool INameCreationService.IsValidName(string name)
        {
            return true;
        }

        void INameCreationService.ValidateName(string name)
        {
        }
    }
}

