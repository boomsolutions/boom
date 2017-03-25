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
    public class ComentariosProducto
    {
        [Key]
        public int IdComentariosProducto { get; set; }
        public int IdProducto { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime FechaComentario { get; set; }

        public int? IdComentariosProductoOrigen { get; set; }
        
        public bool Leido { get; set; }

        public bool EsRespuesta { get; set; }

        public virtual Usuarios Usuario { get; set; }
        public virtual Productos Producto { get; set; }

        [ForeignKey("IdComentariosProductoOrigen")]
        public virtual ComentariosProducto ComentarioProductoOrigen { get; set; }
    }
}