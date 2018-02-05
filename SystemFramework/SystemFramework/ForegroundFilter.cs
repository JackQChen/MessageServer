using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SystemFramework
{
    public class ForegroundFilter
    {
        public ForegroundFilter()
        {
        }

        /// <summary>
        /// 前台进行过滤
        /// </summary>
        /// <param name="OldDataTable"></param>
        /// <param name="QueryString"></param>
        /// <returns></returns>
        public static DataTable Filter(DataTable OldDataTable, string QueryString)
        { 
            if (OldDataTable != null)
            {
                DataView dv = OldDataTable.DefaultView;
                dv.RowFilter = QueryString.Replace("[", "").Replace("]", "");
                return dv.ToTable();
            }
            else
                return null;
        }

    }
}
