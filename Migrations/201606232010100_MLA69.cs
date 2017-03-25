namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA69 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoriaProductos", "Estilo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CategoriaProductos", "Estilo");
        }
    }
}
