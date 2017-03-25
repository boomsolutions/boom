namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA68 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComentariosProductoes", "EsRespuesta", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComentariosProductoes", "EsRespuesta");
        }
    }
}
