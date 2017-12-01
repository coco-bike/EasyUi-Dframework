using Common;
using IService;
using Model;
using Model.Base.Entities;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using UI.Controllers.Base;
using UI.Models;

namespace UI.Controllers
{
    public class DownLoadController : JsonController
    {
        #region 页面
        //
        // GET: /DowLoad/

        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        readonly IGoodsService _goodsService;
        readonly IUserDataService _userDataService;
        readonly IUserService _userService;
        public DownLoadController(IGoodsService goodsService, IUserService userService, IUserDataService userDataService)
        {
            this._goodsService = goodsService;
            this._userService = userService;
            this._userDataService = userDataService;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取模型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonBackResult GetModel(long id, string nickName)
        {
            var goods= this._goodsService.GetList(t => t.Id == id && t.State > 0).Select(t => new { t.GoodsName, t.ImgUrl1, t.ImgUrl2, t.ImgUrl3, t.TagModels, t.Company, t.Description }).FirstOrDefault();
            var user = this._userService.GetList(t => t.NickName == nickName).FirstOrDefault();
            bool whetherCollect=false;
            if (user.UserDataModels != null)
            {
                var userDatas = user.UserDataModels.Where(t => t.Name == goods.GoodsName).FirstOrDefault();
                if (userDatas != null)
                { 
                    whetherCollect=true;
                }
            }
            return JsonBackResult(true, new GoodsInfoWModel() {  WhetherCollect=whetherCollect, GoodsInfo=goods});
        }
        #endregion

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="id">模型主键</param>
        /// <param name="uName">用户名</param>
        /// <returns></returns>
        public JsonBackResult CollectModel(long id, string nickName)
        {
            if (this._userDataService.CollectGood(id, nickName))
                return JsonBackResult(ResultStatus.Success);
            return JsonBackResult(ResultStatus.Fail);
        }

        /// <summary>
        /// 获取下载链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonBackResult DownLodModel(long id)
        {
          var goods=  this._goodsService.GetList(t => t.Id == id&&t.State>0).FirstOrDefault();
          if (goods == null)
              return JsonBackResult(ResultStatus.Fail);
          return JsonBackResult(ResultStatus.Success, goods.ModelUrl);
        }
    }
}
