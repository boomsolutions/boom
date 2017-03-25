namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "Zona", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "Zona");
        }
    }
}
