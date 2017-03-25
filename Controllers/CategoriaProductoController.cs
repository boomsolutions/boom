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
    public class CategoriaProductoController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /CategoriaProducto/
        [Authorize]
        public ViewResult Index()
        {
            return View(db.CategoriaProductoes.ToList());
        }

        //
        // GET: /CategoriaProducto/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            CategoriaProductos categoriaproductos = db.CategoriaProductoes.Find(id);
            return View(categoriaproductos);
        }

        //
        // GET: /CategoriaProducto/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CategoriaProducto/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(CategoriaProductos categoriaproductos)
        {
            if (ModelState.IsValid)
            {
                db.CategoriaProductoes.Add(categoriaproductos);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(categoriaproductos);
        }
        
        //
        // GET: /CategoriaProducto/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            CategoriaProductos categoriaproductos = db.CategoriaProductoes.Find(id);
            return View(categoriaproductos);
        }

        //
        // POST: /CategoriaProducto/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(CategoriaProductos categoriaproductos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriaproductos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaproductos);
        }

        //
        // GET: /CategoriaProducto/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            CategoriaProductos categoriaproductos = db.CategoriaProductoes.Find(id);
            return View(categoriaproductos);
        }

        //
        // POST: /CategoriaProducto/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            CategoriaProductos categoriaproductos = db.CategoriaProductoes.Find(id);
            db.CategoriaProductoes.Remove(categoriaproductos);
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