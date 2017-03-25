namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA58 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Suscriptors", "IdDepartamento", "dbo.Departamentos");
            DropIndex("dbo.Suscriptors", new[] { "IdDepartamento" });
            AlterColumn("dbo.Suscriptors", "IdDepartamento", c => c.Int(nullable: false));
            CreateIndex("dbo.Suscriptors", "IdDepartamento");
            AddForeignKey("dbo.Suscriptors", "IdDepartamento", "dbo.Departamentos", "IdDepartamento", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suscriptors", "IdDepartamento", "dbo.Departamentos");
            DropIndex("dbo.Suscriptors", new[] { "IdDepartamento" });
            AlterColumn("dbo.Suscriptors", "IdDepartamento", c => c.Int());
            CreateIndex("dbo.Suscriptors", "IdDepartamento");
            AddForeignKey("dbo.Suscriptors", "IdDepartamento", "dbo.Departamentos", "IdDepartamento");
        }
    }
}
