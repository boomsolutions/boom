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
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Nombre es un campo requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Apellido es un campo requerido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Email es un campo requerido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Teléfono es un campo requerido.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Fecha de nacimiento es un campo requerido.")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "Sexo es un campo requerido.")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "Usuario es un campo requerido.")]
        public string User { get; set; }

        [Required(ErrorMessage = "Contraseña es un campo requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Las contraseñas deben coincidir.")]
        [Required(ErrorMessage = "Confirmar Contraseña es un campo requerido.")]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }

        public int? IdBarrio { get; set; }

        public int? IdDepartamento { get; set; }

        public int? IdCiudad { get; set; }

        public int? IdPais { get; set; }

        public int IdEstadoUsuario { get; set; }

        public bool? EsUsuarioFacebook { get; set; }

        public string UsuarioFacebookID { get; set; }

        //
       /*
        [Display(Name = "Términos y condiciones")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Debes aceptar los términos y condiciones.")]
        */
        //[MustBeTrue(ErrorMessage = "Debes aceptar los términos y condiciones.")]

        [Required(ErrorMessage = "Debes aceptar los términos y condiciones.")]
        public bool AceptoTerminosYCondiciones { get; set; }

        [ForeignKey("IdBarrio")]
        public virtual Barrios Barrio { get; set; }

        [ForeignKey("IdDepartamento")]
        public virtual Departamentos Departamento { get; set; }

        [ForeignKey("IdCiudad")]
        public virtual Ciudades Ciudad { get; set; }

        [ForeignKey("IdPais")]
        public virtual Paises Pais { get; set; }

        [ForeignKey("IdEstadoUsuario")]
        public virtual EstadoUsuario EstadoUsuario { get; set; }

        public virtual ICollection<Alquileres> Alquileres { get; set; }
        public virtual ICollection<ComentarioAlquileres> ComentariosAlquiler { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }

        public virtual ICollection<Favoritos> Favoritos { get; set; }

        public virtual ICollection<LogAccesos> LogAccesos { get; set; }

        public virtual ICollection<LogBusquedas> LogBusquedas { get; set; }

        public virtual ICollection<LogNavegacion> LogNavegacion { get; set; }

        public virtual ICollection<LogModificacionesCabezal> LogModificacionesCabezal { get; set; }

        
        public virtual ICollection<FotoUsuarios> FotosUsuarios { get; set; }

        public virtual ICollection<Reputacion> ReputacionDada { get; set; }

        public virtual ICollection<Reputacion> ReputacionRecibida { get; set; }

        public virtual ICollection<ComentariosProducto> ComentariosProducto { get; set; }


    }
}