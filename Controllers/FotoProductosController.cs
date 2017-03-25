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
    public class FotoProductosController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /FotoProductos/

        public ViewResult Index()
        {
            var fotoproductos = db.FotoProductos.Include(f => f.Producto);
            return View(fotoproductos.ToList());
        }

        //
        // GET: /FotoProductos/Details/5

        public ViewResult Details(int id)
        {
            FotoProductos fotoproductos = db.FotoProductos.Find(id);
            return View(fotoproductos);
        }

        //
        // GET: /FotoProductos/Create

        public ActionResult Create()
        {
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre");
            return View();
        } 

        //
        // POST: /FotoProductos/Create

        [HttpPost]
        public ActionResult Create(FotoProductos fotoproductos)
        {
            if (ModelState.IsValid)
            {
                db.FotoProductos.Add(fotoproductos);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", fotoproductos.IdProducto);
            return View(fotoproductos);
        }
        
        //
        // GET: /FotoProductos/Edit/5
 
        public ActionResult Edit(int id)
        {
            FotoProductos fotoproductos = db.FotoProductos.Find(id);
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", fotoproductos.IdProducto);
            return View(fotoproductos);
        }

        //
        // POST: /FotoProductos/Edit/5

        [HttpPost]
        public ActionResult Edit(FotoProductos fotoproductos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fotoproductos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", fotoproductos.IdProducto);
            return View(fotoproductos);
        }

        //
        // GET: /FotoProductos/Delete/5
 
        public ActionResult Delete(int id)
        {
            FotoProductos fotoproductos = db.FotoProductos.Find(id);
            return View(fotoproductos);
        }

        //
        // POST: /FotoProductos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            FotoProductos fotoproductos = db.FotoProductos.Find(id);
            db.FotoProductos.Remove(fotoproductos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}