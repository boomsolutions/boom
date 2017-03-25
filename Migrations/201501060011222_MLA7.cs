namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FotoProductos", "IdImagen", "dbo.Imagens");
            DropIndex("dbo.FotoProductos", new[] { "IdImagen" });
            AlterColumn("dbo.FotoProductos", "IdImagen", c => c.Int());
            AddForeignKey("dbo.FotoProductos", "IdImagen", "dbo.Imagens", "IdImagen");
            CreateIndex("dbo.FotoProductos", "IdImagen");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FotoProductos", new[] { "IdImagen" });
            DropForeignKey("dbo.FotoProductos", "IdImagen", "dbo.Imagens");
            AlterColumn("dbo.FotoProductos", "IdImagen", c => c.Int(nullable: false));
            CreateIndex("dbo.FotoProductos", "IdImagen");
            AddForeignKey("dbo.FotoProductos", "IdImagen", "dbo.Imagens", "IdImagen", cascadeDelete: true);
        }
    }
}
