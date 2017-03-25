using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using MvcApplication4.Controllers;

namespace MvcApplication4.Models
{
    public class Configuraciones
    {
        [Key]
        public int IdConfiguracion { get; set; }

        [Required]
        public string Parametro { get; set; }
        [Required]
        public string Valor { get; set; }
        [Required]
        public string Descripcion { get; set; }
        
    }
}