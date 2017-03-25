namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA43 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ciudades",
                c => new
                    {
                        IdCiudad = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdCiudad);
            
            CreateTable(
                "dbo.Departamentos",
                c => new
                    {
                        IdDepartamento = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdDepartamento);
            
            CreateTable(
                "dbo.Paises",
                c => new
                    {
                        IdPais = c.Int(nullable: false, identity: true),
                        DescripcionI1 = c.String(),
                        DescripcionI2 = c.String(),
                        DescripcionI3 = c.String(),
                    })
                .PrimaryKey(t => t.IdPais);
            
            AddColumn("dbo.Usuarios", "IdDepartamento", c => c.Int(nullable: true));
            AddColumn("dbo.Usuarios", "IdCiudad", c => c.Int(nullable: true));
            AddColumn("dbo.Usuarios", "IdPais", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "IdDepartamento", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "IdCiudad", c => c.Int(nullable: true));
            AddColumn("dbo.Productos", "IdPais", c => c.Int(nullable: true));
            
            CreateIndex("dbo.Usuarios", "IdDepartamento");
            CreateIndex("dbo.Usuarios", "IdCiudad");
            CreateIndex("dbo.Usuarios", "IdPais");
            CreateIndex("dbo.Productos", "IdDepartamento");
            CreateIndex("dbo.Productos", "IdCiudad");
            CreateIndex("dbo.Productos", "IdPais");
            AddForeignKey("dbo.Productos", "IdCiudad", "dbo.Ciudades", "IdCiudad", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdCiudad", "dbo.Ciudades", "IdCiudad", cascadeDelete: false);
            AddForeignKey("dbo.Productos", "IdDepartamento", "dbo.Departamentos", "IdDepartamento", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdDepartamento", "dbo.Departamentos", "IdDepartamento", cascadeDelete: false);
            AddForeignKey("dbo.Productos", "IdPais", "dbo.Paises", "IdPais", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdPais", "dbo.Paises", "IdPais", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "IdPais", "dbo.Paises");
            DropForeignKey("dbo.Productos", "IdPais", "dbo.Paises");
            DropForeignKey("dbo.Usuarios", "IdDepartamento", "dbo.Departamentos");
            DropForeignKey("dbo.Productos", "IdDepartamento", "dbo.Departamentos");
            DropForeignKey("dbo.Usuarios", "IdCiudad", "dbo.Ciudades");
            DropForeignKey("dbo.Productos", "IdCiudad", "dbo.Ciudades");
            DropIndex("dbo.Productos", new[] { "IdPais" });
            DropIndex("dbo.Productos", new[] { "IdCiudad" });
            DropIndex("dbo.Productos", new[] { "IdDepartamento" });
            DropIndex("dbo.Usuarios", new[] { "IdPais" });
            DropIndex("dbo.Usuarios", new[] { "IdCiudad" });
            DropIndex("dbo.Usuarios", new[] { "IdDepartamento" });
            DropColumn("dbo.Productos", "IdEstadoUsuario");
            DropColumn("dbo.Productos", "IdPais");
            DropColumn("dbo.Productos", "IdCiudad");
            DropColumn("dbo.Productos", "IdDepartamento");
            DropColumn("dbo.Usuarios", "IdPais");
            DropColumn("dbo.Usuarios", "IdCiudad");
            DropColumn("dbo.Usuarios", "IdDepartamento");
            DropTable("dbo.Paises");
            DropTable("dbo.Departamentos");
            DropTable("dbo.Ciudades");
        }
    }
}
