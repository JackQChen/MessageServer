
namespace CustomSkin.Windows.Forms
{
    /// <summary>
    /// 发送到一个窗口，以确定鼠标在窗口的哪一部分，对应于一个特定的屏幕坐标
    /// </summary>
    public enum WM_NCHITTEST : int
    {
        /// <summary>
        /// 在屏幕背景或窗口之间的分界线
        /// </summary>
        HTERROR = -2,
        /// <summary>
        /// 在目前一个窗口，其他窗口覆盖在同一个线程
        /// （该消息将被发送到相关窗口在同一个线程，直到其中一个返回一个代码，是不是HTTRANSPARENT）
        /// </summary>
        HTTRANSPARENT = -1,
        /// <summary>
        /// 在屏幕背景或窗口之间的分界线上
        /// </summary>
        HTNOWHERE = 0,
        /// <summary>
        /// 在客户端区域
        /// </summary>
        HTCLIENT = 1,
        /// <summary>
        /// 在标题栏
        /// </summary>
        HTCAPTION = 2,
        /// <summary>
        /// 在窗口菜单中，或在一个子窗口的关闭按钮
        /// </summary>
        HTSYSMENU = 3,
        /// <summary>
        /// 在大小框（与HTGROWBO相同）
        /// </summary>
        HTSIZE = 4,
        /// <summary>
        /// 在大小框（与HTSIZE相同）
        /// </summary>
        HTGROWBOX = 4,
        /// <summary>
        /// 在一个菜单
        /// </summary>
        HTMENU = 5,
        /// <summary>
        /// 在水平滚动条
        /// </summary>
        HTHSCROLL = 6,
        /// <summary>
        /// 在垂直滚动条
        /// </summary>
        HTVSCROLL = 7,
        /// <summary>
        /// 在最小化按钮
        /// </summary>
        HTREDUCE = 8,
        /// <summary>
        /// 在最小化按钮
        /// </summary>
        HTMINBUTTON = 8,
        /// <summary>
        /// 在最大化按钮
        /// </summary>
        HTMAXBUTTON = 9,
        /// <summary>
        /// 在最大化按钮
        /// </summary>
        HTZOOM = 9,
        /// <summary>
        /// 在左边框可调整大小的窗口
        /// </summary>
        HTLEFT = 10,
        /// <summary>
        /// 在一个可调整大小的窗口的右边框
        /// </summary>
        HTRIGHT = 11,
        /// <summary>
        /// 在窗口的上边框水平线上
        /// </summary>
        HTTOP = 12,
        /// <summary>
        /// 在窗口的左上边框
        /// </summary>
        HTTOPLEFT = 13,
        /// <summary>
        /// 在窗口的右上边框
        /// </summary>
        HTTOPRIGHT = 14,
        /// <summary>
        /// （用户可以在较低的水平边界可调整大小的窗口单击鼠标，改变窗口的垂直大小）
        /// </summary>
        HTBOTTOM = 15,
        /// <summary>
        /// 在左下角的边框可调整大小的窗口（用户可以通过点击鼠标来调整窗口的大小，对角）
        /// </summary>
        HTBOTTOMLEFT = 16,
        /// <summary>
        /// 在右下角的边框可调整大小的窗口（用户可以通过点击鼠标来调整窗口的大小，对角）
        /// </summary>
        HTBOTTOMRIGHT = 17,
        /// <summary>
        /// 在一个不具有缩放边框的窗口
        /// </summary>
        HTBORDER = 18,
        /// <summary>
        /// 在关闭按钮
        /// </summary>
        HTCLOSE = 20,
        /// <summary>
        /// 在帮助按钮
        /// </summary>
        HTHELP = 21,
    }
}
