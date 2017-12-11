using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using IService;
using Model;
using UI.Controllers.Base;

namespace UI.Areas.Admin.Controllers
{
    public class IndexController : Controller
    {

        #region 页面
        // GET: /Admin/Index/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DataGrid()
        {
            return View();
        }
        public ActionResult SchoolClassTable()
        {
            return View();
        }
        #endregion

        #region 初始化

        readonly IProductAdminService _productService;
        public IndexController(IProductAdminService productService)
        {
            this._productService = productService;
        }
        #endregion
        /// <summary>
        ///分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public JsonResult GetProductListData(string page, string rows, string itemid, string productid)
        { 
           int totalCount;
           var pageIndex = Convert.ToInt32(page);
           var pageSize = Convert.ToInt32(rows);
           if (itemid == null && productid == null||(itemid==""&&productid==""))
           {
               var productlist = this._productService.GetPagingList(pageIndex, pageSize, out totalCount, true, s => s.State == 1, s => s.Id).Select(t => new { t.Id, t.itemid, t.listprice, t.productid, status = t.State, t.unitcost, t.attr1 }).ToList();
               return Json(new { total = totalCount, rows = productlist });
           }
           else
           {
               totalCount = 1;
               var productlist = this._productService.GetList(s => s.itemid == itemid && s.productid == productid).ToList();
               return Json(new { total = totalCount, rows = productlist });
           }
           
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpdateProductData(ProductModel productdata)
        {
            var product = this._productService.GetList(s => s.itemid == productdata.itemid).FirstOrDefault();
            if (product == null)
            {
                return Json(ResultStatus.Fail);
            }
            product.listprice = productdata.listprice;
            product.productid = productdata.productid;
            product.unitcost = productdata.unitcost;
            product.attr1 = productdata.attr1;
            var res = this._productService.Update(product);
            if (res > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        /// <summary>
        /// 保存单个信息
        /// </summary>
        /// <param name="productdata"></param>
        /// <returns></returns>
        public JsonResult SaveProductData(ProductModel productdata)
        {
            var product = this._productService.GetList(t => t.itemid == productdata.itemid).ToList();
            if (product.Count > 0)
            {
                return Json(ResultStatus.Fail);
            }
            var product1 = new ProductModel();
            product1 = productdata;
            this._productService.Add(product1);
            return Json(ResultStatus.Success);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DestroyProduct(int id)
        {
            var item = Convert.ToString(id);
            var product = this._productService.GetList(s => s.itemid == item).FirstOrDefault();
            if (product == null)
            {
                return Json(ResultStatus.Fail);
            }
            int result = this._productService.DeleteFake(t => t.itemid == item, t => new ProductModel() { State = 0 });
            if (result > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
    }
}
