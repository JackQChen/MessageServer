namespace SnControl
{
    using System;
    using System.Drawing;

    [Serializable]
    public class SnPage : ICloneable
    {
        private string dataSetsID;
        private string dataSetsName;
        private Color fixTextColor;
        private Font fixTextFont;
        private string pageName;
        private Color textColor;
        private Font textFont;

        public object Clone()
        {
            SnPage page = new SnPage();
            page.pageName = this.pageName;
            page.dataSetsID = this.dataSetsID;
            page.dataSetsName = this.dataSetsName;
            page.fixTextFont = this.fixTextFont;
            page.fixTextColor = this.fixTextColor;
            page.textFont = this.textFont;
            page.textColor = this.textColor;
            return page;
        }

        public string DataSetsID
        {
            get
            {
                return this.dataSetsID;
            }
            set
            {
                this.dataSetsID = value;
            }
        }

        public string DataSetsName
        {
            get
            {
                return this.dataSetsName;
            }
            set
            {
                this.dataSetsName = value;
            }
        }

        public Color FixTextColor
        {
            get
            {
                return this.fixTextColor;
            }
            set
            {
                this.fixTextColor = value;
            }
        }

        public Font FixTextFont
        {
            get
            {
                return this.fixTextFont;
            }
            set
            {
                this.fixTextFont = value;
            }
        }

        public string PageName
        {
            get
            {
                return this.pageName;
            }
            set
            {
                this.pageName = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                this.textColor = value;
            }
        }

        public Font TextFont
        {
            get
            {
                return this.textFont;
            }
            set
            {
                this.textFont = value;
            }
        }
    }
}

