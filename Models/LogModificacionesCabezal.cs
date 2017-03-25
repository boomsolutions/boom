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
    public class LogModificacionesCabezal //: System.Data.Entity
    {
        [Key]
        public int IdLogModificacionesCabezal { get; set; }


        [Required]
        public DateTime Fecha { get; set; }

        public int IdUsuario { get; set; }

        [Required]
        //public string Accion { get; set; }
        public int Accion { get; set; } //1 - alta; 2 - modificacion; 3 - borrado;

        [Required]
        public string Tabla { get; set; }

        //[Required]
        //public string Descripcion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

        public virtual ICollection<LogModificacionesDetalle> LogModificacionesDetalle { get; set; }
        public virtual ICollection<LogModificacionesCamposClave> LogModificacionesCamposClave { get; set; }


    }

}