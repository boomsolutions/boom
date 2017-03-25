using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace MvcApplication4.Models
{
    public class ComentarioAlquileres
    {
        [Key]
        public int IdComentarioAlquiler { get; set; }
        public int IdAlquiler { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaComentario { get; set; }

        public virtual Usuarios Usuario { get; set; }
        public virtual Alquileres Alquiler { get; set; }
    }
}