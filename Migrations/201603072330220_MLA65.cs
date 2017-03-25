namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA65 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formularios", "IdUsuario", c => c.Int(nullable: false));
            CreateIndex("dbo.Formularios", "IdUsuario");
            AddForeignKey("dbo.Formularios", "IdUsuario", "dbo.Usuarios", "IdUsuario", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Formularios", "IdUsuario", "dbo.Usuarios");
            DropIndex("dbo.Formularios", new[] { "IdUsuario" });
            DropColumn("dbo.Formularios", "IdUsuario");
        }
    }
}
