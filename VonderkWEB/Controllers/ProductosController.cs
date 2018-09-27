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
    public class ProductosController : Controller
    {
        private LuminariaEntities db = new LuminariaEntities();

        // GET: Productos
        public ActionResult Index()
        {
            var productoes = db.Productoes.Include(p => p.Categoria).Include(p => p.Marca);
            return View(productoes.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {

            if (!id.HasValue) { return HttpNotFound(); }
            ProductDetailsViewModel model = new ProductDetailsViewModel(id.Value);

            return View(model);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Producto producto = db.Productoes.Find(id);

            //db.ImagenProductoes.Where(x => x.ProductId == id).OrderBy(x => x.Orden).ToList();

            //if (producto == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre");
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,MarcaID,CategoriaID,Nombre,Descripcion,Caracteristicas,CodigoDeProducto")] Producto producto)
        public ActionResult Create(Producto producto, List<HttpPostedFileBase> imageFiles, List<HttpPostedFileBase> fichaFiles, List<HttpPostedFileBase> iesFiles)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            //var pathImagenes = Server.MapPath("~/Images/");
            
            if (ModelState.IsValid)
            {

                try {

                    //validar ProductID
                    if (db.Productoes.Any(x => x.Nombre == producto.Nombre) || db.Productoes.Any(x => x.CodigoDeProducto == producto.CodigoDeProducto))
                    {
                        throw new Exception("El producto con ese nombre / código de producto ya existe.");
                    }

                    //--------------Guardo el producto--------------------------------------------------------
                    db.Productoes.Add(producto);
                    db.SaveChanges();
                    var guardarID = producto.ID;

                    //--------------Chequeo y creo el directorio-----------------------------------------------
                    string buildPath = "~/Productos/" + producto.ID + "/";
                    string pathProducto = Server.MapPath(buildPath);
                    if (!Directory.Exists(pathProducto))
                    {
                        Directory.CreateDirectory(pathProducto);
                    }

                    //--------------Guardo las imagenes-------------------------------------------------------
                    foreach (HttpPostedFileBase postedFile in imageFiles)
                    {

                        var imagen = new ImagenProducto
                        {
                            ProductId = producto.ID,
                            Nombre = postedFile.FileName

                        };
                        db.ImagenProductoes.Add(imagen);
                        var pathImagenesProducto = Server.MapPath("~/Productos/" + producto.ID + "/Imagenes/");
                        if (!Directory.Exists(pathImagenesProducto))
                        {
                            Directory.CreateDirectory(pathImagenesProducto);
                            postedFile.SaveAs(Path.Combine(pathImagenesProducto, postedFile.FileName));
                        }
                        else {
                            postedFile.SaveAs(Path.Combine(pathImagenesProducto, postedFile.FileName));
                        }
                        
                    }
                    db.SaveChanges();



                    //--------------Guardo los IES----------------------------------------------------------
                    foreach (HttpPostedFileBase postedFile in iesFiles)
                    {

                        var ies = new ArchivoExtra
                        {
                            ProductId = producto.ID,
                            Nombre = postedFile.FileName,
                            Archivo = postedFile.FileName

                        };
                        db.ArchivoExtras.Add(ies);
                        var pathIesProducto = Server.MapPath("~/Productos/" + producto.ID + "/IES/");
                        if (!Directory.Exists(pathIesProducto))
                        {
                            Directory.CreateDirectory(pathIesProducto);
                            postedFile.SaveAs(Path.Combine(pathIesProducto, postedFile.FileName));
                        }
                        else
                        {
                            postedFile.SaveAs(Path.Combine(pathIesProducto, postedFile.FileName));
                        }

                    }
                    db.SaveChanges();

                    //--------------Guardo las fichas-------------------------------------------------------
                    foreach (HttpPostedFileBase postedFile in fichaFiles)
                    {

                        var ficha = new ArchivoFicha
                        {
                            ProductId = producto.ID,
                            Nombre = postedFile.FileName,
                            Archivo = postedFile.FileName

                        };
                        db.ArchivoFichas.Add(ficha);
                        var pathFichasProducto = Server.MapPath("~/Productos/" + producto.ID + "/Fichas/");
                        if (!Directory.Exists(pathFichasProducto))
                        {
                            Directory.CreateDirectory(pathFichasProducto);
                            postedFile.SaveAs(Path.Combine(pathFichasProducto, postedFile.FileName));
                        }
                        else
                        {
                            postedFile.SaveAs(Path.Combine(pathFichasProducto, postedFile.FileName));
                        }

                    }
                    

                    db.SaveChanges();
                    return RedirectToAction("Index","Productos");


                }
                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }

        }

            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre", producto.CategoriaID);
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre", producto.MarcaID);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre", producto.CategoriaID);
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre", producto.MarcaID);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MarcaID,CategoriaID,Nombre,Descripcion,Caracteristicas,CodigoDeProducto")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre", producto.CategoriaID);
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre", producto.MarcaID);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var findFichas = db.ArchivoFichas.Where(x => x.ProductId == id);
            foreach (var item in findFichas)
            {
                db.ArchivoFichas.Remove(item);
            }

            var findIES = db.ArchivoExtras.Where(x => x.ProductId == id);
            foreach (var item in findIES)
            {
                db.ArchivoExtras.Remove(item);
            }

            var findImagenes = db.ImagenProductoes.Where(x => x.ProductId == id);
            foreach (var item in findImagenes)
            {
                db.ImagenProductoes.Remove(item);
            }
            db.SaveChanges();
            Producto producto = db.Productoes.Find(id);
            db.Productoes.Remove(producto);
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
