namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA52 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Suscriptors", "IdDepartamento");
            AddForeignKey("dbo.Suscriptors", "IdDepartamento", "dbo.Departamentos", "IdDepartamento");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suscriptors", "IdDepartamento", "dbo.Departamentos");
            DropIndex("dbo.Suscriptors", new[] { "IdDepartamento" });
        }
    }
}
