using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "UI.Areas.Admin.Controllers" }
            ).DataTokens.Add("area", "Admin");

            routes.MapRoute(
                name: "Web",
                url: " Web/{controller}/{action}/{id}",
                defaults: new { controller = "Web", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "UI.Areas.Web.Controllers" }
            ).DataTokens.Add("area", "Web");
        }
    }
}