namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FotoProductos",
                c => new
                    {
                        IdFotoProducto = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        RutaFoto = c.String(),
                    })
                .PrimaryKey(t => t.IdFotoProducto)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: true)
                .Index(t => t.IdProducto);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FotoProductos", new[] { "IdProducto" });
            DropForeignKey("dbo.FotoProductos", "IdProducto", "dbo.Productos");
            DropTable("dbo.FotoProductos");
        }
    }
}
