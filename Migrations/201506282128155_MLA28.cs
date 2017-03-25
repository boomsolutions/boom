namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reputacions", "IdUsuarioEvaluado", c => c.Int(nullable: false));
            AddColumn("dbo.Reputacions", "Comentario", c => c.String(nullable: false));
            CreateIndex("dbo.Reputacions", "IdUsuarioEvaluado");
            AddForeignKey("dbo.Reputacions", "IdUsuarioEvaluado", "dbo.Usuarios", "IdUsuario", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reputacions", "IdUsuarioEvaluado", "dbo.Usuarios");
            DropIndex("dbo.Reputacions", new[] { "IdUsuarioEvaluado" });
            DropColumn("dbo.Reputacions", "Comentario");
            DropColumn("dbo.Reputacions", "IdUsuarioEvaluado");
        }
    }
}
