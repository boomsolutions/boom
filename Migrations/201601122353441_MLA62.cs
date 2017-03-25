namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA62 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alquileres", "ComentarioExperienciaArrendador", c => c.String());
            AddColumn("dbo.Alquileres", "ComentarioPrivadoArrendador", c => c.String());
            AddColumn("dbo.Alquileres", "CalificacionCuidado", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionComunicacion", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionCumplimientoNormas", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Alquileres", "CalificacionCumplimientoNormas");
            DropColumn("dbo.Alquileres", "CalificacionComunicacion");
            DropColumn("dbo.Alquileres", "CalificacionCuidado");
            DropColumn("dbo.Alquileres", "ComentarioPrivadoArrendador");
            DropColumn("dbo.Alquileres", "ComentarioExperienciaArrendador");
        }
    }
}
