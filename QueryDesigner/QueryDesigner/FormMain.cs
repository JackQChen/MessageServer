using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FileRegister;
using FormsDesigner;
using FormsDesigner.Gui;
using FormsDesigner.Services;
using QueryDataAccess;
using SnControl;
using WinFormsUI.Docking;

namespace QueryDesigner
{
    public partial class FormMain : Form, IWorkbench
    {
        public FormMain(string openPath)
        {
            InitializeComponent();

            Icon = global::QueryDesigner.Properties.Resources.Main;
            QueryDAO.DataType = System.Configuration.ConfigurationManager.AppSettings["dbType"];
            QueryDAO.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[QueryDAO.DataType].ConnectionString;
            WorkbenchSingleton.Workbench = this;
            OpenPath = openPath;
        }

        private List<IViewContent> _workbenchContentCollection = new List<IViewContent>();
        private List<IPadContent> _viewContentCollection = new List<IPadContent>();
        private bool _closeAll;
        private IWorkbenchLayout _layout = null;

        public string OpenPath { get; set; }

        public event ViewContentEventHandler ViewOpened;
        public event ViewContentEventHandler ViewClosed;

        protected virtual void OnViewOpened(ViewContentEventArgs e)
        {
            if (ViewOpened != null)
            {
                ViewOpened(this, e);
            }
        }

        protected virtual void OnViewClosed(ViewContentEventArgs e)
        {
            if (ViewClosed != null)
            {
                ViewClosed(this, e);
            }
        }

        public IViewContent ActiveViewContent
        {
            get
            {
                if (dockPanel.DocumentsCount == 0)
                {
                    return null;
                }
                else if (dockPanel.ActiveDocument == null)
                {
                    return (FormContent)dockPanel.Documents.First();
                }
                else
                {
                    return (FormContent)dockPanel.ActiveDocument;
                }
            }
        }

        private void InitMenuToolBar(bool enabled)
        {
            menuSave.Enabled = menuDelete.Enabled = menuClose.Enabled
                = toolSave.Enabled = toolDelete.Enabled = toolClose.Enabled = enabled;
            Text = "查询方案设计器";
        }

        private void NewSolution(string solutionID, string solution)
        {
            FormContent form = new FormContent();
            form.Solution = SolutionInstance.GetInstance().Solution;
            form.Solution.SolutionID = solutionID;
            form.Solution.SolutionName = solution;
            form.Text = solution + "*";
            form.TabText = solution + "*";
            form.Init();
            form.Show(dockPanel);
            ViewContentCollection.Add(form);
            this.InitDockForm();
        }

        private void OpenSolution(string fileName)
        {
            if (File.Exists(fileName))
            {
                FormContent form = new FormContent();
                form.FileName = fileName;
                form.Init();
                if (form.Controls.Count == 0)
                {
                    MessageBox.Show("该文件可能是版本过旧，无法识别");
                    return;
                }
                form.Show(dockPanel);
                ViewContentCollection.Add(form);
                this.InitDockForm();
            }
        }

        private void InitDockForm()
        {
            FormProperties formProperties = new FormProperties();
            formProperties.Show(dockPanel, DockState.DockRight);
            PadContentCollection.Add(formProperties);
            FormsDesignerViewContent formsDVC = WorkbenchSingleton.Workbench.ActiveContent as FormsDesignerViewContent;
            if (formsDVC != null)
            {
                ToolboxService toolboxService = formsDVC.DesignSurface.GetService(typeof(IToolboxService)) as ToolboxService;
                FormToolBox formTool = new FormToolBox(toolboxService);
                formTool.Show(dockPanel, DockState.DockLeft);
                formTool.DockPanel.DockLeftPortion = 0.15;
                formTool.AutoHidePortion = 0.15;
                PadContentCollection.Add(formTool);
            }
            dockPanel.Documents.First().DockHandler.Activate();
        }

        private bool CheckSave()
        {
            if (ActiveViewContent != null)
            {
                FormContent formContent = ActiveViewContent as FormContent;

                if (ActiveViewContent.TabPageText.IndexOf("*") >= 0)
                {
                    switch (MessageBox.Show("是否保存对该查询方案的修改？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            //formContent.Save();
                            menuSave_Click(menuSave, EventArgs.Empty);
                            formContent.Close();
                            _workbenchContentCollection.Remove(formContent);
                            UnloadPad(GetPad(typeof(FormToolBox)));
                            UnloadPad(GetPad(typeof(PropertyPad)));
                            break;
                        case DialogResult.No:
                            formContent.Close();
                            _workbenchContentCollection.Remove(formContent);
                            UnloadPad(GetPad(typeof(FormToolBox)));
                            UnloadPad(GetPad(typeof(PropertyPad)));
                            break;
                        case DialogResult.Cancel:
                            return true;
                    }
                }
                else
                {
                    formContent.Close();
                    Size = new Size(Size.Width, Size.Height + 1);
                    Size = new Size(Size.Width, Size.Height - 1);
                    _workbenchContentCollection.Remove(formContent);
                    UnloadPad(GetPad(typeof(FormToolBox)));
                    UnloadPad(GetPad(typeof(PropertyPad)));
                    formContent.Dispose();
                }
            }

            return false;
        }

        private string GetCoordinate(int num)
        {
            string temp = num.ToString();
            int tempNum = 4 - temp.Length;
            for (int i = 0; i < tempNum; i++)
            {
                temp += " ";
            }

            return temp;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            //if (!Compare.CompareInitialize())
            //{
            //    MessageBox.Show("该程序没有注册,请注册后再使用");
            //    Dispose();
            //}

            //Size = new Size(Screen.GetWorkingArea(this).Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
            Location = new Point(0, 0);

            FileRegister.FileInfo file = new FileRegister.FileInfo(".qs");
            file.Description = "查询方案";
            file.ExePath = Application.StartupPath + "\\QueryDesigner.exe";
            file.IcoPath = Application.StartupPath + "\\file.ico";

            Register register = new Register();

            if (register.FileRegistered(".qs"))
            {
                register.UpdateFileReg(file);
            }
            else
            {
                register.RegisterFile(file);
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(OpenPath))
            {
                OpenSolution(OpenPath);
            }
        }

        private void dockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            statuServer.Text = "X:" + GetCoordinate(e.X) + "  Y:" + GetCoordinate(e.Y);
        }

        private void dockPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.menuOpen.PerformClick();
        }

        #region IWorkbench 成员

        public object ActiveContent
        {
            get
            {
                if (dockPanel.Documents.Count() > 0)
                {
                    return ((FormContent)dockPanel.Documents.First()).ViewContent;
                }

                return null;
            }
        }

        public IWorkbenchWindow ActiveWorkbenchWindow
        {
            get
            {
                if (_layout == null)
                {
                    return null;
                }

                return _layout.ActiveWorkbenchwindow;
            }
        }

        public event EventHandler ActiveWorkbenchWindowChanged;

        public void CloseAllViews()
        {
            try
            {
                _closeAll = true;
                List<IViewContent> fullList = new List<IViewContent>(_workbenchContentCollection);
                foreach (IViewContent content in fullList)
                {
                    IWorkbenchWindow window = content.WorkbenchWindow;
                    window.CloseWindow(false);
                }
            }
            finally
            {
                _closeAll = false;
                if (!_closeAll && ActiveWorkbenchWindowChanged != null)
                {
                    ActiveWorkbenchWindowChanged(this, EventArgs.Empty);
                }
            }
        }

        public void CloseContent(IViewContent content)
        {
            if (ViewContentCollection.Contains(content))
            {
                ViewContentCollection.Remove(content);
            }

            OnViewClosed(new ViewContentEventArgs(content));

            content.Dispose();
            content = null;
        }

        public FormsDesigner.IPadContent GetPad(Type type)
        {
            foreach (IPadContent pad in PadContentCollection)
            {
                if (pad.GetType() == type)
                {
                    return pad;
                }
            }

            return null;
        }

        public bool IsActiveWindow
        {
            get
            {
                if (dockPanel.Documents.Count() != 0 && dockPanel.Documents.First().DockHandler.IsActivated)
                {
                    return true;
                }

                return false;
            }
        }

        public List<FormsDesigner.IPadContent> PadContentCollection
        {
            get
            {
                System.Diagnostics.Debug.Assert(_viewContentCollection != null);
                return _viewContentCollection;
            }
        }

        public void RedrawAllComponents()
        {
            foreach (IViewContent content in _workbenchContentCollection)
            {
                content.RedrawContent();
                if (content.WorkbenchWindow != null)
                {
                    content.WorkbenchWindow.RedrawContent();
                }
            }

            foreach (IPadContent content in _viewContentCollection)
            {
                content.RedrawContent();
            }

            if (_layout != null)
            {
                _layout.RedrawAllComponents();
            }
        }

        public void ShowPad(FormsDesigner.IPadContent content)
        {
            PadContentCollection.Add(content);

            if (_layout != null)
            {
                _layout.ShowPad(content);
            }
        }

        public void ShowView(IViewContent content)
        {
            System.Diagnostics.Debug.Assert(_layout != null);
            ViewContentCollection.Add(content);

            _layout.ShowView(content);
            content.WorkbenchWindow.SelectWindow();
            OnViewOpened(new ViewContentEventArgs(content));
        }

        public string Title
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
            }
        }

        public void UnloadPad(FormsDesigner.IPadContent content)
        {
            if (content == null)
                return;
            DockContent dockContent = content.Control as DockContent;
            if (dockContent == null)
            {
                dockContent = content.Control.Parent as DockContent;
            }

            dockContent.Close();

            PadContentCollection.Remove(content);

            if (_layout != null)
            {
                _layout.UnloadPad(content);
            }

            content.Dispose();
        }

        public List<IViewContent> ViewContentCollection
        {
            get
            {
                System.Diagnostics.Debug.Assert(_workbenchContentCollection != null);
                return _workbenchContentCollection;
            }
        }

        #endregion

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CheckSave(); SqlCommand a = new SqlCommand();
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            if (!CheckSave())
            {
                FormNewProject formNewProject = new FormNewProject();
                if (formNewProject.ShowDialog(this) == DialogResult.OK)
                {
                    InitMenuToolBar(false);
                    Solution solution = new Solution()
                    {
                        SolutionID = formNewProject.ProjectID,
                        SolutionName = formNewProject.ProjectDepict,
                        SolutionComment = formNewProject.ProjectMemo,
                        SolutionConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[QueryDAO.DataType].ConnectionString,
                        SolutionDataBaseType = QueryDAO.DataType
                    };

                    SolutionInstance.GetInstance().Solution = solution;
                    NewSolution(formNewProject.ProjectID, formNewProject.ProjectDepict);
                    InitMenuToolBar(true);
                    OpenPath = string.Empty;
                }
            }
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            if (!CheckSave())
            {
                openFile.InitialDirectory = Application.StartupPath + @"\Query\";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    OpenSolution(openFile.FileName);
                    OpenPath = openFile.FileName;
                }
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                if (string.IsNullOrEmpty(OpenPath))
                {
                    string filePath = Application.StartupPath + @"\Query\";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    saveFile.FileName = ActiveViewContent.TabPageText.Replace("*", string.Empty) + ".qs";

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        OpenPath = saveFile.FileName;
                        (ActiveViewContent as FormContent).Save(OpenPath);
                    }
                }
                else
                {
                    (ActiveViewContent as FormContent).Save(OpenPath);
                }
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                saveFile.FileName = ActiveViewContent.TabPageText.Replace("*", string.Empty) + ".qs";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    OpenPath = saveFile.FileName;
                    string[] filename = OpenPath.Split('\\');
                    (ActiveViewContent as FormContent).SaveAs(OpenPath, filename[filename.Length - 1].Substring(0, filename[filename.Length - 1].IndexOf('.')));
                }
            }
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            CheckSave();
        }

        private void menuUndo_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                (ActiveContent as FormsDesignerViewContent).Undo();
            }
        }

        private void menuRedo_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                (ActiveContent as FormsDesignerViewContent).Redo();
            }
        }

        private void menuCut_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                (ActiveContent as FormsDesignerViewContent).Cut();
            }
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                (ActiveContent as FormsDesignerViewContent).Copy();
            }
        }

        private void menuPaste_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                (ActiveContent as FormsDesignerViewContent).Paste();
            }
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                (ActiveContent as FormsDesignerViewContent).Delete();
            }
        }

        private void menuToolBox_Click(object sender, EventArgs e)
        {
            IPadContent padContent = GetPad(typeof(FormToolBox));
            if (padContent != null)
            {
                ((DockContent)padContent).DockHandler.Activate();
            }
        }

        private void muneProperty_Click(object sender, EventArgs e)
        {
            IPadContent padContent = GetPad(typeof(FormProperties));
            if (padContent != null)
            {
                ((DockContent)padContent).DockHandler.Activate();
            }
        }

        private void menuAlignLeft_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).AlignLeft();
            }
        }

        private void menuAlignCenter_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).AlignHorizontalCenters();
            }
        }

        private void menuAlignRinght_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).AlignRight();
            }
        }

        private void menuAlginTop_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).AlignTop();
            }
        }

        private void menuAlginMiddle_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).AlignVerticalCenters();
            }
        }

        private void menuAlginBottom_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).AlignBottom();
            }
        }

        private void menuBringFront_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).BringToFront();
            }
        }

        private void menuBringback_Click(object sender, EventArgs e)
        {
            if (ActiveContent != null)
            {
                ((FormsDesignerViewContent)ActiveContent).SendToBack();
            }
        }

        private void menuOverall_Click(object sender, EventArgs e)
        {

        }

        private void menuDataSource_Click(object sender, EventArgs e)
        {
            if (_viewContentCollection.Count > 0)
            {
                FormQueryData form = new FormQueryData();
                form.Solution = SolutionInstance.GetInstance().Solution;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (ActiveViewContent != null)
                    {
                        FormContent fc = (FormContent)ActiveViewContent;
                        fc.Text = SolutionInstance.GetInstance().Solution.SolutionName + "*";
                        fc.IsDirty = true;
                        if (SolutionInstance.GetInstance().Solution.DataSetList.Count == 0)
                            return;

                        SnDataSet ds = SolutionInstance.GetInstance().Solution.DataSetList[0] as SnDataSet;
                        foreach (IComponent ctrl in fc.ViewContent.Host.Container.Components)
                        {
                            if (ctrl is ParamTextBox || ctrl is ParamComboBox || ctrl is ParamDateTimePicker
                                || ctrl is ParamRadioButton || ctrl is Search)
                                (ctrl as ICommonAttribute).DataSetName = ds.DataSetID + "-" + ds.DataSetName;
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("请先定义查询方案！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuUserFunction_Click(object sender, EventArgs e)
        {
            FormAllFunction form = new FormAllFunction();
            form.ShowDialog();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            FormAbout form = new FormAbout();
            form.ShowDialog();
        }

        private void toolRun_Click(object sender, EventArgs e)
        {
            if (ActiveViewContent != null)
            {
                FormContent form = ActiveViewContent as FormContent;

                if (ActiveViewContent.TabPageText.IndexOf("*") >= 0)
                {
                    switch (MessageBox.Show("是否保存对该查询方案的修改？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            //form.Save();
                            menuSave_Click(menuSave, EventArgs.Empty);
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }

                string filePath = string.IsNullOrEmpty(OpenPath) ? Application.StartupPath + "\\Query\\" + SolutionInstance.GetInstance().Solution.SolutionName + ".qs" : OpenPath;

                if (File.Exists(filePath))
                {
                    FormQueryResult formQueryResult = new FormQueryResult();
                    formQueryResult.FileName = filePath;
                    formQueryResult.ShowDialog();
                }
            }
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            RegisterHotKey(Handle, 100, KeyModifiers.Ctrl, Keys.Z);
            RegisterHotKey(Handle, 101, KeyModifiers.Ctrl | KeyModifiers.Shift, Keys.Z);
            RegisterHotKey(Handle, 102, KeyModifiers.Ctrl, Keys.C);
            RegisterHotKey(Handle, 103, KeyModifiers.Ctrl, Keys.X);
            RegisterHotKey(Handle, 104, KeyModifiers.Ctrl, Keys.V);
            RegisterHotKey(Handle, 105, KeyModifiers.Ctrl, Keys.N);
            RegisterHotKey(Handle, 106, KeyModifiers.Ctrl, Keys.F);
            RegisterHotKey(Handle, 107, KeyModifiers.Ctrl, Keys.S);
            RegisterHotKey(Handle, 108, KeyModifiers.None, Keys.F5);
            RegisterHotKey(Handle, 109, KeyModifiers.None, Keys.F1);
            RegisterHotKey(Handle, 110, KeyModifiers.None, Keys.Delete);
            RegisterHotKey(Handle, 111, KeyModifiers.Ctrl, Keys.G);
        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            UnregisterHotKey(Handle, 100);
            UnregisterHotKey(Handle, 101);
            UnregisterHotKey(Handle, 102);
            UnregisterHotKey(Handle, 103);
            UnregisterHotKey(Handle, 104);
            UnregisterHotKey(Handle, 105);
            UnregisterHotKey(Handle, 106);
            UnregisterHotKey(Handle, 107);
            UnregisterHotKey(Handle, 108);
            UnregisterHotKey(Handle, 109);
            UnregisterHotKey(Handle, 110);
            UnregisterHotKey(Handle, 111);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                switch (m.WParam.ToInt32())
                {
                    case 100:
                        menuUndo_Click(menuUndo, EventArgs.Empty);
                        break;
                    case 101:
                        menuRedo_Click(menuRedo, EventArgs.Empty);
                        break;
                    case 102:
                        menuCopy_Click(menuCopy, EventArgs.Empty);
                        break;
                    case 103:
                        menuCut_Click(menuCut, EventArgs.Empty);
                        break;
                    case 104:
                        menuPaste_Click(menuPaste, EventArgs.Empty);
                        break;
                    case 105:
                        menuNew_Click(menuNew, EventArgs.Empty);
                        break;
                    case 106:
                        menuOpen_Click(menuOpen, EventArgs.Empty);
                        break;
                    case 107:
                        menuSave_Click(menuSave, EventArgs.Empty);
                        break;
                    case 108:
                        toolRun_Click(toolRun, EventArgs.Empty);
                        break;
                    case 109:
                        menuAbout_Click(menuAbout, EventArgs.Empty);
                        break;
                    case 110:
                        menuDelete_Click(menuDelete, EventArgs.Empty);
                        break;
                    case 111:
                        menuDataSource_Click(menuDataSource, EventArgs.Empty);
                        break;
                    default:
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }
}