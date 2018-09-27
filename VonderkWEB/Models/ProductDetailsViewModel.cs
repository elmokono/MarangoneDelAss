using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VonderkWEB.Models
{
    public class ProductDetailsViewModel
    {
        private Producto producto;
        private IEnumerable<Producto> relatedProducts;
        private List<string> orderedImages;
        private List<ArchivoFicha> orderedFichas;
        private List<ArchivoExtra> orderedIES;

        public Producto Producto { get { return this.producto; } }
        public IEnumerable<Producto> RelatedProducts { get { return this.relatedProducts; } }
        public List<string> OrderedImages { get { return this.orderedImages; } }
        public List<ArchivoFicha> OrderedFichas { get { return this.orderedFichas; } }
        public List<ArchivoExtra> OrderedIES { get { return this.orderedIES; } }

        private LuminariaEntities db = new LuminariaEntities();

        public ProductDetailsViewModel(int productID)
        {
            this.producto = db.Productoes.First(x => x.ID == productID);
            this.relatedProducts = db.Productoes.Where(x => x.CategoriaID == db.Productoes.FirstOrDefault(m => m.ID == productID).CategoriaID);

            this.orderedImages = new List<string>();
            var order = db.ImagenProductoes.Where(x => x.ProductId == productID).OrderBy(x => x.Orden);
            foreach (var item in order)
            {
                this.orderedImages.Add(item.Nombre);
            }

            this.orderedFichas = new List<ArchivoFicha>();
            var orderFichas = db.ArchivoFichas.Where(x => x.ProductId == productID).OrderBy(x => x.Orden);
            foreach (var item in orderFichas)
            {
                this.orderedFichas.Add(item);
            }

            this.orderedIES = new List<ArchivoExtra>();
            var orderedIES = db.ArchivoExtras.Where(x => x.ProductId == productID).OrderBy(x => x.Orden);
            foreach (var item in orderedIES)
            {
                this.orderedIES.Add(item);
            }

        }


    }
}