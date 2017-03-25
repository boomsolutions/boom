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
    public class Suscriptor
    {
        [Key]
        public int IdSuscriptor { get; set; }

        [Required(ErrorMessage = "Nombre es un campo requerido.")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "Email es un campo requerido.")]
        public string Email { get; set; }

        public DateTime? FechaSuscripto { get; set; }

        [Required(ErrorMessage = "Email es un campo requerido.")]
        public int IdDepartamento { get; set; }

        public int? IdBarrio { get; set; }

        [Required(ErrorMessage = "Fecha de nacimiento es un campo requerido.")]
        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("IdDepartamento")]
        public virtual Departamentos Departamento { get; set; }

        [ForeignKey("IdBarrio")]
        public virtual Barrios Barrio { get; set; }
    }
}