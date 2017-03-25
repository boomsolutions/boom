namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "PrecioHora", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Productos", "MostrarPrecioHora", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "MostrarPrecioHora");
            DropColumn("dbo.Productos", "PrecioHora");
        }
    }
}
