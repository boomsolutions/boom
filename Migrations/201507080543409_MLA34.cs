namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA34 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Productos", "PrecioEnvio", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Productos", "PrecioEnvio", c => c.Decimal(nullable: true, precision: 18, scale: 2));
        }
    }
}
