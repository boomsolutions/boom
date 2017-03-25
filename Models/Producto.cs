﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using Foolproof;

namespace MvcApplication4.Models
{
    public class Productos //: System.Data.Entity
    {
        [Key]
        public int IdProducto {get; set;}

        [Required(ErrorMessage = "Título es un campo requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripción es un campo requerido.")]
        public string Descripcion { get; set; }
        
        public int IdCategoriaProducto { get; set; }
        
        [DataType(DataType.Currency)]
        //[RequiredIfEmpty("PrecioSemanal")]
        public decimal? PrecioDiario { get; set; }

        [DataType(DataType.Currency)]
        //[RequiredIfEmpty("PrecioMensual")]
        public decimal? PrecioSemanal { get; set; }

        [DataType(DataType.Currency)]
        //[RequiredIfEmpty("PrecioFinDeSemana")]
        public decimal? PrecioMensual { get; set; }

        [DataType(DataType.Currency)]
        //[RequiredIf("PrecioDiario", Foolproof.Operator.EqualTo, "")]
        public decimal? PrecioFinDeSemana { get; set; }

        [DataType(DataType.Currency)]
        //[RequiredIf("PrecioDiario", Foolproof.Operator.EqualTo, "")]
        public decimal? PrecioHora { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Garantía es un campo requerido.")]
        public decimal GarantiaDinero { get; set; }

        public string GarantiaExtra { get; set; }
        
        public string CondicionesUso { get; set; }

        public string Ubicacion { get; set; }

        [DataType(DataType.Currency)]
        public decimal? PrecioDeclarado { get; set; }

        public int? OpcionEntrega { get; set; }

        public int IdUsuario { get; set; }

        public int IdMoneda { get; set; }

        public int IdEstadoProducto { get; set; }

        public bool ConEnvio { get; set; }

        [DataType(DataType.Currency)]
        public decimal? PrecioEnvio { get; set; }

        public int? IdBarrio { get; set; }

        public int? IdDepartamento { get; set; }

        public int? IdCiudad { get; set; }

        public int? IdPais { get; set; }

        

        public string Zona { get; set; }

        public bool MostrarPrecioDiario { get; set; }

        public bool MostrarPrecioSemanal { get; set; }

        public bool MostrarPrecioMensual { get; set; }

        public bool MostrarPrecioFinDeSemana { get; set; }

        public bool MostrarPrecioHora { get; set; }

        public int CantVisitas { get; set; }

        public int IdTipoProducto { get; set; }

        public string ComentarioEnvio { get; set; }

        public string NombreBusqueda { get; set; }
        public string DescripcionBusqueda { get; set; }

        // FK

        [ForeignKey("IdCategoriaProducto")]
        public virtual CategoriaProductos CategoriasProducto { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }

        [ForeignKey("IdMoneda")]
        public virtual Moneda Moneda { get; set; }

        [ForeignKey("IdEstadoProducto")]
        public virtual EstadoProducto EstadoProducto { get; set; }

        [ForeignKey("IdBarrio")]
        public virtual Barrios Barrio { get; set; }

        [ForeignKey("IdDepartamento")]
        public virtual Departamentos Departamento { get; set; }

        [ForeignKey("IdCiudad")]
        public virtual Ciudades Ciudad { get; set; }

        [ForeignKey("IdPais")]
        public virtual Paises Pais { get; set; }


        [ForeignKey("IdTipoProducto")]
        public virtual TiposProductos TipoProducto { get; set; }

        // FK multiples

        public virtual ICollection<Alquileres> Alquileres { get; set; }

        public virtual ICollection<FotoProductos> FotosProductos { get; set; }

        public virtual ICollection<ComentariosProducto> ComentariosProducto { get; set; }

        public virtual ICollection<Favoritos> Favoritos { get; set; }
        //con este codigo dejo que pertenezca a mas de una categoria
        //public DbSet<CategoriaProducto> CategoriaProducto { get; set; }
        //public DbSet<SubCategoriaProducto> CategoriaProducto { get; set; }
        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Productos>()
                .HasMany(fp => fp.FotosProductos)
                .WithRequired();

            
            modelBuilder.Entity<ContractMedia>()
                .HasKey(c => new { c.MediaId, c.ContractId });

            modelBuilder.Entity<Contract>()
                .HasMany(c => c.ContractMedias)
                .WithRequired()
                .HasForeignKey(c => c.ContractId);

            modelBuilder.Entity<Media>()
                .HasMany(c => c.ContractMedias)
                .WithRequired()
              *  .HasForeignKey(c => c.MediaId);
             *  
        }*/

        //public Productos()
        //{
            //DbSet<FotoProductos> d = new DbSet<FotoProductos>();
            //FotosProductos = new List<FotoProductos>(); //new DbSet<FotoProductos>();
            //Alquileres = DbSet<Alquileres>();
        //}
    }


    // Custom comparer for the Product class
    class ProductComparer : IEqualityComparer<Productos>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Productos x, Productos y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.IdProducto == y.IdProducto;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Productos product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null.
            //int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = product.IdProducto.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductCode;
        }

    }

}