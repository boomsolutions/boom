namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA23 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ComentariosProductoes", new[] { "IdComentariosProductoOrigen" });
            AlterColumn("dbo.ComentariosProductoes", "IdComentariosProductoOrigen", c => c.Int());
            CreateIndex("dbo.ComentariosProductoes", "IdComentariosProductoOrigen");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ComentariosProductoes", new[] { "IdComentariosProductoOrigen" });
            AlterColumn("dbo.ComentariosProductoes", "IdComentariosProductoOrigen", c => c.Int(nullable: true));
            CreateIndex("dbo.ComentariosProductoes", "IdComentariosProductoOrigen");
        }
    }
}
