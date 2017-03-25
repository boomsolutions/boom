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
    public class LogAccesos //: System.Data.Entity
    {
        [Key]
        public int IdLogAccesos { get; set; }


        
        public DateTime? FechaLogin { get; set; }

        
        public DateTime? FechaLogout { get; set; }

        [Required]
        public string IP { get; set; } //?????????

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

    }
}