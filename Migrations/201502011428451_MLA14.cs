namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComentariosProducto",
                c => new
                {
                    IdComentariosProducto = c.Int(nullable: false, identity: true),
                    IdProducto = c.Int(nullable: false),
                    IdUsuario = c.Int(nullable: true),
                    Comentario = c.String(),
                    FechaComentario = c.DateTime()
                })
                .PrimaryKey(t => t.IdComentariosProducto)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: false)
                .Index(t => t.IdProducto)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: false)
                .Index(t => t.IdUsuario);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ComentariosProducto", new[] { "IdProducto" });
            DropIndex("dbo.ComentariosProducto", new[] { "IdUsuario" });
            DropForeignKey("dbo.ComentariosProducto", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.ComentariosProducto", "IdUsuario", "dbo.Usuarios");
            DropTable("dbo.ComentariosProducto");
        }
    }
}
