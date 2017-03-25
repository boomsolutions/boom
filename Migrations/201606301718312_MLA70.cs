namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA70 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alquileres", "Leido", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Alquileres", "Leido");
        }
    }
}
