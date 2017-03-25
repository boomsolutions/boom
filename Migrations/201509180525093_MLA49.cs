namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA49 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suscriptors",
                c => new
                    {
                        IdSuscriptor = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        IdDepartamento = c.Int(),
                        IdCiudad = c.Int(),
                    })
                .PrimaryKey(t => t.IdSuscriptor);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Suscriptors");
        }
    }
}
