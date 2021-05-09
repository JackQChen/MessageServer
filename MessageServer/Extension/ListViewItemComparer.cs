using System;
using System.Collections;
using System.Windows.Forms;

namespace MessageServer.Extension
{
    class ListViewItemComparer : IComparer
    {
        public ListViewItemComparer()
        {
        }

        public int Compare(object x, object y)
        {
            return Convert.ToInt32(((ListViewItem)x).Name).CompareTo(Convert.ToInt32(((ListViewItem)y).Name));
        }
    }
}
