﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
    {
        //
        // GET: /Admin/UserAdmin/

        public ActionResult register()
        {
            return View();
        }

        public ActionResult forgotpassword()
        {
            return View();
        }

    }
}
