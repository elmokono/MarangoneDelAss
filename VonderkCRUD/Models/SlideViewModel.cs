using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VonderkCRUD.Models
{
    public class SlideViewModel
    {

        public IEnumerable<Categoria> Categorias { get; set; }
        public IEnumerable<SlidePrincipal> Slides { get; set; }
        public IEnumerable<Producto> Productos { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Categoria> GetCategorias()
        {
            List<Categoria> categoria = new List<Categoria>();

            categoria = db.Categorias.OrderBy(p => p.Orden).ToList();

            return categoria;
        }

        public List<SlidePrincipal> GetSlides()
        {
            List<SlidePrincipal> mySlides = new List<SlidePrincipal>();

            mySlides = db.SlidePrincipal.ToList();


            return mySlides;
        }


        public List<Producto> GetProductos()
        {
            List<Producto> myProducts = new List<Producto>();

            myProducts = db.Productos.OrderBy(p => p.Orden).ToList();

            return myProducts;
        }



    }
    

}