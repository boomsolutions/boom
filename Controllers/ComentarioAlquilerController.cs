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
    public class ComentarioAlquilerController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /ComentarioAlquiler/

        public ViewResult Index()
        {
            var comentarioalquilers = db.ComentarioAlquilers.Include(c => c.Usuario).Include(c => c.Alquiler);
            return View(comentarioalquilers.ToList());
        }

        //
        // GET: /ComentarioAlquiler/Details/5

        public ViewResult Details(int id)
        {
            ComentarioAlquileres comentarioalquileres = db.ComentarioAlquilers.Find(id);
            return View(comentarioalquileres);
        }

        //
        // GET: /ComentarioAlquiler/Create

        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            ViewBag.IdAlquiler = new SelectList(db.Alquilers, "IdAlquiler", "IdAlquiler");
            return View();
        } 

        //
        // POST: /ComentarioAlquiler/Create

        [HttpPost]
        public ActionResult Create(ComentarioAlquileres comentarioalquileres)
        {
            if (ModelState.IsValid)
            {
                db.ComentarioAlquilers.Add(comentarioalquileres);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", comentarioalquileres.IdUsuario);
            ViewBag.IdAlquiler = new SelectList(db.Alquilers, "IdAlquiler", "IdAlquiler", comentarioalquileres.IdAlquiler);
            return View(comentarioalquileres);
        }
        
        //
        // GET: /ComentarioAlquiler/Edit/5
 
        public ActionResult Edit(int id)
        {
            ComentarioAlquileres comentarioalquileres = db.ComentarioAlquilers.Find(id);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", comentarioalquileres.IdUsuario);
            ViewBag.IdAlquiler = new SelectList(db.Alquilers, "IdAlquiler", "IdAlquiler", comentarioalquileres.IdAlquiler);
            return View(comentarioalquileres);
        }

        //
        // POST: /ComentarioAlquiler/Edit/5

        [HttpPost]
        public ActionResult Edit(ComentarioAlquileres comentarioalquileres)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentarioalquileres).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", comentarioalquileres.IdUsuario);
            ViewBag.IdAlquiler = new SelectList(db.Alquilers, "IdAlquiler", "IdAlquiler", comentarioalquileres.IdAlquiler);
            return View(comentarioalquileres);
        }

        //
        // GET: /ComentarioAlquiler/Delete/5
 
        public ActionResult Delete(int id)
        {
            ComentarioAlquileres comentarioalquileres = db.ComentarioAlquilers.Find(id);
            return View(comentarioalquileres);
        }

        //
        // POST: /ComentarioAlquiler/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ComentarioAlquileres comentarioalquileres = db.ComentarioAlquilers.Find(id);
            db.ComentarioAlquilers.Remove(comentarioalquileres);
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