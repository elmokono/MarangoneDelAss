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
using VonderkCRUD.Models;

namespace VonderkCRUD
{
    [Authorize]
    public class ProductosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Productos
        [AllowAnonymous]
        public ActionResult Index()
        {
            SlideViewModel mymodel = new SlideViewModel();
            mymodel.Categorias = mymodel.GetCategorias();
            mymodel.Slides = mymodel.GetSlides();
            mymodel.Productos = mymodel.GetProductos();

            return View(mymodel);
        }

        // GET: Productos
        public ActionResult AdminIndex()
        {

            SlideViewModel mymodel = new SlideViewModel();
            mymodel.Categorias = mymodel.GetCategorias();
            mymodel.Slides = mymodel.GetSlides();
            mymodel.Productos = mymodel.GetProductos();

            return View(mymodel);

        }

        // GET: Productos/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue) { return HttpNotFound(); }
            ProductViewModel model = new ProductViewModel(id.Value);

            return View(model);



        }
    


        // GET: Productos/Create
        public ActionResult Create()
        {

            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre");
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre");
            return View();
        }


        [HttpPost]
        public ActionResult FirstAjax(string[] listValues)
        {

            int cont = 0;

            foreach (var item in listValues)
            {
                var m = db.Productos.SingleOrDefault(x => x.Nombre == item);
                m.Orden = cont;
                cont++;
            }
            db.SaveChanges();

            return Json("Se cambio el orden correctamente", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,DescripcionCorta,DescripcionLarga,Imagen,Caracteristicas,Garantia,FichaTecnica,IES,MarcaID,CategoriaID")]List<HttpPostedFileBase> filesuploadtecnica, HttpPostedFileBase filesuploadies, List<HttpPostedFileBase> postedFiles, Producto producto, String hiddenField)
        {
         


            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {

                try
                {

                    //validar ProductID
                    if (db.Productos.Any(x => x.Nombre == producto.Nombre) || db.Productos.Any(x => x.CodigoDeProd == producto.CodigoDeProd))
                    {
                        throw new Exception("El producto con ese nombre / código de producto ya existe.");
                    }

                    StringBuilder sb = new StringBuilder();
                    if (producto.CodigoDeProd != null)
                    {
                        foreach (char c in producto.CodigoDeProd)
                        {
                            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                            {
                                sb.Append(c);
                            }
                        }


                    }
                    else
                    {

                        throw new Exception("You have not specified a Product Code.");

                    }
                    string codProdCorregido = sb.ToString();

                    //-----------STATIC PATHS-------------------
                    var path1 = Server.MapPath("~/Images/");
                    var path3 = Server.MapPath("~/Fichas/");
                    var path5 = Server.MapPath("~/IES/");


                    if (filesuploadtecnica != null)
                    {
                        foreach (HttpPostedFileBase postedFile in filesuploadtecnica)
                        {
                            //string extensionficha = Path.GetExtension(postedFile.FileName);
                            //var path4 = codProdCorregido + extensionficha;
                            postedFile.SaveAs(Path.Combine(path3, postedFile.FileName));
                            var ficha = new FichasTecnica
                            {
                                Archivo = postedFile.FileName,
                                
                                
                            };
                            db.FichasTecnicas.Add(ficha);
                            db.SaveChanges();
                        }
                       
                      
                    }
                    //else
                    //{
                    //    producto.FichaTecnica = "vacio";
                    //    //throw new Exception("You have not specified a file.");
                    //}


                    if (filesuploadies != null && filesuploadies.ContentLength > 0)
                    {
                        string extensionies = Path.GetExtension(filesuploadies.FileName);
                        var path6 = codProdCorregido + extensionies;
                        //filesupload.SaveAs(Server.MapPath("~/IMG") + "\\" + producto.CodigoDeProd);
                        filesuploadies.SaveAs(Path.Combine(path5, path6));
                        producto.IES = path6;
                    }
                    else
                    {
                        producto.IES = "vacio";
                        //throw new Exception("You have not specified a file.");
                    }

                    string[] words;
                    char[] delimiterChars = { ',', '[', ']', '\"' };

                    if (hiddenField.Length > 0)
                    {


                        words = hiddenField.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                    }
                    else {
                        string newHiddenField = null;
                        foreach (HttpPostedFileBase postedFile in postedFiles)
                        {
                            newHiddenField = newHiddenField + postedFile.FileName + ',';
                           

                        }
                        words = newHiddenField.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                    }

                   


                    if (postedFiles != null)
                    {

                        string buildPath = "~/Images/Productos/" + codProdCorregido + "/";
                        string path = Server.MapPath(buildPath);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                      
                       
                            foreach (HttpPostedFileBase postedFile in postedFiles)
                            {                        
                                    //string extension = Path.GetExtension(postedFile.FileName);
                                    var pathFinal = postedFile.FileName;
                                    //string fileName = postedFile.FileName + pathFinal;
                                    postedFile.SaveAs(path + pathFinal);

                            }


                        //if (hiddenField.Length > 0)
                        //{


                        //    var cont = 0;
                        //    foreach (var item in words)
                        //    {

                        //        //Si el item es igual al postedfile de postedFiles, guardarlo
                        //        foreach (HttpPostedFileBase postedFile in postedFiles)
                        //        {
                        //            if (postedFile != null && postedFile.FileName.Equals(item))
                        //            {
                        //                string extension = Path.GetExtension(postedFile.FileName);
                        //                var pathFinal = codProdCorregido + extension;
                        //                string fileName = cont + pathFinal;

                        //                    postedFile.SaveAs(path + fileName);


                        //                cont++;

                        //            }


                        //        }


                        //    }
                        //}
                        //else
                        //{
                        //    var cont = 0;
                        //    //TO DO QUE SE GUARDEN EN EL ORDEN QUE VIENEN EN POSTEDFILES
                        //    foreach (HttpPostedFileBase postedFile in postedFiles)
                        //    {
                        //        if (postedFile != null)
                        //        {
                        //            string extension = Path.GetExtension(postedFile.FileName);
                        //            var pathFinal = codProdCorregido + extension;
                        //            string fileName = cont + pathFinal;
                        //            if (cont == 0)
                        //            {
                        //                postedFile.SaveAs(path + fileName);
                        //                producto.Imagen = fileName;
                        //            }
                        //            else
                        //            {
                        //                postedFile.SaveAs(path + fileName);
                        //            }
                        //            cont++;

                        //        }

                        //    }

                        //    var firstElement = postedFiles.First().FileName;
                        //    producto.Imagen = firstElement;

                        //}

                    }
                    

                    string result = string.Join(",", words);

                    producto.Imagen = result;
                    producto.CodProdCorregido = codProdCorregido;
                    db.Productos.Add(producto);
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

            if (!id.HasValue) { return HttpNotFound(); }
            ProductViewModel model = new ProductViewModel(id.Value);
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Producto producto = db.Productos.Find(id);
            //if (producto == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre", model.Producto.CategoriaID);
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre", model.Producto.MarcaID);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<HttpPostedFileBase> postedFiles, HttpPostedFileBase filesuploadtecnica, HttpPostedFileBase filesuploadies, Producto producto, String hiddenField)
        {



            //Estabilizo el Código de producto, sin caracteres especiales
            StringBuilder sb = new StringBuilder();
            foreach (char c in producto.CodigoDeProd)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }

            string codCorregido = sb.ToString();

            //codCorregido = sb.ToString();

            var p = db.Productos.Single(x => x.ID == producto.ID);

           
            var pathImg = Server.MapPath("~/Images/Productos/");
            var pathFullImg = Server.MapPath("~/Images/Productos/" + producto.CodProdCorregido + "/");
           
            var pathFichas = Server.MapPath("~/Fichas/");
            var pathIes = Server.MapPath("~/IES/");


            if (!Directory.Exists(pathFullImg))
            {
                Directory.CreateDirectory(pathFullImg);
            }



            if (ModelState.IsValid)
            {
                try
                {


                    if (filesuploadtecnica != null)
                    {
                        string extension = Path.GetExtension(filesuploadtecnica.FileName);
                        var path2 = codCorregido + extension;
                        //filesupload.SaveAs(Server.MapPath("~/IMG") + "\\" + producto.CodigoDeProd);
                        filesuploadtecnica.SaveAs(Path.Combine(pathFichas, path2));
                        p.FichaTecnica = path2;
                    }
                    /*else
                    {
                        p.FichaTecnica = pathOnBlankFileTecnica;
                        //throw new Exception("You have not specified a file.");
                    }*/

                    if (filesuploadies != null)
                    {
                        string extension = Path.GetExtension(filesuploadies.FileName);
                        var path2 = codCorregido + extension;
                        //filesupload.SaveAs(Server.MapPath("~/IMG") + "\\" + producto.CodigoDeProd);
                        filesuploadies.SaveAs(Path.Combine(pathIes, path2));
                        p.IES = path2;
                    }


                    string[] words;
                    char[] delimiterChars = { ',', '[', ']', '\"' };
                    words = hiddenField.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                    if (postedFiles[0] != null)
                    {

                        string buildPath = "~/Images/Productos/" + codCorregido + "/";
                        string path = Server.MapPath(buildPath);

                      

                        foreach (HttpPostedFileBase postedFile in postedFiles)
                        {


                            string fileName = postedFile.FileName;
                            string sourcePath = pathFullImg;
                            string targetPath = pathFullImg;

                            // Create the path and file name to check for duplicates.
                            string pathToCheck = pathFullImg + fileName;

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
                                    pathToCheck = pathFullImg + tempfileName;
                                    counter++;

                                }

                                fileName = tempfileName;

                            }

                            var pathFinal = fileName;
                            postedFile.SaveAs(path + pathFinal);

                        }


                    }


                    string result = string.Join(",", words);

                    producto.Imagen = result;


                    p.Imagen = producto.Imagen;
                    p.MarcaID = producto.MarcaID;
                    p.CategoriaID = producto.CategoriaID;
                    p.Nombre = producto.Nombre;
                    p.CodigoDeProd = producto.CodigoDeProd;
                    p.CodProdCorregido = codCorregido;
                    p.Garantia = producto.Garantia;
                    p.Caracteristicas = producto.Caracteristicas;


                    db.SaveChanges();
                    return RedirectToAction("AdminIndex", "Productos");

                }
                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message.ToString();
                }


            }
            else
            {

                ViewBag.Message = "Model State not valid";

            }


            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Nombre", producto.CategoriaID);
            ViewBag.MarcaID = new SelectList(db.Marcas, "ID", "Nombre", producto.MarcaID);
            return RedirectToAction("AdminIndex", "Productos");


            //return View(producto);

        }




        

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
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
            Producto producto = db.Productos.Find(id);


            string buildPath = "~/IMG/" + producto.CodProdCorregido + "/";
            string pathDirectory = Server.MapPath(buildPath);

            if (Directory.Exists(pathDirectory))
            {
                //Limpiar el contenido de la carpeta
                DirectoryInfo di = new DirectoryInfo(pathDirectory);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                Directory.Delete(pathDirectory);

            }

            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("AdminIndex","Productos");
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
