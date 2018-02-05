using System.Collections;
using System.IO;
using System.Xml;
namespace SystemFramework
{
    public class NewXml
    {
        private string _fileName;
        private XmlDocument xml = new XmlDocument();

        public NewXml()
        {
        }

        public XmlDocument Document
        {
            get
            {
                return xml;
            }
        }

        public NewXml(string fileName)
        {
            string name = fileName.ToLower().IndexOf(".xml") > 0 ? fileName : fileName + ".xml";
            _fileName = name;
            xml.Load(_fileName);
        }

        public NewXml(string fileName, string DefaultContent)
        {
            string name = fileName.ToLower().IndexOf(".xml") > 0 ? fileName : fileName + ".xml";
            _fileName = name;
            if (!File.Exists(name))
            {
                //新生成的XML文档内容
                string writestr = string.IsNullOrEmpty(DefaultContent) ? @"<?xml version=""1.0"" encoding=""UTF-8""?><Config></Config>" : DefaultContent;
                //@"<?xml version=""1.0"" encoding=""UTF-8""?>
                //<Config>
                //<DBConfig Type=""SQL"">
                //<OLE Database=""""/>
                //<SQL IP="""" Mode="""" User="""" Password="""" Database=""""></SQL>
                //<DB2 IP="""" Mode="""" User="""" Password="""" Database=""""></DB2>
                //<Oracle IP="""" Mode="""" User="""" Password="""" Database=""""></Oracle>
                //</DBConfig> 
                //<LoginInfo RememberName="""" LastLoginUser=""""/>                
                //<RemotingConfiguration>
                //<ServerProvider TypeFilterLevel=""Full""/>
                //<ChannelInfo Type=""0"" Port=""0"" />  
                //</RemotingConfiguration>
                //</Config>";
                StreamWriter sw = new StreamWriter(name, false, System.Text.Encoding.UTF8);
                sw.Write(writestr);
                sw.Flush();
                sw.Close();
            }
            xml.Load(_fileName);
        }

        public bool SaveItems(string NodeName, string newName)
        {
            Hashtable ht = new Hashtable();
            foreach (XmlNode node in xml.SelectNodes(NodeName))
            {
                if (!ht.Contains(node.InnerText))
                    ht.Add(node.InnerText, null);
            }
            if (!ht.Contains(newName))
            {
                XmlElement element = xml.CreateElement(Document.SelectSingleNode(NodeName).Name);
                element.InnerText = newName;
                Document.SelectSingleNode(NodeName).ParentNode.AppendChild(element);
                xml.Save(_fileName);
            }
            return true;
        }

        public string[] GetItems(string NodeName)
        {
            XmlNodeList list = xml.SelectNodes(NodeName);
            string[] Rst = new string[list.Count];
            for (int i = 0; i < Rst.Length; i++)
            {
                Rst[i] = list[i].InnerText;
            }
            return Rst;
        }

        public void Save()
        {
            xml.Save(_fileName);
        }

    }
}
