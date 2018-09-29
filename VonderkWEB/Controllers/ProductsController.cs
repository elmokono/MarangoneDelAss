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

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product Product = db.Products.Find(id);

            //db.ProductAssets.Where(x => x.ProductID == id).OrderBy(x => x.SortOrder).ToList();

            //if (Product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Product);
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
        public ActionResult Create(Product Product, List<HttpPostedFileBase> imageFiles, List<HttpPostedFileBase> fichaFiles, List<HttpPostedFileBase> iesFiles)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            //var pathImagenes = Server.MapPath("~/Images/");

            if (ModelState.IsValid)
            {

                try
                {

                    //validar ProductID
                    if (db.Products.Any(x => x.Name == Product.Name) || db.Products.Any(x => x.ProductCode == Product.ProductCode))
                    {
                        throw new Exception("El Product con ese nombre / código de Product ya existe.");
                    }

                    //--------------Guardo el Product--------------------------------------------------------
                    db.Products.Add(Product);
                    db.SaveChanges();
                    var guardarID = Product.ProductID;

                    //--------------Chequeo y creo el directorio-----------------------------------------------
                    string buildPath = "~/Products/" + Product.ProductID + "/";
                    string pathProduct = Server.MapPath(buildPath);
                    if (!Directory.Exists(pathProduct))
                    {
                        Directory.CreateDirectory(pathProduct);
                    }

                    //--------------Guardo las imagenes-------------------------------------------------------
                    foreach (HttpPostedFileBase postedFile in imageFiles)
                    {

                        var imagen = new ProductAsset
                        {
                            ProductID = Product.ProductID,
                            Name = postedFile.FileName

                        };
                        db.ProductAssets.Add(imagen);
                        var pathImagenesProduct = Server.MapPath("~/Products/" + Product.ProductID + "/Imagenes/");
                        if (!Directory.Exists(pathImagenesProduct))
                        {
                            Directory.CreateDirectory(pathImagenesProduct);
                            postedFile.SaveAs(Path.Combine(pathImagenesProduct, postedFile.FileName));
                        }
                        else
                        {
                            postedFile.SaveAs(Path.Combine(pathImagenesProduct, postedFile.FileName));
                        }

                    }
                    db.SaveChanges();



                    //--------------Guardo los IES----------------------------------------------------------
                    foreach (HttpPostedFileBase postedFile in iesFiles)
                    {

                        var ies = new ProductAsset
                        {
                            ProductID = Product.ProductID,
                            Name = postedFile.FileName,
                            FileName = postedFile.FileName

                        };
                        db.ProductAssets.Add(ies);
                        var pathIesProduct = Server.MapPath("~/Products/" + Product.ProductID + "/IES/");
                        if (!Directory.Exists(pathIesProduct))
                        {
                            Directory.CreateDirectory(pathIesProduct);
                            postedFile.SaveAs(Path.Combine(pathIesProduct, postedFile.FileName));
                        }
                        else
                        {
                            postedFile.SaveAs(Path.Combine(pathIesProduct, postedFile.FileName));
                        }

                    }
                    db.SaveChanges();

                    //--------------Guardo las fichas-------------------------------------------------------
                    foreach (HttpPostedFileBase postedFile in fichaFiles)
                    {

                        var ficha = new ProductAsset
                        {
                            ProductID = Product.ProductID,
                            Name = postedFile.FileName,
                            FileName = postedFile.FileName

                        };
                        db.ProductAssets.Add(ficha);
                        var pathFichasProduct = Server.MapPath("~/Products/" + Product.ProductID + "/Fichas/");
                        if (!Directory.Exists(pathFichasProduct))
                        {
                            Directory.CreateDirectory(pathFichasProduct);
                            postedFile.SaveAs(Path.Combine(pathFichasProduct, postedFile.FileName));
                        }
                        else
                        {
                            postedFile.SaveAs(Path.Combine(pathFichasProduct, postedFile.FileName));
                        }

                    }


                    db.SaveChanges();
                    return RedirectToAction("Index", "Products");


                }
                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }

            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", Product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", Product.BrandID);
            return View(Product);
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
        public ActionResult Edit([Bind(Include = "ID,BrandID,CategoryID,Name,Description,Features,ProductCode")] Product Product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", Product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", Product.BrandID);
            return View(Product);
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
