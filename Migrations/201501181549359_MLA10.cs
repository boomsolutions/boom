namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuarios", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Apellido", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Sexo", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "User", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Password", c => c.String());
            AlterColumn("dbo.Usuarios", "User", c => c.String());
            AlterColumn("dbo.Usuarios", "Sexo", c => c.String());
            AlterColumn("dbo.Usuarios", "Email", c => c.String());
            AlterColumn("dbo.Usuarios", "Apellido", c => c.String());
            AlterColumn("dbo.Usuarios", "Nombre", c => c.String());
        }
    }
}
