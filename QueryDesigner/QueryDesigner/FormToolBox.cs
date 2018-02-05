using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using FormsDesigner;
using FormsDesigner.Services;
using WinFormsUI.Docking;
using SnControl;
using System.ComponentModel;
using QueryDesigner.Properties;

namespace QueryDesigner
{
    public partial class FormToolBox : DockContent, IPadContent
    {
        public FormToolBox(ToolboxService toolboxService)
        {
            InitializeComponent();

            Icon = global::QueryDesigner.Properties.Resources.Tool;
            ToolboxList = new Hashtable();
            _toolboxService = toolboxService;
            CloseButtonVisible = false;
            ReloadSideTabs();
            _toolboxService.SelectedItemUsed += new EventHandler(_toolboxService_SelectedItemUsed);

        }

        public Hashtable ToolboxList { get; set; }
        private FormsDesigner.Services.ToolboxService _toolboxService;
        public static ArrayList _sideTabs = new ArrayList();
        public event EventHandler TitleChanged;

        public FormsDesigner.Services.ToolboxService ToolboxService
        {
            get { return _toolboxService; }
        }

        public NavBarControl ToolBoxTabList
        {
            get { return toolboxTabList; }
        }

        public NavBarGroup ToolBoxTab
        {
            get { return toolCommon; }
        }

        public string Title
        {
            get { return Text; }
        }

        public void OnTitleChanged(EventArgs e)
        {
            if (TitleChanged != null)
            {
                TitleChanged(this, e);
            }
        }

        public void ReloadSideTabs()
        {
            toolboxTabList.Items.Clear();
            if (!_sideTabs.Contains(toolCommon))
            {
                _sideTabs.Add(toolCommon);
            }

            PopulateToolBox(ToolboxService);
            toolCommon.SelectedLink = toolCommon.ItemLinks[0];
        }

        private void PopulateToolBox(IToolboxService toolbox)
        {
            ToolboxList.Clear();
            NavBarItem item;
            item = toolboxTabList.Items.Add();
            item.Caption = "指针";
            item.SmallImage = global::QueryDesigner.Properties.Resources._5;
            toolCommon.ItemLinks.Add(item);

            ToolboxItem toolboxItem = new ToolboxItem(typeof(Label));
            item = toolboxTabList.Items.Add();
            item.Caption = "Label";
            ToolboxBitmapAttribute toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.Label));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.Label));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(SnControl.ParamTextBox));
            item = toolboxTabList.Items.Add();
            item.Caption = "TextBox";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.TextBox));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.TextBox));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(SnControl.ParamComboBox));
            item = toolboxTabList.Items.Add();
            item.Caption = "ComboBox";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.ComboBox));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.ComboBox));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(SnControl.ParamDateTimePicker));
            item = toolboxTabList.Items.Add();
            item.Caption = "DateTimePicker";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.DateTimePicker));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.DateTimePicker));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(TabControl));
            item = toolboxTabList.Items.Add();
            item.Caption = "TabControl";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.TabControl));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.TabControl));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(GroupBox));
            item = toolboxTabList.Items.Add();
            item.Caption = "GroupBox";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.GroupBox));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.GroupBox));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(Panel));
            item = toolboxTabList.Items.Add();
            item.Caption = "Panel";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.Panel));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.Panel));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(System.Windows.Forms.DataGridView));
            item = toolboxTabList.Items.Add();
            item.Caption = "DataGridView";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.DataGridView));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.DataGridView));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(TitlePanel));
            item = toolboxTabList.Items.Add();
            item.Caption = "TitlePanel";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.GroupBox));
            item.SmallImage = toolboxBitmap.GetImage(typeof(System.Windows.Forms.GroupBox));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(SnControl.Search));
            item = toolboxTabList.Items.Add();
            item.Caption = "Search";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(System.Windows.Forms.ComboBox));
            item.SmallImage = Resources.Search;
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);

            toolboxItem = new ToolboxItem(typeof(SnControl.ParamRadioButton));
            item = toolboxTabList.Items.Add();
            item.Caption = "RadioButton";
            toolboxBitmap = new ToolboxBitmapAttribute(typeof(RadioButton));
            item.SmallImage = toolboxBitmap.GetImage(typeof(RadioButton));
            toolCommon.ItemLinks.Add(item);
            toolbox.AddToolboxItem(toolboxItem);
            ToolboxList.Add(item, toolboxItem);
        }

        private void _toolboxService_SelectedItemUsed(object sender, EventArgs e)
        {
            NavBarGroup tab = toolboxTabList.ActiveGroup;
            if (tab.ItemLinks.Count > 0)
            {
                tab.SelectedLink = tab.ItemLinks[0];
            }
            toolboxTabList.Refresh();
        }

        #region IPadContent 成员

        public Control Control
        {
            get { return this; }
        }

        public void RedrawContent()
        {

        }

        #endregion

        private void toolboxTabList_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
            if (e.Link != null)
            {
                _toolboxService.SetSelectedToolboxItem((ToolboxItem)ToolboxList[e.Link.Item]);
            }
        }
    }

}
