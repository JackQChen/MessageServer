using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SystemFramework
{
    public class ImageHelper
    {

        #region Compress
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="dHeight"></param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns> 
        public static Image Compress(Image iSource, int dWidth, int dHeight, int flag)
        {
            Image iSourceCopy = iSource.Clone() as Image;
            ImageFormat tFormat = iSourceCopy.RawFormat;
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSourceCopy, new Rectangle(0, 0, dWidth, dHeight), 0, 0, iSourceCopy.Width, iSourceCopy.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量 
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100 
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                MemoryStream ms = new MemoryStream();
                if (jpegICIinfo != null)
                    ob.Save(ms, jpegICIinfo, ep);
                else
                    ob.Save(ms, tFormat);
                return Image.FromStream(ms);
            }
            catch
            {
                return null;
            }
            finally
            {
                iSourceCopy.Dispose();
                ob.Dispose();
            }
        }
        #endregion

        public static byte[] ImageToData(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        public static byte[] ImageToDataCompress(Image img)
        {
            Image imgCompress = Compress(img, img.Width, img.Height, 100);
            byte[] bt1 = null, bt2 = null;
            using (MemoryStream ms1 = new MemoryStream())
            {
                imgCompress.Save(ms1, imgCompress.RawFormat);
                bt1 = ms1.ToArray();
            }
            using (MemoryStream ms2 = new MemoryStream())
            {
                img.Save(ms2, img.RawFormat);
                bt2 = ms2.ToArray();
            }
            //压缩成功
            if (bt1.Length < bt2.Length)
                return bt1;
            //压缩失败
            else
                return bt2;
        }

        public static Image DataToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                return Image.FromStream(ms);
            }
        }
    }
}
