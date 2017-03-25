namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA : DbMigration
    {
        public override void Up()
        {
            /*DropForeignKey("dbo.Productoes", "IdCategoriaProducto", "dbo.CategoriaProductoes");
            DropForeignKey("dbo.Alquilers", "IdProducto", "dbo.Productoes");
            DropForeignKey("dbo.Alquilers", "UsuarioArrendatario_IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.ComentarioAlquilers", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.ComentarioAlquilers", "IdAlquiler", "dbo.Alquilers");
            DropIndex("dbo.Productoes", new[] { "IdCategoriaProducto" });
            DropIndex("dbo.Alquilers", new[] { "IdProducto" });
            DropIndex("dbo.Alquilers", new[] { "UsuarioArrendatario_IdUsuario" });
            DropIndex("dbo.ComentarioAlquilers", new[] { "IdUsuario" });
            DropIndex("dbo.ComentarioAlquilers", new[] { "IdAlquiler" });
             */ 
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        IdCategoriaProducto = c.Int(nullable: false),
                        PrecioDiario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioSemanal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioMensual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GarantiaDinero = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GarantiaExtra = c.String(),
                        CondicionesUso = c.String(),
                    })
                .PrimaryKey(t => t.IdProducto)
                .ForeignKey("dbo.CategoriaProductos", t => t.IdCategoriaProducto, cascadeDelete: true)
                .Index(t => t.IdCategoriaProducto);
            
            CreateTable(
                "dbo.CategoriaProductos",
                c => new
                    {
                        IdCategoriaProducto = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdCategoriaProducto);
            
            CreateTable(
                "dbo.Alquileres",
                c => new
                    {
                        IdAlquiler = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        FechaDesde = c.DateTime(nullable: false),
                        FechaHasta = c.DateTime(nullable: false),
                        IdUsuarioArrendatario = c.Int(nullable: false),
                        UsuarioArrendatario_IdUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.IdAlquiler)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioArrendatario_IdUsuario)
                .Index(t => t.IdProducto)
                .Index(t => t.UsuarioArrendatario_IdUsuario);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Email = c.String(),
                        Telefono = c.String(),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Sexo = c.String(),
                        User = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.ComentarioAlquileres",
                c => new
                    {
                        IdComentarioAlquiler = c.Int(nullable: false, identity: true),
                        IdAlquiler = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Comentario = c.String(),
                    })
                .PrimaryKey(t => t.IdComentarioAlquiler)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .ForeignKey("dbo.Alquileres", t => t.IdAlquiler, cascadeDelete: true)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdAlquiler);
            /*
            DropTable("dbo.Productoes");
            DropTable("dbo.CategoriaProductoes");
            DropTable("dbo.Alquilers");
            DropTable("dbo.Usuarios");
            DropTable("dbo.ComentarioAlquilers");*/
        }
        
        public override void Down()
        {
            /*
            CreateTable(
                "dbo.ComentarioAlquilers",
                c => new
                    {
                        IdComentarioAlquiler = c.Int(nullable: false, identity: true),
                        IdAlquiler = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Comentario = c.String(),
                    })
                .PrimaryKey(t => t.IdComentarioAlquiler);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Email = c.String(),
                        Telefono = c.String(),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Sexo = c.String(),
                        User = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Alquilers",
                c => new
                    {
                        IdAlquiler = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        FechaDesde = c.DateTime(nullable: false),
                        FechaHasta = c.DateTime(nullable: false),
                        IdUsuarioArrendatario = c.Int(nullable: false),
                        UsuarioArrendatario_IdUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.IdAlquiler);
            
            CreateTable(
                "dbo.CategoriaProductoes",
                c => new
                    {
                        IdCategoriaProducto = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdCategoriaProducto);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        IdCategoriaProducto = c.Int(nullable: false),
                        PrecioDiario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioSemanal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioMensual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GarantiaDinero = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GarantiaExtra = c.String(),
                        CondicionesUso = c.String(),
                    })
                .PrimaryKey(t => t.IdProducto);
            */
            DropIndex("dbo.ComentarioAlquileres", new[] { "IdAlquiler" });
            DropIndex("dbo.ComentarioAlquileres", new[] { "IdUsuario" });
            DropIndex("dbo.Alquileres", new[] { "UsuarioArrendatario_IdUsuario" });
            DropIndex("dbo.Alquileres", new[] { "IdProducto" });
            DropIndex("dbo.Productos", new[] { "IdCategoriaProducto" });
            DropForeignKey("dbo.ComentarioAlquileres", "IdAlquiler", "dbo.Alquileres");
            DropForeignKey("dbo.ComentarioAlquileres", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Alquileres", "UsuarioArrendatario_IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Alquileres", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.Productos", "IdCategoriaProducto", "dbo.CategoriaProductos");
            DropTable("dbo.ComentarioAlquileres");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Alquileres");
            DropTable("dbo.CategoriaProductos");
            DropTable("dbo.Productos");
            /*CreateIndex("dbo.ComentarioAlquilers", "IdAlquiler");
            CreateIndex("dbo.ComentarioAlquilers", "IdUsuario");
            CreateIndex("dbo.Alquilers", "UsuarioArrendatario_IdUsuario");
            CreateIndex("dbo.Alquilers", "IdProducto");
            CreateIndex("dbo.Productoes", "IdCategoriaProducto");
            AddForeignKey("dbo.ComentarioAlquilers", "IdAlquiler", "dbo.Alquilers", "IdAlquiler", cascadeDelete: true);
            AddForeignKey("dbo.ComentarioAlquilers", "IdUsuario", "dbo.Usuarios", "IdUsuario", cascadeDelete: true);
            AddForeignKey("dbo.Alquilers", "UsuarioArrendatario_IdUsuario", "dbo.Usuarios", "IdUsuario");
            AddForeignKey("dbo.Alquilers", "IdProducto", "dbo.Productoes", "IdProducto", cascadeDelete: true);
            AddForeignKey("dbo.Productoes", "IdCategoriaProducto", "dbo.CategoriaProductoes", "IdCategoriaProducto", cascadeDelete: true);
             */ 
        }
    }
}
