using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication4.Models;
using System.IO;

namespace MvcApplication4.Controllers
{ 
    public class ImagenController : Controller
    {
        private MLAContext db = new MLAContext();

        //
        // GET: /Imagen/

        public ViewResult Index()
        {
            //Busqueda();
            ViewBag.Ruta = "asdasd";
            return View(db.Imagens.ToList());
        }

        //
        // GET: /Imagen/Details/5

        public ViewResult Details(int id)
        {
            Imagen imagen = db.Imagens.Find(id);
            return View(imagen);
        }

        //
        // GET: /Imagen/Create

        public ActionResult Create()
        {
            //return View();
            //Busqueda();
            return View(new ImagenViewModel());
        } 

        //
        // POST: /Imagen/Create

        /*[HttpPost]
        public ActionResult Create(Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                db.Imagens.Add(imagen);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(imagen);
        }*/

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ImagenViewModel model)
        {
         

            
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };



                if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
                {
                    ModelState.AddModelError("ImageUpload", "This field is required");
                }
                else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
                {
                    ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image");
                }

                if (ModelState.IsValid)
                {
                    var image = new Imagen
                    {
                        Titulo = model.Titulo,
                        Caption = model.Caption
                    };

                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        //PENDIENTE: var uploadDir = "~/uploads";
                        var uploadDir = "~/uploads";

                        var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                        var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                        model.ImageUpload.SaveAs(imagePath);
                        image.RutaFoto = imageUrl;
                    }

                    db.Imagens.Add(image);
                    //db.Create(image);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
           return View(model);
        }
        
        //
        // GET: /Imagen/Edit/5
 
        public ActionResult Edit(int id)
        {
            /*
            Imagen imagen = db.Imagens.Find(id);
            return View(imagen);
             * */

            var image = db.Imagens.Find(id);
            
            if (image == null)
            {
                return new HttpNotFoundResult();
            }

            var model = new ImagenViewModel
            {
                Titulo = image.Titulo,
                Caption = image.Caption
            };

            return View(model);
        }

        //
        // POST: /Imagen/Edit/5
        /*
        [HttpPost]
        public ActionResult Edit(Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imagen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imagen);
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ImagenViewModel model)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (model.ImageUpload != null || model.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image");
            }

            if (ModelState.IsValid)
            {
                var image = db.Imagens.Find(id);
                if (image == null)
                {
                    return new HttpNotFoundResult();
                }

                image.Titulo = model.Titulo;
                image.Caption = model.Caption;

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/uploads";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                    model.ImageUpload.SaveAs(imagePath);
                    image.RutaFoto = imageUrl;
                }

                //db.Update(image);
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Imagen/Delete/5
 
        public ActionResult Delete(int id)
        {
            Imagen imagen = db.Imagens.Find(id);
            return View(imagen);
        }

        //
        // POST: /Imagen/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Imagen imagen = db.Imagens.Find(id);
            db.Imagens.Remove(imagen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void Busqueda()
        {
            var categoriasProductos = from cp in db.CategoriaProductoes
                                      where cp.Visible == true
                                      select cp;

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Todos", Value = "" });
            foreach (CategoriaProductos cat in categoriasProductos)
            {
                items.Add(new SelectListItem { Text = cat.DescripcionI1, Value = cat.IdCategoriaProducto.ToString() });
            }

            ViewBag.Categorias = items;
            ViewBag.CategoriasProductos = categoriasProductos.ToList();
        }


        /*
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Prueba(ImagenViewModel model)
        {
            //Busqueda();

            var imagePath = Path.Combine(Server.MapPath("~/uploads"), model.ImageUpload.FileName);
            
            model.ImageUpload.SaveAs(imagePath);

            return View();
        }*/

        public ActionResult Prueba()
        {
            //return View();
            //Busqueda();
            ViewBag.Prueba = "TEXTO";
            return View();
        } 

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}