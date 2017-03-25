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
    public class FotoProductos
    {
        [Key]
        public int IdFotoProducto { get; set; }
        public int IdProducto { get; set; }
        public string RutaFoto { get; set; }
        public int? IdImagen { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Productos Producto { get; set; }

        [ForeignKey("IdImagen")]
        public virtual Imagen Imagen { get; set; }

        //public virtual ImagenViewModel ImagenView { get; set; }
    }
}