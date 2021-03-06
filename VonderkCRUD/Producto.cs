//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VonderkCRUD
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            this.ProductImages = new HashSet<ProductImage>();
            this.FichasTecnicas = new HashSet<FichasTecnica>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string DescripcionCorta { get; set; }
        public string DescripcionLarga { get; set; }
        public string Imagen { get; set; }
        [AllowHtml]
        public string Caracteristicas { get; set; }
        public Nullable<int> Garantia { get; set; }
        public string FichaTecnica { get; set; }
        public string IES { get; set; }
        public Nullable<int> MarcaID { get; set; }
        public Nullable<int> CategoriaID { get; set; }
        public string CodigoDeProd { get; set; }
        public string CodProdCorregido { get; set; }
        public Nullable<int> Orden { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        public virtual Marca Marca { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FichasTecnica> FichasTecnicas { get; set; }
    }
}
