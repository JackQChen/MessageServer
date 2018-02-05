using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;

namespace SystemFramework.BaseControl
{
    [ToolboxItem(false)]
    public partial class WorkPlat : UserControl
    {
        private Hashtable htType = new Hashtable(), htPage = new Hashtable();
        private List<string> privList = new List<string>();

        public WorkPlat()
        {
            InitializeComponent();
        }

        public void AddNewPage(string ReflectionPath, string Priv)
        {
            AddNewPage(ReflectionPath, Priv, null);
        }

        public void AddNewPage(string ReflectionPath, string Priv, Image img)
        {
            string[] info = ReflectionPath.Split(',');
            Type ModuleType = null;
            if (File.Exists(Application.StartupPath + "\\" + info[1] + ".dll"))
                ModuleType = Assembly.LoadFrom(Application.StartupPath + "\\" + info[1] + ".dll").GetType(info[0]);
            if (ModuleType == null)
            {
                MessageBoxEx.Show("菜单加载路径不正确，请联系管理员", "提示", MessageBoxIcon.Error);
                LogService.ErrorMessage(string.Format("请检查文件[{0}.dll]是否存在以及类型[{1}]是否正常", info[1], info[0]));
                return;
            }
            if (!htType.Contains(ModuleType))
            {
                XtraTabPage page = new XtraTabPage();
                page.Image = img;
                PageControl pc = null;
                try
                {
                    pc = Activator.CreateInstance(ModuleType) as PageControl;
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show((ex.InnerException == null ? ex : ex.InnerException).Message, "提示", MessageBoxIcon.Error);
                    LogService.ErrorMessage(
                        ex.Message + "\r\n" + ex.StackTrace +
                        (ex.InnerException == null ? "" : (ex.InnerException.Message
                        + "\r\n" + ex.InnerException.StackTrace)));
                    return;
                }
                object[] o = ModuleType.GetCustomAttributes(typeof(PageTextAttribute), true);
                page.Text = o.Length == 0 ? "新建操作" : (o[0] as PageTextAttribute).PageText;
                privList.Clear();
                foreach (string str in Priv.Split('|'))
                {
                    if (string.IsNullOrEmpty(str.Trim()))
                        continue;
                    privList.Add(str);
                }
                pc.UpdatePriv(privList);
                if (pc.DialogMode)
                {
                    pc.Text = page.Text;
                    pc.ShowDialog(this);
                }
                else
                {
                    pc.PageType = pc.GetType();
                    pc.OwnPlat = this;
                    pc.TopLevel = false;
                    pc.Visible = true;
                    pc.WindowState = FormWindowState.Normal;
                    pc.FormBorderStyle = FormBorderStyle.None;
                    pc.Dock = DockStyle.Fill;
                    page.Controls.Add(pc);
                    htType.Add(ModuleType, page);
                    htPage.Add(page, ModuleType);
                    tcMain.TabPages.Add(page);
                    try
                    {
                        tcMain.SelectedTabPage = page;
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.Show((ex.InnerException == null ? ex : ex.InnerException).Message, "提示", MessageBoxIcon.Error);
                        LogService.ErrorMessage((ex.InnerException == null ? ex : ex.InnerException).Message);
                        return;
                    }
                }
            }
            else
                tcMain.SelectedTabPage = htType[ModuleType] as XtraTabPage;
        }


        public void RemovePage(Type ModuleType)
        {
            if (ModuleType != null)
            {
                if (htType.Contains(ModuleType))
                {
                    XtraTabPage page = htType[ModuleType] as XtraTabPage;
                    tcMain.TabPages.Remove(page);
                    htType.Remove(ModuleType);
                    htPage.Remove(page);
                }
                if (tcMain.TabPages.Count > 0)
                    tcMain.SelectedTabPageIndex = tcMain.TabPages.Count - 1;
            }
        }

        private void 关闭当前选项卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemovePage(htPage[this.tcMain.SelectedTabPage] as Type);
        }

        private void 除此之外ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type tp = htPage[this.tcMain.SelectedTabPage] as Type;
            List<Type> list = new List<Type>();
            foreach (XtraTabPage pg in this.tcMain.TabPages)
                if (!pg.Equals(this.tcMain.SelectedTabPage))
                    list.Add(htPage[pg] as Type);
            foreach (Type rtp in list)
            {
                RemovePage(rtp);
            }
        }

        private void 设置布局RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLayout frm = new FrmLayout();
            frm.LayoutList = (this.tcMain.SelectedTabPage.Controls[0] as PageControl).LayoutList;
            frm.ControlType = htPage[this.tcMain.SelectedTabPage];
            frm.ShowDialog(this);
        }

        private void 全部关闭AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Type> list = new List<Type>();
            foreach (DictionaryEntry de in htType)
            {
                list.Add(de.Key as Type);
            }
            foreach (Type tp in list)
            {
                RemovePage(tp);
            }
        }
        private Point cmsPoint = new Point();

        private void cms_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.tcMain.TabPages.Count == 0)
                e.Cancel = true;
            else
            {
                cmsPoint = this.PointToClient(MousePosition);
                if (cmsPoint.Y - this.tcMain.Top > this.tcMain.SelectedTabPage.Top)
                    e.Cancel = true;
            }
        }

        private void tcMain_CloseButtonClick(object sender, EventArgs e)
        {
            RemovePage(htPage[this.tcMain.SelectedTabPage] as Type);
        }
    }
}
