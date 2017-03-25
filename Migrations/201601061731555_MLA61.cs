namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA61 : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.Alquileres", "CalificacionArrendador", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionArrendatario", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionPositivaArrendador", c => c.Boolean(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionPositivaArrendatario", c => c.Boolean(nullable: false));
            AddColumn("dbo.Alquileres", "ComentarioArrendador", c => c.String());
            AddColumn("dbo.Alquileres", "ComentarioArrendatario", c => c.String());
            
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.Alquileres", "ComentarioArrendatario");
            DropColumn("dbo.Alquileres", "ComentarioArrendador");
            DropColumn("dbo.Alquileres", "CalificacionPositivaArrendatario");
            DropColumn("dbo.Alquileres", "CalificacionPositivaArrendador");
            DropColumn("dbo.Alquileres", "CalificacionArrendatario");
            DropColumn("dbo.Alquileres", "CalificacionArrendador");
            
        }
    }
}
