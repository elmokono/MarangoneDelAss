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
    public class TrabajosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trabajos
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Trabajos.ToList());
        }

        public ActionResult AdminIndex()
        {
            return View(db.Trabajos.ToList());
        }

   
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue) { return HttpNotFound(); }
            TrabajoViewModel model = new TrabajoViewModel(id.Value);

            return View(model);



        }

        // GET: Trabajos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trabajos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Descripcion,Imagen")] Trabajo trabajo, List<HttpPostedFileBase> postedFiles, String hiddenField)
        {

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {

                string[] words;
                char[] delimiterChars = { ',', '[', ']', '\"' };

                if (hiddenField.Length > 0)
                {


                    words = hiddenField.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    string newHiddenField = null;
                    foreach (HttpPostedFileBase postedFile in postedFiles)
                    {
                        newHiddenField = newHiddenField + postedFile.FileName + ',';


                    }
                    words = newHiddenField.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                }

                StringBuilder sb = new StringBuilder();
                if (trabajo.Nombre != null)
                {
                    foreach (char c in trabajo.Nombre)
                    {
                        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                        {
                            sb.Append(c);
                        }
                    }


                }
                else
                {

                    throw new Exception("You have not specified a Name.");

                }
                string nombreCorregido = sb.ToString();

                if (postedFiles != null)
                {


               


                    string buildPath = "~/Images/Trabajos/" + nombreCorregido + "/";
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

                string result = string.Join(",", words);
                trabajo.Imagen = result;
                trabajo.NombreCorregido = nombreCorregido;
                db.Trabajos.Add(trabajo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trabajo);
        }

        // GET: Trabajos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trabajo trabajo = db.Trabajos.Find(id);
            if (trabajo == null)
            {
                return HttpNotFound();
            }
            return View(trabajo);
        }

        // POST: Trabajos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<HttpPostedFileBase> postedFiles, Trabajo trabajo, String hiddenField)
        {

            var p = db.Trabajos.Single(x => x.ID == trabajo.ID);

            if (ModelState.IsValid)
            {

                if (postedFiles[0] != null)
                {

                    string buildPath = "~/Images/Trabajos/" + trabajo.NombreCorregido + "/";
                    string path = Server.MapPath(buildPath);



                    foreach (HttpPostedFileBase postedFile in postedFiles)
                    {


                        string fileName = postedFile.FileName;
                        string sourcePath = buildPath;
                        string targetPath = buildPath;

                        // Create the path and file name to check for duplicates.
                        string pathToCheck = buildPath + fileName;

                        // Create a temporary file name to use for checking duplicates.
                        string tempfileName = "";

                        //                    string finalFileName = 0 + cont + fileName;


                        if (System.IO.File.Exists(pathToCheck))
                        {
                            int counter = 0;
                            while (System.IO.File.Exists(pathToCheck))
                            {
                                // if a file with this name already exists,
                                // prefix the filename with a number.
                                tempfileName = counter.ToString() + fileName;
                                pathToCheck = buildPath + tempfileName;
                                counter++;

                            }

                            fileName = tempfileName;

                        }

                        var pathFinal = fileName;
                        postedFile.SaveAs(path + pathFinal);

                    }


                }


                string[] words;
                char[] delimiterChars = { ',', '[', ']', '\"' };
                words = hiddenField.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                string result = string.Join(",", words);
                trabajo.Imagen = result;
                //db.Entry(trabajo).State = EntityState.Modified;
                p.Descripcion = trabajo.Descripcion;
                p.Nombre = trabajo.Nombre;
                p.NombreCorregido = trabajo.NombreCorregido;
                db.SaveChanges();
                return RedirectToAction("Index","Trabajos");
            }
            return View(trabajo);
        }

        // GET: Trabajos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trabajo trabajo = db.Trabajos.Find(id);
            if (trabajo == null)
            {
                return HttpNotFound();
            }
            return View(trabajo);
        }

        // POST: Trabajos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trabajo trabajo = db.Trabajos.Find(id);
            db.Trabajos.Remove(trabajo);
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
