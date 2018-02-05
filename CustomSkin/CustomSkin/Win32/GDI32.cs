using System;
using System.Collections.Generic; 
using System.Runtime.InteropServices;
using System.Text;

namespace CustomSkin.Windows.Win32
{
    public static class GDI32
    {
        public class API
        {
            /// <summary>
            /// 创建的一个带圆角的矩形区域。
            /// </summary>
            /// <param name="left">指定了x坐标的左上角区域逻辑单位。</param>
            /// <param name="top">指定了x坐标的左上角区域逻辑单位。</param>
            /// <param name="right">指定了x坐标的右下角区域逻辑单位。</param>
            /// <param name="bottom">指定了x坐标的右下角区域逻辑单位。</param>
            /// <param name="widthEllipse">指定创建圆角的宽度逻辑单位。</param>
            /// <param name="heightEllipse">指定创建圆角的高度逻辑单位。</param>
            /// <returns>如果函数成功，返回该区域的句柄。</returns>
            [DllImport("gdi32.dll")]
            public static extern int CreateRoundRectRgn(int left, int top, int right, int bottom, int widthEllipse, int heightEllipse);
        }

    }
}
