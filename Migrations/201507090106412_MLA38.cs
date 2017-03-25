namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "CantVisitas", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "CantVisitas");
        }
    }
}
