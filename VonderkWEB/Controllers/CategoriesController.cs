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
    [Authorize]
    public class CategoriesController : Controller
    {
        private LuminariaEntities db = new LuminariaEntities();


        [HttpPost]
        public ActionResult FirstAjax(string[] listValues)
        {

            short cont = 0;

            foreach (var item in listValues)
            {
                var m = db.Categories.SingleOrDefault(x => x.Name == item);
                m.SortOrder = cont;
                cont++;
            }
            db.SaveChanges();

            return Json("Se cambio el orden correctamente", JsonRequestBehavior.AllowGet);
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category, HttpPostedFileBase imageFile)
        {
            if (category == null) {

                throw new Exception("Completar los datos");

            }
            if (imageFile == null)
            {
                Category cat1 = new Category
                {
                    
                    FileName = "",
                    IsActive = true,
                    Name = category.Name,
                    SortOrder = 0,
                };

                db.Categories.Add(cat1);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            else
            {

                if (ModelState.IsValid)
                {
                    Category cat = new Category
                    {

                        FileName = imageFile.FileName,
                        IsActive = true,
                        Name = category.Name,
                        SortOrder = 0,
                    };

                    db.Categories.Add(cat);
                    db.SaveChanges();




                    var pathAssets = Server.MapPath("~/Images/Categories/");

                    if (!Directory.Exists(pathAssets))
                    {
                        Directory.CreateDirectory(pathAssets);
                    }
                    imageFile.SaveAs(Path.Combine(pathAssets, imageFile.FileName));



                    return RedirectToAction("Index");
                }

            }


            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category, HttpPostedFileBase imageFile)
        {

            if (imageFile == null)
            {
                var p = db.Categories.Where(x => x.CategoryID == category.CategoryID).FirstOrDefault();



                p.FileName = "";
                p.IsActive = true;
                p.Name = category.Name;
                p.SortOrder = category.SortOrder;

                db.SaveChanges();

                return RedirectToAction("Index");

            }
            else {

                if (ModelState.IsValid)
                {
                    db.Entry(category).State = EntityState.Modified;
                    if (imageFile != null)
                    {
                        category.FileName = imageFile.FileName;
                        db.SaveChanges();
                        var pathAssets = Server.MapPath("~/Images/Categories/");
                        imageFile.SaveAs(Path.Combine(pathAssets, imageFile.FileName));
                    }
                    else
                    {
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

            }

           
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
