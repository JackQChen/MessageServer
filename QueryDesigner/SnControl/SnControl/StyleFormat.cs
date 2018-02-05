using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnControl
{
    [Serializable]
	public class StyleFormat
	{
        public int BackColorRed { get; set; }
        public int BackColorGreen { get; set; }
        public int BackColorBlue { get; set; }
        public int ForeColorRed { get; set; }
        public int ForeColorGreen { get; set; }
        public int ForeColorBlue { get; set; }
        public bool ApplyToRow { get; set; }
        public DevExpress.XtraGrid.FormatConditionEnum Condition { get; set; }
        public string Type1 { get; set; }
        public object Value1 { get; set; }
        public string Type2 { get; set; }
        public object Value2 { get; set; }

        public object Clone()
        {
            StyleFormat styleFormat = new StyleFormat();

            styleFormat.BackColorRed = BackColorRed;
            styleFormat.BackColorGreen = BackColorGreen;
            styleFormat.BackColorBlue = BackColorBlue;
            styleFormat.ForeColorRed = ForeColorRed;
            styleFormat.ForeColorGreen = ForeColorGreen;
            styleFormat.ForeColorBlue = ForeColorBlue;
            styleFormat.ApplyToRow = ApplyToRow;
            styleFormat.Condition = Condition;
            styleFormat.Type1 = Type1;
            styleFormat.Value1 = Value1;
            styleFormat.Type2 = Type2;
            styleFormat.Value2 = Value2;

            return styleFormat;
        }
	}
}
