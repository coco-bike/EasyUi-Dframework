using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers.Base
{
    public class JsonController : Controller
    {
        protected internal JsonBackResult JsonBackResult(bool status, string msg = null)
        {
            return new JsonBackResult().GetJsonResult(status, msg);
        }

        protected internal JsonBackResult JsonBackResult(ResultStatus status, string msg = null)
        {
            return new JsonBackResult().GetJsonResult(status, msg);
        }


        protected internal JsonBackResult JsonBackResult(bool status, object data, string msg = null)
        {
            return new JsonBackResult().GetJsonResult(status, data, msg);
        }

        protected internal JsonBackResult JsonBackResult(ResultStatus status, object data, string msg = null)
        {
            return new JsonBackResult().GetJsonResult(status, data, msg);
        }
    }
}