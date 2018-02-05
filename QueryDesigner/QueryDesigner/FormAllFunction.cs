using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QueryDataAccess;

namespace QueryDesigner
{
    public partial class FormAllFunction : Form
    {
        public FormAllFunction()
        {
            InitializeComponent();
            _dao = new QueryDAO();
        }

        private DataTable _dt;
        private QueryDAO _dao;

        private void GetData()
        {  
            _dt = _dao.GetMethodList();

            functionList.Items.Clear();

            foreach (DataRow row in _dt.Rows)
            {
                ListViewItem item = functionList.Items.Add(row["MID"].ToString());
                item.SubItems.Add(row["COMMENTARY"].ToString());
                item.SubItems.Add(row["METHODTYPE"].ToString());
                item.SubItems.Add(row["VALUETYPE"].ToString());
                item.Tag = row;
            }
        }

        private void FormAllFunction_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            GetData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FormFunction form = new FormFunction();
            form.IsEdit = false;

            if (form.ShowDialog() == DialogResult.OK)
            {
                DataRow row = _dt.NewRow();

                row["MID"] = form.MID;
                row["MNAME"] = form.MID;
                row["COMMENTARY"] = form.COMMENTARY;
                row["DETAIL"] = form.DETAIL;
                row["METHODTYPE"] = "USER";
                row["VALUETYPE"] = "数据集";

                _dt.Rows.Add(row);

                try
                {
                    _dao.UpdateMethod(_dt.GetChanges());
                    _dt.AcceptChanges();
                }
                catch (Exception ex)
                {
                    _dt.RejectChanges();
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GetData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (functionList.SelectedItems.Count > 0)
            {
                DataRow row = (DataRow)functionList.SelectedItems[0].Tag;

                if (row == null)
                {
                    return;
                }

                if (row["METHODTYPE"].ToString() == "SYSTEM")
                {
                    MessageBox.Show("系统函数不能编辑！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FormFunction form = new FormFunction();
                form.MID = row["MID"].ToString();
                form.MNAME = row["MNAME"].ToString();
                form.COMMENTARY = row["COMMENTARY"].ToString();
                form.DETAIL = row["DETAIL"].ToString();
                form.IsEdit = true;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    row["MID"] = form.MID;
                    row["MNAME"] = form.MID;
                    row["COMMENTARY"] = form.COMMENTARY;
                    row["DETAIL"] = form.DETAIL;

                    try
                    {
                        _dao.UpdateMethod(_dt.GetChanges());
                        _dt.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    GetData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (functionList.SelectedItems.Count > 0)
            {
                DataRow row = (DataRow)functionList.SelectedItems[0].Tag;

                if (row["METHODTYPE"].ToString() == "SYSTEM")
                {
                    MessageBox.Show("系统函数不能删除！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("是否确认删除？", "信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        _dao.DeleteMethod(row);

                        row.Delete();
                        _dt.AcceptChanges();

                        MessageBox.Show("删除成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GetData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void functionList_DoubleClick(object sender, EventArgs e)
        {
            btnEdit_Click(btnEdit, EventArgs.Empty);
        }
    }
}
