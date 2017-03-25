namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "IdUsuario", c => c.Int(nullable: true));
            AddForeignKey("dbo.Productos", "IdUsuario", "dbo.Usuarios", "IdUsuario", cascadeDelete: false);
            CreateIndex("dbo.Productos", "IdUsuario");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Productos", new[] { "IdUsuario" });
            DropForeignKey("dbo.Productos", "IdUsuario", "dbo.Usuarios");
            DropColumn("dbo.Productos", "IdUsuario");
        }
    }
}
