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
    public class LogBusquedas //: System.Data.Entity
    {
        [Key]
        public int IdLogBusquedas { get; set; }

        [Required]
        public DateTime FechaBusqueda { get; set; }

        public string TextoBusqueda { get; set; }

        [Required]
        public string IP { get; set; }

        public int? IdCategoriaProducto { get; set; }


        
        public int? IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

        [ForeignKey("IdCategoriaProducto")]
        public virtual CategoriaProductos CategoriaProducto { get; set; }

    }

}