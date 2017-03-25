namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Productos", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Productos", "Descripcion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Productos", "Descripcion", c => c.String());
            AlterColumn("dbo.Productos", "Nombre", c => c.String());
        }
    }
}
