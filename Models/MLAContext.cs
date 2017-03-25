using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace MvcApplication4.Models
{
    public class MLAContext: DbContext
    {
        public MLAContext()
            : base("name=DefaultConnection")
        {
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=USUARIO-HP\SQLEXPRESS;Initial Catalog=MLA;Integrated Security=SSPI");
            //Productos = (DbSet)Productos.Where(p => p.CategoriasProducto.Visible == true).ToList();
        }

        public DbSet<Productos> Productos { get; set; }

        public DbSet<Alquileres> Alquilers { get; set; }

        public DbSet<CategoriaProductos> CategoriaProductoes { get; set; }

        public DbSet<ComentarioAlquileres> ComentarioAlquilers { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<FotoProductos> FotoProductos { get; set; }

        public DbSet<Imagen> Imagens { get; set; }

        public DbSet<ComentariosProducto> ComentariosProducto { get; set; }

        public DbSet<Moneda> Monedas { get; set; }

        public DbSet<EstadoProducto> EstadosProducto { get; set; }

        public DbSet<EstadoAlquiler> EstadosAlquiler { get; set; }

        public DbSet<EstadoUsuario> EstadosUsuario { get; set; }

        public DbSet<LogAccesos> LogsAccesos { get; set; }

        public DbSet<LogBusquedas> LogsBusquedas { get; set; }

        public DbSet<LogModificacionesCabezal> LogsModificacionesCabezales { get; set; }

        public DbSet<LogModificacionesCamposClave> LogsModificacionesCamposClaves { get; set; }

        public DbSet<LogModificacionesDetalle> LogsModificacionesDetalles { get; set; }

        public DbSet<LogNavegacion> LogsNavegaciones { get; set; }

        public DbSet<Favoritos> Favoritos { get; set; }

        public DbSet<FotoUsuarios> FotosUsuarios { get; set; }

        public DbSet<Barrios> Barrios { get; set; }

        public DbSet<LogVisitasProductos> LogsVisitasProductos { get; set; }

        public DbSet<TiposProductos> TiposProductos { get; set; }

        public DbSet<Departamentos> Departamentos { get; set; }

        public DbSet<Ciudades> Ciudades { get; set; }

        public DbSet<Paises> Paises { get; set; }

        public DbSet<LogVisitas> LogsVisitas { get; set; }

        public DbSet<Suscriptor> Suscriptores { get; set; }

        public DbSet<Configuraciones> Configuraciones { get; set; }

        public DbSet<Formulario> Formularios { get; set; }

        //public DbSet<Calificacion> Calificaciones { get; set; }
    }
}