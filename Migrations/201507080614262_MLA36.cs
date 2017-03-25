namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA36 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LogBusquedas", "TextoBusqueda", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogBusquedas", "TextoBusqueda", c => c.String(nullable: true));
        }
    }
}
