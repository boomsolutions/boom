namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ML8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComentarioAlquileres", "FechaComentario", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComentarioAlquileres", "FechaComentario");
        }
    }
}
