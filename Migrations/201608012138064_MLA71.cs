namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA71 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "EsUsuarioFacebook", c => c.Boolean());
            AddColumn("dbo.Usuarios", "UsuarioFacebookID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "UsuarioFacebookID");
            DropColumn("dbo.Usuarios", "EsUsuarioFacebook");
        }
    }
}
