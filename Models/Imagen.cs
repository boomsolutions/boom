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
    public class Imagen
    {
        [Key]
        public int IdImagen { get; set; }
        

        public string Titulo { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }


        [Required]
        [DataType(DataType.ImageUrl)]
        public string RutaFoto { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string RutaFotoThumb { get; set; }

        public DateTime? fechaCreacion;

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion 
        {
            get { return fechaCreacion ?? DateTime.UtcNow; }
            set { fechaCreacion = value; }
        }
    }
}