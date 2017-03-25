namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "RememberMe", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "RememberMe");
        }
    }
}
