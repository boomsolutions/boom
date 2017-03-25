namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogModificacionesDetalles", "ValorAnterior", c => c.String(nullable: false));
            DropColumn("dbo.LogModificacionesDetalles", "ValorAnteior");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LogModificacionesDetalles", "ValorAnteior", c => c.String(nullable: false));
            DropColumn("dbo.LogModificacionesDetalles", "ValorAnterior");
        }
    }
}
