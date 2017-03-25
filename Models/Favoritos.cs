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
    public class Favoritos //: System.Data.Entity
    {
        [Key]
        public int IdFavoritos { get; set; }


        public int IdProducto { get; set; }
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Productos Producto { get; set; }

    }
}