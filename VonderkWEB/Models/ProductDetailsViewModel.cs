using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VonderkWEB.Models
{
    public class ProductDetailsViewModel
    {
        private Product product;
        private IEnumerable<Product> relatedProducts;
        private List<string> orderedImages;
        private List<ProductAsset> orderedFichas;
        private List<ProductAsset> orderedIES;

        public Product Product { get { return this.product; } }
        public IEnumerable<Product> RelatedProducts { get { return this.relatedProducts; } }
        public List<string> OrderedImages { get { return this.orderedImages; } }
        public List<ProductAsset> OrderedFichas { get { return this.orderedFichas; } }
        public List<ProductAsset> OrderedIES { get { return this.orderedIES; } }

        private LuminariaEntities db = new LuminariaEntities();

        public ProductDetailsViewModel(int ProductID)
        {
            this.product = db.Products.First(x => x.ProductID == ProductID);
            this.relatedProducts = db.Products.Where(x => x.CategoryID == db.Products.FirstOrDefault(m => m.ProductID == ProductID).CategoryID);

            this.orderedImages = new List<string>();
            var order = db.ProductAssets.Where(x => x.ProductID == ProductID).OrderBy(x => x.SortOrder);
            foreach (var item in order)
            {
                this.orderedImages.Add(item.Name);
            }

            this.orderedFichas = new List<ProductAsset>();
            var orderFichas = db.ProductAssets.Where(x => x.ProductID == ProductID).OrderBy(x => x.SortOrder);
            foreach (var item in orderFichas)
            {
                this.orderedFichas.Add(item);
            }

            this.orderedIES = new List<ProductAsset>();
            var orderedIES = db.ProductAssets.Where(x => x.ProductID == ProductID).OrderBy(x => x.SortOrder);
            foreach (var item in orderedIES)
            {
                this.orderedIES.Add(item);
            }

        }


    }
}