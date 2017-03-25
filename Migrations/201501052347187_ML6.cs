namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ML6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imagens",
                c => new
                    {
                        IdImagen = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        Caption = c.String(),
                        RutaFoto = c.String(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdImagen);
            
            AddColumn("dbo.FotoProductos", "IdImagen", c => c.Int(nullable: true));
            AddForeignKey("dbo.FotoProductos", "IdImagen", "dbo.Imagens", "IdImagen", cascadeDelete: false);
            CreateIndex("dbo.FotoProductos", "IdImagen");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FotoProductos", new[] { "IdImagen" });
            DropForeignKey("dbo.FotoProductos", "IdImagen", "dbo.Imagens");
            DropColumn("dbo.FotoProductos", "IdImagen");
            DropTable("dbo.Imagens");
        }
    }
}
