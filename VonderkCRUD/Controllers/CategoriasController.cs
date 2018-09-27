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
    public class CategoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categorias
        public ActionResult Index()
        {
            return View(db.Categorias.ToList());
        }


        // GET: Categorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public ActionResult FirstAjax(string[] listValues)
        {

            int cont = 0;
            
            foreach (var item in listValues)
            {
                var m = db.Categorias.Single(x => x.Nombre == item);
                m.Orden = cont;
                cont++;
            }
            db.SaveChanges();

            return Json("Se cambio el orden correctamente", JsonRequestBehavior.AllowGet);
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase filesupload, Categoria categoria)
        {

            if (ModelState.IsValid)
            {

                try
                {

                  
                    var path1 = Server.MapPath("~/Images/Categorias/");
                    string extension = Path.GetExtension(filesupload.FileName);
                    //var path2 = producto.CodigoDeProd + extension;


                    StringBuilder sb = new StringBuilder();
                    foreach (char c in categoria.Nombre)
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


                    categoria.Imagen = path2;

                    db.Categorias.Add(categoria);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Categorias");

                }

                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }


            }

            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase filesupload, Categoria categoria)
        {

            StringBuilder sb = new StringBuilder();
            foreach (char c in categoria.Nombre)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            string nombreCorregido = sb.ToString();

            var m = db.Categorias.Single(x => x.ID == categoria.ID);

    
            var path1 = Server.MapPath("~/Images/Categorias/");

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
         
                        string extension = Path.GetExtension(filesupload.FileName);
             
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

                m.Nombre = categoria.Nombre;

                //db.Entry(marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Categorias");
            }
            else
            {

                ViewBag.Message = "Model State not valid";

            }

            return RedirectToAction("Index", "Categorias");

        }

        // GET: Categorias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            db.Categorias.Remove(categoria);
            db.SaveChanges();
            return RedirectToAction("Index", "Categorias");
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
