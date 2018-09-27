using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VonderkCRUD;
using VonderkCRUD.Models;

namespace VonderkCRUD.Controllers
{
    [Authorize]
    public class SlidePrincipalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SlidePrincipals
        public ActionResult Index()
        {
            return View(db.SlidePrincipal.ToList());
        }

        // GET: SlidePrincipals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlidePrincipal slidePrincipal = db.SlidePrincipal.Find(id);
            if (slidePrincipal == null)
            {
                return HttpNotFound();
            }
            return View(slidePrincipal);
        }

        // GET: SlidePrincipals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidePrincipals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase filesupload, SlidePrincipal slidePrincipal)
        {

            if (ModelState.IsValid)
            {

                try
                {

                  
                    var path1 = Server.MapPath("~/Images/Slides/");
                    string extension = Path.GetExtension(filesupload.FileName);
                   

                  
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in filesupload.FileName)
                    {
                        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                        {
                            sb.Append(c);
                        }
                    }
                    string nombreCorregido = sb.ToString();
                  

                    var path2 = nombreCorregido + extension;

                    if (filesupload != null && filesupload.ContentLength > 0)


                        filesupload.SaveAs(Path.Combine(path1, path2));


                    else
                    {
                        throw new Exception("You have not specified a file.");
                    }


                    slidePrincipal.Image = path2;

                    db.SlidePrincipal.Add(slidePrincipal);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }


            }


            return View(slidePrincipal);
        }

        // GET: SlidePrincipals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlidePrincipal slidePrincipal = db.SlidePrincipal.Find(id);
            if (slidePrincipal == null)
            {
                return HttpNotFound();
            }
            return View(slidePrincipal);
        }

        // POST: SlidePrincipals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Image")] SlidePrincipal slidePrincipal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slidePrincipal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slidePrincipal);
        }

        // GET: SlidePrincipals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlidePrincipal slidePrincipal = db.SlidePrincipal.Find(id);
            if (slidePrincipal == null)
            {
                return HttpNotFound();
            }
            return View(slidePrincipal);
        }

        // POST: SlidePrincipals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SlidePrincipal slidePrincipal = db.SlidePrincipal.Find(id);
            db.SlidePrincipal.Remove(slidePrincipal);
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
