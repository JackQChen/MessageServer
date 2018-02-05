using System;
using System.Data;
using System.Windows.Forms;
using SnControl;
using QueryDataAccess;
using System.Collections;

namespace QueryDesigner
{
    public partial class FormFields : Form
    {
        public FormFields()
        {
            InitializeComponent();
            _currFields = new FieldsCollections();
            _dao = new QueryDAO();
        }

        private QueryDAO _dao;
        private FieldsCollections _currFields;
        private Hashtable htItems = new Hashtable();
        private DataTable dtFields;

        public string DataName { get; set; }
        public FieldsCollections FieldList { get; set; }
        public FieldsCollections FieldListClone { get; set; }

        private void FormFields_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            labDataSource.Text = DataName;

            FieldListClone = FieldList.Clone() as FieldsCollections;

            for (int i = 0; i < FieldList.Count; i++)
            {
                FieldItem temp = FieldListClone[i];
                LB_fldList.Items.Add(temp.DataTableName + "->" + temp.FieldChineseName + "." + temp.FieldName);
            }

            if (FieldList.Count > 0)
            {
                LB_fldList.SetSelected(0, true);
            }
            dtFields = _dao.GetFieldList();
            ShowFields(dtFields);
        }

        private void ShowFields(DataTable dt)
        {
            this.search1.DataSource = dt;
            listView1.Items.Clear();
            htItems.Clear();
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = listView1.Items.Add(row["TABLENAME"].ToString());
                item.SubItems.Add(row["FIELDNAME"].ToString());
                item.SubItems.Add(row["CHINESENAME"].ToString());
                htItems.Add(row["TABLENAME"].ToString() + "." + row["FIELDNAME"].ToString(), item);
            }
        }

        private void search1_SearchDone(object sender, SearchDoneArgs e)
        {
            if (e.DataRow == null)
            {
                ShowFields(dtFields);
            }
            else
            {
                ((ListViewItem)htItems[e.DataRow["TABLENAME"].ToString() + "." + e.DataRow["FIELDNAME"].ToString()])
                    .Selected = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                FieldItem temp = new FieldItem();
                temp.DataTableName = listView1.SelectedItems[0].Text;
                temp.FieldName = listView1.SelectedItems[0].SubItems[1].Text;
                temp.FieldChineseName = listView1.SelectedItems[0].SubItems[2].Text;

                if (LB_fldList.Items.IndexOf(temp.DataTableName + "->" + temp.FieldChineseName + "." + temp.FieldName) == -1)
                {
                    FieldListClone.Add(temp);
                    LB_fldList.Items.Add(temp.DataTableName + "->" + temp.FieldChineseName + "." + temp.FieldName);
                    LB_fldList.SetSelected(LB_fldList.Items.Count - 1, true);
                }
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listView1.Items)
            {
                FieldItem temp = new FieldItem();
                temp.DataTableName = listView1.SelectedItems[0].Text;
                temp.FieldName = listView1.SelectedItems[0].SubItems[1].Text;
                temp.FieldChineseName = listView1.SelectedItems[0].SubItems[2].Text;

                if (LB_fldList.Items.IndexOf(temp.DataTableName + "->" + temp.FieldChineseName + "." + temp.FieldName) == -1)
                {
                    FieldListClone.Add(temp);
                    LB_fldList.Items.Add(temp.DataTableName + "->" + temp.FieldChineseName + "." + temp.FieldName);
                    LB_fldList.SetSelected(LB_fldList.Items.Count - 1, true);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (LB_fldList.Items.Count > 0)
            {
                int index = LB_fldList.SelectedIndex;

                FieldListClone.RemoveAt(index);
                LB_fldList.Items.RemoveAt(index);

                if (index > 0)
                {
                    LB_fldList.SetSelected(index - 1, true);
                }
                else if (LB_fldList.Items.Count > 0 && index == 0)
                {
                    LB_fldList.SetSelected(0, true);
                }
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            while (LB_fldList.Items.Count > 0)
            {
                LB_fldList.Items.RemoveAt(0);
                FieldListClone.RemoveAt(0);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            FieldList = FieldListClone;
            DialogResult = DialogResult.OK;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }

        private void LB_fldList_DoubleClick(object sender, EventArgs e)
        {
            btnRemove_Click(sender, e);
        }

    }
}
