namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA44 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Productos", "IdEstadoUsuario");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Productos", "IdEstadoUsuario", c => c.Int(nullable: false));
        }
    }
}
