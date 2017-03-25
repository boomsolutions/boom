namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA67 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "OpcionEntrega", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "OpcionEntrega");
        }
    }
}
