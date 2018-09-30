using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VonderkWEB.Models;

namespace VonderkWEB.Controllers
{
    public class ProductsController : Controller
    {
        private LuminariaEntities db = new LuminariaEntities();

        // GET: Products
        public ActionResult Index()
        {
            var model = new Models.HomeViewModel().Get();
            return View(model);
            //var Products = db.Products.Include(p => p.Category).Include(p => p.Brand);
            //return View(Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {

            if (!id.HasValue) { return HttpNotFound(); }
            ProductDetailsViewModel model = new ProductDetailsViewModel(id.Value);

            return View(model);
                    }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,BrandID,CategoryID,Nombre,Descripcion,Caracteristicas,ProductCode")] Product Product)
        public ActionResult Create(Product model, String labeledAssets, List<HttpPostedFileBase> imageFiles, List<HttpPostedFileBase> fichaFiles, List<HttpPostedFileBase> iesFiles)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            var pathAssets = Server.MapPath("~/Products/");

            if (ModelState.IsValid)
            {
                model.IsActive = true;
                new ProductDetailsViewModel().New(model, pathAssets, labeledAssets, imageFiles, fichaFiles, iesFiles);
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", model.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", model.BrandID);

            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", Product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", Product.BrandID);
            return View(Product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, String deletedAssets, String labeledAssets, List<HttpPostedFileBase> imageFiles, List<HttpPostedFileBase> fichaFiles, List<HttpPostedFileBase> iesFiles)
        {
            if (ModelState.IsValid)
            {
                var pathAssets = Server.MapPath("~/Products/");
                new ProductDetailsViewModel().Edit(model, pathAssets, deletedAssets, labeledAssets, imageFiles, fichaFiles, iesFiles);
                //db.Entry(Product).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", model.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", model.BrandID);
            return View(model);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var findFichas = db.ProductAssets.Where(x => x.ProductID == id);
            foreach (var item in findFichas)
            {
                db.ProductAssets.Remove(item);
            }

            var findIES = db.ProductAssets.Where(x => x.ProductID == id);
            foreach (var item in findIES)
            {
                db.ProductAssets.Remove(item);
            }

            var findImagenes = db.ProductAssets.Where(x => x.ProductID == id);
            foreach (var item in findImagenes)
            {
                db.ProductAssets.Remove(item);
            }
            db.SaveChanges();
            Product Product = db.Products.Find(id);
            db.Products.Remove(Product);
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
