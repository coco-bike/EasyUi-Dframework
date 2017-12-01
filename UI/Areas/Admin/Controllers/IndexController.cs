using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Admin/Index/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetProductData(string page,string rows)
        {
            List<Product> list = new List<Product>();
            list = GetProductList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public List<Product> GetProductList()
        {
            List<Product> productlist = new List<Product>(){
                new Product(){Id=1,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=2,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=3,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=4,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=5,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=6,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=7,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=8,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=9,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=10,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=11,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=12,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=13,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=14,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=15,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=16,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=17,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=18,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
                new Product(){Id=19,itemid="1",productid="1",listprice="1",status="1",attr1="1",unitcost="1"},
            };
            return productlist;
        }
        public class Product
        {
            public int Id { get; set; }
            public string itemid { get; set; }
            public string productid { get; set; }
            public string listprice { get; set; }
            public string unitcost { get; set; }
            public string attr1 { get; set; }
            public string status { get; set; }
        }
    }
}
