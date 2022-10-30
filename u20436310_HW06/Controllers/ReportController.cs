using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u20436310_HW06.Models;

namespace u20436310_HW06.Controllers
{
    public class ReportController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public string chartData()
        {
            //OrderVM ordersVM = new OrderVM();
            object orders = db.orders.Select(p => new
            {
                Products = db.order_items.Where(y => y.order_id == p.order_id).Select(o => new { category = o.product.category.category_name, Quantity = o.quantity, month = p.order_date.Month }).ToList<dynamic>(),
            }).ToList();

            return JsonConvert.SerializeObject(orders);
            //return Json(Ordersvm, JsonRequestBehavior.AllowGet); Could've used a view model to call the data.
        }
    }
}