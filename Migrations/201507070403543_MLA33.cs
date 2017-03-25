namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogBusquedas", "IP", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogBusquedas", "IP");
        }
    }
}
