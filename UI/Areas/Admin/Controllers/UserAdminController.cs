using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using IService;
using Service;
using UI.Controllers.Base;

namespace UI.Areas.Admin.Controllers
{
    public class UserAdminController : JsonController
    {
        //
        // GET: /Admin/UserAdmin/
        #region 页面
        public ActionResult register()
        {
            return View();
        }
        public ActionResult forgotpassword()
        {
            return View();
        }
        #endregion

        readonly IUserAdminService _userService;
        public UserAdminController(IUserAdminService userService)
        {
            this._userService = userService;
        }
        /// <summary>
        /// 获取注册验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRegisterVerifiedCode()
        {
            string codeStr;
            byte[] arr = new VerifyCode().GetVerifyCode(out codeStr);
            Session.Add("RegisterVerifycode", codeStr);
            return File(arr, @"image/Gif");
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonBackResult RegisterUserData(string email, string password,string password1, string code)
        {
            var usercode = Session["RegisterVerifyCode"].ToString();
            var user = this._userService.GetList(s => s.EMail == email).FirstOrDefault();
            if (user != null)
            {
                return JsonBackResult(ResultStatus.EmailExist, "你输入的电子邮箱已经注册过");
            }
            if (usercode != code)
            {
                return JsonBackResult(ResultStatus.ValidateCodeErr, "验证码错误,请从新输入验证码");
            }
            Model.UserModel userData = new Model.UserModel();
            userData.EMail = email;
            userData.NickName = "小明";
            userData.Count = 0;
            userData.HeadPicUrl = "../Imgs/";
            userData.LoginTime = DateTime.Now.ToString();
            userData.BuildTime = DateTime.Now.ToString();
            userData.Pwd = EncryptionHelper.GetMd5Str(password);
            userData.Status = 1;
            userData.TelNumber = "18251935177";
            userData.UName = "小明";
            this._userService.Add(userData);
            return JsonBackResult(ResultStatus.Success);
        }
        /// <summary>
        /// 检查用户的邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonBackResult CheckUserData(string email)
        {
            var userdata = this._userService.GetList(s => s.EMail == email && s.Status == 1).FirstOrDefault();
            if (userdata == null)
            {
                return JsonBackResult(ResultStatus.UserNoExist);
            }
            if (!RegularHelper.IsEmail(email))
            {
                return JsonBackResult(ResultStatus.EmailErr);
            }

            Tuple<string, bool> items = SendEmail(email, "创客平台", "用户找回密码验证码");
            string code = items.Item1;
            Session.Add("FindVerifycode", code);
            bool sendRes = items.Item2;
            if (sendRes)
            {
                bool res1 = CacheHelper.Set("PwdBackCode", code, DateTime.Now.AddSeconds(90));
                bool res2 = CacheHelper.Set("PwdBackEmail",email, DateTime.Now.AddSeconds(90));
                if (res1 && res2)
                {
                    return JsonBackResult(ResultStatus.Success);
                }
            }
            return JsonBackResult(ResultStatus.Fail);
        }
        /// <summary>
        /// 验证身份
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonBackResult ValidatePwdBackCode(string email, string icode)
        {
            string code = icode.ToLower();
            var usercode = Session["FindVerifycode"].ToString();
            string ucode = usercode.ToLower();
            if (ucode != code)
            {
                return JsonBackResult(ResultStatus.ValidateCodeErr);
            }
            return JsonBackResult(ResultStatus.Success);
        }
        /// <summary>
        /// 用户找回密码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonBackResult UpdateUserData(string email, string password)
        {
            if (email != null && password != null)
            {
                var userdata = this._userService.GetList(s => s.EMail == email && s.Status == 1).FirstOrDefault();
                var md5password = EncryptionHelper.GetMd5Str(password);
                userdata.Pwd = md5password;
                this._userService.Update(userdata);
                return JsonBackResult(ResultStatus.Success);
            }
            return JsonBackResult(ResultStatus.Fail);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="eMail">接收方邮箱</param>
        /// <param name="mailName">发件人名称</param>
        /// <param name="mailTitle">邮件名称</param>
        /// <returns></returns>
        public static Tuple<string, bool> SendEmail(string eMail, string mailName, string mailTitle)
        {
            MailHelper mail = new MailHelper();
            mail.MailServer = "smtp.qq.com";
            mail.MailboxName = "444503829@qq.com";
            mail.MailboxPassword = "fplslqpringqbhhi";//开启QQ邮箱POP3/SMTP服务时给的授权码
            //操作打开QQ邮箱->在账号下方点击"设置"->账户->POP3/IMAP/SMTP/Exchange/CardDAV/CalDAV服务
            //obxxsfowztbideee为2872845261@qq的授权码
            mail.MailName = mailName;
            string code;
            VerifyCode codeHelper = new VerifyCode();
            codeHelper.GetVerifyCode(out code);
            if (code == "")
                return new Tuple<string, bool>("", false);
            if (mail.Send(eMail, mailTitle, code))
                return new Tuple<string, bool>(code, true);
            return new Tuple<string, bool>("", false);
        }

    }
}
