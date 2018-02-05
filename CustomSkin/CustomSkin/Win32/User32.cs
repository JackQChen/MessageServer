using System;
using System.Collections.Generic; 
using System.Runtime.InteropServices;
using System.Text;

namespace CustomSkin.Windows.Win32
{
    public static class User32
    {
        public class API
        {
            /// <summary>
            /// 让显示或隐藏窗口时，产生特殊的效果。 有四种类型的动画：滚动，滑动，折叠或展开，和阿尔法混合褪色。
            /// </summary>
            /// <param name="hwnd">一个要设置动画的窗口句柄，调用线程必须拥有该窗口。</param>
            /// <param name="dwTime">所花费的时间来播放动画，以毫秒为单位。 通常情况下，动画需要200毫秒的时间来发挥。</param>
            /// <param name="dwFlags">动画的类型。 这个参数可以是下列一个或多个值。 需要注意的是，默认情况下，这些标志生效呈现出窗口时。 生效隐藏窗口时，使用和AW_HIDE逻辑OR运算符使用合适的标志。 </param>
            /// <returns></returns>
            [DllImport("user32")]
            internal static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
            /// <summary>
            /// 设置了一个窗口的区域.只有被包含在这个区域内的地方才会被重绘,而不包含在区域内的其他区域系统将不会显示.
            /// </summary>
            /// <param name="hwnd">窗口的句柄</param>
            /// <param name="hRgn">指向的区域.函数起作用后将把窗体变成这个区域的形状.如果这个参数是空值,窗体区域也会被设置成空值,也就是什么也看不到.</param>
            /// <param name="bRedraw">这个参数是用于设置 当函数起作用后,窗体是不是该重绘一次. true 则重绘,false 则相反.如果你的窗体是可见的,通常建议设置为 true.</param>
            /// <returns>如果函数执行成功,就返回非零的数字.如果失败,就返回零.</returns>
            [DllImport("user32.dll")]
            internal static extern int SetWindowRgn(IntPtr hwnd, int hRgn, bool bRedraw);
            /// <summary>
            /// <para>该函数从当前线程中的窗口释放鼠标捕获，并恢复通常的鼠标输入处理。捕获鼠标的窗口接收所有</para>
            /// <para>的鼠标输入（无论光标的位置在哪里），除非点击鼠标键时，光标热点在另一个线程的窗口中。</para>
            /// </summary>
            [DllImport("user32.dll")]
            internal static extern bool ReleaseCapture();
            /// <summary>
            /// 指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。而和函数PostMessage不同，PostMessage是将一个消息寄送到一个线程的消息队列后就立即返回。
            /// </summary>
            /// <param name="hwnd"></param>
            /// <param name="msg"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessageA")]
            public static extern IntPtr SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        }
        /// <summary>
        /// 显示与隐藏窗口时能产生特殊的效果。有两种类型的动画效果：滚动动画和滑动动画。
        /// </summary>
        /// <param name="hwnd">指定产生动画的窗口的句柄。</param>
        /// <param name="time">指明动画持续的时间（以微秒计），完成一个动画的标准时间为200微秒。</param>
        /// <param name="flags">指定动画类型。这个参数可以是一个或多个下列标志的组合。</param>
        /// <returns></returns>
        public static bool AnimateWindow(this IntPtr hwnd, int time, AnimateWindowFlags flags)
        {
            return API.AnimateWindow(hwnd, time, (int)flags);
        }
        /// <summary>
        /// 设置了一个窗口的区域.只有被包含在这个区域内的地方才会被重绘,而不包含在区域内的其他区域系统将不会显示.
        /// </summary>
        /// <param name="hwnd">窗口的句柄</param>
        /// <param name="hRgn">指向的区域.函数起作用后将把窗体变成这个区域的形状.如果这个参数是空值,窗体区域也会被设置成空值,也就是什么也看不到.</param>
        /// <param name="bRedraw">这个参数是用于设置 当函数起作用后,窗体是不是该重绘一次. true 则重绘,false 则相反.如果你的窗体是可见的,通常建议设置为 true.</param>
        /// <returns>如果函数执行成功,就返回非零的数字.如果失败,就返回零.</returns>
        public static bool SetWindowRgn(this IntPtr hwnd, int hRgn, bool bRedraw)
        {
            return API.SetWindowRgn(hwnd, hRgn, bRedraw) != 0;
        }
        public static bool SetWindowRgn(this IntPtr hwnd, int left, int top, int right, int bottom, int widthEllipse, int heightEllipse)
        {
            return API.SetWindowRgn(hwnd, GDI32.API.CreateRoundRectRgn(left, top, right, bottom, widthEllipse, heightEllipse), true) != 0;
        }
        /// <summary>
        /// 设置了一个窗口的区域.只有被包含在这个区域内的地方才会被重绘,而不包含在区域内的其他区域系统将不会显示.
        /// </summary>
        /// <param name="hwnd">窗口的句柄</param>
        /// <param name="left">指定了x坐标的左上角区域逻辑单位。</param>
        /// <param name="top">指定了x坐标的左上角区域逻辑单位。</param>
        /// <param name="right">指定了x坐标的右下角区域逻辑单位。</param>
        /// <param name="bottom">指定了x坐标的右下角区域逻辑单位。</param>
        /// <param name="widthEllipse">指定创建圆角的宽度逻辑单位。</param>
        /// <param name="heightEllipse">指定创建圆角的高度逻辑单位。</param>
        /// <param name="bRedraw">这个参数是用于设置 当函数起作用后,窗体是不是该重绘一次. true 则重绘,false 则相反.如果你的窗体是可见的,通常建议设置为 true.</param>
        /// <returns>如果函数执行成功,就返回非零的数字.如果失败,就返回零.</returns>
        public static bool SetWindowRgn(this IntPtr hwnd, int left, int top, int right, int bottom, int widthEllipse, int heightEllipse, bool bRedraw)
        {
            return API.SetWindowRgn(hwnd, GDI32.API.CreateRoundRectRgn(left, top, right, bottom, widthEllipse, heightEllipse), bRedraw) != 0;
        }
        /// <summary>
        /// 从当前线程中的窗口释放鼠标捕获，并恢复通常的鼠标输入处理。捕获鼠标的窗口接收所有的鼠标输入（无论光标的位置在哪里），除非点击鼠标键时，光标热点在另一个线程的窗口中。
        /// </summary>
        /// <returns></returns>
        public static bool ReleaseCapture()
        {
            return API.ReleaseCapture();
        }
        /// <summary>
        /// 将指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。而和函数PostMessage不同，PostMessage是将一个消息寄送到一个线程的消息队列后就立即返回。
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口。</param>
        /// <param name="msg">指定被发送的消息。</param>
        /// <param name="wParam">指定附加的消息特定信息。</param>
        /// <param name="lParam">指定附加的消息特定信息。</param>
        /// <returns>返回值指定消息处理的结果，依赖于所发送的消息。</returns>
        public static IntPtr SendMessage(this IntPtr hwnd, int msg, int wParam, int lParam)
        {
            return API.SendMessage(hwnd, msg, wParam, lParam);

        }
        public static int SetClassLong(this IntPtr hwnd, int nIndex, int dwNewLong)
        {
            return API.SetClassLong(hwnd, nIndex, dwNewLong);
        }
    }
}
