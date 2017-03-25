namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComentariosProductoes",
                c => new
                    {
                        IdComentariosProducto = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Comentario = c.String(),
                        FechaComentario = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdComentariosProducto)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdProducto)
                .Index(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComentariosProductoes", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.ComentariosProductoes", "IdProducto", "dbo.Productos");
            DropIndex("dbo.ComentariosProductoes", new[] { "IdUsuario" });
            DropIndex("dbo.ComentariosProductoes", new[] { "IdProducto" });
            DropTable("dbo.ComentariosProductoes");
        }
    }
}
