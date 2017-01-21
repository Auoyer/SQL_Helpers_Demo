using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BarcodeLib;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.IO;


namespace Utils
{
    /// <summary>
    /// 条形码/二维码帮助类
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="Code"></param>
        public static void CreateBarCode(string Code, string fileName)
        {
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.CENTER,
                Width = 300,
                Height = 100,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };

            System.Drawing.Image img = barcode.Encode(TYPE.CODE128B, Code);
            img.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
                //img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //HttpContext.Current.Response.ClearContent();
                //HttpContext.Current.Response.ContentType = "image/png";
                //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //}
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="data">二维码内容</param>
        /// <param name="filename">生成的图片路径名称（如：D:\123.png）</param>
        /// <param name="moduleSize">图片大小</param>
        /// <returns></returns>
        public static void CreateQRCode(string data, string filename, int moduleSize = 5)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            MemoryStream ms = new MemoryStream();
            qrEncoder.TryEncode(data, out qrCode);

            //GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two));
            //renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            //System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            //img.Save(filename, System.Drawing.Imaging.ImageFormat.Png);

            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
            }
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        /// <param name="logPath"></param>
        /// <param name="moduleSize"></param>
        public static void CreateQRCodeHasLog(string data, string filename, string logPath, int moduleSize = 5)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(data);
            //保存成png文件
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
            Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
            Graphics g = Graphics.FromImage(map);
            render.Draw(g, qrCode.Matrix);
            //追加Logo图片 ,注意控制Logo图片大小和二维码大小的比例
            System.Drawing.Image img = System.Drawing.Image.FromFile(logPath);
            Point imgPoint = new Point((map.Width - img.Width) / 2, (map.Height - img.Height) / 2);
            g.DrawImage(img, imgPoint.X, imgPoint.Y, img.Width, img.Height);
            map.Save(filename, ImageFormat.Png);
        }
    }
}
