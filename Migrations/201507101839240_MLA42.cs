namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA42 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Productos", "PrecioDiario", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioSemanal", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioMensual", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioFinDeSemana", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioHora", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Productos", "PrecioHora", c => c.Decimal(nullable: true, precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioFinDeSemana", c => c.Decimal(nullable: true, precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioMensual", c => c.Decimal(nullable: true, precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioSemanal", c => c.Decimal(nullable: true, precision: 18, scale: 2));
            AlterColumn("dbo.Productos", "PrecioDiario", c => c.Decimal(nullable: true, precision: 18, scale: 2));
        }
    }
}
