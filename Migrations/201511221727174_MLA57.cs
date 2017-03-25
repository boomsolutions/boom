namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA57 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configuraciones",
                c => new
                    {
                        IdConfiguracion = c.Int(nullable: false, identity: true),
                        Parametro = c.String(nullable: false),
                        Valor = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdConfiguracion);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Configuraciones");
        }
    }
}
