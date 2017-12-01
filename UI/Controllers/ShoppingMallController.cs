using IService;
using Model.Base.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Controllers.Base;
using UI.Models;

namespace UI.Controllers
{
    public class ShoppingMallController : JsonController
    {
        #region 页面
        //
        // GET: /ShoppingMall/

        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        readonly IGoodsAdminService _goodsService;
        readonly ITypeService _typeService;
        public ShoppingMallController(IGoodsAdminService goodsService, ITypeService typeService)
        {
            this._goodsService = goodsService;
            this._typeService = typeService;
        }
        public ShoppingMallController() { }
        #endregion

        #region 查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public JsonBackResult GetPagingList(int pageIndex, int pageSize, string modelName, string typeName)
        {
            int totalCount = 0;
            List<GoodsModel> goodsList = null;
            if (typeName == "")
            {
                goodsList = this._goodsService.GetPagingList(pageIndex, pageSize, out totalCount, true, t => t.State == 2 && t.GoodsName.Contains(modelName), t => t.UpdateTime).ToList();//获取模型信息
            }
            else//根据类型查询
            {
                var typeModel = this._typeService.GetList(t => t.TypeName == typeName && t.State>0).FirstOrDefault();
                if (typeModel.GoodsModels != null)
                {
                    totalCount = typeModel.GoodsModels.Count;
                }
                goodsList = typeModel.GoodsModels.Where(t => t.State ==2).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return JsonBackResult(true, new WebModel() { List = goodsList, TotalCount = totalCount });
        } 
        #endregion



    }
}
