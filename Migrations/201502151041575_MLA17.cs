namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "PrecioFinDeSemana", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "PrecioFinDeSemana");
        }
    }
}
