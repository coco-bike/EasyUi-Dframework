using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
   public class ImgHelper
    {

        //base64编码的文本 转为图片
        public static Tuple<bool, string> Base64StringToImage(string base64File, string relativePath,HttpContextBase http)
        {
            try
            {
                //string byteStr = Request["getDataURL"].ToString();//data:image/jpg;base64,         
                string byteStr = base64File;
                int delLength = byteStr.IndexOf(',') + 1;
                string str = byteStr.Substring(delLength, byteStr.Length - delLength);

                byte[] arr = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                Image returnImage = bmp;
                string noRepeat = string.Format("{0}{1}{2}{3}{4}{5}{6}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                string absolutePath =http.Server.MapPath(relativePath);
                if (!Directory.Exists(absolutePath))
                {
                    Directory.CreateDirectory(absolutePath);
                }
                string fileName = noRepeat + ".jpg";
                returnImage.Save(absolutePath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                return new Tuple<bool, string>(true, fileName);
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(true, null);
            }
        }
    }
}
