namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA54 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuarios", "Telefono", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Telefono", c => c.String());
        }
    }
}
