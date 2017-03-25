namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA60 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "NombreBusqueda", c => c.String());
            AddColumn("dbo.Productos", "DescripcionBusqueda", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "DescripcionBusqueda");
            DropColumn("dbo.Productos", "NombreBusqueda");
        }
    }
}
