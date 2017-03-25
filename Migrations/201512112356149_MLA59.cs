namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA59 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComentariosProductoes", "Leido", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComentariosProductoes", "Leido");
        }
    }
}
