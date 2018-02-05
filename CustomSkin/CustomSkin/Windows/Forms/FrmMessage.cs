using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    public partial class FrmMessage : FormBase
    {
        public FrmMessage()
        {
            InitializeComponent();
        }

        public FrmMessage(MessageBoxButtons mButton)
        {
            InitializeComponent();
            int width = 80, height = 30;
            Button[] btnArr = null;
            switch (mButton)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    btnArr = CreateButtons(3, width, height, 20, this.plBtn);
                    btnArr[0].Text = "终止(&A)";
                    btnArr[0].DialogResult = DialogResult.Abort;
                    btnArr[1].Text = "重试(&R)";
                    btnArr[1].DialogResult = DialogResult.Retry;
                    btnArr[2].Text = "忽略(&I)";
                    btnArr[2].DialogResult = DialogResult.Ignore;
                    break;
                case MessageBoxButtons.OK:
                    btnArr = CreateButtons(1, width, height, 0, this.plBtn);
                    btnArr[0].Text = "确定(&O)";
                    btnArr[0].DialogResult = DialogResult.OK;
                    break;
                case MessageBoxButtons.OKCancel:
                    btnArr = CreateButtons(2, width, height, 30, this.plBtn);
                    btnArr[0].Text = "确定(&O)";
                    btnArr[0].DialogResult = DialogResult.OK;
                    btnArr[1].Text = "取消(&C)";
                    btnArr[1].DialogResult = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.RetryCancel:
                    btnArr = CreateButtons(2, width, height, 30, this.plBtn);
                    btnArr[0].Text = "重试(&R)";
                    btnArr[0].DialogResult = DialogResult.Retry;
                    btnArr[1].Text = "取消(&C)";
                    btnArr[1].DialogResult = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.YesNo:
                    btnArr = CreateButtons(2, width, height, 30, this.plBtn);
                    btnArr[0].Text = "是(&Y)";
                    btnArr[0].DialogResult = DialogResult.Yes;
                    btnArr[1].Text = "否(&N)";
                    btnArr[1].DialogResult = DialogResult.No;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btnArr = CreateButtons(3, width, height, 20, this.plBtn);
                    btnArr[0].Text = "是(&Y)";
                    btnArr[0].DialogResult = DialogResult.Yes;
                    btnArr[1].Text = "否(&N)";
                    btnArr[1].DialogResult = DialogResult.No;
                    btnArr[2].Text = "取消(&C)";
                    btnArr[2].DialogResult = DialogResult.Cancel;
                    break;
                default:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private Button[] CreateButtons(int count, int width, int height, int space, Control pCtl)
        {
            Button[] btnArr = new Button[count];
            for (int i = 0; i < count; i++)
            {
                Button btn = new Button();
                btn.Width = width;
                btn.Height = height;
                btnArr[i] = btn;
                pCtl.Controls.Add(btn);
            }
            int totalWidth = 0;
            for (int i = 0; i < btnArr.Length; i++)
            {
                if (i != 0)
                    totalWidth += space;
                totalWidth += btnArr[i].Width;
            }
            btnArr[0].Left = (pCtl.Width - totalWidth) / 2;
            btnArr[0].Top = (pCtl.Height - btnArr[0].Height) / 2;
            for (int i = 0; i < btnArr.Length; i++)
            {
                if (i == 0)
                    continue;
                btnArr[i].Left = btnArr[i - 1].Left + btnArr[i - 1].Width + space;
                btnArr[i].Top = btnArr[i - 1].Top;
            }
            return btnArr;
        }

        public string Content
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
