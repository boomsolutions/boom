namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoUsuarios",
                c => new
                    {
                        IdEstadoUsuario = c.Int(nullable: false, identity: true),
                        IdEstadoUsuarioFijo = c.Int(nullable: false),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdEstadoUsuario);
            
            AddColumn("dbo.Usuarios", "IdEstadoUsuario", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "MostrarPrecioDiario", c => c.Boolean(nullable: false));
            AddColumn("dbo.Productos", "MostrarPrecioSemanal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Productos", "MostrarPrecioMensual", c => c.Boolean(nullable: false));
            AddColumn("dbo.Productos", "MostrarPrecioFinDeSemana", c => c.Boolean(nullable: false));
            AddColumn("dbo.CategoriaProductos", "Visible", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Usuarios", "IdEstadoUsuario");
            
            AddForeignKey("dbo.Usuarios", "IdEstadoUsuario", "dbo.EstadoUsuarios", "IdEstadoUsuario", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "IdEstadoUsuario", "dbo.EstadoUsuarios");
            
            DropIndex("dbo.Usuarios", new[] { "IdEstadoUsuario" });
            DropColumn("dbo.CategoriaProductos", "Visible");
            DropColumn("dbo.Productos", "MostrarPrecioFinDeSemana");
            DropColumn("dbo.Productos", "MostrarPrecioMensual");
            DropColumn("dbo.Productos", "MostrarPrecioSemanal");
            DropColumn("dbo.Productos", "MostrarPrecioDiario");
            DropColumn("dbo.Usuarios", "IdEstadoUsuario");
            DropTable("dbo.EstadoUsuarios");
        }
    }
}
