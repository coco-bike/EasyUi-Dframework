using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using IService;
using Model;
using UI.Controllers.Base;

namespace UI.Areas.Admin.Controllers
{
    public class LoginController : JsonController
    {
        //
        // GET: /Admin/Login/
        #region 页面
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 初始化
        readonly IUserAdminService _userService;
        public LoginController(IUserAdminService userService)
        {
            this._userService = userService;
        }
        #endregion
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVerifiedCode() {
            string codeStr;
            byte[] arr = new VerifyCode().GetVerifyCode(out codeStr);
            Session.Add("Verifycode", codeStr);
            return File(arr, @"image/Gif");
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <param name="verifyCode"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        public JsonBackResult CheckLoginInfo(string email, string pwd, string verifyCode, string autoLogin)
        {
            if (Session["VerifyCode"] == null)
            {
                return JsonBackResult(ResultStatus.Fail);
            }
            string code = Session["VerifyCode"].ToString();
            if (string.Compare(code, verifyCode, true) != 0)
            {
                return JsonBackResult(ResultStatus.ValidateCodeErr);
            }
            string pwdMd5 = Common.EncryptionHelper.GetMd5Str(pwd);
            var userInfo = this._userService.GetList(s => s.EMail == email && s.Pwd == pwdMd5).FirstOrDefault();
            if (userInfo != null)
            {
                string sessionId = Guid.NewGuid().ToString();
                Response.Cookies["sessionId"].Value = sessionId;
                if (autoLogin=="true")
                {
                    CacheHelper.Set(sessionId, userInfo, DateTime.Now.AddDays(30));
                    Response.Cookies["sessiionId"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    CacheHelper.Set(sessionId, userInfo, DateTime.Now.AddDays(1));
                    Response.Cookies["sessiionId"].Expires = DateTime.Now.AddDays(1);
                }
                userInfo.Count = userInfo.Count + 1;
                userInfo.LoginTime = DateTime.Now;
                var res = this._userService.Update(userInfo);
                if (res > 0)
                {
                    return JsonBackResult(ResultStatus.Success);
                }
            }
            return JsonBackResult(ResultStatus.Fail);
        }
    }
}
