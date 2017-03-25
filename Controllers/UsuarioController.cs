using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication4.Models;
using System.Web.Security;

namespace MvcApplication4.Controllers
{ 
    public class UsuarioController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /Usuario/
        [Authorize(Users="dimapego,pepe,MLAAdmin")]
        public ViewResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        //
        // GET: /Usuario/Details/5
        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ViewResult Details(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            return View(usuarios);
        }

        //
        // GET: /Usuario/Create
        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Usuario/Create

        [HttpPost]
        public ActionResult Create(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(usuarios);
        }
        
        //
        // GET: /Usuario/Edit/5
        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult Edit(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            return View(usuarios);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        public ActionResult Edit(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        //
        // GET: /Usuario/Delete/5
        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult Delete(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            return View(usuarios);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ListarUsuarios()
        {
            return View(db.Usuarios.OrderByDescending(u => u.IdUsuario).ToList());
        }

        public ActionResult LogIn()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LogIn(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                var usuario = from u in db.Usuarios
                              where usuarios.User.Equals(u.User) && usuarios.Password.Equals(u.Password)
                              select u;
                if (usuario.Count() == 0)
                {
                    //USuario no valido
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    
                }
                else 
                {
                    //USuario valido!!!
                    Usuarios user = usuario.First<Usuarios>();
                    FormsAuthentication.SetAuthCookie(user.User, user.RememberMe);

                    
                    //PENDIENTE: NO SIEMPRE REDIRIGIR AL HOME
                    return RedirectToAction("Index", "Home");
                    //return View();
                        
                }

                /*
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
                */
            }
            // If we got this far, something failed, redisplay form
            return View(usuarios);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        public int GetIdUsuarioPorUserName(string username)
        {
            var usuario = from u in db.Usuarios
                          where u.User == username
                          select u.IdUsuario;
            return usuario.First();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}