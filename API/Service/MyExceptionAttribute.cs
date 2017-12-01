using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Service
{
    public class MyExceptionAttribute : HandleErrorAttribute//全局异常处理过滤器
    {
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            ExceptionQueue.Enqueue(filterContext.Exception);//将异常信息添加到队列中。异常在Global开启线程监听队列的数据
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}