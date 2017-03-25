namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Barrios",
                c => new
                    {
                        IdBarrio = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdBarrio);
            
            AddColumn("dbo.Usuarios", "IdBarrio", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "IdBarrio", c => c.Int(nullable: true));
            CreateIndex("dbo.Usuarios", "IdBarrio");
            CreateIndex("dbo.Productos", "IdBarrio");
            AddForeignKey("dbo.Productos", "IdBarrio", "dbo.Barrios", "IdBarrio", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdBarrio", "dbo.Barrios", "IdBarrio", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "IdBarrio", "dbo.Barrios");
            DropForeignKey("dbo.Productos", "IdBarrio", "dbo.Barrios");
            DropIndex("dbo.Productos", new[] { "IdBarrio" });
            DropIndex("dbo.Usuarios", new[] { "IdBarrio" });
            DropColumn("dbo.Productos", "IdBarrio");
            DropColumn("dbo.Usuarios", "IdBarrio");
            DropTable("dbo.Barrios");
        }
    }
}
