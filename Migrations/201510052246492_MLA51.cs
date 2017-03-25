namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA51 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Suscriptors", "FechaSuscripto", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Suscriptors", "FechaSuscripto", c => c.DateTime(nullable: false));
        }
    }
}
