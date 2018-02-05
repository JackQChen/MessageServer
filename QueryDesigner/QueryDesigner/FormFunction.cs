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
    public partial class FormFunction : Form
    {
        public FormFunction()
        {
            InitializeComponent();
            _dao = new QueryDAO();
        }

        private QueryDAO _dao;
        private bool _isEdit;

        public string MID
        {
            get
            {
                return txtFunName.Text;
            }
            set
            {
                txtFunName.Text = value;
            }
        }

        public string MNAME
        {
            get
            {
                return txtFunName.Text;
            }
            set
            {
                txtFunName.Text = value;
            }
        }

        public string DETAIL
        {
            get
            {
                return txtDetial.Text;
            }
            set
            {
                txtDetial.Text = value;
            }
        }

        public string COMMENTARY
        {
            get
            {
                return txtMemo.Text;
            }
            set
            {
                txtMemo.Text = value;
            }
        }

        public bool IsEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                _isEdit = value;
                txtFunName.ReadOnly = _isEdit;
            }
        }

        private bool CheckTextValue()
        {
            if (string.IsNullOrEmpty(txtFunName.Text))
            {
                MessageBox.Show("方法名称不能空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtDetial.Text))
            {
                MessageBox.Show("方法内容不能空！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (!CheckSQL(txtDetial.Text))
                {
                    MessageBox.Show("SQL语法错误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private bool CheckSQL(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            else if (sql.ToUpper().IndexOf("SELECT") > 0)
            {
                return false;
            }

            if (sql.ToUpper().IndexOf("DELETE") >= 0)
            {
                return false;
            }

            if (sql.ToUpper().IndexOf("UPDATE") >= 0)
            {
                return false;
            }

            if (sql.ToUpper().IndexOf("INSERT") >= 0)
            {
                return false;
            }

            if (sql.ToUpper().IndexOf("TRUNCATE") >= 0)
            {
                return false;
            }

            if (!TestExecSQL(sql))
            {
                return false;
            }
            return true;
        }

        private bool TestExecSQL(string sql)
        {
            try
            {
                _dao.ExecSQL(sql);
            }
            catch
            {
                return false;
            }

            return true;
        } 

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (CheckTextValue())
            {
                if (!_isEdit && _dao.GetMethod(txtFunName.Text).Rows.Count > 0)
                {
                    MessageBox.Show("方法ID重复！请修改后重新保存！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult = DialogResult.OK;
            }
        }
    }
}
