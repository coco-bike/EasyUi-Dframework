using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DirectoryHelper
    {
        public static bool CreateDirectory(string folderPath)
        {
            DirectoryInfo dir = null;
            if (Directory.Exists(folderPath) == false)
            {
                dir = Directory.CreateDirectory(folderPath);
            }
            else
            {
                return true;
            }
            if (dir.Exists)//判断文件夹是否创建成功
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
