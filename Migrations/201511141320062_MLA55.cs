namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA55 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suscriptors", "IdBarrio", c => c.Int());
            AddColumn("dbo.Suscriptors", "FechaNacimiento", c => c.DateTime());
            CreateIndex("dbo.Suscriptors", "IdBarrio");
            AddForeignKey("dbo.Suscriptors", "IdBarrio", "dbo.Barrios", "IdBarrio");
            DropColumn("dbo.Suscriptors", "IdCiudad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suscriptors", "IdCiudad", c => c.Int());
            DropForeignKey("dbo.Suscriptors", "IdBarrio", "dbo.Barrios");
            DropIndex("dbo.Suscriptors", new[] { "IdBarrio" });
            DropColumn("dbo.Suscriptors", "FechaNacimiento");
            DropColumn("dbo.Suscriptors", "IdBarrio");
        }
    }
}
