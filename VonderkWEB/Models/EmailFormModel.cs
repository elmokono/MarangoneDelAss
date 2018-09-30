using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VonderkWEB.Models
{
    public class EmailFormModel
    {

        [Required, Display(Name = "Nombre")]
        public string FromName { get; set; }

        [Required, Display(Name = "Telefono"), Phone]
        public string FromPhone { get; set; }

        [Required, Display(Name = "Email"), EmailAddress]
        public string FromEmail { get; set; }

        [Display(Name = "Empresa")]
        public string FromCompany { get; set; }

        [Required, Display(Name = "Mensaje")]
        public string Message { get; set; }

    }
}