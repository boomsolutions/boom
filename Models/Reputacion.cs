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
    public class Reputacion //: System.Data.Entity
    {
        [Key]
        public int IdReputacion { get; set; }

        [Required]
        public int IdUsuarioEvaluado { get; set; }

        [Required]
        public int IdUsuarioEvalua { get; set; }
        //public int IdProductoEvaluado { get; set; }
        //public int IdAlquiler { get; set; }
        [Required]
        public string Comentario { get; set; }

        [ForeignKey("IdUsuarioEvaluado")]
        [InverseProperty("ReputacionRecibida")]
        public virtual Usuarios UsuarioEvaluado { get; set; }

        [ForeignKey("IdUsuarioEvalua")]
        [InverseProperty("ReputacionDada")]
        public virtual Usuarios UsuarioEvalua { get; set; }

    }
}