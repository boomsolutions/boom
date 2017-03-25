namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA30 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LogAccesos", "FechaLogin", c => c.DateTime());
            AlterColumn("dbo.LogAccesos", "FechaLogout", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogAccesos", "FechaLogout", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LogAccesos", "FechaLogin", c => c.DateTime(nullable: false));
        }
    }
}
