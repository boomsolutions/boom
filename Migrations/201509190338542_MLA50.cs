namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA50 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Suscriptors", "Apellido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suscriptors", "Apellido", c => c.String(nullable: false));
        }
    }
}
