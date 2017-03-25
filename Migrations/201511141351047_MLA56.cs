namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA56 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Suscriptors", "FechaNacimiento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Suscriptors", "FechaNacimiento", c => c.DateTime());
        }
    }
}
