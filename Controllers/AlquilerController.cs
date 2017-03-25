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
    public class AlquilerController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /Alquiler/

        public ViewResult Index()
        {
            var alquilers = db.Alquilers.Include(a => a.Producto);
            //.Include(a => a.UsuarioArrendatario).
            
            return View(alquilers.ToList());
        }

        //
        // GET: /Alquiler/Details/5

        public ViewResult Details(int id)
        {
            Alquileres alquileres = db.Alquilers.Find(id);
            return View(alquileres);
        }

        //
        // GET: /Alquiler/Create

        /*public ActionResult Create()
        {
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre");
            ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }*/

        //
        // GET: /Alquiler/Create
        [Authorize]
        public ActionResult Create(int? idproducto)
        {
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", idproducto);
            ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuario.First());
            return View();
        } 

        //
        // POST: /Alquiler/Create

        [HttpPost]
        public ActionResult Create(Alquileres alquileres)
        {
            if (ModelState.IsValid)
            {
                db.Alquilers.Add(alquileres);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View(alquileres);
        }
        
        //
        // GET: /Alquiler/Edit/5
 
        public ActionResult Edit(int id)
        {
            Alquileres alquileres = db.Alquilers.Find(id);
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", alquileres.IdUsuarioArrendatario);
            //ViewBag.FechaDesde = alquileres.FechaDesde;
            //ViewBag.FechaHasta = alquileres.FechaHasta;
            return View(alquileres);
        }

        //
        // POST: /Alquiler/Edit/5

        [HttpPost]
        public ActionResult Edit(Alquileres alquileres)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alquileres).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View(alquileres);
        }

        //
        // GET: /Alquiler/Delete/5
 
        public ActionResult Delete(int id)
        {
            Alquileres alquileres = db.Alquilers.Find(id);
            return View(alquileres);
        }

        //
        // POST: /Alquiler/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Alquileres alquileres = db.Alquilers.Find(id);
            db.Alquilers.Remove(alquileres);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AlquilarFechas(DateTime fechaDesde, DateTime fechaHasta, int idProducto)
        {
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            
            Alquileres alquiler = new Alquileres();
            alquiler.FechaDesde = fechaDesde;
            alquiler.FechaHasta = fechaHasta;
            alquiler.IdProducto = idProducto;
            alquiler.IdUsuarioArrendatario = usuario.First();
            if (ModelState.IsValid)
            {
                db.Alquilers.Add(alquiler);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            //ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            //ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            //return View(alquileres);

            //no se a donde ir
            return View("List");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}