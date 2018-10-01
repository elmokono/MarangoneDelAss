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
    public class WorksController : Controller
    {
        private LuminariaEntities db = new LuminariaEntities();

        // GET: Works
        public ActionResult Index()
        {
            return View(db.Works.ToList());
        }

        public ActionResult AdminIndex()
        {
            return View(db.Works.ToList());
        }

        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // GET: Works/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Works/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "WorkID,Name,Description,IsActive")] Work work)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Works.Add(work);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(work);
        //}

        // POST: Trabajos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Work trabajo, List<HttpPostedFileBase> postedFiles)
        {

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {

                if (trabajo.Name == null)
                {    
                    throw new Exception("You have not specified a Name.");

                }

                db.Works.Add(trabajo);
                db.SaveChanges();

                if (postedFiles != null)
                {

                    string buildPath = "~/Images/Trabajos/" + trabajo.WorkID + "/";
                    string path = Server.MapPath(buildPath);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (HttpPostedFileBase postedFile in postedFiles)
                    {

                        var pathFinal = postedFile.FileName;
                        postedFile.SaveAs(path + pathFinal);

                    }
                }

                return RedirectToAction("Index");
            }

            return View(trabajo);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkID,Name,Description,IsActive")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Works.Find(id);
            db.Works.Remove(work);
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
