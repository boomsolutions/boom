namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA12 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Alquileres", "UsuarioArrendatario_IdUsuario", "dbo.Usuarios");
            //DropIndex("dbo.Alquileres", new[] { "UsuarioArrendatario_IdUsuario" });
            //RenameColumn(table: "dbo.Alquileres", name: "UsuarioArrendatario_IdUsuario", newName: "IdUsuarioArrendatario");
            
            AddForeignKey("dbo.Alquileres", "IdUsuarioArrendatario", "dbo.Usuarios", "IdUsuario", cascadeDelete: false);
            CreateIndex("dbo.Alquileres", "IdUsuarioArrendatario");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Alquileres", new[] { "IdUsuarioArrendatario" });
            DropForeignKey("dbo.Alquileres", "IdUsuarioArrendatario", "dbo.Usuarios");
            RenameColumn(table: "dbo.Alquileres", name: "IdUsuarioArrendatario", newName: "UsuarioArrendatario_IdUsuario");
            CreateIndex("dbo.Alquileres", "UsuarioArrendatario_IdUsuario");
            AddForeignKey("dbo.Alquileres", "UsuarioArrendatario_IdUsuario", "dbo.Usuarios", "IdUsuario");
        }
    }
}
