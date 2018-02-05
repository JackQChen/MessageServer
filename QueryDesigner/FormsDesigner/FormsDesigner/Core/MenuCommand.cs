namespace FormsDesigner.Core
{
    using System;
    using System.Windows.Forms;

    public class MenuCommand : ToolStripMenuItem, IStatusUpdate
    { 
        private string description;
        public static Converter<string, ICommand> LinkCommandCreator;
        private ICommand menuCommand;

        public MenuCommand(string label)
        {
            this.menuCommand = null;
            this.description = "";
            this.RightToLeft = RightToLeft.Inherit; 
            this.Text = label;
        }

        public MenuCommand(string label, EventHandler handler) : this(label)
        {
            base.Click += handler;
        }

        private void CreateCommand()
        {
            try
            {
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception, "Can't create menu command : ");
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ICommand command = this.Command;
            if (command != null)
            {
                command.Run();
            }
        }

        public static Keys ParseShortcut(string shortcutString)
        {
            Keys none = Keys.None;
            if (shortcutString.Length > 0)
            {
                try
                {
                    foreach (string str in shortcutString.Split(new char[] { '|' }))
                    {
                        none |= (Keys) Enum.Parse(typeof(Keys), str);
                    }
                }
                catch (Exception exception)
                {
                    MessageService.ShowError(exception);
                    return Keys.None;
                }
            }
            return none;
        }

        public virtual void UpdateStatus()
        {
        }

        public virtual void UpdateText()
        {
        }

        public ICommand Command
        {
            get
            {
                if (this.menuCommand == null)
                {
                    this.CreateCommand();
                }
                return this.menuCommand;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }
    }
}

