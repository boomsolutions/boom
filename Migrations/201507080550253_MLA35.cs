namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imagens", "RutaFotoThumb", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imagens", "RutaFotoThumb");
        }
    }
}
