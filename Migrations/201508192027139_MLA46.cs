namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA46 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "ComentarioEnvio", c => c.String((nullable: true)));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "ComentarioEnvio");
        }
    }
}
