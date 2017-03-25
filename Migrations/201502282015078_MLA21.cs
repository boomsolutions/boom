namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoAlquilers",
                c => new
                    {
                        IdEstadoAlquiler = c.Int(nullable: false, identity: true),
                        IdEstadoAlquilerFijo = c.Int(nullable: false),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdEstadoAlquiler);
            
            AddColumn("dbo.Alquileres", "IdEstadoAlquiler", c => c.Int(nullable: true));
            CreateIndex("dbo.Alquileres", "IdEstadoAlquiler");
            AddForeignKey("dbo.Alquileres", "IdEstadoAlquiler", "dbo.EstadoAlquilers", "IdEstadoAlquiler", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Alquileres", "IdEstadoAlquiler", "dbo.EstadoAlquilers");
            DropIndex("dbo.Alquileres", new[] { "IdEstadoAlquiler" });
            DropColumn("dbo.Alquileres", "IdEstadoAlquiler");
            DropTable("dbo.EstadoAlquilers");
        }
    }
}
