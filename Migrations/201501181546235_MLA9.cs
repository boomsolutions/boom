namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "ConfirmPassword", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "ConfirmPassword");
        }
    }
}
