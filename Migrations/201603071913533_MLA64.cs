namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA64 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Formularios",
                c => new
                    {
                        IdFormulario = c.Int(nullable: false, identity: true),
                        Marca = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                        IdCategoriaProducto = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        PrecioEstimadoActual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioCompra = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TiempoVidaUtil = c.Int(nullable: false),
                        OpcionEnvio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFormulario)
                .ForeignKey("dbo.CategoriaProductos", t => t.IdCategoriaProducto, cascadeDelete: true)
                .Index(t => t.IdCategoriaProducto);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Formularios", "IdCategoriaProducto", "dbo.CategoriaProductos");
            DropIndex("dbo.Formularios", new[] { "IdCategoriaProducto" });
            DropTable("dbo.Formularios");
        }
    }
}
