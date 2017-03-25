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
    // NO SE SI VAAAA?????
    public class LogNavegacion //: System.Data.Entity
    {
        [Key]
        public int IdLogNavegacion { get; set; }


        [Required]
        public DateTime Fecha { get; set; }


        [Required]
        public string Action { get; set; } //?????????

        [Required]
        public string Controller { get; set; } //?????????

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

    }
}