namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FotoUsuarios",
                c => new
                    {
                        IdFotoUsuario = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        RutaFoto = c.String(),
                        IdImagen = c.Int(),
                    })
                .PrimaryKey(t => t.IdFotoUsuario)
                .ForeignKey("dbo.Imagens", t => t.IdImagen)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdImagen);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FotoUsuarios", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.FotoUsuarios", "IdImagen", "dbo.Imagens");
            DropIndex("dbo.FotoUsuarios", new[] { "IdImagen" });
            DropIndex("dbo.FotoUsuarios", new[] { "IdUsuario" });
            DropTable("dbo.FotoUsuarios");
        }
    }
}
