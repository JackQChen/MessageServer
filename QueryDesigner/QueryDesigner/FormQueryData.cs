using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QueryDataAccess;
using SnControl;
using System.Linq;

namespace QueryDesigner
{
    public partial class FormQueryData : Form
    {
        public FormQueryData()
        {
            InitializeComponent();
            _queryDao = new QueryDAO();
        }

        public Solution Solution
        {
            get;
            set;
        }

        public Hashtable CList
        {
            get;
            set;
        }

        private SnDataSet _currDataSet;

        private QueryDAO _queryDao;

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

        private void FormQueryData_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            this.WindowState = FormWindowState.Maximized;
            ShowDataList();
        }

        private void ShowDataList()
        {
            IEnumerator enumerator = this.Solution.DataSetList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SnDataSet temp = enumerator.Current as SnDataSet;
                ListViewItem item = dataList.Items.Add(temp.DataSetID);
                item.Tag = temp;
                item.SubItems.Add(temp.DataSetName);
                item.SubItems.Add(temp.DataSetType.ToString());
            }
            if (this.Solution.DataSetList.Count > 0)
                dataList.TopItem.Selected = true;
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            this.txtSQL_Leave(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            FormDataSource form = new FormDataSource();
            form.UseType = 0;
            form.DataSetID = "dataSet" + (dataList.Items.Count + 1);
            form.DataSetChineseName = "数据集" + (dataList.Items.Count + 1);

            if (form.ShowDialog() == DialogResult.OK)
            {
                bool hasPage = false;

                foreach (ListViewItem item in dataList.Items)
                {
                    if (item.SubItems[2].Text == "Page")
                    {
                        hasPage = false;
                        break;
                    }
                }

                if (hasPage && ((DataSetType)form.UseType).ToString() == "Page")
                {
                    MessageBox.Show("每一个数据集合只能有一个用查询的Page数据集！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SnDataSet dataset = new SnDataSet();
                dataset.DataSetID = form.DataSetID;
                dataset.DataSetName = form.DataSetChineseName;
                dataset.DataSetType = (DataSetType)form.UseType;
                dataset.ReportPath = form.ReportPath;
                dataset.DataType = "自定义";
                _currDataSet = dataset;
                this.Solution.DataSetList.Add(dataset);
                ListViewItem listViewItem = dataList.Items.Add(dataset.DataSetID);
                listViewItem.Tag = dataset;
                listViewItem.SubItems.Add(dataset.DataSetName);
                listViewItem.SubItems.Add(dataset.DataSetType.ToString());
                listViewItem.Selected = true;
            }
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            if (dataList.SelectedItems.Count == 1)
            {
                for (int i = 0; i < this.Solution.DataSetList.Count; i++)
                {
                    SnDataSet temp = (SnDataSet)this.Solution.DataSetList[i];

                    if (temp.DataSetID == dataList.SelectedItems[0].SubItems[0].Text)
                    {
                        this.Solution.DataSetList.Remove(dataList.SelectedItems[0].Tag);
                        dataList.Items.Remove(dataList.SelectedItems[0]);
                    }
                }
            }
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            if (dataList.Items.Count > 0)
            {
                dataList.Items.Clear();
                this.Solution.DataSetList.Clear();
            }
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataList.SelectedItems.Count != 1)
                return;
            _currDataSet = (SnDataSet)(this.Solution.DataSetList[dataList.SelectedItems[0].Index]);
            this.RefreshSQL();
            this.RefreshParams();
            this.RefreshColumns();
            this.tab.SelectedIndex = 0;
        }

        void RefreshSQL()
        {
            this.txtSQL.Text = _currDataSet.SQLExpression;
            this.txtSQL.SelectionStart = this.txtSQL.Text.Length;
        }

        void RefreshParams()
        {
            if (this._currDataSet == null)
                return;
            this.dgvParam.DataSource = this._currDataSet.ParamList.Cast<SQLParamItem>().ToList();
            this.dgvParam.Refresh();
        }

        void RefreshColumns()
        {
            if (this._currDataSet == null)
                return;
            this.fieldList.Items.Clear();
            foreach (FieldItem fieldItem in _currDataSet.FieldsList)
            {
                ListViewItem listViewItem = fieldList.Items.Add(fieldItem.FieldName);
                listViewItem.SubItems.Add(fieldItem.FieldChineseName);
                listViewItem.SubItems.Add(fieldItem.FieldType);
                listViewItem.SubItems.Add(fieldItem.CalcType);
                listViewItem.SubItems.Add(fieldItem.Converge);
                listViewItem.SubItems.Add(fieldItem.DisplayWidth.ToString());
                listViewItem.SubItems.Add(fieldItem.DecimalDigits.ToString());
                listViewItem.SubItems.Add(fieldItem.ColumnVisible ? "是" : "否");
                listViewItem.Tag = fieldItem;
            }
        }

        private void menuTable_Click(object sender, EventArgs e)
        {
            if (dataList.Items.Count == 0)
            {
                MessageBox.Show("请先定义数据集！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataList.SelectedItems.Count == 1)
            {
                FormSelectTable form = new FormSelectTable();
                form.dataName = _currDataSet.DataSetName;
            }
        }

        private void menuQueryData_Click(object sender, EventArgs e)
        {
            this.CheckParam();
            if (string.IsNullOrEmpty(this.txtSQL.Text))
            {
                MessageBox.Show("请先输入查询内容", "提示");
                return;
            }
            if (_currDataSet == null)
            {
                MessageBox.Show("请先增加或选择数据集合", "提示");
                return;
            }
            if (this.tab.SelectedIndex == 3)
                this.dgvParam.CurrentCell = this.dgvParam.Rows[0].Cells[0];
            try
            {
                if (!string.IsNullOrEmpty(txtSQL.Text))
                {
                    DataTable dt = _queryDao.ExecSQL(txtSQL.Text, GetParams());
                    grdResult.DataSource = dt.DefaultView;
                    tab.SelectedTab = tab.TabPages[3];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fieldList_DoubleClick(object sender, EventArgs e)
        {
            if (fieldList.SelectedItems.Count == 1)
            {
                FormEditField form = new FormEditField();
                form.FieldName = fieldList.SelectedItems[0].Text;
                form.FieldChineseName = fieldList.SelectedItems[0].SubItems[1].Text;
                form.FieldType = fieldList.SelectedItems[0].SubItems[2].Text;
                form.CalcType = fieldList.SelectedItems[0].SubItems[3].Text;
                form.Converge = fieldList.SelectedItems[0].SubItems[4].Text;
                form.DisplayWidth = fieldList.SelectedItems[0].SubItems[5].Text;
                form.DecimalDigits = string.IsNullOrEmpty(fieldList.SelectedItems[0].SubItems[6].Text) ? 2 : Convert.ToInt32(fieldList.SelectedItems[0].SubItems[6].Text);
                form.StyleFormat = _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].StyleFormat;
                form.FieldVisible = fieldList.SelectedItems[0].SubItems[7].Text == "是";

                if (form.ShowDialog() == DialogResult.OK)
                {
                    fieldList.SelectedItems[0].SubItems[1].Text = form.FieldChineseName;
                    fieldList.SelectedItems[0].SubItems[2].Text = form.FieldType;
                    fieldList.SelectedItems[0].SubItems[3].Text = form.CalcType;
                    fieldList.SelectedItems[0].SubItems[4].Text = form.Converge;
                    fieldList.SelectedItems[0].SubItems[5].Text = form.DisplayWidth;
                    fieldList.SelectedItems[0].SubItems[6].Text = form.DecimalDigits.ToString();
                    fieldList.SelectedItems[0].SubItems[7].Text = form.FieldVisible ? "是" : "否";
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].CalcType = form.CalcType;
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].Converge = form.Converge;
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].FieldType = form.FieldType;
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].FieldChineseName = form.FieldChineseName;
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].StyleFormat = form.StyleFormat;
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].DisplayWidth = Convert.ToInt32(form.DisplayWidth);
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].DecimalDigits = Convert.ToInt32(form.DecimalDigits);
                    _currDataSet.FieldsList[fieldList.SelectedItems[0].Text].ColumnVisible = form.FieldVisible;
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (fieldList.SelectedItems != null)
            {
                int index = fieldList.SelectedItems[0].Index;

                if (index > 0)
                {
                    ListViewItem lstItem = (ListViewItem)fieldList.SelectedItems[0].Clone();
                    FieldItem fieldItem = new FieldItem();
                    fieldItem = (FieldItem)fieldList.SelectedItems[0].Tag;
                    _currDataSet.FieldsList.Remove((FieldItem)fieldList.SelectedItems[0].Tag);
                    _currDataSet.FieldsList.Insert(index - 1, fieldItem);
                    fieldList.Items.Remove(fieldList.SelectedItems[0]);
                    fieldList.Items.Insert(index - 1, lstItem);
                    lstItem.Selected = true;
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (fieldList.SelectedItems != null)
            {
                int index = fieldList.SelectedItems[0].Index;

                if (index < fieldList.Items.Count - 1)
                {
                    ListViewItem lstItem = (ListViewItem)fieldList.SelectedItems[0].Clone();
                    FieldItem fieldItem = new FieldItem();
                    fieldItem = (FieldItem)fieldList.SelectedItems[0].Tag;
                    _currDataSet.FieldsList.Remove((FieldItem)fieldList.SelectedItems[0].Tag);
                    _currDataSet.FieldsList.Insert(index + 1, fieldItem);
                    fieldList.Items.Remove(fieldList.SelectedItems[0]);
                    fieldList.Items.Insert(index + 1, lstItem);
                    lstItem.Selected = true;
                }
            }
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            FormAbout form = new FormAbout();
            form.ShowDialog();
        }

        private void FormQueryData_Activated(object sender, EventArgs e)
        {
            RegisterHotKey(Handle, 100, KeyModifiers.Ctrl, Keys.N);
            RegisterHotKey(Handle, 101, KeyModifiers.Ctrl, Keys.S);
            RegisterHotKey(Handle, 102, KeyModifiers.Ctrl, Keys.T);
            RegisterHotKey(Handle, 103, KeyModifiers.Ctrl, Keys.R);
            RegisterHotKey(Handle, 104, KeyModifiers.None, Keys.Escape);
            RegisterHotKey(Handle, 105, KeyModifiers.None, Keys.F1);
        }

        private void FormQueryData_Deactivate(object sender, EventArgs e)
        {
            UnregisterHotKey(Handle, 100);
            UnregisterHotKey(Handle, 101);
            UnregisterHotKey(Handle, 102);
            UnregisterHotKey(Handle, 103);
            UnregisterHotKey(Handle, 104);
            UnregisterHotKey(Handle, 105);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                switch (m.WParam.ToInt32())
                {
                    case 100:
                        menuNew_Click(menuNew, EventArgs.Empty);
                        break;
                    case 101:
                        menuSave_Click(menuSave, EventArgs.Empty);
                        break;
                    case 102:
                        menuTable_Click(menuTable, EventArgs.Empty);
                        break;
                    case 103:
                        menuQueryData_Click(menuChangeParameter, EventArgs.Empty);
                        break;
                    case 104:
                        menuClose_Click(menuClose, EventArgs.Empty);
                        break;
                    case 105:
                        menuAbout_Click(menuAbout, EventArgs.Empty);
                        break;
                    default:
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void dataList_DoubleClick(object sender, EventArgs e)
        {
            if (this.dataList.SelectedItems != null && dataList.SelectedItems.Count > 0)
            {
                SnDataSet dataset = (SnDataSet)dataList.SelectedItems[0].Tag;
                FormDataSource form = new FormDataSource();
                form.UseType = dataset.DataSetType.GetHashCode();
                form.DataSetID = dataset.DataSetID;
                form.DataSetChineseName = dataset.DataSetName;
                form.ReportPath = dataset.ReportPath;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    bool hasPage = false;

                    foreach (ListViewItem item in dataList.Items)
                    {
                        if (item.SubItems[2].Text == "Page")
                        {
                            hasPage = false;
                            break;
                        }
                    }
                    if (hasPage && ((DataSetType)form.UseType).ToString() == "Page")
                    {
                        MessageBox.Show("每一个数据集合只能有一个用查询的Page数据集！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataset.DataSetID = form.DataSetID;
                    dataset.DataSetName = form.DataSetChineseName;
                    dataset.DataSetType = (DataSetType)form.UseType;
                    dataset.ReportPath = form.ReportPath;
                }
            }
        }

        private Hashtable GetParams()
        {
            if (this.dgvParam.DataSource == null)
                return null;
            Hashtable ht = new Hashtable();
            foreach (DataGridViewRow dr in this.dgvParam.Rows)
            {
                string pName = dr.Cells["clmParamName"].Value.ToString(), pTest = "";
                if (dr.Cells["clmParamTest"].Value == null)
                    pTest = "";
                else
                    pTest = dr.Cells["clmParamTest"].Value.ToString();
                switch (dr.Cells["clmParamType"].Value.ToString())
                {
                    case "String":
                        ht.Add(pName, pTest);
                        break;
                    case "Int":
                        int tVal = 0;
                        try
                        {
                            tVal = Convert.ToInt32(pTest);
                        }
                        catch
                        {
                            dr.Cells["clmParamTest"].Value = 0;
                        }
                        ht.Add(pName, tVal);
                        break;
                    case "Decimal":
                        decimal tVal2 = 0;
                        try
                        {
                            tVal2 = Convert.ToInt32(pTest);
                        }
                        catch
                        {
                            dr.Cells["clmParamTest"].Value = 0.00m;
                        }
                        ht.Add(pName, tVal2);
                        break;
                    case "DateTime":
                        DateTime tVal3 = DateTime.Now;
                        try
                        {
                            tVal3 = Convert.ToDateTime(pTest);
                        }
                        catch
                        {
                            dr.Cells["clmParamTest"].Value = tVal3;
                        }
                        ht.Add(pName, tVal3);
                        break;
                }
            }
            return ht;
        }

        private void btnGetColumns_Click(object sender, EventArgs e)
        {
            this.CheckParam();
            if (string.IsNullOrEmpty(this.txtSQL.Text))
            {
                MessageBox.Show("请先输入查询内容", "提示");
                return;
            }
            if (_currDataSet == null)
            {
                MessageBox.Show("请先增加或选择数据集合", "提示");
                return;
            }
            if (_currDataSet.FieldsList.Count > 0)
            {
                if (MessageBox.Show("是否清空原有列信息？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    _currDataSet.FieldsList.Clear();
                else
                    return;
            }
            DataTable dt = _queryDao.ExecSQL(txtSQL.Text, this.GetParams());
            foreach (DataColumn dc in dt.Columns)
            {
                FieldItem fi = new FieldItem();
                if (dc.DataType == typeof(int))
                    fi.FieldType = "Int";
                else if (dc.DataType == typeof(decimal)
                    || dc.DataType == typeof(double))
                    fi.FieldType = "Decimal";
                else if (dc.DataType == typeof(DateTime))
                    fi.FieldType = "DateTime";
                else
                    fi.FieldType = "String";
                fi.FieldName = dc.Caption;
                fi.FieldChineseName = dc.Caption;
                fi.ColumnVisible = true;
                _currDataSet.FieldsList.Add(fi);
            }
            this.RefreshColumns();
            this.tab.SelectedIndex = 2;
        }

        private void CheckParam()
        {
            if (this.txtSQL.Text.Contains("@") && (this.dgvParam.DataSource as IList).Count == 0)
                this.btnGetParam.PerformClick();
        }

        private void btnGetParam_Click(object sender, EventArgs e)
        {
            if (_currDataSet == null)
            {
                MessageBox.Show("请先增加或选择数据集合", "提示");
                return;
            }
            if (string.IsNullOrEmpty(this.txtSQL.Text))
            {
                MessageBox.Show("请先输入查询内容", "提示");
                return;
            }
            Regex regex = new Regex(@"(?<p>@[a-zA-Z]\w*)", RegexOptions.Multiline);
            var queryText = this.txtSQL.Text.Trim();
            var qParams = (from qNew in
                               from m in regex.Matches(queryText).Cast<Match>()
                               group m by m.Value into g
                               select g.Key
                           join qOld in this._currDataSet.ParamList.Cast<SQLParamItem>()
                           on qNew equals qOld.ParamName into temp
                           from result in temp.DefaultIfEmpty()
                           select new { Name = qNew, Type = result == null ? "String" : result.ParamType }).ToList();
            this._currDataSet.ParamList.Clear();
            foreach (var p in qParams)
                this._currDataSet.ParamList.Add(new SQLParamItem()
                {
                    ParamName = p.Name,
                    ParamType = p.Type
                });
            this.RefreshParams();
            this.tab.SelectedIndex = 1;
        }

        private void btnRemoveSection_Click(object sender, EventArgs e)
        {
            if (_currDataSet != null)
            {
                if (this.fieldList.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in this.fieldList.SelectedItems)
                    {
                        FieldItem fi = item.Tag as FieldItem;
                        this.fieldList.Items.Remove(item);
                        this._currDataSet.FieldsList.Remove(fi);
                    }
                }
            }
        }

        private void txtSQL_Leave(object sender, EventArgs e)
        {
            _currDataSet.SQLExpression = this.txtSQL.Text;
        }

    }
}