namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA66 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "Ubicacion", c => c.String());
            AddColumn("dbo.Productos", "PrecioDeclarado", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "PrecioDeclarado");
            DropColumn("dbo.Productos", "Ubicacion");
        }
    }
}
