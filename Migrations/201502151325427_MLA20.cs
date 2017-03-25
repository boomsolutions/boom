namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Monedas", "CodigoISO", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Monedas", "CodigoISO");
        }
    }
}
