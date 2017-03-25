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
    public class Alquileres
    {
        [Key]
        public int IdAlquiler { get; set; }
        public int IdProducto { get; set; }
        public int IdUsuarioArrendatario { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaDesde { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; }

        public int IdEstadoAlquiler { get; set; }

        public bool Leido { get; set; }

        //Evaluacion del Arrendatario  hacia el Arrendador
        public int CalificacionArrendatario { get; set; }
        public bool CalificacionPositivaArrendatario { get; set; }
        public string ComentarioArrendatario { get; set; }

        public string ComentarioExperienciaArrendatario { get; set; }
        public string ComentarioPrivadoArrendatario { get; set; }

        public int CalificacionCuidadoArrendatario { get; set; }
        public int CalificacionComunicacionArrendatario { get; set; }
        public int CalificacionCumplimientoNormasArrendatario { get; set; }

        //Evaluacion del Arrendador hacia el Arrendatario

        public string ComentarioExperienciaArrendador { get; set; }
        public string ComentarioPrivadoArrendador { get; set; }
 
        public int CalificacionCuidadoArrendador {get; set;}
        public int CalificacionComunicacionArrendador { get; set; }
        public int CalificacionCumplimientoNormasArrendador { get; set; }

        public int CalificacionArrendador { get; set; }
        public string ComentarioArrendador { get; set; }
        public bool CalificacionPositivaArrendador { get; set; }

        //FK

        public virtual Productos Producto { get; set; }

        [ForeignKey("IdUsuarioArrendatario")]
        public virtual Usuarios UsuarioArrendatario { get; set; }

        [ForeignKey("IdEstadoAlquiler")]
        public virtual EstadoAlquiler EstadoAlquiler { get; set; }


        public virtual ICollection<ComentarioAlquileres> ComentariosAlquiler { get; set; }

        

    }
}