using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using FormsDesigner;
using FormsDesigner.Gui;
using FormsDesigner.Xml;
using SnControl;
using WinFormsUI.Docking;
using System.Reflection;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using FormsDesigner.Services;
using System.Runtime.Serialization.Formatters.Binary;

namespace QueryDesigner
{
    public partial class FormContent : DockContent, IViewContent
    {
        public FormContent()
        {
            InitializeComponent();
        }
        public void Init()
        {
            DockHandler.CloseButtonVisible = false;

            if (string.IsNullOrEmpty(_fileName))
            {
                Initialize();
            }
            else
            {
                LoadFromFileName(_fileName);
                if (_solution != null)
                    TabText = _solution.SolutionName;
            }

            MenuService.OnMenuClick += new EventHandler<ToolStripItemClickedEventArgs>(MenuService_OnMenuClick);
        }

        protected DesignPanel _designPanel;
        private FormsDesignerViewContent _designerViewContent;
        private bool _isDirty = false;
        private Solution _solution;
        private string _fileName;

        public Solution Solution
        {
            get
            {
                return _solution;
            }
            set
            {
                _solution = value;
            }
        }

        public FormsDesignerViewContent ViewContent
        {
            get
            {
                return _designerViewContent;
            }
        }

        private void CreateDesignerHost()
        {
            _designerViewContent = new FormsDesignerViewContent(this);
            _designerViewContent.Reload();
            _designPanel = new DesignPanel(_designerViewContent.DesignSurface);
            _designPanel.Dock = DockStyle.Fill;
            Controls.Add(_designPanel);
        }

        private void Initialize()
        {
            CreateDesignerHost();
            Form form = _designerViewContent.Host.CreateComponent(typeof(Form)) as Form;
            form.Width = 700;
            form.Height = 500;
            form.TopLevel = false;
            form.Text = _solution.SolutionName;

            Panel panel = _designerViewContent.Host.CreateComponent(typeof(Panel)) as Panel;
            TabControl tabControl = _designerViewContent.Host.CreateComponent(typeof(TabControl)) as TabControl;
            TabPage tabPage = _designerViewContent.Host.CreateComponent(typeof(TabPage)) as TabPage;
            DataGridView dataGridView = _designerViewContent.Host.CreateComponent(typeof(DataGridView)) as DataGridView;

            form.SuspendLayout();

            panel.Dock = DockStyle.Top;
            panel.Height = 200;
            panel.TabIndex = 0;

            tabControl.Dock = DockStyle.Fill;
            tabControl.Name = "tabMain";
            tabControl.TabIndex = 1;
            tabControl.Controls.Add(tabPage);

            tabPage.Name = "pageMain";
            tabPage.Controls.Add(dataGridView);

            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Name = "grdEntity";

            form.Controls.Add(tabControl);
            form.Controls.Add(panel);
            form.ResumeLayout(false);
            _designPanel.SetRootDesigner();
            _designerViewContent.PropertyContainer.Host = _designPanel.Host;
            _designerViewContent.PropertyContainer.SelectableObjects = _designerViewContent.Host.Container.Components;
        }

        #region IViewContent 成员

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        public event EventHandler FileNameChanged;

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsUntitled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsViewOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void LoadFromFileName(string fileName)
        {
            Stream stream = new FileStream(fileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(stream, System.Text.Encoding.Unicode);
            byte[] buffer = binaryReader.ReadBytes((int)stream.Length);

            //string xmlContent = File.ReadAllText(fileName);

            try
            {
                string xmlContent = System.Text.Encoding.Unicode.GetString(buffer);

                binaryReader.Close();
                stream.Close();

                if (xmlContent.IndexOf("<SnControl.Solution>") == -1)
                {
                    xmlContent = File.ReadAllText(fileName);
                }

                string xmlSolutionContent = xmlContent.Substring(xmlContent.IndexOf("<SnControl.Solution>"), xmlContent.IndexOf("</SnControl.Solution>") - xmlContent.IndexOf("<SnControl.Solution>"));
                xmlSolutionContent += "</SnControl.Solution>";
                string xmlFormContent = xmlContent.Substring(xmlContent.IndexOf("<System.Windows.Forms.Form"), xmlContent.IndexOf("</QuerySolution>") - xmlContent.IndexOf("<System.Windows.Forms.Form"));

                XmlSolutionReader xmlSolutionReader = new XmlSolutionReader(_solution);
                _solution = xmlSolutionReader.SetUpSolution(xmlSolutionContent);
                if (_solution == null)
                {
                    Controls.Clear();
                    return;
                }
                SolutionInstance.GetInstance().Solution = _solution;

                IDesignerLoaderProvider loaderProvider = new XmlDesignerLoaderProvider(xmlFormContent);
                IDesignerGenerator generator = new XmlDesignerGenerator();

                _designerViewContent = new FormsDesignerViewContent(this, loaderProvider, generator);
                //加载XML内容
                _designerViewContent.Reload();
                _designPanel = new DesignPanel(_designerViewContent.DesignSurface);
                _designPanel.Dock = DockStyle.Fill;
                Controls.Add(_designPanel);

                if (!_designPanel.SetRootDesigner())
                {
                    Controls.Clear();
                    return;
                }
                _designerViewContent.PropertyContainer.Host = _designPanel.Host;
                _designerViewContent.PropertyContainer.SelectableObjects = _designerViewContent.Host.Container.Components;
            }
            catch (ApplicationException e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Save(string fileName)
        {
            _fileName = fileName;
            XmlDocument doc = new XmlDocument();
            XmlElement element = doc.CreateElement("QuerySolution");
            XmlAttribute attribute = doc.CreateAttribute("version");
            attribute.InnerText = "1.0";
            element.Attributes.Append(attribute);
            doc.AppendChild(element);

            element = new XmlSolutionGenerator().GetElementFor(doc, SolutionInstance.GetInstance().Solution);

            foreach (XmlNode node in element.ChildNodes)
            {
                doc.DocumentElement.AppendChild(doc.ImportNode(node, true));
            }

            element = new XmlDesignerGenerator().GetElementFor(doc, _designPanel.Host);

            foreach (XmlNode node in element.ChildNodes)
            {
                doc.DocumentElement.AppendChild(doc.ImportNode(node, true));
            }

            doc.Save(fileName);

            //Stream stream = new FileStream(fileName, FileMode.Create);
            //BinaryWriter binaryWriter = new BinaryWriter(stream, System.Text.Encoding.Unicode);
            //byte[] buffer = System.Text.Encoding.Unicode.GetBytes(doc.InnerXml);
            //binaryWriter.Write(buffer);

            //binaryWriter.Close();
            //stream.Close();

            IsDirty = false;
            OnDirtyChanged(this, EventArgs.Empty);
        }

        public void Save()
        {
            string filePath = Application.StartupPath + @"\Query\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath += _solution.SolutionName + ".qs";
            Save(filePath);
            IsDirty = false;
        }

        public void SaveAs(string filePath, string solutionName)
        {
            _solution.SolutionID = solutionName;
            _solution.SolutionName = solutionName;
            Save(filePath);
        }

        public event SaveEventHandler Saved;

        public event EventHandler Saving;

        public List<ISecondaryViewContent> SecondaryViewContents
        {
            get { throw new NotImplementedException(); }
        }

        public string TitleName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler TitleNameChanged;

        public string UntitledName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IBaseViewContent 成员

        public Control Control
        {
            get { return this; }
        }

        public void Deselected()
        {
            throw new NotImplementedException();
        }

        public void Deselecting()
        {
            throw new NotImplementedException();
        }

        public void RedrawContent()
        {
            throw new NotImplementedException();
        }

        public void Selected()
        {
            throw new NotImplementedException();
        }

        public void SwitchedTo()
        {
            throw new NotImplementedException();
        }

        public string TabPageText
        {
            get
            {
                return TabText;
            }
        }

        public IWorkbenchWindow WorkbenchWindow
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICanBeDirty 成员

        public event EventHandler DirtyChanged;

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                _isDirty = value;
                OnDirtyChanged(this, EventArgs.Empty);
            }
        }

        protected void OnDirtyChanged(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                if (TabText.IndexOf("*") < 0)
                {
                    TabText = TabText + "*";
                }
            }
            else
            {
                TabText = _solution.SolutionName;
            }

            if (DirtyChanged != null)
            {
                DirtyChanged(sender, e);
            }
        }

        #endregion

        private void FormContent_Load(object sender, EventArgs e)
        {

        }

        private void MenuService_OnMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "锁定控件":
                    ViewContent.LockControls();
                    break;
                case "粘贴":
                    ViewContent.Paste();
                    break;
                case "置于顶层":
                    ViewContent.BringToFront();
                    break;
                case "置于底层":
                    ViewContent.SendToBack();
                    break;
                case "对齐到网格":
                    ViewContent.SnapToGrid();
                    break;
                case "剪切":
                    ViewContent.Cut();
                    break;
                case "复制":
                    ViewContent.Copy();
                    break;
                case "删除":
                    ViewContent.Delete();
                    break;
                case "属性":
                    if (DockPanel != null)
                        for (int i = 0; i < DockPanel.Contents.Count; i++)
                        {
                            if (DockPanel.Contents[i] is FormProperties)
                            {
                                DockPanel.Contents[i].DockHandler.Activate();
                                break;
                            }
                        }
                    break;
            }
        }

        private void FormContent_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (IDockContent content in DockPanel.Documents)
            {
                if (content is FormProperties || content is FormToolBox)
                {
                    content.DockHandler.DockState = DockState.Hidden;
                    content.DockHandler.Close();
                }
            }

            FormMain form = DockPanel.Parent as FormMain;
            form.OpenPath = string.Empty;
        }
    }
}
