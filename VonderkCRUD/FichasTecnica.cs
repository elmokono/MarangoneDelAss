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
    
    public partial class FichasTecnica
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public string Archivo { get; set; }
    
        public virtual Producto Producto { get; set; }
    }
}
