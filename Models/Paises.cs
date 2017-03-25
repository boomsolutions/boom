using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace MvcApplication4.Models
{
    public class Paises
    {
        [Key]
        public int IdPais { get; set; }


        public string DescripcionI1 { get; set; }
        public string DescripcionI2 { get; set; }
        public string DescripcionI3 { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}