using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VonderkWEB.Models
{
    public class HomeViewModel
    {


        private IEnumerable<Product> products;
        private IEnumerable<Category> categories;
        private IEnumerable<Slide> slides;
        //private IEnumerable<Brand> brands;
        //private List<string> orderedImages;



        public IEnumerable<Product> Products { get { return this.products; } }
        public IEnumerable<Category> Categories { get { return this.categories; } }
        public IEnumerable<Slide> Slides { get { return this.slides; } }
        //public IEnumerable<Brand> Brands { get { return this.brands; } }

        private LuminariaEntities db = new LuminariaEntities();

        public HomeViewModel()
        {

        }

        public HomeViewModel Get()
        {
            return Get(String.Empty, 0, 0);
        }

        public HomeViewModel Get(string text = "", int categoryID = 0, int brandID = 0)
        {
            return new HomeViewModel
            {
                categories = db.Categories.Where(x => x.IsActive && (categoryID == 0 || x.CategoryID == categoryID)),
                products = db.Products.Where(x => x.IsActive && (text == String.Empty || (x.Description.Contains(text) || x.Features.Contains(text) || x.Name.Contains(text)))),
                slides = db.Slides.Where(x => x.IsActive),
            };
        }








    }
}
