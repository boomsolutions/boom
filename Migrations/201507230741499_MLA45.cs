namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA45 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuarios", "IdBarrio", "dbo.Barrios");
            DropForeignKey("dbo.Usuarios", "IdCiudad", "dbo.Ciudades");
            DropForeignKey("dbo.Usuarios", "IdDepartamento", "dbo.Departamentos");
            DropForeignKey("dbo.Usuarios", "IdPais", "dbo.Paises");
            DropForeignKey("dbo.Productos", "IdBarrio", "dbo.Barrios");
            DropForeignKey("dbo.Productos", "IdCiudad", "dbo.Ciudades");
            DropForeignKey("dbo.Productos", "IdDepartamento", "dbo.Departamentos");
            DropForeignKey("dbo.Productos", "IdPais", "dbo.Paises");
            DropIndex("dbo.Usuarios", new[] { "IdBarrio" });
            DropIndex("dbo.Usuarios", new[] { "IdDepartamento" });
            DropIndex("dbo.Usuarios", new[] { "IdCiudad" });
            DropIndex("dbo.Usuarios", new[] { "IdPais" });
            DropIndex("dbo.Productos", new[] { "IdBarrio" });
            DropIndex("dbo.Productos", new[] { "IdDepartamento" });
            DropIndex("dbo.Productos", new[] { "IdCiudad" });
            DropIndex("dbo.Productos", new[] { "IdPais" });
            AlterColumn("dbo.Usuarios", "IdBarrio", c => c.Int());
            AlterColumn("dbo.Usuarios", "IdDepartamento", c => c.Int());
            AlterColumn("dbo.Usuarios", "IdCiudad", c => c.Int());
            AlterColumn("dbo.Usuarios", "IdPais", c => c.Int());
            AlterColumn("dbo.Productos", "IdBarrio", c => c.Int());
            AlterColumn("dbo.Productos", "IdDepartamento", c => c.Int());
            AlterColumn("dbo.Productos", "IdCiudad", c => c.Int());
            AlterColumn("dbo.Productos", "IdPais", c => c.Int());
            CreateIndex("dbo.Usuarios", "IdBarrio");
            CreateIndex("dbo.Usuarios", "IdDepartamento");
            CreateIndex("dbo.Usuarios", "IdCiudad");
            CreateIndex("dbo.Usuarios", "IdPais");
            CreateIndex("dbo.Productos", "IdBarrio");
            CreateIndex("dbo.Productos", "IdDepartamento");
            CreateIndex("dbo.Productos", "IdCiudad");
            CreateIndex("dbo.Productos", "IdPais");
            AddForeignKey("dbo.Usuarios", "IdBarrio", "dbo.Barrios", "IdBarrio");
            AddForeignKey("dbo.Usuarios", "IdCiudad", "dbo.Ciudades", "IdCiudad");
            AddForeignKey("dbo.Usuarios", "IdDepartamento", "dbo.Departamentos", "IdDepartamento");
            AddForeignKey("dbo.Usuarios", "IdPais", "dbo.Paises", "IdPais");
            AddForeignKey("dbo.Productos", "IdBarrio", "dbo.Barrios", "IdBarrio");
            AddForeignKey("dbo.Productos", "IdCiudad", "dbo.Ciudades", "IdCiudad");
            AddForeignKey("dbo.Productos", "IdDepartamento", "dbo.Departamentos", "IdDepartamento");
            AddForeignKey("dbo.Productos", "IdPais", "dbo.Paises", "IdPais");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "IdPais", "dbo.Paises");
            DropForeignKey("dbo.Productos", "IdDepartamento", "dbo.Departamentos");
            DropForeignKey("dbo.Productos", "IdCiudad", "dbo.Ciudades");
            DropForeignKey("dbo.Productos", "IdBarrio", "dbo.Barrios");
            DropForeignKey("dbo.Usuarios", "IdPais", "dbo.Paises");
            DropForeignKey("dbo.Usuarios", "IdDepartamento", "dbo.Departamentos");
            DropForeignKey("dbo.Usuarios", "IdCiudad", "dbo.Ciudades");
            DropForeignKey("dbo.Usuarios", "IdBarrio", "dbo.Barrios");
            DropIndex("dbo.Productos", new[] { "IdPais" });
            DropIndex("dbo.Productos", new[] { "IdCiudad" });
            DropIndex("dbo.Productos", new[] { "IdDepartamento" });
            DropIndex("dbo.Productos", new[] { "IdBarrio" });
            DropIndex("dbo.Usuarios", new[] { "IdPais" });
            DropIndex("dbo.Usuarios", new[] { "IdCiudad" });
            DropIndex("dbo.Usuarios", new[] { "IdDepartamento" });
            DropIndex("dbo.Usuarios", new[] { "IdBarrio" });
            AlterColumn("dbo.Productos", "IdPais", c => c.Int(nullable: true));
            AlterColumn("dbo.Productos", "IdCiudad", c => c.Int(nullable: true));
            AlterColumn("dbo.Productos", "IdDepartamento", c => c.Int(nullable: true));
            AlterColumn("dbo.Productos", "IdBarrio", c => c.Int(nullable: true));
            AlterColumn("dbo.Usuarios", "IdPais", c => c.Int(nullable: true));
            AlterColumn("dbo.Usuarios", "IdCiudad", c => c.Int(nullable: true));
            AlterColumn("dbo.Usuarios", "IdDepartamento", c => c.Int(nullable: true));
            AlterColumn("dbo.Usuarios", "IdBarrio", c => c.Int(nullable: true));
            CreateIndex("dbo.Productos", "IdPais");
            CreateIndex("dbo.Productos", "IdCiudad");
            CreateIndex("dbo.Productos", "IdDepartamento");
            CreateIndex("dbo.Productos", "IdBarrio");
            CreateIndex("dbo.Usuarios", "IdPais");
            CreateIndex("dbo.Usuarios", "IdCiudad");
            CreateIndex("dbo.Usuarios", "IdDepartamento");
            CreateIndex("dbo.Usuarios", "IdBarrio");
            AddForeignKey("dbo.Productos", "IdPais", "dbo.Paises", "IdPais", cascadeDelete: false);
            AddForeignKey("dbo.Productos", "IdDepartamento", "dbo.Departamentos", "IdDepartamento", cascadeDelete: false);
            AddForeignKey("dbo.Productos", "IdCiudad", "dbo.Ciudades", "IdCiudad", cascadeDelete: false);
            AddForeignKey("dbo.Productos", "IdBarrio", "dbo.Barrios", "IdBarrio", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdPais", "dbo.Paises", "IdPais", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdDepartamento", "dbo.Departamentos", "IdDepartamento", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdCiudad", "dbo.Ciudades", "IdCiudad", cascadeDelete: false);
            AddForeignKey("dbo.Usuarios", "IdBarrio", "dbo.Barrios", "IdBarrio", cascadeDelete: false);
        }
    }
}
