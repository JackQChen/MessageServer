using System;
using System.Text;

namespace SystemFramework
{
    /// <summary>
    /// 解决拼接SQL方法。
    /// 解决问题：用户录入值中，如果有特殊字符 ',*, %, [ 等，则程序报错，或查询不出数据，或过滤不出数据。
    /// </summary>
    public sealed class SqlSplice
    {
        /// <summary>
        /// 拼接查询语句模式
        /// </summary>
        /// <param name="IniText"></param>
        /// <returns></returns>
        public static SqlSplice AsSqlSplice(string IniText)
        {
            return new SqlSplice(IniText, false);
        }

        /// <summary>
        /// 拼接过滤语句模式
        /// </summary>
        /// <param name="IniText"></param>
        /// <returns></returns>
        public static SqlSplice AsRowFilter(string IniText)
        {
            return new SqlSplice(IniText, true);
        }

        public static SpliceParameter AsQueryParameter(object b)
        {
            return new SpliceParameter(b);
        }

        //字符串参数的处理进度
        private enum SPStep
        {
            New,		//开始或者已完成一次字符串参数的拼接。
            Para,	    //拼接时遇到一个单引号结束,下次传入的应该是参数值。
            End		    //处理结束
        }

        private StringBuilder _ResultText = new StringBuilder();

        //是否行过滤
        private bool _IsRowFilter;
        private SPStep _Step = SPStep.New;

        public SqlSplice(string IniText, bool IsRowFilter)
        {
            _ResultText.Append(IniText);
            _IsRowFilter = IsRowFilter;
        }

        public override string ToString()
        {
            return _ResultText.ToString();
        }

        private void AddSqlText(string AddText)
        {
            if (_Step == SPStep.New)
            {
                AddText = AddText.TrimEnd();
                if (AddText.IndexOf("'") >= 0)
                {
                    // 遇到一个单引号结束
                    _ResultText.Append(AddText);
                    _Step = SPStep.Para;
                }
                //if (AddText[AddText.Length - 1] == '\'')
                //{	
                //    // 遇到一个单引号结束
                //    _ResultText.Append(AddText);
                //    _Step = SPStep.Para;
                //}
                //else if (AddText[AddText.Length - 2] == '\'')
                //{	
                //    // 遇到一个单引号结束
                //    _ResultText.Append(AddText);
                //    _Step = SPStep.Para;
                //}
                else
                {
                    _ResultText.Append(AddText);
                }
            }
            else if (_Step == SPStep.Para)
            {
                // 此时的s应该是字符串参数，不是SQL语句的一部分
                // _Step 在AddParameter方法中统一修改，防止中途拼接非字符串数据。
                this.AddParameter(SqlSplice.AsQueryParameter(AddText));
            }
            else if (_Step == SPStep.End)
            {
                AddText = AddText.Trim();

                if (AddText.IndexOf("'") < 0)
                {
                    throw new ArgumentException("正在等待以单引号开始的字符串，但参数不符合预期格式。");
                }

                if (AddText.Substring(AddText.IndexOf("'") + 1).Length == 0)
                {
                    _Step = SPStep.New;
                }
                else
                {
                    _Step = SPStep.Para;
                }

                //if (AddText.Length == 1)
                //{
                //    if (AddText[0] != '\'')
                //    {
                //        throw new ArgumentException("正在等待以单引号开始的字符串，但参数不符合预期格式。");
                //    }
                //    _Step = SPStep.New;
                //}
                //else if (AddText.Length == 2)
                //{
                //    if (AddText[0] != '\'' && AddText[1] != '\'')
                //    {
                //        throw new ArgumentException("正在等待以单引号开始的字符串，但参数不符合预期格式。");
                //    }
                //    _Step = SPStep.New;
                //}
                //else
                //{
                //    if (AddText[0] != '\'' && AddText[1] != '\'')
                //    {
                //        throw new ArgumentException("正在等待以单引号开始的字符串，但参数不符合预期格式。");
                //    }
                //    _Step = SPStep.Para;
                //}

                // 找到单引号的闭合输入。
                _ResultText.Append(AddText);
            }
        }

        private void AddParameter(SpliceParameter ParameterValue)
        {
            if (_Step == SPStep.End)
            {
                throw new InvalidOperationException("正在等待以单引号开始的字符串，此时不允许再拼接其它参数。");
            }

            //替换参数值的特殊字符 *****
            string ReplaceText = ParameterValue.Value.ToString();
            ReplaceText = ReplaceText.Replace("'", "''");
            ReplaceText = ReplaceText.Replace("[", "[[]");

            if (_IsRowFilter)
            {
                ReplaceText = ReplaceText.Replace("%", "[%]");
                ReplaceText = ReplaceText.Replace("*", "[*]");
            }

            //处理结束
            _ResultText.Append(ReplaceText);
            if (_Step == SPStep.Para)
            {
                _Step = SPStep.End;
            }
        }


        public static SqlSplice operator +(SqlSplice Query, string AddText)
        {
            Query.AddSqlText(AddText);
            return Query;
        }

        public static SqlSplice operator +(SqlSplice Query, SpliceParameter ParameterValue)
        {
            Query.AddParameter(ParameterValue);
            return Query;
        }
    }

    public sealed class SpliceParameter
    {
        private object _val;

        public SpliceParameter(object val)
        {
            _val = val;
        }

        public object Value
        {
            get { return _val; }
        }

        public static explicit operator SpliceParameter(string a)
        {
            return new SpliceParameter(a);
        }

        public static implicit operator SpliceParameter(int a)
        {
            return new SpliceParameter(a);
        }

        public static implicit operator SpliceParameter(decimal a)
        {
            return new SpliceParameter(a);
        }

        public static implicit operator SpliceParameter(DateTime a)
        {
            return new SpliceParameter(a);
        }
        // 其它需要支持的隐式类型转换操作符重载请自行添加。
    }

}
