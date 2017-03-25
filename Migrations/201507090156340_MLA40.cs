namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TiposProductos", "IdTipoProductoFijo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TiposProductos", "IdTipoProductoFijo");
        }
    }
}
