using System;
using System.Data;
using System.Windows.Forms;
using SnControl;
using QueryDataAccess;

namespace QueryDesigner
{
    public partial class FormSelectTable : Form
    {
        public FormSelectTable()
        {
            InitializeComponent();
            _dao = new QueryDAO();
        }

        private DataView _dv;
        private QueryDAO _dao;

        public string dataName { get; set; }

        private void LoadListViewData(DataView dv)
        {
            lvTable.Items.Clear();
            foreach (DataRowView row in dv)
            {
                ListViewItem item = lvTable.Items.Add(row["TABLENAME"].ToString());
                item.SubItems.Add(row["CHINESENAME"].ToString());
                item.Tag = row;
            }
        }

        private bool IsExists(string aliasName)
        {
            foreach (ListViewItem item in lsvSelectedTable.Items)
            {
                if (item.SubItems[1].Text == aliasName)
                {
                    return true;
                }
            }

            return false;
        }

        private string CreateAliasName()
        {
            int i = lsvSelectedTable.Items.Count;
            i = i + 1;
            string aliasName = "T" + i.ToString();
            FormSetAliasName form = new FormSetAliasName();
            if (!IsExists(aliasName))
            {
                form.AliasName = aliasName;
            }
            if (form.ShowDialog() == DialogResult.OK)
            {
                return form.AliasName;
            }
            else
            {
                return string.Empty;
            }
        }

        private void FormSelectTable_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            labDataSource.Text = dataName;

            _dv = _dao.GetAllTableList().DefaultView;

            LoadListViewData(_dv);

            lsvSelectedTable.BeginUpdate();

            lsvSelectedTable.Items.Clear();

            lsvSelectedTable.EndUpdate();

            if (lsvSelectedTable.Items.Count > 0)
            {
                lsvSelectedTable.Items[0].Selected = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lvTable.SelectedItems.Count > 0)
            {
                string aliasName = CreateAliasName();
                if (!string.IsNullOrEmpty(aliasName))
                {
                    if (!IsExists(aliasName))
                    {
                        string selectTable = lvTable.SelectedItems[0].Text;
                        ListViewItem item = new ListViewItem(selectTable);
                        item.SubItems.Add(aliasName);
                        lsvSelectedTable.Items.Add(item);
                        lsvSelectedTable.Items[lsvSelectedTable.Items.Count - 1].Selected = true;
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lsvSelectedTable.Items.Count > 0)
            {
                if (lsvSelectedTable.SelectedItems[0] != null)
                {
                    lsvSelectedTable.Items.Remove(lsvSelectedTable.SelectedItems[0]);
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string tables = string.Empty;
            DialogResult = DialogResult.OK;
        }

        private void txtCond_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCond.Text))
            {
                _dv.RowFilter = "TABLENAME like '%" + txtCond.Text + "%' or CHINESENAME like '%" + txtCond.Text + "%'";
                LoadListViewData(_dv);
            }
            else
            {
                _dv.RowFilter = string.Empty;
                LoadListViewData(_dv);
            }
        }

        private void lvTable_DoubleClick(object sender, EventArgs e)
        {
            string aliasName = CreateAliasName();
            if (!string.IsNullOrEmpty(aliasName))
            {
                if (!IsExists(aliasName))
                {
                    string selectTable = lvTable.SelectedItems[0].Text;
                    ListViewItem item = new ListViewItem(selectTable);
                    item.SubItems.Add(aliasName);
                    lsvSelectedTable.Items.Add(item);
                    lsvSelectedTable.Items[lsvSelectedTable.Items.Count - 1].Selected = true;
                }
            }
        }
    }
}
