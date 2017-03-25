namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA48 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogVisitas", "IdCampaign", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogVisitas", "IdCampaign");
        }
    }
}
