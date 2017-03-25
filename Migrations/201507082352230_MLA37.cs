namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA37 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogVisitasProductos",
                c => new
                    {
                        IdLogVisitaProducto = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        IP = c.String(nullable: false),
                        IdProducto = c.Int(nullable: false),
                        IdUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.IdLogVisitaProducto)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario)
                .Index(t => t.IdProducto)
                .Index(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogVisitasProductos", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.LogVisitasProductos", "IdProducto", "dbo.Productos");
            DropIndex("dbo.LogVisitasProductos", new[] { "IdUsuario" });
            DropIndex("dbo.LogVisitasProductos", new[] { "IdProducto" });
            DropTable("dbo.LogVisitasProductos");
        }
    }
}
