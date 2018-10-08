using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace VonderkWEB.Models
{
    public class WorkDetailsViewModel
    {
        private Work work;
        private IEnumerable<Work> relatedWorks;

        public Work Work { get { return this.work; } }
        public IEnumerable<Work> RelatedWorks { get { return this.relatedWorks; } }

        private LuminariaEntities db = new LuminariaEntities();
        public WorkDetailsViewModel()
        { }

        public WorkDetailsViewModel(int WorkID)
        {
            this.work = db.Works.First(x => x.WorkID == WorkID);
            this.relatedWorks = db.Works.Where(x => x.IsActive == true);

        }

        public WorkDetailsViewModel(string WorkName)
        {
            var WorkID = db.Works.First(x => x.Name == WorkName).WorkID;
            this.work = db.Works.First(x => x.WorkID == WorkID);
            this.relatedWorks = db.Works.Where(x => x.IsActive == true);

        }

        public void Edit(Work model, string rootDir, string imagesList, string deletedAssets, List<HttpPostedFileBase> imageFiles)
        {
            model.IsActive = true;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            var assetIDs = deletedAssets.Split(',').Where(x => x != "").Select(x => int.Parse(x));
            foreach (var item in assetIDs)
            {
                db.WorkAssets.Remove(db.WorkAssets.First(x => x.AssetID == item));
            }

           
            SaveAssets(model.WorkID, rootDir, "IMG", imageFiles);
         
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

        private void SaveAssets(int workID, string rootDir, string type, IEnumerable<HttpPostedFileBase> files)
        {
            //--------------Chequeo y creo el directorio-----------------------------------------------
            string pathWork = rootDir + "\\" + workID;
            if (!Directory.Exists(pathWork))
            {
                Directory.CreateDirectory(pathWork);
            }

            foreach (HttpPostedFileBase postedFile in files.Where(x => x != null))
            {
                WorkAsset asset = new WorkAsset
                {
                    WorkID = workID,
                    Name = postedFile.FileName,
                    AssetType = type,
                    IsActive = true,
                    FileName = "/Images/Works/" + workID + "/" + postedFile.FileName,
                    SortOrder = 0,
                };
                db.WorkAssets.Add(asset);

              
                var pathImagenesProduct = pathWork + "\\" ;
                if (!Directory.Exists(pathImagenesProduct))
                {
                    Directory.CreateDirectory(pathImagenesProduct);
                }
                postedFile.SaveAs(Path.Combine(pathImagenesProduct, postedFile.FileName));
            }


        }

        private void SaveImagesOrder(string imagesList, Work model)
        {

            if (imagesList != null)
            {
                var img = imagesList.Split('|').Where(x => x != "");
                short cont = 0;
                foreach (var item in img)
                {
                    var wrk = db.WorkAssets.Where(p => p.WorkID == model.WorkID).FirstOrDefault(x => x.Name == item);
                    wrk.SortOrder = cont;
                    cont++;
                }

                db.SaveChanges();
            }

        }

        public void New(Work model, string rootDir, string imagesList, List<HttpPostedFileBase> imageFiles)
        {
            

            //--------------Guardo el Product--------------------------------------------------------
            db.Works.Add(model);
            db.SaveChanges();

            SaveAssets(model.WorkID, rootDir, "IMG", imageFiles);
         
            db.SaveChanges();

            SaveImagesOrder(imagesList, model);
        }

    }
}