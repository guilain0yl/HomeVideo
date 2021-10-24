using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Util
* 项目描述 ：
* 类 名 称 ：ImageHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/29 10:05:28
* 更新时间 ：2019/10/29 10:05:28
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public class ImageHelper
    {
        /// <summary>
        /// 缩小或放大图片
        /// </summary>
        /// <param name="picPath">图片地址</param>
        /// <param name="targetWidth">目标宽</param>
        /// <param name="targetHeight">目标高</param>
        /// <param name="targetPath">目的地址</param>
        /// <returns></returns>
        public static bool ResizeImage(string picPath, int targetWidth, int targetHeight, string targetPath) => ResizeImage(new Bitmap(picPath), targetWidth, targetHeight, targetPath);

        /// <summary>
        /// 缩小或放大图片
        /// </summary>
        /// <param name="sourceStream">源流</param>
        /// <param name="targetWidth">目标宽</param>
        /// <param name="targetHeight">目标高</param>
        /// <param name="targetPath">目的地址</param>
        /// <returns></returns>
        public static bool ResizeImage(Stream sourceStream, int targetWidth, int targetHeight, string targetPath) => ResizeImage(new Bitmap(sourceStream), targetWidth, targetHeight, targetPath);

        private static bool ResizeImage(Image sourceImage, int targetWidth, int targetHeight, string targetPath)
        {
            Image resultImage = null;
            try
            {
                resultImage = new Bitmap(targetWidth, targetHeight);
                using (Graphics g = Graphics.FromImage(resultImage))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(sourceImage, new Rectangle(0, 0, targetWidth, targetHeight), new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
                    g.Dispose();
                }

                resultImage.Save(targetPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sourceImage?.Dispose();
                resultImage?.Dispose();
            }
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">验证码</param>
        /// <returns>二进制数据流</returns>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                // 生成随机生成器
                Random random = new Random();

                // 清空图片背景色
                g.Clear(Color.White);

                // 画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);
                LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);

                // 画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                // 画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                // 保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                // 输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="str">内容字符串</param>
        /// <returns></returns>
        public static byte[] GenerateQrcode(string str)
        {
            Bitmap qrCodeImage = null;
            QRCodeGenerator qRCodeGenerator = null;
            QRCodeData qRCodeData = null;
            QRCode qrCode = null;
            try
            {
                qRCodeGenerator = new QRCodeGenerator();
                qRCodeData = qRCodeGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
                qrCode = new QRCode(qRCodeData);
                qrCodeImage = qrCode.GetGraphic(20);

                // 保存图片数据
                MemoryStream stream = new MemoryStream();
                qrCodeImage.Save(stream, ImageFormat.Jpeg);

                // 输出图片流
                return stream.ToArray();
            }
            catch (Exception)
            {
            }
            finally
            {
                qrCode?.Dispose();
                qRCodeData?.Dispose();
                qRCodeGenerator?.Dispose();
                qrCodeImage?.Dispose();
            }

            return null;
        }

        /// <summary>
        /// 生成包含文字的二维码图片
        /// </summary>
        /// <param name="str">内容字符串</param>
        /// <param name="str">下标题</param>
        /// <returns></returns>
        public static byte[] GenerateQrcode(string str, string title)
        {
            Bitmap qrCodeImage = null;
            QRCodeGenerator qRCodeGenerator = null;
            QRCodeData qRCodeData = null;
            QRCode qrCode = null;
            MemoryStream stream = null;
            Font font = null;
            SolidBrush brush = null;
            StringFormat format = null;
            Bitmap image = null;

            try
            {
                qRCodeGenerator = new QRCodeGenerator();
                qRCodeData = qRCodeGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
                qrCode = new QRCode(qRCodeData);
                qrCodeImage = qrCode.GetGraphic(20);

                font = new Font("宋体", 18, FontStyle.Bold);
                brush = new SolidBrush(Color.Black);
                format = new StringFormat(StringFormatFlags.NoClip);

                int fontWidth = 0;
                int fontHeight = 0;

                Graphics g1 = Graphics.FromImage(qrCodeImage);
                SizeF sizeF = g1.MeasureString(title, font, PointF.Empty, format);
                fontWidth = ConvertToInt(sizeF.Width);
                fontHeight = ConvertToInt(sizeF.Height);
                g1.Dispose();

                image = new Bitmap(Math.Max(fontWidth, qrCodeImage.Width), qrCodeImage.Height + fontHeight);
                Graphics g = Graphics.FromImage(image);
                g.Clear(Color.White);
                g.DrawImage(qrCodeImage, 0, 0);
                g.DrawString(title, font, brush, new PointF((image.Width - fontWidth) / 2, qrCodeImage.Height));
                g.Dispose();

                // 保存图片数据
                stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                // 输出图片流
                return stream.ToArray();
            }
            catch (Exception)
            {
            }
            finally
            {
                qrCode?.Dispose();
                qRCodeData?.Dispose();
                qRCodeGenerator?.Dispose();
                qrCodeImage?.Dispose();
                stream?.Dispose();
                font?.Dispose();
                brush?.Dispose();
                format?.Dispose();
                image.Dispose();
            }

            return null;
        }

        /// <summary>
        /// 图片拼接
        /// </summary>
        /// <param name="srcImage">原始图片</param>
        /// <param name="splitImage">需要被拼接的图片</param>
        /// <param name="splitX">拼接图片X坐标</param>
        /// <param name="splitY">拼接图片Y坐标</param>
        /// <param name="imageFormat">生成目标图片格式</param>
        /// <returns></returns>
        public static byte[] ImageSplicing(byte[] srcImage, byte[] splitImage, int splitX, int splitY, ImageFormat imageFormat)
        {
            MemoryStream ms = null;
            Image srcImg = null;
            Image splitImg = null;
            Bitmap bitmap = null;
            Graphics graphics = null;

            try
            {
                srcImg = ByteToImage(srcImage);
                if (srcImg.Width < (splitImg.Width + splitX) || srcImg.Height < (splitImg.Height + splitY)) throw new Exception("");
                splitImg = ByteToImage(splitImage);
                graphics = Graphics.FromImage(srcImg);
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.DrawImage(splitImg, new RectangleF(0, 0, splitX, splitY), new RectangleF(0, 0, splitImg.Width, splitImg.Height), GraphicsUnit.Pixel);
                ms.Dispose();
                ms = new MemoryStream();
                bitmap.Save(ms, imageFormat);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ms.Dispose();
                srcImg.Dispose();
                splitImg.Dispose();
                bitmap.Dispose();
                graphics.Dispose();
            }
        }

        static Image ByteToImage(byte[] image) => Image.FromStream(new MemoryStream(image));

        static int ConvertToInt(float num)
        {
            int result = (int)num;

            if ((num - result) > 0f)
            {
                result += 1;
            }

            return result;
        }
    }
}
