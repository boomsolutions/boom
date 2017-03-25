namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA53 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoriaProductos", "FotoCategoria", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CategoriaProductos", "FotoCategoria");
        }
    }
}
