using System.Drawing;

namespace CustomSkin.Drawing
{
    public static class GraphicsHelp
    {
        /// <summary>
        /// 在指定位置并且按指定大小绘制指定的 System.Drawing.Image。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image">要绘制的 System.Drawing.Image。</param>
        /// <param name="rect">System.Drawing.Rectangle 结构，它指定所绘制图像的位置和大小。</param>
        public static void DrawImage(this System.Drawing.Graphics g, Image image, Rectangle rect)
        {
            g.DrawImage(image, rect);
        }
        /// <summary>
        /// 在指定位置并且按指定大小绘制指定的 System.Drawing.Image 的指定部分。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image">要绘制的 System.Drawing.Image。</param>
        /// <param name="destRect">System.Drawing.Rectangle 结构，它指定所绘制图像的位置和大小。 将图像进行缩放以适合该矩形。</param>
        /// <param name="srcX">要绘制的源图像部分的左上角的 x 坐标。</param>
        /// <param name="srcY">要绘制的源图像部分的左上角的 y 坐标。</param>
        /// <param name="srcWidth">要绘制的源图像部分的宽度。</param>
        /// <param name="srcHeight">要绘制的源图像部分的高度。</param>
        /// <param name="srcUnit">System.Drawing.GraphicsUnit 枚举的成员，它指定用于确定源矩形的度量单位。</param>
        public static void DrawImage(this System.Drawing.Graphics g, Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel);
        }
        /// <summary>
        /// 在指定位置并且按指定大小绘制指定的 System.Drawing.Image 的指定部分。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image">要绘制的 System.Drawing.Image。</param>
        /// <param name="x">矩形左上角的 x 坐标。</param>
        /// <param name="y">矩形左上角的 y 坐标。</param>
        /// <param name="width">矩形的宽度。</param>
        /// <param name="height">矩形的高度。</param>
        /// <param name="srcX">要绘制的源图像部分的左上角的 x 坐标。</param>
        /// <param name="srcY">要绘制的源图像部分的左上角的 y 坐标。</param>
        /// <param name="srcWidth">要绘制的源图像部分的宽度。</param>
        /// <param name="srcHeight">要绘制的源图像部分的高度。</param>
        /// <param name="srcUnit">System.Drawing.GraphicsUnit 枚举的成员，它指定用于确定源矩形的度量单位。</param>
        public static void DrawImage(this System.Drawing.Graphics g, Image image, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            g.DrawImage(image, new Rectangle(x, y, width, height), srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel);
        }
        public static void RendererBackground(this Graphics g, Rectangle rect, Image backgroundImage, bool method)
        {
            if (!method)
            {
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 0, rect.Y, 5, rect.Height), 0, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 5, rect.Y, rect.Width - 10, rect.Height), 5, 0, backgroundImage.Width - 10, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - 5, rect.Y, 5, rect.Height), backgroundImage.Width - 5, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
            }
            else
            {
                g.RendererBackground(rect, 5, backgroundImage);
            }
        }
        /// <summary>
        /// 渲染背景图片,使背景图片不失真
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cut"></param>
        /// <param name="backgroundImage"></param>
        public static void RendererBackground(this Graphics g, Rectangle rect, int cut, Image backgroundImage)
        {
            //左上角 
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y, cut, cut), 0, 0, cut, cut, GraphicsUnit.Pixel);
            //上边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y, rect.Width - cut * 2, cut), cut, 0, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //右上角
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y, cut, cut), backgroundImage.Width - cut, 0, cut, cut, GraphicsUnit.Pixel);
            //左边
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + cut, cut, rect.Height - cut * 2), 0, cut, cut, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //左下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + rect.Height - cut, cut, cut), 0, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //右边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y + cut, cut, rect.Height - cut * 2), backgroundImage.Width - cut, cut, cut, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //右下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y + rect.Height - cut, cut, cut), backgroundImage.Width - cut, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //下边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y + rect.Height - cut, rect.Width - cut * 2, cut), cut, backgroundImage.Height - cut, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //平铺中间
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y + cut, rect.Width - cut * 2, rect.Height - cut * 2), cut, cut, backgroundImage.Width - cut * 2, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
        }

    }
}
