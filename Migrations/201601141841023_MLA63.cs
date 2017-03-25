namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA63 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alquileres", "ComentarioExperienciaArrendatario", c => c.String());
            AddColumn("dbo.Alquileres", "ComentarioPrivadoArrendatario", c => c.String());
            AddColumn("dbo.Alquileres", "CalificacionCuidadoArrendatario", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionComunicacionArrendatario", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionCumplimientoNormasArrendatario", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionCuidadoArrendador", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionComunicacionArrendador", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionCumplimientoNormasArrendador", c => c.Int(nullable: false));
            DropColumn("dbo.Alquileres", "CalificacionCuidado");
            DropColumn("dbo.Alquileres", "CalificacionComunicacion");
            DropColumn("dbo.Alquileres", "CalificacionCumplimientoNormas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Alquileres", "CalificacionCumplimientoNormas", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionComunicacion", c => c.Int(nullable: false));
            AddColumn("dbo.Alquileres", "CalificacionCuidado", c => c.Int(nullable: false));
            DropColumn("dbo.Alquileres", "CalificacionCumplimientoNormasArrendador");
            DropColumn("dbo.Alquileres", "CalificacionComunicacionArrendador");
            DropColumn("dbo.Alquileres", "CalificacionCuidadoArrendador");
            DropColumn("dbo.Alquileres", "CalificacionCumplimientoNormasArrendatario");
            DropColumn("dbo.Alquileres", "CalificacionComunicacionArrendatario");
            DropColumn("dbo.Alquileres", "CalificacionCuidadoArrendatario");
            DropColumn("dbo.Alquileres", "ComentarioPrivadoArrendatario");
            DropColumn("dbo.Alquileres", "ComentarioExperienciaArrendatario");
        }
    }
}
