using API.Service;
using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace API
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region Autofac在MVC中注册
            ContainerBuilder builder = new ContainerBuilder();
            var service = Assembly.Load("IService");
            var service1 = Assembly.Load("Service");
            var service2 = Assembly.Load("Model");
            Assembly[] assemblyArr = new Assembly[] { service, service1, service2 };
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(assemblyArr).AsImplementedInterfaces();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion

            //MiniProfilerEF6.Initialize();//注册MiniProfiler，网页性能插件

            log4net.Config.XmlConfigurator.Configure();//读取Log4Net配置信息

            //WaitCallback
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    if (MyExceptionAttribute.ExceptionQueue.Count > 0)
                    {
                        Exception ex = MyExceptionAttribute.ExceptionQueue.Dequeue();//出队
                        //string fileName = DateTime.Now.ToString("yyyy-MM-dd")+".txt";
                        //File.AppendAllText(fileLogPath + fileName, ex.ToString(), System.Text.Encoding.Default);
                        //ILog logger = LogManager.GetLogger("errorMsg");
                        ILog logger = log4net.LogManager.GetLogger("logger");
                        logger.Error(ex.ToString());

                        #region 发送邮件
                        //MailHelper mail = new MailHelper();
                        //mail.MailServer = "smtp.qq.com";
                        //mail.MailboxName = "2872845261@qq.com";
                        //mail.MailboxPassword = "obxxsfowztbideee";//开启QQ邮箱POP3/SMTP服务时给的授权码
                        ////操作打开QQ邮箱->在账号下方点击"设置"->账户->POP3/IMAP/SMTP/Exchange/CardDAV/CalDAV服务
                        ////obxxsfowztbideee为2872845261@qq的授权码
                        //mail.MailName = "Error";
                        //try
                        //{
                        //    mail.Send("1015934551@qq.com", "Error", ex.ToString());
                        //}
                        //catch
                        //{ } 
                        #endregion

                    }
                    else
                    {
                        Thread.Sleep(3000);//如果队列中没有数据，则休息为了避免占用CPU的资源.
                    }
                }
            });
        }
    }
}