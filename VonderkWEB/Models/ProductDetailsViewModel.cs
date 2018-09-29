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
        private IEnumerable<ProductAsset> orderedImages;
        private IEnumerable<ProductAsset> orderedFichas;
        private IEnumerable<ProductAsset> orderedIES;

        public Product Product { get { return this.product; } }
        public IEnumerable<Product> RelatedProducts { get { return this.relatedProducts; } }
        public IEnumerable<ProductAsset> OrderedImages { get { return this.orderedImages; } }
        public IEnumerable<ProductAsset> OrderedFichas { get { return this.orderedFichas; } }
        public IEnumerable<ProductAsset> OrderedIES { get { return this.orderedIES; } }

        private LuminariaEntities db = new LuminariaEntities();

        public ProductDetailsViewModel(int ProductID)
        {
            this.product = db.Products.First(x => x.ProductID == ProductID);
            this.relatedProducts = db.Products.Where(x => x.CategoryID == db.Products.FirstOrDefault(m => m.ProductID == ProductID).CategoryID);
            this.orderedImages = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "IMG").OrderBy(x => x.SortOrder);
            this.orderedFichas = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "PDF").OrderBy(x => x.SortOrder);
            this.orderedIES = db.ProductAssets.Where(x => x.ProductID == ProductID && x.AssetType == "IES").OrderBy(x => x.SortOrder);
        }


    }
}