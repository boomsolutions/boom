namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoProductoes",
                c => new
                    {
                        IdEstadoProducto = c.Int(nullable: false, identity: true),
                        IdEstadoProductoFijo = c.Int(nullable: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdEstadoProducto);
            
            CreateTable(
                "dbo.Monedas",
                c => new
                    {
                        IdMoneda = c.Int(nullable: false, identity: true),
                        Simbolo = c.String(),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdMoneda);
            
            AddColumn("dbo.Productos", "IdMoneda", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "IdEstadoProducto", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "ConEnvio", c => c.Boolean(nullable: false));
            AddColumn("dbo.Productos", "PrecioEnvio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Productos", "IdMoneda");
            CreateIndex("dbo.Productos", "IdEstadoProducto");
            AddForeignKey("dbo.Productos", "IdEstadoProducto", "dbo.EstadoProductoes", "IdEstadoProducto", cascadeDelete: true);
            AddForeignKey("dbo.Productos", "IdMoneda", "dbo.Monedas", "IdMoneda", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "IdMoneda", "dbo.Monedas");
            DropForeignKey("dbo.Productos", "IdEstadoProducto", "dbo.EstadoProductoes");
            DropIndex("dbo.Productos", new[] { "IdEstadoProducto" });
            DropIndex("dbo.Productos", new[] { "IdMoneda" });
            DropColumn("dbo.Productos", "PrecioEnvio");
            DropColumn("dbo.Productos", "ConEnvio");
            DropColumn("dbo.Productos", "IdEstadoProducto");
            DropColumn("dbo.Productos", "IdMoneda");
            DropTable("dbo.Monedas");
            DropTable("dbo.EstadoProductoes");
        }
    }
}
