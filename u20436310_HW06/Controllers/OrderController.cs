using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using u20436310_HW06.Models;

namespace u20436310_HW06.Controllers
{
    public class OrderController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: Order
        public ActionResult Index(int? page, string search)
        {
            List<OrderVM> productinfo = db.orders.Select(p => new OrderVM
            {
                OrderId = p.order_id,
                Products = db.order_items.Where(x => x.order_id == p.order_id)
                .Select(o => new { product_name = o.product.product_name, price = o.list_price, Quantity = o.quantity, Total = (o.list_price * o.quantity) }).ToList<dynamic>(),
                OrderDate = db.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date
            }).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                productinfo = db.orders.Select(p => new OrderVM
                {
                    OrderId = p.order_id,
                    Products = db.order_items.Where(x => x.product.product_name.Contains(search) && x.order_id == p.order_id).Select(o => new { product_name = o.product.product_name, price = o.list_price, Quantity = o.quantity, Total = (o.list_price * o.quantity) }).ToList<dynamic>(),

                    OrderDate = db.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date
                }).ToList();
            }


            return View(productinfo.ToPagedList(page ?? 1, 10));
        }
    }
}
