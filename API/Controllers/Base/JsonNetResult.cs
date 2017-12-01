using API.Models;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace API.Controllers.Base
{
    /// <summary>
    /// Api返回类型s
    /// </summary>
    public class JsonNetResult : HttpResponseMessage
    {
        public const ResultStatus Success = ResultStatus.Success;
        public const ResultStatus Fail = ResultStatus.Fail;

        #region 返回前端字段
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Object Data { get; set; }

        #endregion

        #region 方法重载
        public JsonNetResult GetJsonResult(bool status, string msg)
        {
            SetStatus((status ? Success : Fail), msg);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(new ResultData() { Status = this.Status, Data = this.Data, Msg = this.Msg });
            JsonNetResult result = new JsonNetResult { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public JsonNetResult GetJsonResult(ResultStatus status, string msg)
        {
            SetStatus(status, msg);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(new ResultData() { Status = this.Status, Data = this.Data, Msg = this.Msg });
            JsonNetResult result = new JsonNetResult { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public JsonNetResult GetJsonResult(ResultStatus status, object data, string msg)
        {
            SetStatus(status, data, msg);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(new ResultData() { Status = this.Status, Data = this.Data, Msg = this.Msg });
            JsonNetResult result = new JsonNetResult { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public JsonNetResult GetJsonResult(bool status, object data, string msg = null)
        {
            SetStatus((status ? Success : Fail), data, msg);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(new ResultData() { Status = this.Status, Data = this.Data, Msg = this.Msg });
            JsonNetResult result = new JsonNetResult { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public JsonNetResult SetStatus(ResultStatus status, object data = null, string message = null)
        {
            this.Status = (int)status;
            this.Msg = message ?? Helper.ResultMsg(status);
            this.Data = data;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(new ResultData() { Status = this.Status, Data = this.Data, Msg = this.Msg });
            JsonNetResult result = new JsonNetResult { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        #endregion

        //public override void ExecuteResult(ControllerContext context)
        //{
        //    HttpResponseBase httpResponseBase = context.HttpContext.Response;
        //    httpResponseBase.ContentType = "application/json";
        //    httpResponseBase.ContentEncoding = Encoding.GetEncoding("UTF-8");
        //    var writer = new JsonTextWriter(httpResponseBase.Output);

        //    #region 处理EF死循环
        //    JsonSerializerSettings settings = new JsonSerializerSettings();
        //    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        //    JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
        //    #endregion
        //    jsonSerializer.Converters.Add(new ChinaDateTimeConverter());
        //    jsonSerializer.Serialize(writer, this);
        //    writer.Flush();
        //}

    }
    public class ChinaDateTimeConverter : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
   class ResultData 
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Object Data { get; set; }
    }
}