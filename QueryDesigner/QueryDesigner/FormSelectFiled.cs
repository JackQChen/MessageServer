using System;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

namespace QueryDesigner
{
    public partial class FormSelectFiled : Form
    {
        public FormSelectFiled(GridColumnCollection _columns)
        {
            InitializeComponent();
            Columns = _columns;
        }

        public GridColumnCollection Columns { get; set; }

        private void FormSelectFiled_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            foreach (GridColumn column in Columns)
            {
                fieldList.Items.Add(column.FieldName, column.Visible);
            }
        }

        private void fieldList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Columns[e.Index].Visible = e.NewValue == CheckState.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
