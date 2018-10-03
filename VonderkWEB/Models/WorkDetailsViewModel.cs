using System;
using System.Collections.Generic;
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


    }
}