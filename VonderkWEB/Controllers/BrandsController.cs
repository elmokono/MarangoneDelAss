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
    public class BrandsController : Controller
    {
        private LuminariaEntities db = new LuminariaEntities();

        // GET: Brands
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        // GET: Brands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Brands/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "BrandID,Name,IsActive")] Brand brand)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Brands.Add(brand);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(brand);
        //}

        // POST: Marcas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brand, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    Brand brn = new Brand
                    {

                        FileName = imageFile.FileName,
                        IsActive = true,
                        Name = brand.Name                      
                    };

                    db.Brands.Add(brn);
                    db.SaveChanges();


                    if (imageFile != null)
                    {

                        var pathAssets = Server.MapPath("~/Images/Brands/");

                        if (!Directory.Exists(pathAssets))
                        {
                            Directory.CreateDirectory(pathAssets);
                        }
                        imageFile.SaveAs(Path.Combine(pathAssets, imageFile.FileName));

                    }

                    return RedirectToAction("Index", "Brands");

                }

                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }


            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand brand, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                if (imageFile != null)
                {
                    brand.FileName = imageFile.FileName;
                    db.SaveChanges();
                    var pathAssets = Server.MapPath("~/Images/Brands/");
                    imageFile.SaveAs(Path.Combine(pathAssets, imageFile.FileName));
                }
                else
                {
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            //if (ModelState.IsValid)
            //{
            //    db.Entry(brand).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(brand);
        }

        // GET: Brands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
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
