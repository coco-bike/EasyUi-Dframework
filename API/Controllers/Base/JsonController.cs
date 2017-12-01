using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace API.Controllers.Base
{
    public class JsonController : ApiController
    {
        protected internal JsonNetResult JsonNetResult(bool status, string msg = null)
        {
            return new JsonNetResult().GetJsonResult(status, msg);
        }

        protected internal JsonNetResult JsonNetResult(ResultStatus status, string msg = null)
        {
            return new JsonNetResult().GetJsonResult(status, msg);
        }

        protected internal JsonNetResult JsonNetResult(bool status, object data, string msg = null)
        {
            return new JsonNetResult().GetJsonResult(status, data, msg);
        }

        protected internal JsonNetResult JsonNetResult(ResultStatus status, object data, string msg = null)
        {
            return new JsonNetResult().GetJsonResult(status, data, msg);
        }
    }
}
