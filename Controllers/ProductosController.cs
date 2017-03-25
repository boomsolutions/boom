using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication4.Models;

namespace MvcApplication4.Controllers
{ 
    public class ProductosController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /Productos/

        public ViewResult Index()
        {
            var productos = db.Productos.Include(p => p.CategoriasProducto);
            return View(productos.ToList());
        }

        //
        // GET: /Productos/Details/5

        public ViewResult Details(int id)
        {
            Productos productos = db.Productos.Find(id);
            return View(productos);
        }

        //
        // GET: /Productos/Create
        [Authorize]
        public ActionResult Create()
        {
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;
            

            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes, "IdCategoriaProducto", "DescripcionI1");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuario.First());
            return View();
        } 

        //
        // POST: /Productos/Create

        [HttpPost]
        public ActionResult Create(Productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(productos);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes, "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", productos.IdUsuario);
            
            return View(productos);
        }
        
        //
        // GET: /Productos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Productos productos = db.Productos.Find(id);
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes, "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", productos.IdUsuario);
            return View(productos);
        }

        //
        // POST: /Productos/Edit/5

        [HttpPost]
        public ActionResult Edit(Productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes, "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", productos.IdUsuario);
            return View(productos);
        }

        //
        // GET: /Productos/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Productos productos = db.Productos.Find(id);
            return View(productos);
        }

        //
        // POST: /Productos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //funciones mias!!

        //
        // POST: /Productos/Create

        [HttpPost]
        public ActionResult Buscar()
        {
            /*if (ModelState.IsValid)
            {
                db.Productos.Add(productos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }*/

            ViewData["Productos"] = db.Productos.ToList<Productos>();
            //ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes, "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
            return View();
        }
    }
}