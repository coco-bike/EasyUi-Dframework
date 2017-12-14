using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Services
{
    // 登录认证特性
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["sessionId"] == null)
            {
                filterContext.Result = new RedirectResult("../user/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}