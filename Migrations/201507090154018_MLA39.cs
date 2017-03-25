namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA39 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TiposProductos",
                c => new
                    {
                        IdTipoProducto = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdTipoProducto);
            
            AddColumn("dbo.Productos", "IdTipoProducto", c => c.Int(nullable: true));
            CreateIndex("dbo.Productos", "IdTipoProducto");
            AddForeignKey("dbo.Productos", "IdTipoProducto", "dbo.TiposProductos", "IdTipoProducto", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "IdTipoProducto", "dbo.TiposProductos");
            DropIndex("dbo.Productos", new[] { "IdTipoProducto" });
            DropColumn("dbo.Productos", "IdTipoProducto");
            DropTable("dbo.TiposProductos");
        }
    }
}
