using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SupplierPlatform.Extensions;

namespace SupplierPlatform.Controllers
{
    public class CaptchaController : BaseController
    {
        public ActionResult GetCaptcha()
        {
            string code = this.CreateCAPTCHACode(4);
            HttpCookie CaptchaCookie = new HttpCookie("SupplierPlatform");
            CaptchaCookie.Expires = DateTime.Now.AddMinutes(5);
            // Hash Code
            string Salted = "lutrrasadf1234zxcv";
            string hashValue = code + Salted;
            CaptchaCookie.Values.Add("captcha",  hashValue.SHA256());

            this.Response.Cookies.Add(CaptchaCookie);

            HttpCookie requestCookie = this.Request.Cookies["SupplierPlatform"];

            if(requestCookie == null)
            {
                this.Response.Cookies.Add(CaptchaCookie);
            }

            byte[] bytes = this.CreateCAPTCHAGraphic(code);
            return this.File(bytes, @"image/jpeg");
        }

        /// <summary>產生驗證碼</summary>
        /// <param name="length">驗證碼長度</param>
        /// <returns></returns>
        private string CreateCAPTCHACode(int length)
        {
            Regex regex = new Regex(@"^(?=.*\d)"); // 只有數字
            Random rand = new Random(Environment.TickCount);

            char[] allowChar = { };

            allowChar = ConfigurationManager.AppSettings["Allow_Char"].ToCharArray();

            if (allowChar.Length == 0)
            {
                allowChar = new char[] { '0', '0', '0', '0' };
            }

            var code = new char[length];
            do
            {
                for (int i = 0; i < length; i++)
                {
                    code[i] = allowChar[rand.Next(0, allowChar.Length - 1)];
                }
            } while (!regex.IsMatch(new string(code)));

            return new string(code);
        }


        /// <summary>建立驗證碼圖片</summary>
        /// <param name="CAPTCHACode">要输出到的page对象</param>
        private byte[] CreateCAPTCHAGraphic(string validateNum)
        {
            decimal iFontSize = 52;

            decimal iImgWidth = validateNum.Length * iFontSize * 2;

            decimal iImgHeight = 130;

            Bitmap _Image = new Bitmap((int)Math.Ceiling(iImgWidth), (int)Math.Ceiling(iImgHeight));

            try
            {
                Graphics _Graphics = Graphics.FromImage(_Image);
                try
                {
                    Random random = new Random(Guid.NewGuid().GetHashCode());

                    //清空背景色(白色底色)
                    _Graphics.Clear(Color.White);

                    //SCM-2016071103 SCM滲透測試服務初檢改善：低安全性的CAPTCHA, add
                    char[] arrNum = validateNum.Trim().ToCharArray();
                    string[] arrFamily = new string[] { "Arial", "Verdana", "Times New Roman", "Comic Sans MS", "Courier New" };
                    float xCurr = 1;
                    decimal distance = (_Image.Width - 2 * 3) / arrNum.Length;

                    //繪圖樣式
                    LinearGradientBrush _Brush = new LinearGradientBrush(
                        new Rectangle(0, 0, _Image.Width, _Image.Height),
                        Color.Brown, Color.FromArgb(random.Next()),
                        60f,
                        true);

                    for (int i = 0; i < validateNum.Length; i++)
                    {
                        Pen _Pen = new Pen(Color.FromArgb(random.Next(50, 100), random.Next(50, 100), random.Next(50, 100)), 10);
                        GraphicsPath _gp = new GraphicsPath();

                        _gp.AddString(arrNum[i].ToString(), new FontFamily(arrFamily[random.Next(arrFamily.Length - 1)]), (int)FontStyle.Italic, random.Next(96, 108), new RectangleF(xCurr, 0, _Image.Width, _Image.Height), null);
                        _Graphics.DrawPath(_Pen, _gp);
                        _Graphics.FillPath(Brushes.LightGray, _gp);
                        _Pen.Dispose();
                        _gp.Dispose();

                        //xCurr += (float)distance - 5 ;
                        //每個字的緊靠程度-越多代表越緊湊
                        //改了這個值  RectangleF cloneRect = new RectangleF(0, 0, validateNum.Length * 100, _Image.Height); 這個也要跟著改
                        xCurr += (float)distance - 20;
                        //_Font.Dispose();
                    }
                    _Brush.Dispose();

                    //畫背景線條：預設25、Pen(Color.Black, 2);
                    //for (int i = 0; i < 25; i++)
                    for (int i = 0; i < 15; i++)
                    {
                        int x1 = random.Next(_Image.Width - 3);
                        int x2 = random.Next(_Image.Width - 3);
                        int y1 = random.Next(_Image.Height - 3);
                        int y2 = random.Next(_Image.Height - 3);

                        //SCM-2016071103 SCM滲透測試服務初檢改善：低安全性的CAPTCHA, upd
                        //Pen _Pen = new Pen(Color.FromArgb(random.Next()), 1);
                        float f = random.Next(2, 4);
                        //float f = 1;
                        Pen _Pen = new Pen(Color.FromArgb(random.Next()), f);
                        _Graphics.DrawLine(_Pen, x1, y1, x2, y2);
                    }

                    //繪出:雜訊點
                    for (int i = 0; i < 300; i++)
                    {
                        int x = random.Next(_Image.Width);
                        int y = random.Next(_Image.Height);
                        int z = 3; // 點的大小

                        Pen _Pen2 = new Pen(Color.LightGray, z + 1);
                        Pen _Pen3 = new Pen(Color.White, z);
                        if (i % 2 == 0)
                        {
                            _Graphics.DrawEllipse(_Pen2, x, y, z + 1, z + 1);
                        }
                        _Graphics.DrawEllipse(_Pen3, random.Next(_Image.Width), random.Next(_Image.Height), z, z);
                    }

                    //繪出:圖片的外框 
                    //_Graphics.DrawRectangle(new Pen(Color.Gray,5), 0, 0, _Image.Width - 1, _Image.Height - 1);

                    //100代表帆布的大小，越小看的到的字越少
                    RectangleF cloneRect = new RectangleF(0, 0, validateNum.Length * 100, _Image.Height);
                    System.Drawing.Imaging.PixelFormat format = _Image.PixelFormat;
                    Bitmap cloneBitmap = _Image.Clone(cloneRect, format);

                    //繪出:將繪圖完成資料寫入(格式為JPEG)
                    MemoryStream stream = new MemoryStream();
                    //_Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    cloneBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                    long imgLen = stream.Length;
                    byte[] bytes = new byte[imgLen];

                    bytes = stream.GetBuffer();

                    cloneBitmap.Dispose();

                    return bytes;
                }
                finally
                {
                    _Graphics.Dispose();
                }
            }
            finally
            {
                _Image.Dispose();
            }
        }
    }
}
