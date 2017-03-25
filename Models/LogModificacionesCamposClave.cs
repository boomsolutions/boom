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
    public class LogModificacionesCamposClave //: System.Data.Entity
    {
        [Key]
        public int IdLogModificacionesDetalle { get; set; }


        [Required]
        public int IdLogModificacionesCabezal { get; set; }

        [Required]
        public string CampoClave { get; set; }

        [Required]
        public string Valor { get; set; }

        [ForeignKey("IdLogModificacionesCabezal ")]
        public virtual LogModificacionesCabezal LogModificacionCabezal { get; set; }

    }
}