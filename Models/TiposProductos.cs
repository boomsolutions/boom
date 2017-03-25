using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication4.Models
{
    public class TiposProductos //: System.Data.Entity
    {
        [Key]
        public int IdTipoProducto { get; set; }

        [Required]
        public int IdTipoProductoFijo { get; set; }
        
        [Required]
        public string DescripcionI1 { get; set; }
        /*
        public string DescripcionI2 { get; set; }

        public string DescripcionI3 { get; set; }
        */

        public virtual ICollection<Productos> Productos { get; set; }
        
    }


}