namespace MvcApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MLA25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favoritos",
                c => new
                    {
                        IdFavoritos = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFavoritos)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdProducto)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.LogAccesos",
                c => new
                    {
                        IdLogAccesos = c.Int(nullable: false, identity: true),
                        FechaLogin = c.DateTime(nullable: false),
                        FechaLogout = c.DateTime(nullable: false),
                        IP = c.String(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLogAccesos)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.LogBusquedas",
                c => new
                    {
                        IdLogBusquedas = c.Int(nullable: false, identity: true),
                        FechaBusqueda = c.DateTime(nullable: false),
                        TextoBusqueda = c.String(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLogBusquedas)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.LogModificacionesCabezals",
                c => new
                    {
                        IdLogModificacionesCabezal = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Accion = c.Int(nullable: false),
                        Tabla = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdLogModificacionesCabezal)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.LogModificacionesCamposClaves",
                c => new
                    {
                        IdLogModificacionesDetalle = c.Int(nullable: false, identity: true),
                        IdLogModificacionesCabezal = c.Int(nullable: false),
                        CampoClave = c.String(nullable: false),
                        Valor = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdLogModificacionesDetalle)
                .ForeignKey("dbo.LogModificacionesCabezals", t => t.IdLogModificacionesCabezal, cascadeDelete: true)
                .Index(t => t.IdLogModificacionesCabezal);
            
            CreateTable(
                "dbo.LogModificacionesDetalles",
                c => new
                    {
                        IdLogModificacionesDetalle = c.Int(nullable: false, identity: true),
                        IdLogModificacionesCabezal = c.Int(nullable: false),
                        CampoModificado = c.String(nullable: false),
                        ValorAnteior = c.String(nullable: false),
                        ValorActual = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdLogModificacionesDetalle)
                .ForeignKey("dbo.LogModificacionesCabezals", t => t.IdLogModificacionesCabezal, cascadeDelete: true)
                .Index(t => t.IdLogModificacionesCabezal);
            
            CreateTable(
                "dbo.LogNavegacions",
                c => new
                    {
                        IdLogNavegacion = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Action = c.String(nullable: false),
                        Controller = c.String(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLogNavegacion)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Reputacions",
                c => new
                    {
                        IdReputacion = c.Int(nullable: false, identity: true),
                        IdUsuarioEvalua = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdReputacion)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuarioEvalua, cascadeDelete: true)
                .Index(t => t.IdUsuarioEvalua);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reputacions", "IdUsuarioEvalua", "dbo.Usuarios");
            DropForeignKey("dbo.LogNavegacions", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.LogModificacionesCabezals", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.LogModificacionesDetalles", "IdLogModificacionesCabezal", "dbo.LogModificacionesCabezals");
            DropForeignKey("dbo.LogModificacionesCamposClaves", "IdLogModificacionesCabezal", "dbo.LogModificacionesCabezals");
            DropForeignKey("dbo.LogBusquedas", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.LogAccesos", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Favoritos", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Favoritos", "IdProducto", "dbo.Productos");
            DropIndex("dbo.Reputacions", new[] { "IdUsuarioEvalua" });
            DropIndex("dbo.LogNavegacions", new[] { "IdUsuario" });
            DropIndex("dbo.LogModificacionesDetalles", new[] { "IdLogModificacionesCabezal" });
            DropIndex("dbo.LogModificacionesCamposClaves", new[] { "IdLogModificacionesCabezal" });
            DropIndex("dbo.LogModificacionesCabezals", new[] { "IdUsuario" });
            DropIndex("dbo.LogBusquedas", new[] { "IdUsuario" });
            DropIndex("dbo.LogAccesos", new[] { "IdUsuario" });
            DropIndex("dbo.Favoritos", new[] { "IdUsuario" });
            DropIndex("dbo.Favoritos", new[] { "IdProducto" });
            DropTable("dbo.Reputacions");
            DropTable("dbo.LogNavegacions");
            DropTable("dbo.LogModificacionesDetalles");
            DropTable("dbo.LogModificacionesCamposClaves");
            DropTable("dbo.LogModificacionesCabezals");
            DropTable("dbo.LogBusquedas");
            DropTable("dbo.LogAccesos");
            DropTable("dbo.Favoritos");
        }
    }
}
