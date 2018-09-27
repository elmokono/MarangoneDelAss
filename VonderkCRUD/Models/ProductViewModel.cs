using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VonderkCRUD.Models
{
    public class ProductViewModel
    {
        private Producto producto;
        private IEnumerable<Producto> relatedProducts;
        private List<string> orderedImages;
 


        public Producto Producto { get { return this.producto; } }
        public IEnumerable<Producto> RelatedProducts { get { return this.relatedProducts; } }
        public List<string> OrderedImages { get { return this.orderedImages; } }


        private ApplicationDbContext db = new ApplicationDbContext();

        //public ProductViewModel() {

        //}

        public ProductViewModel(int productID)
        {
            this.producto = db.Productos.First(x => x.ID == productID);
            this.relatedProducts = db.Productos.Where(x => x.CategoriaID == db.Productos.FirstOrDefault(m => m.ID == productID).CategoriaID);

            var order = this.producto.Imagen.Split(',');
            this.orderedImages = new List<string>();
            for (int i = 0; i < order.Length; i++) { this.orderedImages.Add("/Images/Productos/" + this.producto.CodProdCorregido + "/" + order[i]); }
   

        }



    }
}
