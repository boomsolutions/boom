namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComentariosProductoes", "IdComentariosProductoOrigen", c => c.Int(nullable: true));
            CreateIndex("dbo.ComentariosProductoes", "IdComentariosProductoOrigen");
            AddForeignKey("dbo.ComentariosProductoes", "IdComentariosProductoOrigen", "dbo.ComentariosProductoes", "IdComentariosProducto");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComentariosProductoes", "IdComentariosProductoOrigen", "dbo.ComentariosProductoes");
            DropIndex("dbo.ComentariosProductoes", new[] { "IdComentariosProductoOrigen" });
            DropColumn("dbo.ComentariosProductoes", "IdComentariosProductoOrigen");
        }
    }
}
