using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace VonderkWEB.Models
{
    public class ProductDetailsViewModel
    {
        private Product product;
        private IEnumerable<Product> relatedProducts;
        private IEnumerable<ProductAsset> orderedImages;
        private IEnumerable<ProductAsset> orderedFichas;
        private IEnumerable<ProductAsset> orderedIES;

        public Product Product { get { return this.product; } }
        public IEnumerable<Product> RelatedProducts { get { return this.relatedProducts; } }
        public IEnumerable<ProductAsset> OrderedImages { get { return this.orderedImages; } }
        public IEnumerable<ProductAsset> OrderedFichas { get { return this.orderedFichas; } }
        public IEnumerable<ProductAsset> OrderedIES { get { return this.orderedIES; } }

        private LuminariaEntities db = new LuminariaEntities();
        public ProductDetailsViewModel()
        { }

        public ProductDetailsViewModel(int ProductID)
        {
            this.product = db.Products.First(x => x.ProductID == ProductID);
            this.relatedProducts = db.Products.Where(x => x.CategoryID == db.Products.FirstOrDefault(m => m.ProductID == ProductID).CategoryID);
            this.orderedImages = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "IMG").OrderBy(x => x.SortOrder);
            this.orderedFichas = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "PDF").OrderBy(x => x.SortOrder);
            this.orderedIES = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "IES").OrderBy(x => x.SortOrder);
        }

        public ProductDetailsViewModel(string ProductName)
        {
            var ProductID = db.Products.First(x => x.Name == ProductName).ProductID;
            this.product = db.Products.First(x => x.ProductID == ProductID);
            this.relatedProducts = db.Products.Where(x => x.CategoryID == db.Products.FirstOrDefault(m => m.ProductID == ProductID).CategoryID);
            this.orderedImages = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "IMG").OrderBy(x => x.SortOrder);
            this.orderedFichas = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "PDF").OrderBy(x => x.SortOrder);
            this.orderedIES = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "IES").OrderBy(x => x.SortOrder);
        }

        private void SaveAssets(int productID, string rootDir, string type, IEnumerable<HttpPostedFileBase> files, IEnumerable<KeyValuePair<string, string>> labels)
        {
            //--------------Chequeo y creo el directorio-----------------------------------------------
            string pathProduct = rootDir + "\\" + productID;
            if (!Directory.Exists(pathProduct))
            {
                Directory.CreateDirectory(pathProduct);
            }

            foreach (HttpPostedFileBase postedFile in files.Where(x => x != null))
            {
                ProductAsset asset = new ProductAsset
                {
                    ProductID = productID,
                    Name = labels.FirstOrDefault(x => x.Key == postedFile.FileName).Value,
                    AssetType = type,
                    IsActive = true,
                    FileName = "/Products/" + productID + "/" + type + "/" + postedFile.FileName,
                    SortOrder = 0,
                };
                db.ProductAssets.Add(asset);

                //No guarda dentro de la carpeta del producto
                //var pathImagenesProduct = rootDir + "\\" + type + "\\";
                var pathImagenesProduct = pathProduct + "\\" + type + "\\";
                if (!Directory.Exists(pathImagenesProduct))
                {
                    Directory.CreateDirectory(pathImagenesProduct);
                }
                postedFile.SaveAs(Path.Combine(pathImagenesProduct, postedFile.FileName));
            }

            foreach (var item in labels)
            {
                ProductAsset prd = db.ProductAssets.Where(x => x.FileName == item.Key).FirstOrDefault();
                if (prd != null) {

                    prd.Name = item.Value;

                }
            }

            //List<string> allKeys = (from kvp in labels select kvp.Key).ToList();
            //for (short i = 0; i < allKeys.Count; i++)
            //{
            //    var singleKey = allKeys[i];
            //    var m = db.ProductAssets.SingleOrDefault(x => x.Name == singleKey);
            //    m.SortOrder = i;

            //}

        }

        private void SaveImagesOrder(string imagesList, Product model) {

            if (imagesList != null) {
                var img = imagesList.Split('|').Where(x => x != "");
                short cont = 0;
                foreach (var item in img)
                {
                    var prod = db.ProductAssets.Where(p => p.ProductID == model.ProductID).FirstOrDefault(x => x.Name == item);
                    prod.SortOrder = cont;
                    cont++;
                }

                db.SaveChanges();
            }
           
        }

        public void Edit(Product model, string rootDir, string imagesList, string deletedAssets, string labeledAssets, List<HttpPostedFileBase> imageFiles, List<HttpPostedFileBase> fichaFiles, List<HttpPostedFileBase> iesFiles)
        {
            model.IsActive = true;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            var assetIDs = deletedAssets.Split(',').Where(x => x != "").Select(x => int.Parse(x));
            foreach (var item in assetIDs)
            {
                db.ProductAssets.Remove(db.ProductAssets.First(x => x.AssetID == item));
            }

            var lbls = labeledAssets.Split('|')
                .Where(x => x != "")
                .Select(x => new KeyValuePair<string, string>(x.Split('^')[0], x.Split('^')[1]));

            SaveAssets(model.ProductID, rootDir, "IMG", imageFiles, lbls);
            SaveAssets(model.ProductID, rootDir, "PDF", fichaFiles, lbls);
            SaveAssets(model.ProductID, rootDir, "IES", iesFiles, lbls);

            //db.SaveChanges();

            try
            {
                db.SaveChanges();
                SaveImagesOrder(imagesList, model);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        public void New(Product model, string rootDir,string imagesList, string labeledAssets, List<HttpPostedFileBase> imageFiles, List<HttpPostedFileBase> fichaFiles, List<HttpPostedFileBase> iesFiles)
        {
            //validar ProductID
            if (db.Products.Any(x => x.Name == model.Name))
            {
                throw new Exception("El Product con ese nombre / código de Product ya existe.");
            }

            //--------------Guardo el Product--------------------------------------------------------
            db.Products.Add(model);
            db.SaveChanges();

            var lbls = labeledAssets.Split('|')
                .Where(x => x != "")
                .Select(x => new KeyValuePair<string, string>(x.Split('^')[0], x.Split('^')[1]));

            SaveAssets(model.ProductID, rootDir, "IMG", imageFiles, lbls);
            SaveAssets(model.ProductID, rootDir, "PDF", fichaFiles, lbls);
            SaveAssets(model.ProductID, rootDir, "IES", iesFiles, lbls);
            
            db.SaveChanges();

            SaveImagesOrder(imagesList, model);
        }
    }
}