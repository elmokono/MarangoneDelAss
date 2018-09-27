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
    public class MarcasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Marcas
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Marcas.ToList());
        }

        // GET: Marcas/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // GET: Marcas/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase filesupload, Marca marca)
        {
            if (ModelState.IsValid)
            {

                try
                {


                 
                    var path1 = Server.MapPath("~/Images/Marcas/");
                    string extension = Path.GetExtension(filesupload.FileName);
                    //var path2 = producto.CodigoDeProd + extension;


                    StringBuilder sb = new StringBuilder();
                    foreach (char c in marca.Nombre)
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


                    marca.Imagen = path2;

                    db.Marcas.Add(marca);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Marcas");

                }

                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }


            }

            return View(marca);
        }

        // GET: Marcas/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase filesupload, Marca marca)
        {

            StringBuilder sb = new StringBuilder();
            foreach (char c in marca.Nombre)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            string nombreCorregido = sb.ToString();

            var m = db.Marcas.Single(x => x.ID == marca.ID);

           
            var path1 = Server.MapPath("~/Images/Marcas/");

            //string pathOnBlankFile = "";

            //if (filesupload == null)
            //{

            //    string value = m.Imagen;

            //    string extension = '.' + value.Split('.').Last();

            //    pathOnBlankFile = nombreCorregido + extension;

            //}


            if (ModelState.IsValid)
            {
                try
                {

                    if (filesupload != null)
                    {
                        //Al editar, busco el producto por su ID
                        string extension = Path.GetExtension(filesupload.FileName);
                        //var path2 = producto.CodigoDeProd + extension;
                        var path2 = nombreCorregido + extension;
                        filesupload.SaveAs(Path.Combine(path1, path2));
                        m.Imagen = path2;
                    }
                    //else
                    //{
                    //    m.Imagen = pathOnBlankFile;
                    //    //throw new Exception("You have not specified a file.");
                    //}
                }
                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }

                m.Nombre = marca.Nombre;

                //db.Entry(marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Marcas");
            }
            else
            {

                ViewBag.Message = "Model State not valid";

            }

            return RedirectToAction("Index", "Marcas");
            //return View(marca);
        }

        // GET: Marcas/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marca marca = db.Marcas.Find(id);
            db.Marcas.Remove(marca);
            db.SaveChanges();
            return RedirectToAction("Index", "Marcas");
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
