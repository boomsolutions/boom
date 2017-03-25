namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogBusquedas", "IdCategoriaProducto", c => c.Int());
            CreateIndex("dbo.LogBusquedas", "IdCategoriaProducto");
            AddForeignKey("dbo.LogBusquedas", "IdCategoriaProducto", "dbo.CategoriaProductos", "IdCategoriaProducto");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogBusquedas", "IdCategoriaProducto", "dbo.CategoriaProductos");
            DropIndex("dbo.LogBusquedas", new[] { "IdCategoriaProducto" });
            DropColumn("dbo.LogBusquedas", "IdCategoriaProducto");
        }
    }
}
