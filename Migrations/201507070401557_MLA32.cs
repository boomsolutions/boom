namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA32 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogBusquedas", "IdUsuario", "dbo.Usuarios");
            DropIndex("dbo.LogBusquedas", new[] { "IdUsuario" });
            AlterColumn("dbo.LogBusquedas", "IdUsuario", c => c.Int());
            CreateIndex("dbo.LogBusquedas", "IdUsuario");
            AddForeignKey("dbo.LogBusquedas", "IdUsuario", "dbo.Usuarios", "IdUsuario");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogBusquedas", "IdUsuario", "dbo.Usuarios");
            DropIndex("dbo.LogBusquedas", new[] { "IdUsuario" });
            AlterColumn("dbo.LogBusquedas", "IdUsuario", c => c.Int(nullable: false));
            CreateIndex("dbo.LogBusquedas", "IdUsuario");
            AddForeignKey("dbo.LogBusquedas", "IdUsuario", "dbo.Usuarios", "IdUsuario", cascadeDelete: true);
        }
    }
}
