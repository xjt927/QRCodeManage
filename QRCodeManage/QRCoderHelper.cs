/******************************************************************************** 
** Copyright(c) 2015  All Rights Reserved. 
** auth： 薛江涛 
** mail： xjt927@126.com 
** date： 2015/8/19 14:02:36 
** desc： 尚未编写描述 
** Ver :  V1.0.0 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace QRCodeManage
{
    public class QRCoderHelper
    {
        ////Asp生成二维码
        //public void ProcessRequest(HttpContext context)
        //{

        //    String data = context.Request["CodeText"];
        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        //        qrCodeEncoder.QRCodeScale = 4;
        //        qrCodeEncoder.QRCodeVersion = 8;
        //        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        //        System.Drawing.Image image = qrCodeEncoder.Encode(data);
        //        System.IO.MemoryStream MStream = new System.IO.MemoryStream();
        //        image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);

        //        System.IO.MemoryStream MStream1 = new System.IO.MemoryStream();
        //        CombinImage(image, context.Server.MapPath("~/images/201292891051540.jpg"))
        //            .Save(MStream1, System.Drawing.Imaging.ImageFormat.Png);
        //        context.Response.ClearContent();
        //        context.Response.ContentType = "image/png";
        //        context.Response.BinaryWrite(MStream1.ToArray());

        //        //image.Dispose();  
        //        MStream.Dispose();
        //        MStream1.Dispose();
        //    }

        //    context.Response.Flush();
        //    context.Response.End();
        //}


        //程序路径    
        //string currentPath = Application.StartupPath + @"\BarCode_Images"; 

        readonly string currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\BarCode_Images";

        /// <summary>  
        /// 批量生成二维码图片  
        /// </summary>  
        public void Create_CodeImages(List<Tuple<string, string>> qrCantentList, int imgSize)
        {
            try
            {
                string backPath = "";
                if (qrCantentList != null)
                {
                    if (qrCantentList.Any())
                    {
                        //清空目录  
                        DeleteDir(currentPath);
                        foreach (Tuple<string, string> item in qrCantentList)
                        {
                            if (item != null)
                            {

                                //    string encrypRsult = SymmetricMethod.Instance.Encrypto(qrContent);
                                //string decryptoRsult = SymmetricMethod.Instance.Decrypto(encrypRsult);
                                string encrypRsult = item.Item1;
                                //string encodingResult = HttpUtility.UrlEncode(encrypRsult);// Encoding
                                //encrypRsult = @"http://hl.hlshihua.com/web/saomafu/saomafu.aspx?qrParam=" + encodingResult;

                                //生成图片   
                                Image image = Create_ImgCode(encrypRsult, imgSize);
                                ////二维码图片中间加logo
                                //image = CombinImage(image, logoPath) as Bitmap;
                                ////合成带背景的图片
                                //image = CombinImage(backImage, image) as Bitmap;
                                //保存图片  
                                SaveImg(currentPath, image, item.Item2);
                                //添加水印文字
                                //ImageWaterMark.addWaterMark(currentPath + "\\" + qrName + "1.png", currentPath + "\\" + qrName + ".png", WaterMarkHelper.WaterMarkType.TextMark, WaterMarkHelper.WaterMarkPosition.WM_TOP_Center, qrName);
                                ImageWaterMark.SetPicDescription(currentPath + "\\" + item.Item2 + "1.png", currentPath + "\\" + item.Item2 + ".png", item.Item2);
                                MainWindow.LogAction("成功创建二维码：" + item.Item2);
                            }
                        }
                        ////打开文件夹  
                        //Open_File(currentPath);
                        qrCantentList = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow.LogAction(ex.ToString());
            }
        }

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeNumber">要生成二维码的字符串</param>       
        /// <param name="size">大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        Image Create_ImgCode(string codeNumber, int size)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片  
            //System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber);
            //GC.Collect();
            //return image;

            var pbImg = qrCodeEncoder.Encode(codeNumber, System.Text.Encoding.UTF8);
            var width = pbImg.Width / 10;
            var dwidth = width * 2;
            Bitmap bmp = new Bitmap(pbImg.Width + dwidth, pbImg.Height + dwidth);
            Graphics g = Graphics.FromImage(bmp);
            var c = System.Drawing.Color.White;
            g.FillRectangle(new SolidBrush(c), 0, 0, pbImg.Width + dwidth, pbImg.Height + dwidth);
            g.DrawImage(pbImg, width, width);
            g.Dispose();
            return bmp;

        }


        /// <summary>  
        /// 保存图片  
        /// </summary>  
        /// <param name="strPath">保存路径</param>  
        /// <param name="img">图片</param>  
        void SaveImg(string strPath, Image img, string qrContent)
        {
            //保存图片到目录  
            if (Directory.Exists(strPath))
            {
                Save(strPath, img, qrContent);
            }
            else
            {
                //当前目录不存在，则创建  
                Directory.CreateDirectory(strPath);
                Save(strPath, img, qrContent);
            }
            GC.Collect();
        }

        void Save(string strPath, Image img, string qrContent)
        {

            //文件名称  
            string guid = Guid.NewGuid().ToString().Replace("-", "") + "1.png";
            guid = qrContent + "1.png";
            img.Save(strPath + "/" + guid, System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>  
        /// 删除目录下所有文件  
        /// </summary>  
        /// <param name="aimPath">路径</param>  
        void DeleteDir(string aimPath)
        {
            try
            {
                //目录是否存在  
                if (Directory.Exists(aimPath))
                {
                    // 检查目标目录是否以目录分割字符结束如果不是则添加之  
                    if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                        aimPath += Path.DirectorySeparatorChar;
                    // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组  
                    // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法  
                    string[] fileList = Directory.GetFiles(aimPath);
                    //string[] fileList = Directory.GetFileSystemEntries(aimPath);  
                    // 遍历所有的文件和目录  
                    foreach (string file in fileList)
                    {
                        // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件  
                        if (Directory.Exists(file))
                        {
                            DeleteDir(aimPath + Path.GetFileName(file));
                        }
                        // 否则直接Delete文件  
                        else
                        {
                            File.Delete(aimPath + Path.GetFileName(file));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //   LogHelper.WriteErrorLogShow(ex.ToString(), ex);
            }
        }

        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>    
        /// <param name="imgBack">粘贴的源图片</param>    
        /// <param name="logoImg">粘贴的目标图片</param>    
        static Image CombinImage(Image imgBack, string logoImg)
        {
            Image img = Image.FromFile(logoImg); //照片图片      
            int logoWidth = 165;
            int logoHight = 165;
            if (imgBack.Width == 862)
            {
                logoWidth = 230;
                logoHight = 230;
            }

            if (img.Height != logoHight || img.Width != logoWidth)
            {
                img = KiResizeImage(img, logoWidth, logoHight, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>    
        /// <param name="imgBack">粘贴的源图片</param>    
        /// <param name="img">粘贴的目标图片</param>    
        static Image CombinImage(Image imgBack, Image img)
        {
            int height = 0;
            ////指定图片大小
            //if (img.Height != 193 || img.Width != 193)
            //{
            //    img = KiResizeImage(img, 193, 193, 0);
            //}

            if (imgBack.Width != 1772)
            {
                height = 85;
            }
            else
            {
                height = 60;
            }
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   
            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    
            //g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, (imgBack.Height / 2 - img.Height / 2) - height, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        /// <summary>    
        /// Resize图片    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度</param>    
        /// <param name="Mode">保留着，暂时未用</param>    
        /// <returns>处理以后的图片</returns>    
        static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                    GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
