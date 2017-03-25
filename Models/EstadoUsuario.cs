using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace MvcApplication4.Models
{
    public class EstadoUsuario
    {
        [Key]
        public int IdEstadoUsuario { get; set; }

        public int IdEstadoUsuarioFijo { get; set; }

        public string DescripcionI1 { get; set; }
        public string DescripcionI2 { get; set; }
        public string DescripcionI3 { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}