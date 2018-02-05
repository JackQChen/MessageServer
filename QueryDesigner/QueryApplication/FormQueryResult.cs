using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using DevExpress.Data;
using DevExpress.XtraExport;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using QueryDataAccess;
using QueryException;
using SnControl;

namespace QueryLauncher
{
    public partial class FormQueryResult : Form
    {
        public FormQueryResult()
        {
            InitializeComponent();
            _dao = new QueryDAO();
            _toolTips = new Hashtable();
            _solution = new Solution();
            _componentContainer = new ArrayList();
            _fontRegex = new Regex(@"Name=(.+)\,\s+Size=(\d+)\,.+\s+Style=(.+)\]");
        }

        private Hashtable _toolTips;
        private string _acceptButton;
        private string _cancelButton;
        private ArrayList _componentContainer;
        private Solution _solution;
        private DataTable _masterDataSource;
        private QueryDAO _dao;

        //private int _maxRecordCount;
        //private int _currentRecordCount;

        private static Regex _fontRegex;
        private readonly static Regex _propertySet = new Regex(@"(?<Property>[\w]+)\s*=\s*(?<Value>[\w\d]+)", RegexOptions.Compiled);

        public string FileName { get; set; }

        public ToolStrip ToolBar
        {
            get
            {
                return mainTool;
            }
            set
            {
                mainTool = value;
            }
        }

        private Type GetTypeByName(string typeName)
        {
            ITypeResolutionService typeResolutionService = GetService(typeof(ITypeResolutionService)) as ITypeResolutionService;

            if (typeResolutionService != null)
            {
                return typeResolutionService.GetType(typeName);
            }

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = asm.GetType(typeName);

                if (type != null)
                {
                    return type;
                }
            }

            return Type.GetType(typeName);
        }

        private object GetValueByTypeName(string typeName, object value)
        {
            Type t = Type.GetType(typeName);
            MethodInfo[] methods = t.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] paras = method.GetParameters();
                if (method.Name.IndexOf("Parse") >= 0 && paras.Length == 1 && paras[0].ParameterType == typeof(System.String))
                {
                    return method.Invoke(t, new object[] { value });
                }
            }

            return value.ToString();
        }

        private object CreateObject(string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            return CreateObject(doc.DocumentElement, true);
        }

        private object CreateObject(XmlElement xmlElement, bool suspend)
        {
            Type type = GetTypeByName(xmlElement.Name);

            if (xmlElement.Attributes["value"] != null)
            {
                try
                {
                    return Convert.ChangeType(xmlElement.Attributes["value"].InnerText, type);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (type == null)
            {
                return null;
            }

            object newObj;

            if (type.FullName == "System.Windows.Forms.Form")
            {
                newObj = this.mainPanel;
            }
            else if (type.FullName == "System.Windows.Forms.DataGridView")
            {
                GridControl grdEntity = new GridControl();
                grdEntity.Name = "grdEntity";
                grdEntity.Dock = DockStyle.Fill;
                grdEntity.LookAndFeel.UseWindowsXPTheme = true;
                XmlNode nodeTmp = xmlElement.SelectSingleNode("DataMember");
                if (nodeTmp != null)
                    grdEntity.Text = nodeTmp.Attributes["value"].Value;

                GridView viewEntity = new GridView();
                viewEntity.Appearance.GroupPanel.Options.UseBackColor = true;
                viewEntity.Appearance.GroupPanel.BackColor = Color.SkyBlue;
                viewEntity.Appearance.GroupPanel.BackColor2 = Color.White;
                viewEntity.Appearance.FocusedRow.Options.UseBackColor = true;
                viewEntity.Appearance.FocusedRow.BackColor = Color.FromArgb(51, 153, 255);
                viewEntity.Appearance.HideSelectionRow.Options.UseBackColor = true;
                viewEntity.Appearance.HideSelectionRow.BackColor = Color.FromArgb(51, 153, 255);
                viewEntity.Appearance.HideSelectionRow.Options.UseForeColor = true;
                viewEntity.Appearance.HideSelectionRow.ForeColor = Color.White;

                viewEntity.OptionsView.ColumnAutoWidth = false;
                //viewEntity.OptionsView.ShowGroupPanel = false;
                //viewEntity.OptionsCustomization.AllowFilter = false;
                viewEntity.GroupPanelText = "查询结果";
                viewEntity.OptionsBehavior.Editable = false;
                viewEntity.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
                viewEntity.OptionsView.ShowDetailButtons = false;
                viewEntity.OptionsView.ShowFooter = true;
                viewEntity.FocusRectStyle = DrawFocusRectStyle.None;
                viewEntity.GridControl = grdEntity;
                viewEntity.Name = "GirdView";
                viewEntity.OptionsSelection.EnableAppearanceFocusedCell = false;
                viewEntity.PaintStyleName = "WindowsXP";
                viewEntity.RowHeight = 25;
                viewEntity.ColumnPanelRowHeight = 30;
                viewEntity.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                GridGroupSummaryItem summaryItem;

                IEnumerator enumerator = _solution.DataSetList.GetEnumerator();
                SnDataSet temp = null;
                while (enumerator.MoveNext())
                {
                    temp = enumerator.Current as SnDataSet;
                    if (temp.DataSetID != grdEntity.Text && !string.IsNullOrEmpty(grdEntity.Text))
                        continue;

                    for (int i = 0; i < temp.FieldsList.Count; i++)
                    {
                        GridColumn gridColumn = new GridColumn();
                        gridColumn.FieldName = temp.FieldsList[i].FieldName;
                        gridColumn.Caption = temp.FieldsList[i].FieldChineseName;
                        gridColumn.Width = temp.FieldsList[i].DisplayWidth > 0 ? temp.FieldsList[i].DisplayWidth : 90;
                        if (!temp.FieldsList[i].ColumnVisible)
                        {
                            gridColumn.Visible = false;
                            gridColumn.VisibleIndex = -1;
                        }
                        else
                            gridColumn.VisibleIndex = i;
                        switch (temp.FieldsList[i].CalcType)
                        {
                            case "Sum":
                                summaryItem = new GridGroupSummaryItem();
                                summaryItem.FieldName = temp.FieldsList[i].FieldChineseName;
                                summaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                summaryItem.DisplayFormat = "合计：{0:0.00}";
                                summaryItem.ShowInGroupColumnFooter = gridColumn;
                                viewEntity.GroupSummary.Add(summaryItem);
                                gridColumn.SummaryItem.DisplayFormat = "合计：{0:0.00}";
                                gridColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                break;
                            case "Average":
                                summaryItem = new GridGroupSummaryItem();
                                summaryItem.FieldName = temp.FieldsList[i].FieldChineseName;
                                summaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                                summaryItem.DisplayFormat = "均值：{0:c2}";
                                summaryItem.ShowInGroupColumnFooter = gridColumn;
                                viewEntity.GroupSummary.Add(summaryItem);
                                gridColumn.SummaryItem.DisplayFormat = "均值：{0:c}";
                                gridColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                                break;
                            case "Max":
                                gridColumn.SummaryItem.DisplayFormat = "最大:{0:c}";
                                gridColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Max;
                                break;
                            case "Min":
                                gridColumn.SummaryItem.DisplayFormat = "最小:{0:c}";
                                gridColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Min;
                                break;
                            case "Count":
                                gridColumn.SummaryItem.DisplayFormat = "行数:{0:d}";
                                gridColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                                break;
                            default:
                                gridColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.None;
                                break;
                        }

                        foreach (StyleFormat s in temp.FieldsList[i].StyleFormat)
                        {
                            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition = new DevExpress.XtraGrid.StyleFormatCondition();
                            styleFormatCondition.Column = gridColumn;
                            styleFormatCondition.Appearance.Options.UseForeColor = true;
                            styleFormatCondition.Appearance.Options.UseBackColor = true;

                            styleFormatCondition.Appearance.BackColor =
                                System.Drawing.Color.FromArgb(s.BackColorRed, s.BackColorGreen, s.BackColorBlue);

                            styleFormatCondition.Appearance.ForeColor =
                                System.Drawing.Color.FromArgb(s.ForeColorRed, s.ForeColorGreen, s.ForeColorBlue);

                            styleFormatCondition.ApplyToRow = s.ApplyToRow;

                            styleFormatCondition.Condition = s.Condition;

                            if (s.Value1 != null)
                            {
                                styleFormatCondition.Value1 = s.Value1.ToString() != "<Null>"
                                    ? GetValueByTypeName(s.Type1, s.Value1) : "<Null>";
                            }

                            if (s.Value2 != null)
                            {
                                styleFormatCondition.Value2 = s.Value2.ToString() != "<Null>"
                                    ? GetValueByTypeName(s.Type2, s.Value2) : "<Null>";
                            }

                            viewEntity.FormatConditions.Add(styleFormatCondition);
                        }

                        switch (temp.FieldsList[i].FieldType.ToUpper())
                        {
                            case "TIMESTAMP":
                            case "DATETIME":
                                gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                                break;
                            case "DECIMAL":
                                if (temp.FieldsList[i].DecimalDigits == 0 && temp.FieldsList[i].DecimalDigits > 6)
                                {
                                    temp.FieldsList[i].DecimalDigits = 2;
                                }

                                string tempstr = "0.";

                                for (int j = 0; j < temp.FieldsList[i].DecimalDigits; j++)
                                {
                                    tempstr += "0";
                                }

                                gridColumn.DisplayFormat.FormatString = tempstr;
                                gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                break;
                            case "INT":
                                gridColumn.DisplayFormat.FormatString = "0";
                                gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                break;
                        }

                        viewEntity.Columns.Add(gridColumn);
                    }
                    if (File.Exists(FileName + grdEntity.Text + ".xml"))
                    {
                        viewEntity.RestoreLayoutFromXml(FileName + grdEntity.Text + ".xml");
                    }
                }

                grdEntity.MainView = viewEntity;
                newObj = grdEntity;
                _componentContainer.Add(newObj);
            }
            else
            {
                newObj = type.Assembly.CreateInstance(type.FullName);
                _componentContainer.Add(newObj);
            }

            string componentName = null;

            if (xmlElement["Name"] != null && xmlElement["Name"].Attributes["value"] != null)
            {
                componentName = xmlElement["Name"].Attributes["value"].InnerText;
            }

            bool hasSuspended = false;
            if (suspend && newObj is ContainerControl)
            {
                hasSuspended = true;
                ((Control)newObj).SuspendLayout();
            }

            foreach (XmlNode subNode in xmlElement.ChildNodes)
            {
                if (subNode is XmlElement)
                {
                    XmlElement subElement = (XmlElement)subNode;

                    if (subElement.Attributes["value"] != null)
                    {
                        try
                        {
                            SetValue(newObj, subElement.Name, subElement.Attributes["value"].InnerText);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(subElement.Name);
                        object pList = propertyInfo.GetValue(newObj, null);

                        if (pList is IList)
                        {
                            foreach (XmlNode node in subElement.ChildNodes)
                            {
                                if (node is XmlElement)
                                {
                                    XmlElement cNode = node as XmlElement;
                                    object collectionObj = CreateObject(cNode, false);

                                    if (collectionObj != null)
                                    {
                                        try
                                        {
                                            ((IList)pList).Add(collectionObj);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (_acceptButton != null && newObj is Form)
            {
                ((Form)newObj).AcceptButton = (IButtonControl)Container.Components[_acceptButton];
                _acceptButton = null;
            }

            if (_cancelButton != null && newObj is Form)
            {
                ((Form)newObj).CancelButton = (IButtonControl)Container.Components[_cancelButton];
                _cancelButton = null;
            }

            if (hasSuspended)
            {
                ((Control)newObj).ResumeLayout(false);
            }

            return newObj;
        }

        private void SetValue(object newObj, string propertyName, string val)
        {
            try
            {
                PropertyInfo propertyInfo = newObj.GetType().GetProperty(propertyName);

                if (propertyInfo == null)
                {
                    return;
                }

                switch (propertyName)
                {
                    case "AcceptButton":
                        _acceptButton = val.Split(' ')[0];
                        return;
                    case "CancelButton":
                        _cancelButton = val.Split(' ')[0];
                        return;
                    case "ToolTip":
                        _toolTips[newObj] = val;
                        return;
                }

                if (val.StartsWith("{") && val.EndsWith("}"))
                {
                    val = val.Substring(1, val.Length - 2);

                    object propertyObj = null;

                    if (propertyInfo.CanWrite)
                    {
                        Type type = GetTypeByName(propertyInfo.PropertyType.FullName);
                        propertyObj = type.Assembly.CreateInstance(propertyInfo.PropertyType.FullName);
                    }
                    else
                    {
                        propertyObj = propertyInfo.GetValue(newObj, null);
                    }

                    Match match = _propertySet.Match(val);

                    while (true)
                    {
                        if (!match.Success)
                        {
                            break;
                        }

                        SetValue(propertyObj, match.Result("${Property}"), match.Result("${Value}"));
                        match = match.NextMatch();
                    }

                    if (propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(newObj, propertyObj, null);
                    }
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    propertyInfo.SetValue(newObj, Enum.Parse(propertyInfo.PropertyType, val), null);
                }
                else if (propertyInfo.PropertyType == typeof(Color))
                {
                    string color = val.Substring(val.IndexOf('[') + 1).Replace("]", "");
                    string[] argb = color.Split(',', '=');

                    if (argb.Length > 1)
                    {
                        propertyInfo.SetValue(newObj, Color.FromArgb(Convert.ToInt32(argb[1]),
                            Convert.ToInt32(argb[3]), Convert.ToInt32(argb[5]), Convert.ToInt32(argb[7])), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(newObj, Color.FromName(color), null);
                    }
                }
                else
                {
                    if (val.Length > 0)
                    {
                        TypeConverter convert = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                        propertyInfo.SetValue(newObj, convert.ConvertFromInvariantString(val), null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExeQuery()
        {
            string sql = string.Empty;
            IEnumerator enumerator = _solution.DataSetList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SnDataSet temp = enumerator.Current as SnDataSet;

                switch (temp.DataSetType)
                {
                    case DataSetType.Page:
                        #region Page
                        Hashtable htParam = new Hashtable();
                        sql = temp.SQLExpression;
                        IEnumerator enumControl = _componentContainer.GetEnumerator();
                        while (enumControl.MoveNext())
                        {
                            ICommonAttribute commonAttribute = enumControl.Current as ICommonAttribute;
                            if (commonAttribute != null && !string.IsNullOrEmpty(commonAttribute.ParamName))
                            {
                                if (commonAttribute.DataSetName == "所有数据集"
                                    || temp.DataSetID + "-" + temp.DataSetName == commonAttribute.DataSetName)
                                {
                                    string paramName = commonAttribute.ParamName, paramType = commonAttribute.ParamType, value = commonAttribute.Value;
                                    if (!htParam.ContainsKey(paramName))
                                        switch (paramType)
                                        {
                                            case "String":
                                                htParam.Add(paramName, value);
                                                break;
                                            case "Int":
                                                htParam.Add(paramName, Convert.ToInt32(value));
                                                break;
                                            case "Decimal":
                                                htParam.Add(paramName, Convert.ToDecimal(value));
                                                break;
                                            case "DateTime":
                                                htParam.Add(paramName, Convert.ToDateTime(value));
                                                break;
                                        }
                                }
                            }
                        }
                        string strParam = "";
                        if (htParam != null)
                        {
                            foreach (DictionaryEntry de in htParam)
                            {
                                strParam += string.Format("{0}:{1}->{2}\r\n",
                                   de.Value.GetType().Name, de.Key, de.Value);
                            }
                        }
                        WriteLog(sql + "\r\n" + strParam);
                        try
                        {
                            _masterDataSource = _dao.ExecSQL(sql, htParam);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "查询失败");
                            WriteLog(ex.Message);
                            return;
                        }
                        IEnumerator enumGridControl = _componentContainer.GetEnumerator();
                        while (enumGridControl.MoveNext())
                        {
                            GridControl grd = enumGridControl.Current as GridControl;
                            if (grd != null)
                            {
                                grd.BeginUpdate();
                                if (string.IsNullOrEmpty(grd.Text))
                                    grd.DataSource = _masterDataSource.DefaultView;
                                else if (grd.Text == temp.DataSetID)
                                    grd.DataSource = _masterDataSource.DefaultView;
                                //_currentRecordCount = _masterDataSource.Rows.Count; 
                                grd.EndUpdate();
                                //toolMoreRow.Enabled = (_currentRecordCount != _maxRecordCount); 
                                Size = new Size(Size.Width, Size.Height + 1);
                                Size = new Size(Size.Width, Size.Height - 1);
                            }
                        }
                        #endregion
                        break;
                    case DataSetType.Proc:
                        #region Proc
                        //string procName = temp.DataSetName;
                        //enumControl = _componentContainer.GetEnumerator();

                        //Dictionary<string, object> dic = new Dictionary<string, object>();
                        //while (enumControl.MoveNext())
                        //{
                        //    ICommonAttribute commonAttribute = enumControl.Current as ICommonAttribute;

                        //    if (commonAttribute != null)
                        //    {
                        //        if (temp.DataSetID + "-" + temp.DataSetName == commonAttribute.DataSetName)
                        //        {
                        //            if (string.IsNullOrEmpty(commonAttribute.ProcParamName))
                        //                throw new Exception("存储过程参数名称不能为空");
                        //            Type t = Type.GetType("System." + commonAttribute.ProcParamType, false, true);

                        //            if (commonAttribute is SnControl.ParamComboBox)
                        //            {
                        //                SnControl.ParamComboBox combox = (commonAttribute as SnControl.ParamComboBox);
                        //                if (!string.IsNullOrEmpty(combox.ValueMember))
                        //                {
                        //                    dic.Add(commonAttribute.ProcParamName.ToUpper(), combox.SelectedValue);
                        //                    continue;
                        //                }
                        //            }
                        //            else if (commonAttribute is SnControl.ParamRadioButton)
                        //            {
                        //                SnControl.ParamRadioButton radioButton = (commonAttribute as SnControl.ParamRadioButton);
                        //                if (radioButton.Checked)
                        //                    dic.Add(commonAttribute.ProcParamName.ToUpper(), radioButton.Value);
                        //                continue;
                        //            }
                        //            else if (commonAttribute is SnControl.Search)
                        //            {
                        //                SnControl.Search search = (commonAttribute as SnControl.Search);
                        //                if (!string.IsNullOrEmpty(search.Value))
                        //                {
                        //                    dic.Add(commonAttribute.ProcParamName.ToUpper(), search.Value);
                        //                    continue;
                        //                }
                        //            }
                        //            if (t.Name == "String")
                        //            {
                        //                dic.Add(commonAttribute.ProcParamName.ToUpper(), commonAttribute.Text);
                        //            }
                        //            else
                        //            {
                        //                object value = null;
                        //                try
                        //                {
                        //                    if (commonAttribute.Text == string.Empty)
                        //                        value = t.IsValueType ? Activator.CreateInstance(t) : null;
                        //                    else
                        //                        value = t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { commonAttribute.Text });
                        //                }
                        //                catch (Exception)
                        //                {
                        //                    throw new Exception("数据类型不匹配，请检查");
                        //                }
                        //                dic.Add(commonAttribute.ProcParamName.ToUpper(), value);
                        //            }
                        //        }
                        //    }
                        //}

                        //string strParam2 = "";
                        //foreach (var de in dic)
                        //{
                        //    strParam2 += string.Format("{0}<->{1}<->{2}\r\n",
                        //       de.Value.GetType().Name, de.Key, de.Value);
                        //}
                        //WriteLog(procName + "\r\n" + strParam2);
                        //_masterDataSource = _dao.ExecProc(procName, dic);

                        //enumGridControl = _componentContainer.GetEnumerator();

                        //while (enumGridControl.MoveNext())
                        //{
                        //    GridControl grd = enumGridControl.Current as GridControl;
                        //    if (grd != null)
                        //    {
                        //        grd.BeginUpdate();

                        //        grd.DataSource = _masterDataSource.DefaultView;

                        //        //_currentRecordCount = _masterDataSource.Rows.Count;

                        //        grd.EndUpdate();
                        //        //toolMoreRow.Enabled = (_currentRecordCount != _maxRecordCount);

                        //        Size = new Size(Size.Width, Size.Height + 1);
                        //        Size = new Size(Size.Width, Size.Height - 1);
                        //    }
                        //}
                        #endregion
                        break;
                }
            }
        }

        private GridView GetGridView()
        {
            GridView gv = null;
            IEnumerator enumGridControl = _componentContainer.GetEnumerator();

            while (enumGridControl.MoveNext())
            {
                GridControl grd = enumGridControl.Current as GridControl;

                if (grd != null)
                {
                    gv = grd.MainView as GridView;
                    break;
                }
            }

            return gv;
        }

        private void FormQueryResult_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            Stream stream = new FileStream(FileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(stream, System.Text.Encoding.Unicode);
            byte[] buffer = binaryReader.ReadBytes((int)stream.Length);

            //string xmlContent = File.ReadAllText(fileName);
            string xmlContent;
            string solutionContent;
            string xmlForm = string.Empty;
            XmlDocument doc = new XmlDocument();

            try
            {
                xmlContent = System.Text.Encoding.Unicode.GetString(buffer);

                binaryReader.Close();
                stream.Close();

                if (xmlContent.IndexOf("<SnControl.Solution>") == -1)
                {
                    xmlContent = File.ReadAllText(FileName);
                }

                solutionContent = xmlContent.Substring(xmlContent.IndexOf("<SnControl.Solution>"),
                    xmlContent.IndexOf("</SnControl.Solution>") - xmlContent.IndexOf("<SnControl.Solution>")) + "</SnControl.Solution>";

                XmlSolutionReader xmlSolutionReader = new XmlSolutionReader(_solution);
                _solution = xmlSolutionReader.SetUpSolution(solutionContent);
                SolutionInstance.GetInstance().Solution = _solution;
                Text = _solution.SolutionName;

                xmlForm = xmlContent.Substring(xmlContent.IndexOf("<System.Windows.Forms.Form"), xmlContent.IndexOf("</QuerySolution>") - xmlContent.IndexOf("<System.Windows.Forms.Form"));
                doc.LoadXml(xmlForm);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (doc.DocumentElement.Attributes["version"] == null)
            {
                CreateObject(xmlForm);
            }
            else
            {
                foreach (XmlElement element in doc.DocumentElement.ChildNodes)
                {
                    CreateObject(element, true);
                }
            }

            string resultType = "";
            object result = string.Empty;
            IEnumerator enumControl = _componentContainer.GetEnumerator();
            Method method = new Method(this._dao);
            while (enumControl.MoveNext())
            {
                var ctl = enumControl.Current as ICommonAttribute;
                if (ctl == null)
                    continue;
                if (string.IsNullOrEmpty(ctl.Function))
                    continue;
                method.ExecMethod(ctl.Function, out resultType, out result);
                switch (resultType)
                {
                    case "字符":
                    case "日期":
                        ((Control)ctl).Text = result.ToString();
                        break;
                    case "数据集":
                        if (ctl is ParamComboBox)
                        {
                            ((ParamComboBox)ctl).DataSource = ((DataTable)result).DefaultView;
                            ((ParamComboBox)ctl).SelectedItem = null;
                        }
                        break;
                    default: break;
                }
            }

            Size = new Size(Size.Width, Size.Height + 1);
            Size = new Size(Size.Width, Size.Height - 1);
        }

        private void FormQueryResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int length = ((ParamTextBox)sender).MaxLength;
                if (length <= 15)
                {
                    ((ParamTextBox)sender).Text = ((ParamTextBox)sender).Text.PadLeft(length, '0');
                }
            }
        }

        private void FormQueryResult_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SelectNextControl(this.ActiveControl, true, true, true, false);
            }
        }

        private void toolQuery_Click(object sender, EventArgs e)
        {
            this.Focus();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ExeQuery();
            }
            catch (LogonException ex)
            {
                ExceptionManager.Publish(ex);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception err)
            {
                MessageBox.Show("数据生成失败！" + err.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void toolPrint_Click(object sender, EventArgs e)
        {
            IEnumerator enumControl = _componentContainer.GetEnumerator();
            DataView dt = null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            SnDataSet temp = _solution.DataSetList[0] as SnDataSet;
            GridControl grd = null;
            IEnumerator enumerator = _solution.DataSetList.GetEnumerator();

            while (enumControl.MoveNext())
            {
                ICommonAttribute commonAttribute = enumControl.Current as ICommonAttribute;
                if (commonAttribute != null)
                {
                    if (temp.DataSetName == commonAttribute.DataSetName)
                        dic.Add(commonAttribute.ParamName, commonAttribute.Value);
                }
            }
            var enumGridControl = _componentContainer.GetEnumerator();
            Hashtable htControls = new Hashtable();
            while (enumGridControl.MoveNext())
            {
                GridControl grid = enumGridControl.Current as GridControl;
                if (grid != null)
                {
                    List<string> filter = new List<string>();
                    DevExpress.XtraGrid.Views.Grid.GridView view = grid.Views[0] as DevExpress.XtraGrid.Views.Grid.GridView;

                    foreach (GridColumn gcc in view.SortedColumns)
                    {
                        filter.Add(gcc.FieldName + " " + (gcc.SortOrder == ColumnSortOrder.Descending ? "DESC" : ""));
                    }
                    DataView dv = grid.DataSource as DataView;
                    if (dv != null)
                    {
                        dv.Sort = string.Join(",", filter.ToArray());
                        dt = dv;
                    }
                    grd = grid;
                    //break;
                }
                if (!enumGridControl.Current.GetType().Equals(typeof(Column)))
                {
                    Control ctl = (Control)enumGridControl.Current;
                    htControls.Add(ctl.Name, ctl.Text);
                }
            }
            if (dt == null)
            {
                MessageBox.Show("数据源没有数据");
            }
            else
            {
                if (string.IsNullOrEmpty(temp.ReportPath))
                {
                    if (grd != null)
                    {
                        if (PrintHelper.IsPrintingAvailable)
                        {
                            PrintHelper.Print(grd);
                        }
                        else
                        {
                            MessageBox.Show("打印组件库没有发现！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    Print(htControls, dic, dt, temp.ReportPath, true);
                }
            }
        }

        private void toolView_Click(object sender, EventArgs e)
        {
            IEnumerator enumControl = _componentContainer.GetEnumerator();
            DataView dt = null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            SnDataSet temp = _solution.DataSetList[0] as SnDataSet;
            GridControl grd = null;
            while (enumControl.MoveNext())
            {
                ICommonAttribute commonAttribute = enumControl.Current as ICommonAttribute;
                if (commonAttribute != null)
                {
                    if (temp.DataSetName == commonAttribute.DataSetName)
                        dic.Add(commonAttribute.ParamName, commonAttribute.Value);
                }
            }
            var enumGridControl = _componentContainer.GetEnumerator();
            Hashtable htControls = new Hashtable();
            while (enumGridControl.MoveNext())
            {
                GridControl grid = enumGridControl.Current as GridControl;
                if (grid != null)
                {
                    List<string> filter = new List<string>();
                    DevExpress.XtraGrid.Views.Grid.GridView view = grid.Views[0] as DevExpress.XtraGrid.Views.Grid.GridView;

                    foreach (GridColumn gcc in view.SortedColumns)
                    {
                        filter.Add(gcc.FieldName + " " + (gcc.SortOrder == ColumnSortOrder.Descending ? "DESC" : ""));
                    }
                    DataView dv = grid.DataSource as DataView;
                    if (dv != null)
                    {
                        dv.Sort = string.Join(",", filter.ToArray());
                        dt = dv;
                    }
                    grd = grid;
                    //break;
                }
                if (!enumGridControl.Current.GetType().Equals(typeof(Column)))
                {
                    Control ctl = (Control)enumGridControl.Current;
                    htControls.Add(ctl.Name, ctl.Text);
                }
            }
            if (dt == null)
            {
                MessageBox.Show("数据源没有数据");
            }
            else
            {
                if (string.IsNullOrEmpty(temp.ReportPath))
                {
                    if (grd != null)
                    {
                        if (PrintHelper.IsPrintingAvailable)
                        {
                            PrintHelper.ShowPreview(grd);
                        }
                        else
                        {
                            MessageBox.Show("打印组件库没有发现！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    Print(htControls, dic, dt, temp.ReportPath, false);
                }
            }
        }

        private void toolField_Click(object sender, EventArgs e)
        {
        }

        private void toolCard_Click(object sender, EventArgs e)
        {
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            IEnumerator enumGridControl = _componentContainer.GetEnumerator();
            while (enumGridControl.MoveNext())
            {
                GridControl grd = enumGridControl.Current as GridControl;

                if (grd != null)
                {
                    BaseView oldView = grd.MainView;

                    DevExpress.XtraGrid.StyleFormatCondition[] styles = new DevExpress.XtraGrid.StyleFormatCondition[grd.MainView.FormatConditions.Count];

                    for (int i = 0; i < styles.Length; i++)
                    {
                        styles[i] = grd.MainView.FormatConditions[i];
                    }

                    grd.MainView = grd.CreateView(toolCard.Text == "卡片" ? "CardView" : "GridView");

                    if (grd.MainView is GridView)
                    {
                        GridView gv = grd.MainView as GridView;
                        gv.OptionsView.ColumnAutoWidth = false;
                        gv.OptionsView.ShowGroupPanel = false;
                        gv.OptionsCustomization.AllowFilter = false;
                        gv.OptionsBehavior.Editable = false;
                        gv.OptionsView.ShowDetailButtons = false;
                        gv.OptionsView.ShowFooter = true;
                        gv.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                        gv.OptionsSelection.EnableAppearanceFocusedCell = false;
                        gv.PaintStyleName = "WindowsXP";
                        gv.RowHeight = 25;
                        gv.ColumnPanelRowHeight = 30;
                    }
                    else if (grd.MainView is CardView)
                    {
                        CardView gv = grd.MainView as CardView;
                        gv.OptionsBehavior.Editable = false;
                        gv.PaintStyleName = "WindowsXP";
                    }

                    grd.MainView.FormatConditions.AddRange(styles);

                    if (toolCard.Text == "卡片")
                    {
                        toolCard.Text = "表格";
                    }
                    else
                    {
                        toolCard.Text = "卡片";
                    }

                    if (oldView != null)
                    {
                        oldView.Dispose();
                    }

                    Cursor.Current = currentCursor;
                }
            }
        }

        private void toolExport_Click(object sender, EventArgs e)
        {
            IEnumerator enumGridControl = _componentContainer.GetEnumerator();

            while (enumGridControl.MoveNext())
            {
                GridControl grd = enumGridControl.Current as GridControl;

                if (grd != null)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        IExportProvider provider = new ExportXlsProvider(saveFileDialog1.FileName);
                        BaseExportLink link = grd.MainView.CreateExportLink(provider);
                        link.ExportTo(true);
                    }
                }
            }
        }

        private void toolFirt_Click(object sender, EventArgs e)
        {
            GridView gv = GetGridView();
            if (gv != null && gv.RowCount != 0)
            {
                gv.FocusedRowHandle = 0;
            }
        }

        private void toolPrevious_Click(object sender, EventArgs e)
        {
            GridView gv = GetGridView();
            if (gv != null && gv.RowCount != 0 && gv.FocusedRowHandle > 0)
            {
                gv.FocusedRowHandle = gv.FocusedRowHandle - 1;
            }
        }

        private void toolNext_Click(object sender, EventArgs e)
        {
            GridView gv = GetGridView();
            if (gv != null && gv.RowCount != 0 && gv.FocusedRowHandle < gv.RowCount - 1)
            {
                gv.FocusedRowHandle = gv.FocusedRowHandle + 1;
            }
        }

        private void toolLast_Click(object sender, EventArgs e)
        {
            GridView gv = GetGridView();
            if (gv != null && gv.RowCount != 0)
            {
                gv.FocusedRowHandle = gv.RowCount - 1;
            }
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void Print(Hashtable htControls, Dictionary<string, string> dic, DataView dt, string path, bool isPrint)
        {
            //调用打印
            /*
            if (!File.Exists(Application.StartupPath + path))
            {
                MessageBox.Show(string.Format("报表文件[{0}]不存在，请检查.", Application.StartupPath + path));
                return;
            }
            if (!Directory.Exists(Application.StartupPath + "\\ReportXML\\Query"))
                Directory.CreateDirectory(Application.StartupPath + "\\ReportXML\\Query");
            dt.Table.WriteXml(Application.StartupPath + "\\ReportXML\\Query\\" + Path.GetFileName(path));
            if (Path.GetExtension(path).ToLower() == ".rmf")
            {
                RMReportEngine.RMReport r = new RMReportEngine.RMReport();
                r.Init(this, RMReportEngine.RMReportType.rmrtReport);
                foreach (DictionaryEntry de in htControls)
                {
                    r.AddVariable(de.Key.ToString(), de.Value.ToString(), true);
                }
                foreach (string key in dic.Keys)
                {
                    r.AddVariable(key, dic[key], true);
                }
                r.AddDataSet(dt, "report");
                r.LoadFromFile(Application.StartupPath + path);
                r.ShowPrintDialog = false;
                r.ShowProgress = false;
                if (isPrint)
                    r.PrintReport();
                else
                    r.ShowReport();
                r.Destroy();
                r.Dispose();
            }
            else
            {
                PrintManager printer = new PrintManager();
                printer.InitReport(Path.GetFileNameWithoutExtension(path));
                foreach (DictionaryEntry de in htControls)
                    printer.AddParam(de.Key.ToString(), de.Value.ToString());
                foreach (string key in dic.Keys)
                    printer.AddParam(key, dic[key]);
                printer.AddData(dt.Table, "report");
                if (isPrint)
                    printer.Print();
                else
                    printer.PreView();
            }
             * */
        }

        private void toolLayout_Click(object sender, EventArgs e)
        {
            foreach (string strFile in Directory.GetFiles(this.FileName.Substring(0, this.FileName.LastIndexOf('\\') + 1), "*.xml", SearchOption.TopDirectoryOnly))
            {
                if (strFile.Substring(strFile.LastIndexOf('\\') + 1).StartsWith(this.FileName.Substring(this.FileName.LastIndexOf('\\') + 1)))
                    File.Delete(strFile);
            }
            IEnumerator enumGridControl = _componentContainer.GetEnumerator();
            while (enumGridControl.MoveNext())
            {
                GridControl grd = enumGridControl.Current as GridControl;

                if (grd != null)
                {
                    grd.MainView.SaveLayoutToXml(
                        this.FileName.Substring(0, this.FileName.LastIndexOf('\\') + 1) +
                        this.FileName.Substring(this.FileName.LastIndexOf('\\') + 1) + grd.Text + ".xml");
                }
            }
        }

        private void WriteLog(string log)
        {
            File.AppendAllText(Application.StartupPath + "\\QueryExpression.txt",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + log + "\r\n", Encoding.UTF8);
        }
    }
}