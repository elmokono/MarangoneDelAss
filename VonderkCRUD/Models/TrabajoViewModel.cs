using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VonderkCRUD.Models
{
    public class TrabajoViewModel
    {

        private Trabajo trabajo;
        private IEnumerable<Trabajo> relatedTrabajos;
        private List<string> orderedImages;


        public Trabajo Trabajo { get { return this.trabajo; } }
        public IEnumerable<Trabajo> RelatedTrabajos { get { return this.relatedTrabajos; } }
        public List<string> OrderedImages { get { return this.orderedImages; } }


        private ApplicationDbContext db = new ApplicationDbContext();


        public TrabajoViewModel(int trabajoID)
        {
            this.trabajo = db.Trabajos.First(x => x.ID == trabajoID);
            this.relatedTrabajos = db.Trabajos;

            var order = this.trabajo.Imagen.Split(',');
            this.orderedImages = new List<string>();
            for (int i = 0; i < order.Length; i++) { this.orderedImages.Add("/Images/Trabajos/" + this.trabajo.NombreCorregido + "/" + order[i]); }


        }


    }
}