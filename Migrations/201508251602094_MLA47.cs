namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA47 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "AceptoTerminosYCondiciones", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "AceptoTerminosYCondiciones");
        }
    }
}
