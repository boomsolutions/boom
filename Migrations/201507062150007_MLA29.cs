namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA29 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LogModificacionesDetalles", "ValorAnterior", c => c.String());
            AlterColumn("dbo.LogModificacionesDetalles", "ValorActual", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogModificacionesDetalles", "ValorActual", c => c.String(nullable: false));
            AlterColumn("dbo.LogModificacionesDetalles", "ValorAnterior", c => c.String(nullable: false));
        }
    }
}
