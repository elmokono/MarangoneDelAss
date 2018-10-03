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
    public class SlidesController : Controller
    {
        private LuminariaEntities db = new LuminariaEntities();

        // GET: Slides
        public ActionResult Index()
        {
            return View(db.Slides.ToList());
        }

        //// GET: Slides/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Slide slide = db.Slides.Find(id);
        //    if (slide == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(slide);
        //}

        // GET: Slides/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: SlidePrincipals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase filesupload, Slide slide)
        {

            if (ModelState.IsValid)
            {

                try
                {


                    var path1 = Server.MapPath("~/Images/Slides/");
                    string extension = Path.GetExtension(filesupload.FileName);

                    if (filesupload != null && filesupload.ContentLength > 0)

                        filesupload.SaveAs(Path.Combine(path1, filesupload.FileName));

                    else
                    {
                        throw new Exception("You have not specified a file.");
                    }


                    slide.FileName = filesupload.FileName;
                    slide.IsActive = true;
                    db.Slides.Add(slide);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }


            }


            return View(slide);
        }

        //// GET: Slides/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Slide slide = db.Slides.Find(id);
        //    if (slide == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(slide);
        //}

        //// POST: Slides/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SlideID,FileName,IsActive")] Slide slide)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(slide).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(slide);
        //}

        // GET: Slides/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // POST: Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slide slide = db.Slides.Find(id);
            db.Slides.Remove(slide);
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
