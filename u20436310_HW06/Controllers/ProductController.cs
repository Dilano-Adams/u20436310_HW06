using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using u20436310_HW06.Models;

namespace u20436310_HW06.Controllers
{
    public class ProductController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: Brands
        public string Brands() // Used to populate the modal dropdown. See Index View
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<brand> brands = db.brands.ToList();
            return JsonConvert.SerializeObject(brands);
        }
        // GET: Catergories
        public string Categories() // Used to populate the modal dropdown. See Index View
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<category> categories = db.categories.ToList();
            return JsonConvert.SerializeObject(categories);
        }

        // GET: Product
        public ActionResult Index(string search,  int? page)
        {
            var products = db.products.Include(p => p.brand).Include(p => p.category);

            if (search != null)
            {
                products = db.products.Where(x => x.product_name.Contains(search)).Include(p => p.brand).Include(p => p.category);
            }

            return View(products.ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //product product = db.products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);

            return PartialView(id);
        }
        [HttpPost]
        public string Details(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            object product = db.stocks.Where(y => y.product_id == id)
                .Include(v => v.product)
                .Select(p => new {
                    product_name = p.product.product_name,
                    model_year = p.product.model_year,
                    list_price = p.product.list_price,
                    shops = db.stocks.Where(s => s.product_id == id).Select(n => new { store_name = n.store.store_name, quantity = n.quantity }),
                    brand_name = p.product.brand.brand_name,
                    category_name = p.product.category.category_name
            }).FirstOrDefault();

            return JsonConvert.SerializeObject(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return PartialView();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //product product = db.products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            //ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            //return View(product);
            return PartialView();
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
