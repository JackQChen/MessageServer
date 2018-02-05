
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using System.Xml;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using ControlLib;
using System.Text;

namespace SystemFramework.BaseControl
{
    public partial class FrmLayout : DevExpress.XtraEditors.XtraForm
    {
        public string category;
        public List<object> LayoutList
        {
            get;
            set;
        }

        public object ControlType
        {
            get;
            set;
        }

        public static void SetLayout(List<object> layoutList, Type type)
        {
            if (layoutList.Count > 0)
            {
                foreach (object obj in layoutList)
                {
                    Type tp = obj.GetType();
                    string fullname = Application.StartupPath + "\\Layout\\" + tp.Name + "\\"
                        + type.Name + "[" + layoutList.IndexOf(obj) + "].xml";
                    if (File.Exists(fullname))
                        if (tp.Equals(typeof(Search)))
                        {
                            Search srh = obj as Search;
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fullname);
                            int srhWidth = 400, srhHeight = 200;
                            int.TryParse(doc.SelectSingleNode("Search/Popup").Attributes["Width"].Value,
                                out srhWidth);
                            int.TryParse(doc.SelectSingleNode("Search/Popup").Attributes["Height"].Value,
                                out srhHeight);
                            srh.SearchWidth = srhWidth;
                            srh.SearchHeight = srhHeight;
                            srh.LayoutContent = doc.SelectSingleNode("Search/GridView").InnerText;
                        }
                        else if (tp.Equals(typeof(GridControl)))
                            (obj as GridControl).MainView.RestoreLayoutFromXml(fullname);
                        else if (tp.Equals(typeof(GridView)))
                            (obj as GridView).RestoreLayoutFromXml(fullname);
                        else if (tp.Equals(typeof(LayoutControl)))
                            (obj as LayoutControl).RestoreLayoutFromXml(fullname);
                        else if (tp.Equals(typeof(TreeList)))
                            (obj as TreeList).RestoreLayoutFromXml(fullname);
                        else if (tp.Equals(typeof(SplitContainerControl)))
                        {
                            int position = 100;
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fullname);
                            int.TryParse(doc.SelectSingleNode("SplitContainerControl/Position").Attributes["Value"].Value,
                                out position);
                            (obj as SplitContainerControl).SplitterPosition = position;
                        }
                }
            }
        }

        public FrmLayout()
        {
            InitializeComponent();
        }

        private void FrmLayout_Load(object sender, System.EventArgs e)
        {
            category = Application.StartupPath + "\\Layout";
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (LayoutList.Count > 0)
            {
                foreach (object obj in LayoutList)
                {
                    Type tp = obj.GetType();
                    string tpCategory = category + "\\" + tp.Name,
                     name = "\\" + (ControlType as Type).Name + "[" + LayoutList.IndexOf(obj) + "].xml";
                    if (!Directory.Exists(tpCategory))
                        Directory.CreateDirectory(tpCategory);
                    if (tp.Equals(typeof(Search)))
                    {
                        Search srh = obj as Search;
                        XmlDocument xml = new XmlDocument();
                        XmlDeclaration xd = xml.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        XmlElement xeRoot = xml.CreateElement("Search");
                        XmlElement xePopup = xml.CreateElement("Popup");
                        XmlAttribute xaW = xml.CreateAttribute("Width");
                        xaW.Value = srh.SearchWidth.ToString();
                        XmlAttribute xaH = xml.CreateAttribute("Height");
                        xaH.Value = srh.SearchHeight.ToString();
                        xePopup.Attributes.Append(xaW);
                        xePopup.Attributes.Append(xaH);
                        MemoryStream stream = new MemoryStream();
                        srh.gvSearch.SaveLayoutToStream(stream);
                        byte[] btData = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(btData, 0, btData.Length);
                        XmlElement xeGridView = xml.CreateElement("GridView");
                        XmlCDataSection xCdata = xml.CreateCDataSection("");
                        xCdata.Data = Encoding.UTF8.GetString(btData);
                        xeGridView.AppendChild(xCdata);
                        xeRoot.AppendChild(xePopup);
                        xeRoot.AppendChild(xeGridView);
                        xml.AppendChild(xeRoot);
                        xml.Save(tpCategory + name);
                    }
                    else if (tp.Equals(typeof(GridControl)))
                    {
                        (obj as GridControl).MainView.SaveLayoutToXml(tpCategory + name);
                    }
                    else if (tp.Equals(typeof(GridView)))
                    {
                        (obj as GridView).SaveLayoutToXml(tpCategory + name);
                    }
                    else if (tp.Equals(typeof(LayoutControl)))
                    {
                        (obj as LayoutControl).SaveLayoutToXml(tpCategory + name);
                    }
                    else if (tp.Equals(typeof(TreeList)))
                    {
                        (obj as TreeList).SaveLayoutToXml(tpCategory + name);
                    }
                    else if (tp.Equals(typeof(SplitContainerControl)))
                    {
                        XmlDocument xml = new XmlDocument();
                        XmlDeclaration xd = xml.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        XmlElement xeRoot = xml.CreateElement("SplitContainerControl");
                        XmlElement xePos = xml.CreateElement("Position");
                        XmlAttribute xa = xml.CreateAttribute("Value");
                        xa.Value = (obj as SplitContainerControl).SplitterPosition.ToString();
                        xePos.Attributes.Append(xa);
                        xeRoot.AppendChild(xePos);
                        xml.AppendChild(xeRoot);
                        xml.Save(tpCategory + name);
                    }
                }
            }
            MessageBoxEx.Show("布局保存成功！", "提示", MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            if (Directory.Exists(category))
                foreach (string fileName in Directory.GetFiles(category, "*.xml", SearchOption.AllDirectories))
                {
                    string name = new FileInfo(fileName).Name;
                    if (name.Split('[').Length > 1)
                        if (name.Split('[')[0] == (ControlType as Type).Name)
                            File.Delete(fileName);
                }
            MessageBoxEx.Show("布局删除成功！", "提示", MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}