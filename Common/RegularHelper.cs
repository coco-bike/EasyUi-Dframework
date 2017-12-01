using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RegularHelper
    {
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static bool IsPhoneNum(string phoneNum)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNum, @"^[1]+\d{10}");
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str_Email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"\w*@\w*.(com|cn)");
        }

        /// <summary>
        /// 验证是否全是字母
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static bool IsLetter(string letter)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(letter, @"^[a-zA-Z]+$");
        }
    }
}
