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
    public class LogModificacionesDetalle //: System.Data.Entity
    {
        [Key]
        public int IdLogModificacionesDetalle { get; set; }


        [Required]
        public int IdLogModificacionesCabezal { get; set; }

        [Required]
        public string CampoModificado { get; set; }

        
        public string ValorAnterior { get; set; }

        
        public string ValorActual { get; set; }

        [ForeignKey("IdLogModificacionesCabezal ")]
        public virtual LogModificacionesCabezal LogModificacionCabezal { get; set; }


    }


}