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
    public class FotoUsuarios
    {
        [Key]
        public int IdFotoUsuario { get; set; }
        public int IdUsuario { get; set; }
        public string RutaFoto { get; set; }
        public int? IdImagen { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

        [ForeignKey("IdImagen")]
        public virtual Imagen Imagen { get; set; }

        //public virtual ImagenViewModel ImagenView { get; set; }
    }
}