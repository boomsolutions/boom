using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using MvcApplication4.Models;
using System.Web.Security;
using System.IO;
using System.Net.Mail;
using System.Globalization;
using System.Security.Cryptography;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Media;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Helpers;
using Quartz;
using Quartz.Impl;
using Twilio;

namespace MvcApplication4.Controllers
{
    [ValidateInput(false)]
    public class HomeController : Controller
    {
        private MLAContext db = new MLAContext();

        public ActionResult Index(string searchString, string categorias)
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            var productos = from p in db.Productos
                            where p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1
                            select p;

            Busqueda();

            if (categorias != null)
            {
                categorias = Funciones.Decrypt(HttpUtility.UrlDecode(categorias));
            }
            
            List<Productos> resultadoProductos = new List<Productos>();


            if (!String.IsNullOrEmpty(searchString))
            {

                string searchClean = RemoveDiacritics(searchString);


                //productos = productos.Where(s => RemoveDiacritics(s.Nombre).Contains(RemoveDiacritics(searchString)) || RemoveDiacritics(s.Descripcion).Contains(RemoveDiacritics(searchString)));

                string[] arraySearch = searchClean.Split(' ');


                foreach (string str in arraySearch)
                {
                    resultadoProductos.AddRange(productos.Where(s => (s.NombreBusqueda.Contains(str) || s.DescripcionBusqueda.Contains(str))));
                    //productos = 
                }


            }
            else
            {
                resultadoProductos = productos.ToList<Productos>();
            }


            //resultadoProductos = resultadoProductos.Distinct().ToList();

            IEnumerable<Productos> res = resultadoProductos.Distinct(); // new List<Productos>();
            
            if (!String.IsNullOrEmpty(categorias))
            {
                int catFilter = Convert.ToInt32(categorias);
                res = res.Where(s => s.IdCategoriaProducto.Equals(catFilter));
                //productos = productos.Where(s => s.IdCategoriaProducto.Equals(catFilter));
            }

            ViewData["Productos"] = res.ToList<Productos>(); // productos.ToList<Productos>();

            if (searchString == null && categorias == null)
            {
                return View();
            }
            else
            {
                return Listar(searchString, categorias);
            }
        }

        public ActionResult Listar(string searchString, string categorias)
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            
            var productos = from p in db.Productos
                            where p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1
                            select p;

            Busqueda();

            //HttpUtility.UrlEncode(Encrypt("JULIA"));
            //Decrypt(HttpUtility.UrlDecode(searchString));
            string searchClean = searchString; // Decrypt(HttpUtility.UrlDecode(searchString));
            string catClean = categorias; // Decrypt(HttpUtility.UrlDecode(categorias));

            List<Productos> resultadoProductos = new List<Productos>();

            
            if (!String.IsNullOrEmpty(searchString))
            {
                searchClean = RemoveDiacritics(searchString);

                //productos = productos.Where(s => s.Nombre.Contains(RemoveDiacritics(searchClean)) || s.Descripcion.Contains(RemoveDiacritics(searchClean)));
                //productos = productos.Where(s => s.NombreBusqueda.Contains(searchClean) || s.DescripcionBusqueda.Contains(searchClean));

                string[] arraySearch = searchClean.Split(' ');

                
                foreach (string str in arraySearch)
                {
                    resultadoProductos.AddRange(productos.Where(s => s.NombreBusqueda.Contains(str) || s.DescripcionBusqueda.Contains(str)));
                    //productos = 
                }
         
            }
            else
            {
                resultadoProductos = productos.ToList<Productos>();
            }

            IEnumerable<Productos> res = resultadoProductos.Distinct(); // new List<Productos>();

            int catFilter = 0; 
            if (!String.IsNullOrEmpty(catClean))
            {
                catFilter = Convert.ToInt32(catClean);
                res = res.Where(s => s.IdCategoriaProducto.Equals(catFilter));
                //productos = productos.Where(s => s.IdCategoriaProducto.Equals(catFilter));
            }

            
            
                
            GrabarLogBusqueda(searchString, catFilter);




            ViewBag.Productos = res.ToList<Productos>(); // productos.ToList<Productos>();
            
            return View("List");
        }


        public ActionResult About()
        {
            Busqueda();

            return View();
        }

        public ActionResult Buscar(string searchString)
        {
            var productos = from p in db.Productos
                            where p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(s => s.Nombre.Contains(searchString));
            }

            
            return View(productos);
        }

        //
        // GET: /Productos/Details/5
        //[Authorize]
        public ViewResult Details(string id)
        {

            //HttpUtility.UrlEncode(Encrypt("JULIA"));
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            
            Productos productos = db.Productos.Find(idProd);


            ViewBag.Message = "Welcome to ASP.NET MVC!";

            /*
            var productos = from p in db.Productos
                            select p;
            */

            Busqueda();

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;
            if (usuario.Count() > 0)
            {
                ViewBag.IdUsuarioLogueado = usuario.First();
            }
            ViewBag.IdUsuarioProducto = productos.IdUsuario;
            
            //GRABO EL LOG VISITAS
            GrabarLogVisitasProductos(productos);

            return View(productos);
        }

        //COMENTARIO ALQUILER
        

        //
        // POST: /ComentarioAlquiler/Create
        //BORRAR?????
        [HttpPost]
        public ActionResult CreateComentarioAlquiler(ComentarioAlquileres comentarioalquileres)
        {
            comentarioalquileres.FechaComentario = DateTime.Now;
            comentarioalquileres.IdUsuario = 1; //PENDIENTE
            comentarioalquileres.IdAlquiler = 1004;

            if (ModelState.IsValid)
            {
                db.ComentarioAlquilers.Add(comentarioalquileres);
                db.SaveChanges();

                GrabarLog(1, comentarioalquileres.IdUsuario, "ComentarioAlquileres", null, db.Entry(comentarioalquileres));

                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", comentarioalquileres.IdUsuario);
            ViewBag.IdAlquiler = new SelectList(db.Alquilers, "IdAlquiler", "IdAlquiler", comentarioalquileres.IdAlquiler);
            return View(comentarioalquileres);
        }

        //
        // POST: /ComentarioAlquiler/Create
        //BORRAR?????????
        [HttpPost]
        public ActionResult CreateComentariosProducto(ComentariosProducto comentarioproducto)
        {
            comentarioproducto.FechaComentario = DateTime.Now;
            //comentarioproducto.IdUsuario = 1; //PENDIENTE
            //comentarioproducto.IdAlquiler = 1004;

            if (ModelState.IsValid)
            {
                db.ComentariosProducto.Add(comentarioproducto);
                //db.ComentarioAlquilers.Add(comentarioalquileres);
                db.SaveChanges();

                GrabarLog(1, comentarioproducto.IdUsuario, "ComentariosProductoes", null, db.Entry(comentarioproducto));


                return RedirectToAction("Details", new { id = comentarioproducto.IdProducto });
            }

            //ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", comentarioalquileres.IdUsuario);
            //ViewBag.IdAlquiler = new SelectList(db.Alquilers, "IdAlquiler", "IdAlquiler", comentarioalquileres.IdAlquiler);
            return View(comentarioproducto);
        }

        #region Funciones Usuario

        public ActionResult LogInFacebookOK()
        {
            Busqueda();
            return View();
        }

        public ActionResult LogInFacebook(string email, string nombre, string id)
        {
            Busqueda();

            //string resp = email;
            ViewBag.Email = email;
            string nombreUser = "";
            string apellidoUser = "";
            if (nombre.Split(' ').Count() > 1)
            {
                nombreUser = nombre.Split(' ')[0];
                apellidoUser = nombre.Split(' ')[1];
                ViewBag.Nombre = nombreUser;
                ViewBag.Apellido = apellidoUser;
            }
            else
            {
                nombreUser = nombre.Split(' ')[0];
                apellidoUser = "N/A";
                ViewBag.Nombre = nombreUser;
                ViewBag.Apellido = apellidoUser;
            }
            //Estado ACTIVO
           
            
            var estadoUsuarioActivo = from eu in db.EstadosUsuario
                                        where eu.IdEstadoUsuarioFijo == 1
                                        select eu.IdEstadoUsuario;

            int idestadoactivo = estadoUsuarioActivo.First();

            //string passwordEncriptada = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usuarios.Password));

            var usuario = from u in db.Usuarios
                            where email.Equals(u.Email) && u.EstadoUsuario.IdEstadoUsuario == idestadoactivo
                            select u;
                
            if (usuario.Count() == 0)
            {
                //USuario no valido
                //ModelState.AddModelError("", "El usuario o contraseña no son correctos.");
                
                //GENERO UN USUARIO NUEVO y lo identifico para saber que se logeo con facebook
                Usuarios userNuevo = new Usuarios();
                userNuevo.Nombre = nombreUser;
                userNuevo.Apellido = apellidoUser;
                userNuevo.Email = email;
                userNuevo.Telefono = "PENDIENTE";
                userNuevo.FechaNacimiento = DateTime.Now;
                userNuevo.Sexo = "N/A";
                userNuevo.User = email.Split('@')[0];
                userNuevo.Password = "N/A";
                userNuevo.ConfirmPassword = "N/A";
                userNuevo.EsUsuarioFacebook = true;
                userNuevo.UsuarioFacebookID = id;
                userNuevo.IdEstadoUsuario = 1;

                db.Usuarios.Add(userNuevo);

                db.SaveChanges();

                GrabarLog(1, userNuevo.IdUsuario, "Usuarios", null, db.Entry(userNuevo));

                FormsAuthentication.SetAuthCookie(userNuevo.User, true);

                return RedirectToAction("LogInFacebookOK", "Home");
                //return View();

                /*
                string message = "¡Bienvenido!";
                string messageDetalle = "Te registraste correctamente con tu cuenta de facebook, debes completar tus datos personales <a href='" + Url.Content("~/Home/EditUser") + "'>aquí</a>.";
                string strLink = "/Home/EditUser";
                string strLinkDesc = "aquí";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle, link = strLink, linkdesc = strLinkDesc });
                 */ 
            }
            else
            {
                //USuario valido!!!
                Usuarios user = usuario.First<Usuarios>();
                FormsAuthentication.SetAuthCookie(user.User, user.RememberMe);

                //GrabarLogAccesoLogin(user.IdUsuario);

                //return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home");
                
            }

            
        }

        public ActionResult LogIn(string returnUrl)
        {
            Busqueda();
            
            ViewBag.ReturnUrl = returnUrl;
            /*ViewBag.UrlReferrer = Request.UrlReferrer.AbsolutePath;
            */
            //ViewBag.UrlReferrer = Request.UrlReferrer.ToString();

            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Usuarios usuarios, string ReturnUrl)
        {
            Busqueda();

            //string[] url = Regex.Split(ReturnUrl,"%2f");
            
            
            //string textofijo = "/MvcApplication4/Home/LogIn?ReturnUrl=%2fMvcApplication4%2fHome%2f";
            //returnUrl = returnUrl.Replace(textofijo, "");
            //if (ModelState.IsValid)
            {
                //Estado ACTIVO
                var estadoUsuarioActivo = from eu in db.EstadosUsuario
                                          where eu.IdEstadoUsuarioFijo == 1
                                          select eu.IdEstadoUsuario;

                int idestadoactivo = estadoUsuarioActivo.First();

                string passwordEncriptada = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usuarios.Password));

                var usuario = from u in db.Usuarios
                              where usuarios.User.Equals(u.User) && u.Password.Equals(passwordEncriptada) && u.EstadoUsuario.IdEstadoUsuario == idestadoactivo
                              select u;
                
                if (usuario.Count() == 0)
                {
                    //USuario no valido
                    ModelState.AddModelError("", "El usuario o contraseña no son correctos.");

                }
                else
                {
                    //USuario valido!!!
                    Usuarios user = usuario.First<Usuarios>();
                    FormsAuthentication.SetAuthCookie(user.User, user.RememberMe);

                    GrabarLogAccesoLogin(user.IdUsuario);

                    //PENDIENTE: NO SIEMPRE REDIRIGIR AL HOME

                    /*if (ReturnUrl.Contains("/AlquilarFechas") || ReturnUrl.Contains("/Consultar"))
                    {
                        return Redirect(UrlReferrer);
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }*/


                    //return Redirect(ReturnUrl);
                    return RedirectToAction("Index", "Home");
                    
                    /*
                    if (url.Length < 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction(url[url.Length - 1], url[url.Length - 2]);
                    }
                    */

                }

                
            }

            /*
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.UrlReferrer = UrlReferrer;
            */

            // If we got this far, something failed, redisplay form
            return View(usuarios);
        }

        public ActionResult LogOut()
        {
            var usuario = from u in db.Usuarios
                          where User.Identity.Name == u.User
                          select u;
            
            FormsAuthentication.SignOut();


            GrabarLogAccesoLogout(usuario.FirstOrDefault().IdUsuario);
            
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Usuario/Create

        public ActionResult CreateUser()
        {
            Busqueda();
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            ViewBag.ListaDias = DevolverDias(0); //listaDias; //.ToList()ñ new SelectList(listaDias);
            ViewBag.ListaMeses = DevolverMeses(0); // listaMeses;
            ViewBag.ListaAnios = DevolverAnios(0); // listaAnios;


            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        public ActionResult CreateUser(ICollection<HttpPostedFileBase> ImageUpload, Usuarios usuarios, int ListaDias, int ListaMeses, int ListaAnios, bool? InscribirseNL)
        {

            Busqueda();

            ModelState.Clear();

            var existeEmail = from u1 in db.Usuarios
                             where u1.Email == usuarios.Email
                             select u1.IdUsuario;

            var existeUser = from u1 in db.Usuarios
                             where u1.User == usuarios.User
                             select u1.IdUsuario;

            //ValidacionCampoEmail

            if (usuarios.Email.Contains("@") && usuarios.Email.Contains("."))
            {
                if (existeEmail.Count() != 0)
                {
                    ModelState.AddModelError("Email", "El Email ingresado ya se encuentra en uso!");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "El Email ingresado no tiene el formato correcto.");
            }

            // usuarios.User.Length > 8

            if (usuarios.User.Contains("@"))
            {
                ModelState.AddModelError("User", "El nombre de usuario no puede contener '@'.");
            }
            else
            {
                if(usuarios.User.Contains(" "))
                {
                    ModelState.AddModelError("User", "El nombre de usuario no puede contener espacios.");
                }
                else
                {
                    if (existeUser.Count() != 0)
                    {
                        ModelState.AddModelError("User", "El Usuario ingresado ya se encuentra en uso!");
                    }
                }
            }
            
            if (!usuarios.AceptoTerminosYCondiciones)
            {
                ModelState.AddModelError("AceptoTerminosYCondiciones", "Debes aceptar los términos y condiciones.");
            }

            if (ListaMeses == 0 || ListaDias == 0 || ListaAnios == 0)
            {
                ModelState.AddModelError("FechaNacimiento", "Fecha de nacimiento es un campo requerido.");
            }

            if (usuarios.Password.Equals(usuarios.ConfirmPassword))
            {
                //OK
            }
            else
            {
                ModelState.AddModelError("ConfirmPassword", "La contraseñas deben coincidir.");
            }

            //validacion telefono
            if(String.IsNullOrEmpty(usuarios.Telefono))
            {
                //ERROR
            }
            else
            {
                //CELULAR
                if (usuarios.Telefono.Trim().StartsWith("09"))
                {
                    if (usuarios.Telefono.Trim().Length == 9)
                    {
                        //OK
                    }
                    else 
                    {
                        ModelState.AddModelError("Telefono", "El formato del telefono no es correcto.");
                    }
                }

                //internacional - +598 94 123456 o +598 2600 1234
                if (usuarios.Telefono.Trim().StartsWith("+"))
                {
                    if (usuarios.Telefono.Trim().Length == 12)
                    {
                        //OK
                    }
                    else
                    {
                        ModelState.AddModelError("Telefono", "El formato del telefono no es correcto.");
                    }
                }

                //de linea - 2600 1234
                if (usuarios.Telefono.Trim().StartsWith("2"))
                {
                    if (usuarios.Telefono.Trim().Length == 8)
                    {
                        //OK
                    }
                    else
                    {
                        ModelState.AddModelError("Telefono", "El formato del telefono no es correcto.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                
                
                var estadopend = from eu in db.EstadosUsuario
                                   where eu.IdEstadoUsuarioFijo == 2
                                   select eu.IdEstadoUsuario;

                //usuarios.IdBarrio = 2;
                
                //usuarios.IdDepartamento = 10;
                //usuarios.IdBarrio = 2;

                usuarios.IdEstadoUsuario = estadopend.First(); //PENDIENTE
                //usuarios.Sexo = "N/A"; se volvio a habilitar
                usuarios.FechaNacimiento = new DateTime(ListaAnios, ListaMeses, ListaDias);
                 
                //ENCRIPTO LA PASSWORD
                usuarios.Password = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usuarios.Password));
                usuarios.ConfirmPassword = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usuarios.ConfirmPassword));
                //DATOS FIJOS POR AHORA!!
                //usuarios.IdPais = 1;
                //usuarios.IdCiudad = 1;
                usuarios.Sexo = "N/A";
                
                db.Usuarios.Add(usuarios);

                Suscriptor sus = new Suscriptor();

                if (InscribirseNL.Value)
                {    
                    sus.Email = usuarios.Email;
                    sus.Nombre = usuarios.Nombre;
                    sus.IdBarrio = usuarios.IdBarrio;
                    sus.IdDepartamento = Convert.ToInt32(usuarios.IdDepartamento);
                    sus.FechaNacimiento = usuarios.FechaNacimiento;
                    sus.FechaSuscripto = DateTime.Now;

                    db.Suscriptores.Add(sus);
                }
                //EntityState estado = db.Entry(usuarios).State;
                
                db.SaveChanges();

                GrabarLog(1, usuarios.IdUsuario, "Usuarios", null, db.Entry(usuarios));


                //SUBO LA IMAGEN
                foreach (HttpPostedFileBase file in ImageUpload)
                {
                    if (file != null)
                    {
                        int idusuariogenerado = usuarios.IdUsuario;
                        ImagenViewModel ivm = new ImagenViewModel();
                        ivm.ImageUpload = file;
                        ivm.Titulo = file.FileName;
                        ivm.Caption = file.FileName;

                        Imagen nuevaimagen = CreateImagen(ivm);

                        FotoUsuarios fu = new FotoUsuarios();
                        fu.IdUsuario = idusuariogenerado;
                        fu.IdImagen = nuevaimagen.IdImagen;
                        db.FotosUsuarios.Add(fu);
                        db.SaveChanges();

                        //PENDIENTE
                        //GrabarLog(1, comentarioproducto.IdUsuario, "ComentariosProductoes", null, db.Entry(comentarioproducto));

                    }
                }

                EnviarMailConfirmarUsuarioNuevo(usuarios);

                if (InscribirseNL.Value)
                {
                    EnviarMailSuscriptor(sus);
                }

                string message = "¡Bienvenido!";
                string messageDetalle = "Te hemos enviado un correo electrónico de confirmación, recuerda revisar tu carpeta de SPAM.";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }

            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", usuarios.IdBarrio);
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", usuarios.IdDepartamento);

            ViewBag.ListaDias = DevolverDias(ListaDias); //listaDias; //.ToList()ñ new SelectList(listaDias);
            ViewBag.ListaMeses = DevolverMeses(ListaMeses); // listaMeses;
            ViewBag.ListaAnios = DevolverAnios(ListaAnios); // listaAnios;

            return View(usuarios);
        }

        //
        // GET: /Usuario/Edit/5
        [Authorize]
        public ActionResult EditUser(string id)
        {
            Busqueda();


            int idUsuario = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Usuarios usuarios = db.Usuarios.Find(idUsuario);

            if (User.Identity.Name == usuarios.User)
            {

                ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
                ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

                ViewBag.ListaDias = DevolverDias(usuarios.FechaNacimiento.Day); //listaDias; //.ToList()ñ new SelectList(listaDias);
                ViewBag.ListaMeses = DevolverMeses(usuarios.FechaNacimiento.Month); // listaMeses;
                ViewBag.ListaAnios = DevolverAnios(usuarios.FechaNacimiento.Year); // listaAnios;


                return View(usuarios);

                //return View("ViewUser",usuarios);
            }
            else
            {
                string message = "¡RUTA NO VALIDA!";
                string messageDetalle = "";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        public ActionResult EditUser(ICollection<HttpPostedFileBase> ImageUpload, string IdUsuarioEncrypt, /*string User */Usuarios usuarios, int ListaDias, int ListaMeses, int ListaAnios)
        {

            Busqueda();

            ModelState.Clear();


            var existeEmail = from u1 in db.Usuarios
                              where u1.Email == usuarios.Email
                              select u1.IdUsuario;

            var existeUser = from u1 in db.Usuarios
                             where u1.User == usuarios.User
                             select u1.IdUsuario;

            

            

            int id = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdUsuarioEncrypt)));
            usuarios.IdUsuario = id;
            
            var usuarioOriginal = db.Usuarios.AsNoTracking().Where(u => u.IdUsuario == usuarios.IdUsuario).FirstOrDefault();
            usuarios.IdEstadoUsuario = usuarioOriginal.IdEstadoUsuario;

            //var fotosUsuarioOriginal = db.FotosUsuarios.AsNoTracking().Where(fu => fu.IdUsuario == usuarios.IdUsuario).FirstOrDefault();
            //var fotosUsuarioOriginal = db.FotosUsuarios.AsNoTracking().Where(fu => fu.IdUsuario == usuarios.IdUsuario);
            //usuarios.FotosUsuarios = fotosUsuarioOriginal.ToList();

            //usuarios.FotosUsuarios = fotosUsuarioOriginal; // new List<FotoUsuarios>(); //asigno la foto al objeto usuario

            //foreach (FotoUsuarios fu in usuarioOriginal.FotosUsuarios)
            //{
            //    usuarios.FotosUsuarios.Add(fu);
            //}

            if (ListaMeses == 0 || ListaDias == 0 || ListaAnios == 0)
            {
                ModelState.AddModelError("FechaNacimiento", "Fecha de nacimiento es un campo requerido.");
            }
            /*
            if (usuarioOriginal.FotosProductos.Count() == 0 && ImageUpload.First() == null)
            {
                ModelState.AddModelError("ImageUpload", "Debes subir al menos una foto.");
            }
            */
            if (ModelState.IsValid)
            {
                usuarios.User = usuarioOriginal.User;
                usuarios.FechaNacimiento = new DateTime(ListaAnios, ListaMeses, ListaDias);
                usuarios.Email = usuarioOriginal.Email;
                usuarios.Password = usuarioOriginal.Password;
                usuarios.ConfirmPassword = usuarioOriginal.ConfirmPassword;
                usuarios.RememberMe = false;
                usuarios.Sexo = usuarioOriginal.Sexo;

                db.Entry(usuarios).State = EntityState.Modified;

                //GrabarLog(2, usuarios.IdUsuario, "Usuarios", db.Entry(usuarioOriginal), db.Entry(usuarios));

                ////GRABO CABEZAL
                //LogModificacionesCabezal lmc = new LogModificacionesCabezal();
                //lmc.Accion = 2;
                //lmc.Fecha = DateTime.Now;
                //lmc.IdUsuario = usuarios.IdUsuario;
                //lmc.Tabla = "Usuarios";
                //db.LogsModificacionesCabezales.Add(lmc);
                //db.SaveChanges();

                //DbPropertyValues valoresActuales = db.Entry(usuarios).CurrentValues;

                ////GRABO DETALLE
                //LogModificacionesDetalle lmd = new LogModificacionesDetalle();

                //foreach (string campo in valoresActuales.PropertyNames)
                //{

                //    if (!db.Entry(usuarioOriginal).Property(campo).CurrentValue.ToString().Equals(db.Entry(usuarios).Property(campo).CurrentValue.ToString()))
                //    {
                //        lmd.CampoModificado = campo;
                //        lmd.IdLogModificacionesCabezal = lmc.IdLogModificacionesCabezal;
                //        lmd.ValorAnterior = db.Entry(usuarioOriginal).Property(campo).CurrentValue.ToString();
                //        lmd.ValorActual = db.Entry(usuarios).Property(campo).CurrentValue.ToString();

                //        db.LogsModificacionesDetalles.Add(lmd);
                //    }
                //}

                
                //db.SaveChanges();

                //GUARDO EL USUARIO

                db.SaveChanges();
                /*
                try
                {
                    
                }
                catch (Exception e)
                {
                    string msj = e.Message;
                }
                */
                foreach (HttpPostedFileBase file in ImageUpload)
                {
                    if (file != null)
                    {
                        int idusugenerado = usuarios.IdUsuario;
                        ImagenViewModel ivm = new ImagenViewModel();
                        ivm.ImageUpload = file;
                        ivm.Titulo = file.FileName;
                        ivm.Caption = file.FileName;

                        Imagen nuevaimagen = CreateImagen(ivm);

                        FotoUsuarios fu = new FotoUsuarios();
                        fu.IdUsuario = idusugenerado;
                        fu.IdImagen = nuevaimagen.IdImagen;
                        db.FotosUsuarios.Add(fu);
                        db.SaveChanges();

                        //PENDIENTE
                        //GrabarLog(1, comentarioproducto.IdUsuario, "ComentariosProductoes", null, db.Entry(comentarioproducto));

                    }
                }

                //return RedirectToAction("Index");
                //return View("ViewUser");
                return RedirectToAction("ViewUser");
            }

            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            ViewBag.ListaDias = DevolverDias(ListaDias); //listaDias; //.ToList()ñ new SelectList(listaDias);
            ViewBag.ListaMeses = DevolverMeses(ListaMeses); // listaMeses;
            ViewBag.ListaAnios = DevolverAnios(ListaAnios); // listaAnios;

            //chanchada para que vuelvan a aparecer las fotos
            usuarios.FotosUsuarios = usuarioOriginal.FotosUsuarios.ToList();


            return View("ViewUser",usuarios);
            //return RedirectToAction("ViewUser",usuarios);
        }

        //
        // GET: /Usuario/Delete/5

        public ActionResult DeleteUser(string id)
        {
            //HttpUtility.UrlEncode(Encrypt("JULIA"));
            //Decrypt(HttpUtility.UrlDecode(searchString));
            int idUsuario = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            Usuarios usuarios = db.Usuarios.Find(idUsuario);
            return View(usuarios);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirmed(string id)
        {
            
            int idUsuario = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            Usuarios usuarios = db.Usuarios.Find(idUsuario);
            db.Usuarios.Remove(usuarios);

            GrabarLog(3, usuarios.IdUsuario, "Usuarios", db.Entry(usuarios), null);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        //[HttpPost]
        public ActionResult ErrorPage(string m1, string m2/*, string link, string linkdesc*/)
        {
            ViewBag.ErrorMessage = m1;
            ViewBag.ErrorMessageDetalle = m2;

            /*
            if (!String.IsNullOrEmpty(link))
            {
                ViewBag.Link = link;
                ViewBag.LinkDesc = linkdesc;
            }*/

            Busqueda();
            
            return View();
        }

        #region Funciones Alquiler
        //, LoggedOrAuthorizedAttribute
        [HttpPost, Authorize]
        public ActionResult AlquilarFechas(DateTime fechaDesde, DateTime fechaHasta, string IdProductoEncrypt)
        {
            Busqueda();

            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Json(new { url = Url.Action("LogIn") });
            //    //return RedirectToAction("LogIn", "Home");
            //}

            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdProductoEncrypt)));
            

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u;

            var usuarioMail = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.Email;

            //PENDIENTE = SOLICITADO
            var estadoalquiler = from ea in db.EstadosAlquiler
                          where ea.IdEstadoAlquilerFijo == 1
                          select ea.IdEstadoAlquiler;


            Alquileres alquiler = new Alquileres();
            alquiler.FechaDesde = fechaDesde;
            alquiler.FechaHasta = fechaHasta;
            alquiler.IdProducto = idProd;
            alquiler.IdUsuarioArrendatario = usuario.First().IdUsuario;
            alquiler.IdEstadoAlquiler = estadoalquiler.First();
            Productos productoAlq = db.Productos.Find(idProd);

            string mailFrom = usuarioMail.First();
            
            //en el futuro deberia ser una marca que se validaron los datos personales.
            if (usuario.First().Telefono.ToUpper() == "PENDIENTE")
            {
                ModelState.AddModelError("", "Debes completar tus datos personales antes de realizar una reserva");
            }

            if (ModelState.IsValid)
            {
                db.Alquilers.Add(alquiler);
                db.SaveChanges();

                GrabarLog(1, alquiler.IdUsuarioArrendatario, "Alquileres", null, db.Entry(alquiler));


                string msj = EnviarMailSolicitud(mailFrom, productoAlq.Usuario.Email, alquiler);
                
                EnviarSMSSolicitud(/*mailFrom, productoAlq.Usuario.Email, alquiler*/);
                
                ViewBag.ErrorMessage = msj;
                //ViewBag.ErrorMessageTitle = msj;
                TempData["msjSolicitud"] = "Tu solicitud ha sido enviada correctamente.";
                //return RedirectToAction("ErrorPage");
            }

            //ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            //ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            //return View(alquileres);

            
            
            //no se a donde ir
            return RedirectToAction("Details", new { id = IdProductoEncrypt });
            //return View("ErrorPage");
        }


        public void EnviarSMSSolicitud()
        {

            //http://bit.ly/2fu2p2Q

            Busqueda();
            // Find your Account Sid and Auth Token at twilio.com/user/account
            string AccountSid = "ACefeff6e4af493bda816a410099e0d430";

            string AccountSidTest = "ACd716ad7bc888de9414371d06b572fcc2";

            string AuthToken = "a77954720996fc86ef3e79279a13a448";

            string AuthTokenTest = "26babd61d0ded0040ebd4fdc139420c1";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage(
                "+19283623170", "+59894170358",
                "Hola! Recibiste una solicitud de alquiler para uno de tus articulos. Ingresa en: http://bit.ly/2fu2p2Q - MejorLoAlquilo",
                new string[] { /*"https://c1.staticflickr.com/3/2899/14341091933_1e92e62d12_b.jpg"*/ }
            );

            //return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ConsultarDesdeCuenta(ComentariosProducto comentarioproducto, string IdProductoEncrypt, string IdUsuarioEncrypt/*, String Consulta, int IdProducto*/)
        {
            Busqueda();

            ModelState.Clear();

            //prod donde estoy parado
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdProductoEncrypt)));
            comentarioproducto.IdProducto = idProd;

            //dueño de prod.
            int idUsu = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdUsuarioEncrypt)));

            // - REVISAR
            //comentarioproducto.IdUsuario = idUsu;

            //usuario logueado que hace la consulta
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u;

            Usuarios usuarioConsultante = usuario.First();

            if (usuarioConsultante.IdUsuario == idUsu)
            {
                ModelState.AddModelError("", "No puedes realizarte una consulta a tí mismo!");
            }


            if (!ValidarDatosPermitidos(comentarioproducto.Comentario))
            {
                ModelState.AddModelError("Comentario", "No se permiten datos personales en el texto del comentario.");
            }

            if (ModelState.IsValid)
            {
                comentarioproducto.EsRespuesta = false;
                comentarioproducto.IdUsuario = usuarioConsultante.IdUsuario;
                comentarioproducto.FechaComentario = DateTime.Now;
                comentarioproducto.Leido = false;

                db.ComentariosProducto.Add(comentarioproducto);
                //db.ComentarioAlquilers.Add(comentarioalquileres);
                db.SaveChanges();

                GrabarLog(1, usuarioConsultante.IdUsuario, "ComentariosProductoes", null, db.Entry(comentarioproducto));

                Productos prod = db.Productos.Find(comentarioproducto.IdProducto);
                string msj = EnviarMailConsulta(comentarioproducto.Comentario, prod);

                //string msj = EnviarMailConsulta(Consulta, prod.Usuario.Email);
                ViewBag.ErrorMessage = msj;
                //return RedirectToAction("ErrorPage");
                //return new JavaScriptResult { Script = "alert('mensaje Consultar')" };
                //return RedirectToAction("Details", new { id = comentarioproducto.IdProducto });
                TempData["msjConsulta"] = "Tu consulta ha sido enviada correctamente.";
            }
            //return Json(ModelState);

            //ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            //ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            //return View(alquileres);

            return RedirectToAction("ViewConsultasDelUsuario");


            //no se a donde ir
            //return new JavaScriptResult { Script = "alert('mensaje Consultar')" };
            //return View(comentarioproducto);
            //return View("ErrorPage");
        }


        [HttpPost]
        [Authorize]
        public ActionResult Consultar(ComentariosProducto comentarioproducto, string IdProductoEncrypt, string IdUsuarioEncrypt/*, String Consulta, int IdProducto*/)
        {
            Busqueda();

            ModelState.Clear();

            //prod donde estoy parado
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdProductoEncrypt)));
            comentarioproducto.IdProducto = idProd;
            
            //dueño de prod.
            int idUsu = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdUsuarioEncrypt)));
            comentarioproducto.IdUsuario = idUsu;

            //usuario logueado que hace la consulta
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u;

            Usuarios usuarioConsultante = usuario.First();

            if (usuarioConsultante.IdUsuario == idUsu)
            {
                ModelState.AddModelError("", "No puedes realizarte una consulta a tí mismo!");
            }


            if (!ValidarDatosPermitidos(comentarioproducto.Comentario))
            {
                ModelState.AddModelError("Comentario", "No se permiten datos personales en el texto del comentario.");
            }

            if (ModelState.IsValid)
            {
                comentarioproducto.IdUsuario = usuarioConsultante.IdUsuario;
                comentarioproducto.FechaComentario = DateTime.Now;
                comentarioproducto.Leido = false;

                db.ComentariosProducto.Add(comentarioproducto);
                //db.ComentarioAlquilers.Add(comentarioalquileres);
                db.SaveChanges();

                GrabarLog(1, usuarioConsultante.IdUsuario, "ComentariosProductoes", null, db.Entry(comentarioproducto));

                Productos prod = db.Productos.Find(comentarioproducto.IdProducto);
                string msj = EnviarMailConsulta(comentarioproducto.Comentario, prod);
                
                //string msj = EnviarMailConsulta(Consulta, prod.Usuario.Email);
                ViewBag.ErrorMessage = msj;
                //return RedirectToAction("ErrorPage");
                //return new JavaScriptResult { Script = "alert('mensaje Consultar')" };
                //return RedirectToAction("Details", new { id = comentarioproducto.IdProducto });
                TempData["msjConsulta"] = "Tu consulta ha sido enviada correctamente.";
            }
            //return Json(ModelState);
            
            //ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", alquileres.IdProducto);
            //ViewBag.IdUsuarioArrendatario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            //return View(alquileres);

            return RedirectToAction("Details", new { id = IdProductoEncrypt });
            
            
            //no se a donde ir
            //return new JavaScriptResult { Script = "alert('mensaje Consultar')" };
            //return View(comentarioproducto);
            //return View("ErrorPage");
        }

        [HttpPost]
        public ActionResult ResponderConsulta(ComentariosProducto respuestaComentarioproducto, int IdUsuarioConsulta)
        {
            Busqueda();
            
            //usuario que realizo la consulta, al cual estamos respondiendo
            var usuario = from u in db.Usuarios
                          where u.IdUsuario == IdUsuarioConsulta //respuestaComentarioproducto.IdUsuario
                          select u;

            Usuarios usuarioRespuesta = usuario.First();

            if (ModelState.IsValid)
            {
                respuestaComentarioproducto.EsRespuesta = true;
                respuestaComentarioproducto.FechaComentario = DateTime.Now;
                db.ComentariosProducto.Add(respuestaComentarioproducto);
                //db.ComentarioAlquilers.Add(comentarioalquileres);
                db.SaveChanges();

                GrabarLog(1, respuestaComentarioproducto.IdUsuario, "ComentariosProductoes", null, db.Entry(respuestaComentarioproducto));


                Productos prod = db.Productos.Find(respuestaComentarioproducto.IdProducto);
                string msj = EnviarMailRespuestaConsulta(respuestaComentarioproducto.Comentario, usuarioRespuesta, prod);

                //string msj = EnviarMailConsulta(Consulta, prod.Usuario.Email);
                ViewBag.ErrorMessage = msj;
                //return RedirectToAction("ErrorPage");
                //return RedirectToAction("ViewConsultasParaUsuario"); //, new { id = respuestaComentarioproducto.IdProducto });
            }

            return RedirectToAction("ViewConsultasParaUsuario");
            //no se a donde ir
            //return View(respuestaComentarioproducto);
            
        }
        

        public void EnviarMailSolicitudRecordatorio()
        { }

        public string EnviarMailSolicitud(string usuarioFrom, string usuarioTo, Alquileres alq)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                //message.To.Add("postmaster@mejorloalquilo.com");
                message.To.Add(usuarioTo);
                //if (ccList != null && ccList != string.Empty)
                  //  message.CC.Add(ccList);
                message.Subject = "MLA - Solicitud de Reserva";
                message.IsBodyHtml = true;


                string IdProductoEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(alq.IdProducto.ToString())).Replace("%", "%25");


                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + alq.Producto.Usuario.Nombre +", el usuario <strong>" + User.Identity.Name + "</strong> ha enviado una solicitud de reserva para el articulo <a href='http://www.mejorloalquilo.com/Home/Details/" + IdProductoEncrypt + "'>" + alq.Producto.Nombre + "</a> desde el " + alq.FechaDesde.ToShortDateString() + " hasta el " + alq.FechaHasta.ToShortDateString() + ".<br><br><a href='http://www.mejorloalquilo.com/Home/ViewSolicitudesParaUsuario'>Ver solicitudes pendientes</a>";
                //smtpClient.Host = "mail.mejorloalquilo.com";   // We use gmail as our smtp client
                //smtpClient.Host = "mail.mejorloalquilo.com";
                //smtpClient.Host = "relay-hosting.secureserver.net";
             
                //smtpClient.Port = 465; //465
                //smtpClient.EnableSsl = true;
                smtpClient.Timeout = 1000000;
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Successful<BR>";
                //ViewBag.ErrorMessage = "Envio de mail exitoso - Solicitud de Reserva";
            }
            catch (Exception ex)
            {
                msg = ex.Message /*+ " - " + ex.InnerException.Message*/;
                //ViewBag.ErrorMessage = msg;
            }

            return msg;
            //Response.Write(msg);
            
        }

        public string EnviarMailConsulta(String textoConsulta, Productos prod)
        {
            string mailTo = prod.Usuario.Email;
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(mailTo);
                //if (ccList != null && ccList != string.Empty)
                //  message.CC.Add(ccList);
                message.Subject = "MLA - Consulta al propietario";
                message.IsBodyHtml = true;

                string linkProd = "http://www.mejorloalquilo.com/Home/Details/" + HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(prod.IdProducto.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + prod.Usuario.Nombre + ", el usuario <strong>" + User.Identity.Name + "</strong> te envía la siguiente consulta sobre <a href='" + linkProd + "' >" + prod.Nombre + "</a>:<br><br><p>" + textoConsulta + "</p><br>";
                message.Body = message.Body + "<a style='background-color: #ff7e82; text-decoration:none; border-radius: 4px;font-weight: bold;padding: 10px 20px;display: inline-block;line-height: 20px;border: none;font-size: 15px;color: #fff;text-transform: capitalize;' href='http://www.mejorloalquilo.com/Home/ViewConsultasParaUsuario'>Ver consulta</a>";
                

                //smtpClient.Port = 465;
                //smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Successful<BR>";
                //ViewBag.ErrorMessage = "Envio de mail exitoso - Solicitud de Reserva";
            }
            catch (Exception ex)
            {
                msg = ex.Message /*+ " - " + ex.InnerException.Message*/;
                //ViewBag.ErrorMessage = msg;
            }

            return msg;
            //Response.Write(msg);

        }

        

        public string EnviarMailRespuestaConsulta(String textoConsulta, Usuarios usuarioResp, Productos prod)
        {
            string mailTo = usuarioResp.Email;
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(mailTo);
                //if (ccList != null && ccList != string.Empty)
                //  message.CC.Add(ccList);
                message.Subject = "MLA - Respuesta del propietario";
                message.IsBodyHtml = true;

                string linkProd = "http://www.mejorloalquilo.com/Home/Details/" + HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(prod.IdProducto.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + usuarioResp.Nombre +", el usuario <strong>" + User.Identity.Name + "</strong> respondio a su consulta sobre <a href='" + linkProd + "' >" + prod.Nombre + "</a>:<br><br><p>" + textoConsulta + "</p><br>";
                message.Body = message.Body + "<a style='background-color: #ff7e82; text-decoration:none; border-radius: 4px;font-weight: bold;padding: 10px 20px;display: inline-block;line-height: 20px;border: none;font-size: 15px;color: #fff;text-transform: capitalize;' href='http://www.mejorloalquilo.com/Home/ViewConsultasDelUsuario'>Ver respuesta</a>";
                //smtpClient.Host = "mail.mejorloalquilo.com";   // We use gmail as our smtp client
                //smtpClient.Host = "mail.mejorloalquilo.com";

                //smtpClient.Port = 465;
                //smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Successful<BR>";
                //ViewBag.ErrorMessage = "Envio de mail exitoso - Solicitud de Reserva";
            }
            catch (Exception ex)
            {
                msg = ex.Message /*+ " - " + ex.InnerException.Message*/;
                //ViewBag.ErrorMessage = msg;
            }

            return msg;
            //Response.Write(msg);

        }


        public ActionResult EnviarMailContacto(string nombre, string email, string asunto, string mensaje)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add("info@mejorloalquilo.com");
                //if (ccList != null && ccList != string.Empty)
                //  message.CC.Add(ccList);
                message.Subject = "MLA - Formulario de contacto";
                message.IsBodyHtml = true;
                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 100px;' /></a>";
                message.Body += "<br><br><strong>Nombre: </strong>" + nombre + "<br><br><strong>Email: </strong>" + email + "<br><br>";
                message.Body += "<strong>Asunto: </strong>" + asunto + "<br><br><strong>Mensaje: </strong><p>" + mensaje + "</p><br>";
                
                smtpClient.Timeout = 10000;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Successful<BR>";
                //ViewBag.ErrorMessage = "Envio de mail exitoso - Solicitud de Reserva";
            }
            catch (Exception ex)
            {
                msg = ex.Message /*+ " - " + ex.InnerException.Message*/;
                //ViewBag.ErrorMessage = msg;
            }

            //return msg;
            //Response.Write(msg);
            //ViewBag.Message = "Email enviado correctamente!";

            /*TempData["UI_Message"] = "CORREEEEEEETTTTTOOOOOOOOOO!";
            return RedirectToAction("Contact");
            *///return View();

            string titulo1 = "¡Tu mensaje ha sido enviado correctamente!";
            string messageDetalle = "Estaremos respondiendo a tu mensaje a la brevedad.";

            
            return RedirectToAction("ErrorPage", new { m1 = titulo1, m2 = messageDetalle });
        }
        
        #endregion



        public ActionResult ViewAlquileresPorUsuario()
        {
            Busqueda();

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true)
                            select p;

            // ALQUILERES CONCRETADOS DE MIS PRODUCTOS
            var alquileres = from a in db.Alquilers
                             where a.Producto.Usuario.User == User.Identity.Name && a.EstadoAlquiler.IdEstadoAlquilerFijo == 3
                             orderby a.IdAlquiler descending
                             select a;

            ViewBag.Productos = productos.ToList<Productos>();
            ViewBag.IdUsuario = usuario.First();
            ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            return View();
        }

        public ActionResult ViewAlquileresDelUsuario()
        {
            Busqueda();

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true)
                            select p;

            // MIS ALQUILERES CONCRETADOS SOBRE PRODUCTOS DE OTROS
            var alquileres = from a in db.Alquilers
                             where a.UsuarioArrendatario.User == User.Identity.Name && a.EstadoAlquiler.IdEstadoAlquilerFijo == 3
                             orderby a.IdAlquiler descending
                             select a;

            MarcarReservasAprobadasComoLeidas();

            ViewBag.Productos = productos.ToList<Productos>();
            ViewBag.IdUsuario = usuario.First();
            ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            return View();
        }

        [Authorize]
        public ActionResult ViewUser()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            
            ViewBag.IdUsuario = usuario.First();

            var usuarioTodo = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u;

            Usuarios usuCompleto = usuarioTodo.First(); 
            ViewBag.Usuario = usuCompleto;

            //string name = HttpUtility.UrlEncode(Encrypt("JULIA"));
            /*
            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true)
                            orderby p.IdProducto descending
                            select p;
            */
            /*&& (p.EstadoProducto.IdEstadoProductoFijo == 1 || p.EstadoProducto.IdEstadoProductoFijo == 3))*/
            /*
            var alquileres = from a in db.Alquilers
                             where a.UsuarioArrendatario.User == User.Identity.Name
                             orderby a.IdAlquiler descending
                             select a;
            */
            Busqueda();
            /*
            if (productos.Count() > 0)
            {
                ViewBag.Productos = productos.ToList<Productos>();
            
            }

            if (alquileres.Count() > 0)
            {
                ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            
            }
            */
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";

            if (User.Identity.Name == usuCompleto.User)
            {
                ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", usuCompleto.IdBarrio);
                ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", usuCompleto.IdDepartamento);

                ViewBag.ListaDias = DevolverDias(usuCompleto.FechaNacimiento.Day); //listaDias; //.ToList()ñ new SelectList(listaDias);
                ViewBag.ListaMeses = DevolverMeses(usuCompleto.FechaNacimiento.Month); // listaMeses;
                ViewBag.ListaAnios = DevolverAnios(usuCompleto.FechaNacimiento.Year); // listaAnios;


                return View(usuCompleto);
            }
            else
            {
                string message = "¡RUTA NO VALIDA!";
                string messageDetalle = "";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }
        }

        
        public ActionResult ViewPublicaciones()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;


            ViewBag.IdUsuario = usuario.First();
            //string name = HttpUtility.UrlEncode(Encrypt("JULIA"));

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true)
                            orderby p.IdProducto descending
                            select p;

            /*&& (p.EstadoProducto.IdEstadoProductoFijo == 1 || p.EstadoProducto.IdEstadoProductoFijo == 3))*/

            var alquileres = from a in db.Alquilers
                             where a.UsuarioArrendatario.User == User.Identity.Name
                             orderby a.IdAlquiler descending
                             select a;

            Busqueda();

            if (productos.Count() > 0)
            {
                ViewBag.Productos = productos.ToList<Productos>();

            }

            if (alquileres.Count() > 0)
            {
                ViewBag.Alquileres = alquileres.ToList<Alquileres>();

            }

            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";

            return View();
        }

        [Authorize]
        public ActionResult ViewConsultasParaUsuario()
        {
            Busqueda();

            /*
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            ViewBag.IdUsuario = usuario.First();
            */
            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true) && p.ComentariosProducto.Count() != 0
                            select p;
            /*
            var alquileres = from a in db.Alquilers
                             where a.UsuarioArrendatario.User == User.Identity.Name
                             select a;
            */
            //NO SE USA
            /*var comentariosproductos = from cp in db.ComentariosProducto
                                       where cp.Usuario.User == User.Identity.Name
                                       select cp;
            */

            //productos con comentarios donde el USUARIO logueado es el duenho
            var productosConComentarios = from cp in db.ComentariosProducto
                                          where (cp.Producto.Usuario.User == User.Identity.Name)
                                          orderby cp.IdComentariosProducto descending
                                          select cp.Producto;

            //traigo los usuarios que hicieron consultas sobre mis productos
            var usuariosConsultaronMisProductos = from cp in db.ComentariosProducto
                                                  where (cp.Producto.Usuario.User == User.Identity.Name)
                                                  orderby cp.IdComentariosProducto descending
                                                  select cp.Usuario;

            //comentarios sobre productos donde el USUARIO logueado es el duenho
            var comentarios = from cp in db.ComentariosProducto
                                          where (cp.Producto.Usuario.User == User.Identity.Name)
                                          orderby cp.IdComentariosProducto descending
                                          select cp;

            if (productosConComentarios.Count() > 0)
            {
                //el unico que se usa!!
                ViewBag.Productos = productosConComentarios.Distinct().ToList<Productos>();

                ViewBag.UsuariosConsultantes = usuariosConsultaronMisProductos.Distinct().ToList<Usuarios>();

                ViewBag.Comentarios = comentarios.ToList<ComentariosProducto>();
                //ViewBag.Alquileres = alquileres.ToList<Alquileres>();
                //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer();
            }
            /*
            if (comentariosproductos.Count() > 0)
            {
                ViewBag.ComentariosProductos = comentariosproductos.ToList<ComentariosProducto>();

            }
            */
            MarcarConsultasComoLeidas();

            ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            ViewBag.CantidadConsultasSinLeer = DevolverCantidadConsultasSinLeer();

            return View();
        }

        //Mis alquileres - Preguntas
        public ActionResult ViewConsultasDelUsuario()
        {
            Busqueda();

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;
            
            ViewBag.IdUsuario = usuario.First();

            //productos con comentarios DEL usuario logueado
            var productosConComentarios = from cp in db.ComentariosProducto
                            where (cp.Usuario.User == User.Identity.Name && cp.IdComentariosProductoOrigen == null)
                            orderby cp.IdComentariosProducto descending
                            select cp.Producto;


            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1)
                            select p;

            
            if (productosConComentarios.Count() > 0)
            {
                ViewBag.Productos = productosConComentarios.ToList<Productos>();
            }
            
            MarcarRespuestasComoLeidas();

            ViewBag.CantidadRespuestasSinLeerString = DevolverCantidadRespuestasSinLeer() == 0 ? "" : " (" + DevolverCantidadRespuestasSinLeer() + ")";
            ViewBag.CantidadRespuestasSinLeerInt = DevolverCantidadRespuestasSinLeer();

            return View();
        }

        [Authorize]
        public ActionResult ViewSolicitudesParaUsuario()
        {
            Busqueda();
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            ViewBag.IdUsuario = usuario.First();

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1)
                            select p;

            //ESTADO PENDIENTES
            var estadoalquiler = from ea in db.EstadosAlquiler
                             where ea.IdEstadoAlquilerFijo == 1
                             select ea.IdEstadoAlquiler;
            
            int idestado = estadoalquiler.First();

            var alquileres = from a in db.Alquilers
                             where a.Producto.Usuario.User == User.Identity.Name && a.IdEstadoAlquiler == idestado
                             select a;

            if (productos.Count() > 0)
            {
                ViewBag.Productos = productos.ToList<Productos>();
                
            }
            if (alquileres.Count() > 0)
            {
                ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            }
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            return View();
        }


        public ActionResult ViewSolicitudesParaUsuarioRechazadas()
        {
            Busqueda();
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            ViewBag.IdUsuario = usuario.First();

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1)
                            select p;

            //ESTADO PENDIENTES
            //var estadoalquiler = from ea in db.EstadosAlquiler
            //                     where ea.IdEstadoAlquilerFijo == 1
            //                     select ea.IdEstadoAlquiler;

            //int idestado = estadoalquiler.First();

            //ALQUILERES RECHAZADOS
            var alquileres = from a in db.Alquilers
                             where a.Producto.Usuario.User == User.Identity.Name && a.EstadoAlquiler.IdEstadoAlquilerFijo == 2
                             select a;

            if (productos.Count() > 0)
            {
                ViewBag.Productos = productos.ToList<Productos>();

            }
            if (alquileres.Count() > 0)
            {
                ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            }
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            return View();
        }

        public ActionResult ViewSolicitudesDelUsuario()
        {
            Busqueda();

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            ViewBag.IdUsuario = usuario.First();

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1)
                            select p;

            var alquileres = from a in db.Alquilers
                             where a.UsuarioArrendatario.User == User.Identity.Name && a.EstadoAlquiler.IdEstadoAlquilerFijo == 1
                             select a;

            if (productos.Count() > 0)
            {
                ViewBag.Productos = productos.ToList<Productos>();
                
            }
            if (alquileres.Count() > 0)
            {
                ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            }
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            return View();
        }

        public ActionResult ViewSolicitudesDelUsuarioRechazadas()
        {
            Busqueda();

            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            ViewBag.IdUsuario = usuario.First();

            var productos = from p in db.Productos
                            where (p.Usuario.User == User.Identity.Name && p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1)
                            select p;

            var alquileres = from a in db.Alquilers
                             where a.UsuarioArrendatario.User == User.Identity.Name && a.EstadoAlquiler.IdEstadoAlquilerFijo == 2
                             select a;

            if (productos.Count() > 0)
            {
                ViewBag.Productos = productos.ToList<Productos>();

            }
            if (alquileres.Count() > 0)
            {
                ViewBag.Alquileres = alquileres.ToList<Alquileres>();
            }
            //ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            MarcarReservasRechazadasComoLeidas();

            return View();
        }

        #region Funciones en Comun

        //Fucnion Base que inicializa variables y mantiene datos en comun.
        public void Busqueda()
        {
            var productosMasVisitados = (from p in db.Productos.AsNoTracking()
                                         where p.EstadoProducto.IdEstadoProductoFijo == 1 && p.CategoriasProducto.Visible == true
                                         orderby p.CantVisitas descending
                                         select p).Take(10);

            var productosRandom = (from p in db.Productos.AsNoTracking()
                                   where p.EstadoProducto.IdEstadoProductoFijo == 1 && p.CategoriasProducto.Visible == true
                                        //orderby Guid.NewGuid() descending
                                         select p);//.Take(10);

            //var randomUsers = users.OrderBy(x => Guid.NewGuid()).Take(15);

            var productosRecientes = (from p in db.Productos.AsNoTracking()
                                      where p.EstadoProducto.IdEstadoProductoFijo == 1 && p.CategoriasProducto.Visible == true
                                         orderby p.IdProducto descending
                                         select p).Take(15);
 
            Random r = new Random();
            List<Productos> listaProductosRandom = new List<Productos>();
            List<Productos> listaProductosRandom2 = new List<Productos>();


            ProductComparer pc = new ProductComparer();

            int contRandom = 0;

            while(contRandom < 20)
            //for(int i = 0; i < 10; i++)
            {
                int index = r.Next(0, productosRandom.Count());
                Productos prodRandom = db.Productos.AsNoTracking().Where(p => p.EstadoProducto.IdEstadoProductoFijo == 1 && p.CategoriasProducto.Visible == true).ToList().ElementAt(index);

                if (contRandom < 10)
                {
                    if (!listaProductosRandom.Contains(prodRandom, pc))
                    {
                        listaProductosRandom.Add(prodRandom);
                        contRandom++;
                    }
                }
                else
                {
                    if (!listaProductosRandom.Contains(prodRandom, pc) && !listaProductosRandom2.Contains(prodRandom, pc))
                    {
                        listaProductosRandom2.Add(prodRandom);
                        contRandom++;
                    }
                }
            }


            //categorias visibles
            var categoriasProductos = from cp in db.CategoriaProductoes
                                      where cp.Visible == true
                                      orderby cp.DescripcionI1 ascending
                                      select cp;
            
            List<CategoriaProductos> listaCat = categoriasProductos.ToList();

            //categorias visibles - traigo las categorias que tienen productos ordenada por idproducto
            var categoriasConProductos = (from  p in db.Productos
                                          join cp in db.CategoriaProductoes on p.IdCategoriaProducto equals cp.IdCategoriaProducto
                                          where cp.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1
                                      orderby p.IdProducto descending
                                      select cp).Distinct();

            List<CategoriaProductos> listaCat2 = categoriasConProductos.ToList();

            //4 ultimos productos
            var productos = (from p in db.Productos.AsNoTracking()
                             where p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 1
                             orderby p.IdProducto descending
                             select p).Take(4);

            if (!String.IsNullOrEmpty(User.Identity.Name))
            {
                //EL AsNoTracking previene que el objeto quede como "TOCADO"
                var usuario = db.Usuarios.AsNoTracking().Where(u => u.User == User.Identity.Name);

                ViewBag.NombreUsuario = usuario.First().Nombre;

                if (usuario.First().FotosUsuarios.Count() > 0)
                {
                    ViewBag.FotoUser = usuario.First().FotosUsuarios.First().Imagen.RutaFotoThumb;
                }
                else
                {
                   ViewBag.FotoUser = "~/assets/images/default-avatar.jpg";
                }
            }
            
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Todas las Categorias", Value = "" });

            foreach (CategoriaProductos cat in listaCat)
            {
                items.Add(new SelectListItem { Text = cat.DescripcionI1, Value = HttpUtility.UrlEncode(Funciones.Encrypt(cat.IdCategoriaProducto.ToString())) });
            }

            List<Productos> prodsPorCatItems = new List<Productos>();
            List<Productos> prodsPorCatItemsCol1 = new List<Productos>();
            List<Productos> prodsPorCatItemsCol2 = new List<Productos>();
            List<Productos> prodsPorCatItemsCol3 = new List<Productos>();
            int columnas = 3;
            int cant = 1;
            int mod = 0;
            int ultimos = 3;

            foreach (CategoriaProductos cat in listaCat2)
            {
                //items.Add(new SelectListItem { Text = cat.DescripcionI1, Value = HttpUtility.UrlEncode(Funciones.Encrypt(cat.IdCategoriaProducto.ToString())) });

                //Traigo los productos para luego tomar 1 por categoria
                var prodsPorCat = (from p2 in db.Productos.AsNoTracking()
                                   where p2.IdCategoriaProducto == cat.IdCategoriaProducto && p2.EstadoProducto.IdEstadoProductoFijo == 1 && p2.CategoriasProducto.Visible == true
                                  orderby p2.IdProducto descending
                                  select p2).Take(ultimos); //los N ultimos de cada cat

                if (prodsPorCat.Count() > 0)
                {
                    foreach (Productos prod1 in prodsPorCat)
                    {
                        //Productos prod1 = prodsPorCat.FirstOrDefault(); //.Where(p3 => p3.IdCategoriaProducto == cat.IdCategoriaProducto).OrderByDescending(p3 => p3.IdProducto).FirstOrDefault();
                        prodsPorCatItems.Add(prod1);

                        mod = cant % columnas;
                        if (mod == 1)
                        {
                            prodsPorCatItemsCol1.Add(prod1);
                        }

                        if (mod == 2)
                        {
                            prodsPorCatItemsCol2.Add(prod1);
                        }

                        if (mod == 0)
                        {
                            prodsPorCatItemsCol3.Add(prod1);
                        }

                        cant++;
                    }
                }

                

            }

 
                
            ViewBag.Categorias = items;
            ViewBag.CategoriasProductos = categoriasProductos.ToList();
            ViewBag.ProductosDestacados = productos.ToList();
            ViewBag.ProductosUltimos = prodsPorCatItems.ToList();
            ViewBag.ProductosUltimosCol1 = prodsPorCatItemsCol1.ToList();
            ViewBag.ProductosUltimosCol2 = prodsPorCatItemsCol2.ToList();
            ViewBag.ProductosUltimosCol3 = prodsPorCatItemsCol3.ToList();
            ViewBag.ProductosMayorVisitas = productosMasVisitados.ToList();
            ViewBag.ProductosRecientes = productosRecientes.ToList();
            ViewBag.ProductosRandom = listaProductosRandom; // productosRecientes.ToList();
            ViewBag.ProductosRandom2 = listaProductosRandom2; // productosRecientes.ToList();

            ViewBag.CantidadConsultas = DevolverCantidadConsultasSinLeer() == 0 ? "" : " (" + DevolverCantidadConsultasSinLeer() + ")";
            ViewBag.CantidadConsultasSinLeer = DevolverCantidadConsultasSinLeer();

            ViewBag.CantidadRespuestasSinLeerString = DevolverCantidadRespuestasSinLeer() == 0 ? "" : " (" + DevolverCantidadRespuestasSinLeer() + ")";
            ViewBag.CantidadRespuestasSinLeerInt = DevolverCantidadRespuestasSinLeer();

            ViewBag.CantidadSolicitudesPendientesString = DevolverCantidadSolicitudesPendientes() == 0 ? "" : " (" + DevolverCantidadSolicitudesPendientes() + ")";
            ViewBag.CantidadSolicitudesPendientesInt = DevolverCantidadSolicitudesPendientes();

            ViewBag.CantidadReservasAprobadasString = DevolverCantidadReservasAprobadas() == 0 ? "" : " (" + DevolverCantidadReservasAprobadas() + ")";
            ViewBag.CantidadReservasAprobadasInt = DevolverCantidadReservasAprobadas();

            ViewBag.CantidaReservasRechazadasString = DevolverCantidadReservasRechazadas() == 0 ? "" : " (" + DevolverCantidadReservasRechazadas() + ")";
            ViewBag.CantidadReservasRechazadasInt = DevolverCantidadReservasRechazadas();

        }

        #endregion

        #region Funciones Producto

        //FUNCIONES PRODUCTO

        //
        // GET: /Productos/Create
        [Authorize]
        public ActionResult CreateProduct()
        {
            Busqueda();
            
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true).OrderBy(c => c.DescripcionI1), "IdCategoriaProducto", "DescripcionI1");
            ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo");
            ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1");
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            return View();
        }

        //
        // POST: /Productos/Create

        [HttpPost]
        public ActionResult CreateProduct(ICollection<HttpPostedFileBase> ImageUpload, Productos productos)
        {
            Busqueda();

            //Validaciones CUSTOM
            
            ModelState.Clear();

            //var existeEmail = from u1 in db.Usuarios
            //                  where u1.Email == usuarios.Email
            //                  select u1.IdUsuario;

            //var existeUser = from u1 in db.Usuarios
            //                 where u1.User == usuarios.User
            //                 select u1.IdUsuario;

            if (productos.PrecioHora == null && productos.PrecioFinDeSemana == null && productos.PrecioMensual == null && productos.PrecioSemanal == null && productos.PrecioDiario == null)
            {
                ModelState.AddModelError("IdMoneda", "Debes ingresar al menos UN precio!");
            }

            /*
            if (!productos.MostrarPrecioHora && !productos.MostrarPrecioDiario && !productos.MostrarPrecioFinDeSemana && !productos.MostrarPrecioMensual && !productos.MostrarPrecioSemanal)
            {
                ModelState.AddModelError("IdMoneda", "Debes seleccionar UN precio a mostrar!");
            }
            else
            {*/
                if ((productos.MostrarPrecioHora && !productos.MostrarPrecioDiario && !productos.MostrarPrecioFinDeSemana && !productos.MostrarPrecioMensual && !productos.MostrarPrecioSemanal)
                    || (!productos.MostrarPrecioHora && productos.MostrarPrecioDiario && !productos.MostrarPrecioFinDeSemana && !productos.MostrarPrecioMensual && !productos.MostrarPrecioSemanal)
                    || (!productos.MostrarPrecioHora && !productos.MostrarPrecioDiario && productos.MostrarPrecioFinDeSemana && !productos.MostrarPrecioMensual && !productos.MostrarPrecioSemanal)
                    || (!productos.MostrarPrecioHora && !productos.MostrarPrecioDiario && !productos.MostrarPrecioFinDeSemana && productos.MostrarPrecioMensual && !productos.MostrarPrecioSemanal)
                    || (!productos.MostrarPrecioHora && !productos.MostrarPrecioDiario && !productos.MostrarPrecioFinDeSemana && !productos.MostrarPrecioMensual && productos.MostrarPrecioSemanal))
                {
                    //ESTA TODO OK!!
                }
                else
                {
                    ModelState.AddModelError("IdMoneda", "Debes seleccionar sólo UN precio a mostrar!");
                }
            //}

            if (productos.MostrarPrecioHora)
            {
                if (productos.PrecioHora == null)
                {
                    ModelState.AddModelError("MostrarPrecioHora", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioFinDeSemana)
            {
                if (productos.PrecioFinDeSemana == null)
                {
                    ModelState.AddModelError("MostrarPrecioFinDeSemana", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioDiario)
            {
                if (productos.PrecioDiario == null)
                {
                    ModelState.AddModelError("MostrarPrecioDiario", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioMensual)
            {
                if (productos.PrecioMensual == null)
                {
                    ModelState.AddModelError("MostrarPrecioMensual", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioSemanal)
            {
                if (productos.PrecioSemanal == null)
                {
                    ModelState.AddModelError("MostrarPrecioSemanal", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            
            if (ImageUpload.First() == null)
            {
                ModelState.AddModelError("ImageUpload", "Debes subir al menos una foto.");
            }

            if (!ValidarDatosPermitidos(productos.Descripcion))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en la Descripción de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.GarantiaExtra))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en la Garantía de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.Nombre))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en el Nombre de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.CondicionesUso))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en las Condiciones de uso de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.Ubicacion))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en la UBICACION de la publicación.");
            }

            // END VALIDACIONES CUSTOM

            if (ModelState.IsValid)
            {
                var usuario = from u in db.Usuarios
                              where u.User == User.Identity.Name
                              select u.IdUsuario;

                var estado = from e in db.EstadosProducto
                             where e.IdEstadoProductoFijo == 3
                             select e.IdEstadoProducto;

                productos.IdTipoProducto = 1;
                productos.IdUsuario = usuario.First();
                productos.IdEstadoProducto = estado.First();
                productos.CantVisitas = 0;
                //productos.IdBarrio = 2;
                productos.NombreBusqueda = RemoveDiacritics(productos.Nombre);
                productos.DescripcionBusqueda = RemoveDiacritics(productos.Descripcion);

                db.Productos.Add(productos);
                db.SaveChanges();

                GrabarLog(1, productos.IdUsuario, "Productos", null, db.Entry(productos));

                foreach (HttpPostedFileBase file in ImageUpload)
                {
                    if (file != null)
                    {
                        int idprodgenerado = productos.IdProducto;
                        ImagenViewModel ivm = new ImagenViewModel();
                        ivm.ImageUpload = file;
                        ivm.Titulo = file.FileName;
                        ivm.Caption = file.FileName;
                        
                        Imagen nuevaimagen = CreateImagen(ivm);
                        
                        FotoProductos fp = new FotoProductos();
                        fp.IdProducto = idprodgenerado;
                        fp.IdImagen = nuevaimagen.IdImagen;
                        db.FotoProductos.Add(fp);
                        db.SaveChanges();

                        //PENDIENTE
                        //GrabarLog(1, respuestaComentarioproducto.IdUsuario, "ComentariosProductoes", null, db.Entry(respuestaComentarioproducto));

                    }
                }

                EnviarMailNuevoPublicacion();

                string message = "¡Tu publicación se ha creado correctamente y se encuentra en estado de revisión, te enviaremos un mail cuando esté disponible!";
                string messageDetalle = "";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
                //return RedirectToAction("Index");
            }

            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", productos.IdBarrio);
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true), "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", productos.IdUsuario);
            ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo", productos.IdMoneda);
            ViewBag.IdEstadoProducto = new SelectList(db.EstadosProducto, "IdEstadoProducto", "DescripcionI1", productos.IdEstadoProducto);
            ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1", productos.IdTipoProducto);
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", productos.IdDepartamento);
            ViewBag.ImageUpload = ImageUpload;

            return View(productos);
        }

        //
        // GET: /Productos/Edit/5
        [Authorize]
        public ActionResult EditProduct(string id)
        {
            Busqueda();

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Productos productos = db.Productos.Find(idProd);

            if (User.Identity.Name == productos.Usuario.User)
            {

                ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true), "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
                ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", productos.IdUsuario);
                ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo", productos.IdMoneda);
                ViewBag.IdEstadoProducto = new SelectList(db.EstadosProducto, "IdEstadoMoneda", "DescripcionI1", productos.IdEstadoProducto);
                ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", productos.IdBarrio);
                ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1", productos.IdTipoProducto);
                ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", productos.IdDepartamento);
                //ViewBag.FotosProductosBAK = productos.FotosProductos;


                return View(productos);
            }
            else
            {
                string message = "¡RUTA NO VALIDA!";
                string messageDetalle = "";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }
        }

        //
        // POST: /Productos/Edit/5

        [HttpPost]
        public ActionResult EditProduct(ICollection<HttpPostedFileBase> ImageUpload, string IdProductoEncrypt, Productos productos)
        {
            Busqueda();

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int id = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdProductoEncrypt)));
            productos.IdProducto = id;

            //SEGURIDAD!!!
            //SETEO LOS VALORES QUE NO PUEDEN SER CAMBIADOS POR PANTALLA

            
            //List<FotoProductos> listaFotos = ViewBag.FotosProductosBAK;

            var productoOriginal = db.Productos.AsNoTracking().Where(p => p.IdProducto == productos.IdProducto).FirstOrDefault();
            productos.IdEstadoProducto = productoOriginal.IdEstadoProducto;
            productos.IdUsuario = productoOriginal.IdUsuario;
            productos.IdTipoProducto = productoOriginal.IdTipoProducto;
            

            /*
            foreach(FotoProductos fp in productos.FotosProductos.ToList())
            {
                
            }*/
            //ImageUpload.Add();

            // BEGIN Validaciones CUSTOM

            ModelState.Clear();

            //var existeEmail = from u1 in db.Usuarios
            //                  where u1.Email == usuarios.Email
            //                  select u1.IdUsuario;

            //var existeUser = from u1 in db.Usuarios
            //                 where u1.User == usuarios.User
            //                 select u1.IdUsuario;

            if (productos.PrecioHora == null && productos.PrecioFinDeSemana == null && productos.PrecioMensual == null && productos.PrecioSemanal == null && productos.PrecioDiario == null)
            {
                ModelState.AddModelError("IdMoneda", "Debes ingresar al menos UN precio!");
            }

            if (!productos.MostrarPrecioHora && !productos.MostrarPrecioDiario && !productos.MostrarPrecioFinDeSemana && !productos.MostrarPrecioMensual && !productos.MostrarPrecioSemanal)
            {
                ModelState.AddModelError("IdMoneda", "Debes seleccionar al menos UN precio a mostrar!");
            }

            if (productos.MostrarPrecioHora)
            {
                if (productos.PrecioHora == null)
                {
                    ModelState.AddModelError("MostrarPrecioHora", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioFinDeSemana)
            {
                if (productos.PrecioFinDeSemana == null)
                {
                    ModelState.AddModelError("MostrarPrecioFinDeSemana", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioDiario)
            {
                if (productos.PrecioDiario == null)
                {
                    ModelState.AddModelError("MostrarPrecioDiario", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioMensual)
            {
                if (productos.PrecioMensual == null)
                {
                    ModelState.AddModelError("MostrarPrecioMensual", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            if (productos.MostrarPrecioSemanal)
            {
                if (productos.PrecioSemanal == null)
                {
                    ModelState.AddModelError("MostrarPrecioSemanal", "No puedes seleccionar mostrar este precio sin un valor asignado.");
                }
            }

            //if (productos.FotosProductos.Count() == 0 && ImageUpload.First() == null)
            if (productoOriginal.FotosProductos.Count() == 0 && ImageUpload.First() == null)
            {
                ModelState.AddModelError("ImageUpload", "Debes subir al menos una foto.");
            }

            if (!ValidarDatosPermitidos(productos.Descripcion))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en la Descripción de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.GarantiaExtra))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en la Garantía de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.Nombre))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en el Nombre de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.CondicionesUso))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en las Condiciones de uso de la publicación.");
            }

            if (!ValidarDatosPermitidos(productos.Ubicacion))
            {
                ModelState.AddModelError("Descripcion", "No se permiten datos personales en la UBICACION de la publicación.");
            }

            // END VALIDACIONES CUSTOM

            if (ModelState.IsValid)
            {
                //productos.IdBarrio = 2;
                
                productos.NombreBusqueda = RemoveDiacritics(productos.Nombre);
                productos.DescripcionBusqueda = RemoveDiacritics(productos.Descripcion);

                //Productos prod = db.Productos.Find(productos.IdProducto);
                db.Entry(productos).State = EntityState.Modified;


                GrabarLog(2, productos.IdUsuario, "Productos", db.Entry(productoOriginal), db.Entry(productos));

                db.SaveChanges();

                foreach (HttpPostedFileBase file in ImageUpload)
                {
                    if (file != null)
                    {
                        int idprodgenerado = productos.IdProducto;
                        ImagenViewModel ivm = new ImagenViewModel();
                        ivm.ImageUpload = file;
                        ivm.Titulo = file.FileName;
                        ivm.Caption = file.FileName;

                        Imagen nuevaimagen = CreateImagen(ivm);

                        FotoProductos fp = new FotoProductos();
                        fp.IdProducto = idprodgenerado;
                        fp.IdImagen = nuevaimagen.IdImagen;
                        db.FotoProductos.Add(fp);
                        db.SaveChanges();

                        //PENDIENTE
                        //GrabarLog(1, respuestaComentarioproducto.IdUsuario, "ComentariosProductoes", null, db.Entry(respuestaComentarioproducto));

                    }
                }

                return RedirectToAction("ViewPublicaciones");
            }

            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true), "IdCategoriaProducto", "DescripcionI1", productos.IdCategoriaProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", productos.IdUsuario);
            ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo", productos.IdMoneda);
            ViewBag.IdEstadoProducto = new SelectList(db.EstadosProducto, "IdEstadoMoneda", "DescripcionI1", productos.IdEstadoProducto);
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", productos.IdBarrio);
            ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1", productos.IdTipoProducto);
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", productos.IdDepartamento);
            
           
            //var productoYaModificado = db.Productos.AsNoTracking().Where(p => p.IdProducto == productos.IdProducto).FirstOrDefault();

            //chanchada para que vuelvan a aparecer las fotos
            productos.FotosProductos = productoOriginal.FotosProductos.ToList();

            return View(productos);
        }

        //
        // GET: /Productos/Delete/5
        [Authorize]
        public ActionResult DeleteProduct(string id)
        {
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            Productos productos = db.Productos.Find(idProd);
            return View(productos);
        }

        //
        // POST: /Productos/Delete/5

        [HttpPost]
        public ActionResult DeleteProductConfirmed(string id)
        {
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            Productos productos = db.Productos.Find(idProd);
            db.Productos.Remove(productos);

            GrabarLog(3, productos.IdUsuario, "Productos", db.Entry(productos), null);

            db.SaveChanges();
            
            return RedirectToAction("ViewUser");
        }



        #endregion

        
        public ActionResult Contact()
        {
            Busqueda();

            //if (Request.IsAjaxRequest())
            //{
            //    return Json(new { p1 = "bar", p2 = "Blech" }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
                return View();
            //}
        }

        public ActionResult ComoFunciona()
        {
            Busqueda();

            return View();
        
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public Imagen CreateImagen(ImagenViewModel model)
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
                ModelState.AddModelError("ImageUpload", "Este campo es requerido!");
            }
            else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Por favor selecciona una imagen GIF, JPG o PNG");
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
                    var uploadDirThumb = "~/uploads/thumb";
                    
                    //SACAR ESPACIOS
                    string nombreArchivo = Guid.NewGuid() + "_" + model.ImageUpload.FileName.Replace(" ","");

                    var imagePath = Path.Combine(Server.MapPath(uploadDir), nombreArchivo);
                    var imageUrl = Path.Combine(uploadDir, nombreArchivo);
                    var imagePathThumb = Path.Combine(Server.MapPath(uploadDirThumb), nombreArchivo);
                    var imageUrlThumb = Path.Combine(uploadDirThumb, nombreArchivo);

                    
                    //OLD
                    //model.ImageUpload.SaveAs(imagePath);
                    
                    image.RutaFoto = imageUrl;
                    image.RutaFotoThumb = imageUrlThumb;

                    //NEW
                    // si no supera los 700 no deberia hacer nada.
                    int altoMaxImagen = Convert.ToInt32(ObtenerConfiguracion("ALTO_MAX_IMAGEN"));
                    int anchoMaxImagen = Convert.ToInt32(ObtenerConfiguracion("ANCHO_MAX_IMAGEN"));

                    int altoMaxImagenThumb = Convert.ToInt32(ObtenerConfiguracion("ALTO_MAX_IMAGEN_THUMB"));
                    int anchoMaxImagenThumb = Convert.ToInt32(ObtenerConfiguracion("ANCHO_MAX_IMAGEN_THUMB"));

                    //140 de altura tiene la foto de perfil en Details

                    WebImage imgOriginal2 = new WebImage(model.ImageUpload.InputStream);
                    WebImage imgOriginalResize = imgOriginal2.Clone();// new WebImage(model.ImageUpload.InputStream);
                    WebImage imgOriginalResizeThumb = imgOriginal2.Clone();//new WebImage(model.ImageUpload.InputStream);
                    if ((Convert.ToDecimal(imgOriginal2.Width) / Convert.ToDecimal(imgOriginal2.Height)) > 1)
                    {
                        if (imgOriginal2.Height > altoMaxImagen)
                        {
                            imgOriginalResize.Resize((imgOriginal2.Width * altoMaxImagen) / imgOriginal2.Height, altoMaxImagen);
                        }

                        if (imgOriginal2.Height > altoMaxImagenThumb)
                        {
                            imgOriginalResizeThumb.Resize((imgOriginal2.Width * altoMaxImagenThumb) / imgOriginal2.Height, altoMaxImagenThumb);
                        }
                    }
                    else
                    {
                        if (imgOriginal2.Width > anchoMaxImagen)
                        {
                            imgOriginalResize.Resize(anchoMaxImagen, (imgOriginal2.Height * anchoMaxImagen) / imgOriginal2.Width);
                        }

                        if (imgOriginal2.Width > anchoMaxImagenThumb)
                        {
                            imgOriginalResizeThumb.Resize(anchoMaxImagenThumb, (imgOriginal2.Height * anchoMaxImagenThumb) / imgOriginal2.Width);
                        }
                    }
                    imgOriginalResize.Save(imagePath);
                    imgOriginalResizeThumb.Save(imagePathThumb);

                    /*
                    Image imgOriginal = Image.FromFile(imagePath);
                    
                    Image thumb = DevolverThumb(imgOriginal);
                    
                    thumb.Save(imagePathThumb);
                    */

                }

                db.Imagens.Add(image);
                //db.Create(image);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return image;
            }

            return null;
            //return View(model);
        }

        [Authorize(Users = "dimapego,pepe")]
        public ActionResult ListProducts()
        {
            var productos = from p in db.Productos
                            where p.CategoriasProducto.Visible == true
                            orderby p.IdProducto descending
                            select p;

            Busqueda();

            ViewBag.Productos = productos.ToList<Productos>();

            return View();
        }

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ListarProductosPendientesAprobacion()
        {
            var productos = from p in db.Productos
                            where p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 3
                            orderby p.IdProducto descending
                            select p;

            Busqueda();

            ViewBag.Productos = productos.ToList<Productos>();

            return View();
        }

        //[Authorize(Users = "dimapego,pepe")]
        //public ActionResult ListarProductosSinAprobar()
        //{
        //    var productos = from p in db.Productos
        //                    where p.CategoriasProducto.Visible == true && p.EstadoProducto.IdEstadoProductoFijo == 
        //                    select p;

        //    Busqueda();

        //    ViewBag.Productos = productos.ToList<Productos>();

        //    return View();
        //}

        [Authorize(Users = "dimapego,pepe")]
        public ActionResult ListarUsuarios()
        {
            var usuarios = from u in db.Usuarios
                            orderby u.IdUsuario descending
                            select u;

            Busqueda();

            ViewBag.ListaUsuarios = usuarios.ToList<Usuarios>();

            return View();
        }

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ListarSuscriptores()
        {
            /*
            var sus = from s in db.Suscriptores
                           orderby s.IdSuscriptor descending
                           select s;

            Busqueda();

            ViewBag.ListaSuscriptores = sus.ToList<Suscriptor>();
            */
            Busqueda();
            return View(db.Suscriptores.OrderByDescending(s => s.IdSuscriptor).ToList());
        }

        [HttpPost]
        public void DeleteFotoConfirmed(string id)
        {
            int idFoto = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            
            FotoProductos fotoproductos = db.FotoProductos.Find(idFoto);
            var fullPath = Server.MapPath(fotoproductos.Imagen.RutaFoto); // Path.Combine(Server.MapPath("~/uploads"), fotoproductos.Imagen.RutaFoto);
            //var fullPath = Server.MapPath("~/Images/Cakes/" + fotoproductos.Imagen.RutaFoto);
            //var path2 = Url.Content("~/uploads/" + fotoproductos.Imagen.RutaFoto);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                ViewBag.deleteSuccess = "true";
            }

            int idUsuario = fotoproductos.Producto.IdUsuario;

            db.FotoProductos.Remove(fotoproductos);


            GrabarLog(3, idUsuario, "FotoProductos", db.Entry(fotoproductos), null);

            db.SaveChanges();
            //return RedirectToAction("Index");
            //return new EmptyResult();
        }

        [HttpPost]
        public void DeleteFotoUserConfirmed(string id)
        {
            int idFoto = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));


            FotoUsuarios fotousuarios = db.FotosUsuarios.Find(idFoto);
            var fullPath = Server.MapPath(fotousuarios.Imagen.RutaFoto); // Path.Combine(Server.MapPath("~/uploads"), fotoproductos.Imagen.RutaFoto);
            //var fullPath = Server.MapPath("~/Images/Cakes/" + fotoproductos.Imagen.RutaFoto);
            //var path2 = Url.Content("~/uploads/" + fotoproductos.Imagen.RutaFoto);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                ViewBag.deleteSuccess = "true";
            }

            int idUsuario = fotousuarios.IdUsuario;
            db.FotosUsuarios.Remove(fotousuarios);

            GrabarLog(3, idUsuario, "FotoUsuarios", db.Entry(fotousuarios), null);

            db.SaveChanges();
            //return RedirectToAction("Index");
            //return new EmptyResult();
        }

        [HttpPost]
        public void DeleteProductConfirmedAjax(string id)
        {
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            Productos productos = db.Productos.Find(idProd);
            db.Productos.Remove(productos);

            GrabarLog(3, productos.IdUsuario, "Productos", db.Entry(productos), null);

            db.SaveChanges();

            //return RedirectToAction("ViewUser");
        }

        [HttpPost]
        public void AprobarProducto(string id)
        {
            int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            Productos productos = db.Productos.Find(idProd);
            //db.Productos.Remove(productos);

            productos.IdEstadoProducto = 1; //ACTIVO - usar el idestadofijo luego

            db.Entry(productos).State = EntityState.Modified;

            EnviarMailPublicacionAprobada(productos, id);
            //GrabarLog(3, productos.IdUsuario, "Productos", db.Entry(productos), null);

            db.SaveChanges();

            //return RedirectToAction("ViewUser");
        }


        [HttpPost]
        public ActionResult RotarFoto(string id)
        {
            Busqueda();

            //if (!String.IsNullOrEmpty(msjRechazo))
            {
                int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

                //var productoOriginal = db.Productos.AsNoTracking().Where(p => p.IdProducto == productos.IdProducto).FirstOrDefault();

                Productos productos = db.Productos.Find(idProd);
                //db.Productos.Remove(productos);
                
                foreach(FotoProductos fp in productos.FotosProductos)
                {
                    var uploadDir = "~/uploads";
                    var uploadDirThumb = "~/uploads/thumb";

                    var imagePath = Path.Combine(Server.MapPath(uploadDir), fp.Imagen.RutaFoto.Replace(uploadDir+"\\",""));
                    var imagePathThumb = Path.Combine(Server.MapPath(uploadDirThumb), fp.Imagen.RutaFotoThumb.Replace(uploadDirThumb + "\\", ""));

                    var path = Server.MapPath(fp.Imagen.RutaFoto);
                    var pathThumb = Server.MapPath(fp.Imagen.RutaFotoThumb);

                    Image imgOriginal = Image.FromFile(imagePath);
                    Image thumbOriginal = Image.FromFile(imagePathThumb);

                    imgOriginal.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    thumbOriginal.RotateFlip(RotateFlipType.Rotate90FlipNone);

                    imgOriginal.Save(imagePath);
                    thumbOriginal.Save(imagePathThumb);

                    imgOriginal.Dispose();
                    thumbOriginal.Dispose();
                    //Image thumb = DevolverThumb(imgOriginal);
                    
                    //thumb.Save(imagePathThumb);
                    
                    
                }
                /*
                productos.IdEstadoProducto = 2; //FINALIZADO - usar el idestadofijo luego

                db.Entry(productos).State = EntityState.Modified;
                */
                //EnviarMailPublicacionRechazada(productos, id, msjRechazo);
                //GrabarLog(2, productos.IdUsuario, "Productos", db.Entry(productoOriginal), db.Entry(productos));
                //GrabarLog(2, productos.IdUsuario, "Productos", db.Entry(productos), null);
                //db.SaveChanges();
            }


            return RedirectToAction("ListarProductosPendientesAprobacion");
        }

        //public void RechazarProducto(string id, string msjRechazo)
        [HttpPost]
        public ActionResult RechazarProducto(string id, string msjRechazo)
        {
            Busqueda();

            //if (!String.IsNullOrEmpty(msjRechazo))
            {
                int idProd = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

                //var productoOriginal = db.Productos.AsNoTracking().Where(p => p.IdProducto == productos.IdProducto).FirstOrDefault();

                Productos productos = db.Productos.Find(idProd);
                //db.Productos.Remove(productos);
                productos.IdEstadoProducto = 2; //FINALIZADO - usar el idestadofijo luego

                db.Entry(productos).State = EntityState.Modified;

                EnviarMailPublicacionRechazada(productos, id, msjRechazo);
                //GrabarLog(2, productos.IdUsuario, "Productos", db.Entry(productoOriginal), db.Entry(productos));
                //GrabarLog(2, productos.IdUsuario, "Productos", db.Entry(productos), null);
                db.SaveChanges();
            }
            

            return RedirectToAction("ListarProductosPendientesAprobacion");
        }

        [HttpPost]
        public ActionResult ConfirmarAlquiler(string id)
        {

            if (ModelState.IsValid)
            {
                int idAlq = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

                var estadoConfirmado = from ea in db.EstadosAlquiler
                                       where ea.IdEstadoAlquilerFijo == 3
                                       select ea.IdEstadoAlquiler;

                Alquileres alquiler = db.Alquilers.Find(idAlq);
                alquiler.IdEstadoAlquiler = estadoConfirmado.First();
                db.Entry(alquiler).State = EntityState.Modified;

                var alquilerOriginal = db.Alquilers.AsNoTracking().Where(a => a.IdAlquiler == idAlq).FirstOrDefault();

                GrabarLog(2, alquiler.Producto.IdUsuario, "Alquileres", db.Entry(alquilerOriginal), db.Entry(alquiler));

                db.SaveChanges();
                //return RedirectToAction("Index");
                EnviarMailConfirmarSolicitud(alquiler.Producto.Usuario.Email, alquiler.UsuarioArrendatario.Email, alquiler); 
            }
            return RedirectToAction("ViewSolicitudesParaUsuario");
        }

        [HttpPost]
        public ActionResult RechazarAlquiler(string id, string msjRechazo)
        {

            if (ModelState.IsValid)
            {
                int idAlq = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

                var estadoRechazado = from ea in db.EstadosAlquiler
                                       where ea.IdEstadoAlquilerFijo == 2
                                       select ea.IdEstadoAlquiler;

                Alquileres alquiler = db.Alquilers.Find(idAlq);
                alquiler.IdEstadoAlquiler = estadoRechazado.First();
                db.Entry(alquiler).State = EntityState.Modified;

                var alquilerOriginal = db.Alquilers.AsNoTracking().Where(a => a.IdAlquiler == idAlq).FirstOrDefault();

                GrabarLog(2, alquiler.Producto.IdUsuario, "Alquileres", db.Entry(alquilerOriginal), db.Entry(alquiler));

                db.SaveChanges();

                //Ingreso la razon de rechazo como un comentario de alquiler
                ComentarioAlquileres comentarioAlquiler = new ComentarioAlquileres();
                comentarioAlquiler.IdAlquiler = idAlq;
                comentarioAlquiler.IdUsuario = alquiler.Producto.IdUsuario;
                comentarioAlquiler.FechaComentario = DateTime.Now;
                comentarioAlquiler.Comentario = msjRechazo;
                
                db.ComentarioAlquilers.Add(comentarioAlquiler);
                db.SaveChanges();

                GrabarLog(1, comentarioAlquiler.IdUsuario, "ComentarioAlquileres", null, db.Entry(comentarioAlquiler));

                EnviarMailRechazarSolicitud(alquiler.Producto.Usuario.Email, alquiler.UsuarioArrendatario.Email, alquiler, msjRechazo); 
                //return RedirectToAction("Index");
            }
            return RedirectToAction("ViewSolicitudesParaUsuario");
            
        }

        public string EnviarMailConfirmarSolicitud(string usuarioFrom, string usuarioTo, Alquileres alq)
        {
            //ENVIO DE MAIL DE CONFIRMACION AL ARRENDATARIO
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(usuarioTo);
                message.Subject = "MLA - Confirmación de Solicitud de Reserva";
                message.IsBodyHtml = true;

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>El usuario <strong>" + User.Identity.Name + "</strong> ha CONFIRMADO la solicitud de reserva para el art&iacute;culo '" + alq.Producto.Nombre + "' desde el " + alq.FechaDesde.ToShortDateString() + " hasta el " + alq.FechaHasta.ToShortDateString() + ".<br><br><strong>DATOS DE CONTACTO</strong><br><br>Nombre: " + alq.Producto.Usuario.Nombre + " " + alq.Producto.Usuario.Apellido + "<br>Tel&eacute;fono: " + alq.Producto.Usuario.Telefono + "<br>Email: " + alq.Producto.Usuario.Email;
                message.Body = message.Body + "<br><br>Aqu&iacute; puedes ver nuestro: <a href='http://www.mejorloalquilo.com/assets/docs/ContratoArrendamiento.doc'>Contrato modelo</a>";
                
                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Successful 1<BR>";
              
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }


            //ENVIO DE MAIL DE CONFIRMACION AL ARRENDADOR
            message = new MailMessage();
            smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(alq.Producto.Usuario.Email);
                message.Subject = "MLA - Confirmación de Solicitud de Reserva";
                message.IsBodyHtml = true;

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + User.Identity.Name + ", has CONFIRMADO la solicitud de reserva para el art&iacute;culo '" + alq.Producto.Nombre + "' desde el " + alq.FechaDesde.ToShortDateString() + " hasta el " + alq.FechaHasta.ToShortDateString() + ".<br><br><strong>DATOS DE CONTACTO</strong><br><br>Nombre: " + alq.UsuarioArrendatario.Nombre + " " + alq.UsuarioArrendatario.Apellido + "<br>Tel&eacute;fono: " + alq.UsuarioArrendatario.Telefono + "<br>Email: " + alq.UsuarioArrendatario.Email;
                message.Body = message.Body + "<br><br>Aqu&iacute; puedes ver nuestro: <a href='http://www.mejorloalquilo.com/assets/docs/ContratoArrendamiento.doc'>Contrato modelo</a>";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = msg + "Successful 2<BR>";

            }
            catch (Exception ex)
            {
                msg = msg + " - " + ex.Message;
            }


            return msg;
            
        }

        public string EnviarMailRechazarSolicitud(string usuarioFrom, string usuarioTo, Alquileres alq, string mensajeRechazo)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(usuarioTo);
                message.Subject = "MLA - Rechazo de Solicitud de Reserva";
                message.IsBodyHtml = true;
                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png'  style='width: 200px;' /></a><br><br>El usuario <strong>" + User.Identity.Name + "</strong> ha RECHAZADO la solicitud de reserva para el articulo '" + alq.Producto.Nombre + "' desde el " + alq.FechaDesde.ToShortDateString() + " hasta el " + alq.FechaHasta.ToShortDateString() + ".<br><br><strong>Motivo:</strong><br><br>" + mensajeRechazo;

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Successful<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string GetFraction(decimal num)
        {
            
            //ver que separador de comas usar
            string sep = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            return num.ToString().Substring(num.ToString().IndexOf(sep) + 1, 2);
        }

        public string EnviarMailConfirmarUsuarioNuevo(Usuarios usuario)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(usuario.Email);
                message.Subject = "MLA - Confirmacion de Nuevo Usuario";
                message.IsBodyHtml = true;

                string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%","%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + usuario.Nombre + "!!<br><br>Te damos la bienvenida a MejorLoAlquilo, confirma tu direccion de correo electronico y empieza a disfrutar!!";
                message.Body = message.Body + "<p style='text-align:left'>- Ud estara siendo miembro de un grupo cerrado de usuarios (que publican) previamente seleccionados.<br>";
                message.Body = message.Body + "- El n&uacute;mero de participantes es limitado.<br>";
                message.Body = message.Body + "- La prueba de testeo se extendera hasta Mayo del 2016.<br>";
                message.Body = message.Body + "- MLA no cobrara ningun tipo de arancel durante el tiempo que dure el testeo.<br>";
                message.Body = message.Body + "- Estan prohibidos los datos personales dentro de los anuncios tanto as&iacute; como dentro de la comunicaci&oacute;n con los usuarios interesados.<br>";
                message.Body = message.Body + "- Toda transacci&oacute;n debe ser ejecutada dentro del sitio.<br>";
                message.Body = message.Body + "</p>";
                message.Body = message.Body + "<br><br><a style='background-color: #ff7e82; text-decoration:none; border-radius: 4px;font-weight: bold;padding: 10px 20px;display: inline-block;line-height: 20px;border: none;font-size: 15px;color: #fff;text-transform: capitalize;' href='http://www.mejorloalquilo.com/Home/ConfirmarNuevoUsuario/" + IdUsuarioEncrypt + "'>Confirmar Correo</a>";
                message.Body = message.Body + "<br><br>Gracias,<br>equipo de MejorLoAlquilo";
                 
                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Nuevo usuario creado<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string EnviarMailSuscriptor(Suscriptor suscriptor)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress("postmaster@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(suscriptor.Email);
                message.Subject = "¿Ya pensaste qué artículos publicar? Mira los consejos que tenemos para vos.";
                message.IsBodyHtml = true;

                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");
                message.Body = ObtenerConfiguracion("NEWSLETTER");

                //message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + usuario.Nombre + "!!<br><br>Te damos la bienvenida a MejorLoAlquilo, confirma tu direccion de correo electronico y empieza a disfrutar!!";
                //message.Body = message.Body + "<p style='text-align:left'>- Ud estara siendo miembro de un grupo cerrado de usuarios (que publican) previamente seleccionados.<br>";
                //message.Body = message.Body + "- El n&uacute;mero de participantes es limitado.<br>";
                //message.Body = message.Body + "- La prueba de testeo se extendera durante el resto del a&ntilde;o 2015.<br>";
                //message.Body = message.Body + "- MLA no cobrara ningun tipo de arancel durante el tiempo que dure el testeo.<br>";
                //message.Body = message.Body + "- Estan prohibidos los datos personales dentro de los anuncios tanto as&iacute; como dentro de la comunicaci&oacute;n con los usuarios interesados.<br>";
                //message.Body = message.Body + "- Toda transacci&oacute;n debe ser ejecutada dentro del sitio.<br>";
                //message.Body = message.Body + "</p>";
                //message.Body = message.Body + "<br><br><a style='background-color: #ff7e82; text-decoration:none; border-radius: 4px;font-weight: bold;padding: 10px 20px;display: inline-block;line-height: 20px;border: none;font-size: 15px;color: #fff;text-transform: capitalize;' href='http://www.mejorloalquilo.com/Home/ConfirmarNuevoUsuario/" + IdUsuarioEncrypt + "'>Confirmar Correo</a>";
                //message.Body = message.Body + "<br><br>Gracias,<br>equipo de MejorLoAlquilo";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@mejorloalquilo.com", "Dimapego84*");

                smtpClient.Send(message);
                msg = "Nuevo suscriptor creado<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string EnviarMailCambioPassword(Usuarios usuario)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(usuario.Email);
                message.Subject = "MLA - Cambio de contraseña";
                message.IsBodyHtml = true;

                string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>No te preocupes, a todos nos paso alguna vez :). Dirígete a este link para realizar el cambio de contraseña:";
                message.Body = message.Body + "<br><br><a style='background-color: #ff7e82; text-decoration:none; border-radius: 4px;font-weight: bold;padding: 10px 20px;display: inline-block;line-height: 20px;border: none;font-size: 15px;color: #fff;text-transform: capitalize;' href='http://www.mejorloalquilo.com/Home/RecuperarPassword/" + IdUsuarioEncrypt + "'>Cambio de contraseña</a>";
                message.Body = message.Body + "<br><br>Gracias,<br>equipo de MejorLoAlquilo";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Cambio password<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public static void EnviarMailRecordatorioConsultasSinResponder(string emailTo, string nombreTo)
        {
            
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(emailTo);
                message.Subject = "MLA - RECORDATORIO: Tenés consultas sin leer!!";
                message.IsBodyHtml = true;


                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");


                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + nombreTo + ", tenés consultas sin responder, ingresá <a href='http://www.mejorloalquilo.com/Home/ViewConsultasParaUsuario'>aquí</a> para verlas.";
                message.Body = message.Body + "<br><br>Gracias,<br>equipo de MejorLoAlquilo";
                message.Body = "HORA DE ENVIO: " + DateTime.Now.ToString();

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Cambio password<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            //return msg;

        }


        public static void EnviarMailPRUEBA()
        { 
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add("dimapego@gmail.com");
                message.Subject = "MLA - PRUEBA MAIL automatico";
                message.IsBodyHtml = true;

                
                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "HORA DE ENVIO: " + DateTime.Now.ToString();

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Cambio password<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            //return msg;

        }

        public string EnviarMailOlvidoUser(Usuarios usuario)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                message.To.Add(usuario.Email);
                message.Subject = "MLA - Olvido de usuario";
                message.IsBodyHtml = true;

                string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>No te preocupes, a todos nos paso alguna vez :). Este es el usuario asociado a tu cuenta de correo: <strong>" + usuario.User + "</strong>";
                message.Body = message.Body + "<br><br>Gracias,<br>equipo de MejorLoAlquilo";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Olvido User<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string EnviarMailNuevoFormulario(List<Formulario> listaFormulario)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                //message.To.Add("andresvinik@gmail.com");
                string mailsTo = ObtenerConfiguracion("MAILEnvioFormulario");
                message.To.Add(mailsTo);
                message.Subject = "MLA - Nuevo formulario ingresado";
                message.IsBodyHtml = true;

                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Formulario:";

                message.Body = message.Body + "<table><tr><th>Categoria</th><th>Marca/Modelo</th><th>Descripcion</th><th>Cantidad</th><th>Estado</th><th>Precio Estimado</th><th>Precio Compra</th><th>Vida util</th></tr>";

                foreach (Formulario f in listaFormulario)
                {
                    message.Body = message.Body + "<tr>";
                    message.Body = message.Body + "<td>" + f.CategoriasProducto.DescripcionI1 + "</td>";
                    message.Body = message.Body + "<td>" + f.Marca + "</td>";
                    message.Body = message.Body + "<td>" + f.Descripcion + "</td>";
                    message.Body = message.Body + "<td>" + f.Cantidad.ToString() + "</td>";
                    message.Body = message.Body + "<td>" + (f.Estado == 1 ? "Nuevo" : "Usado") + "</td>";
                    message.Body = message.Body + "<td>" + f.PrecioEstimadoActual.ToString() + "</td>";
                    message.Body = message.Body + "<td>" + f.PrecioCompra.ToString() + "</td>";
                    message.Body = message.Body + "<td>" + f.TiempoVidaUtil.ToString() + "</td>";
                    message.Body = message.Body + "</tr>";
                }

                message.Body = message.Body + "</table><br><br>";
                message.Body = message.Body + "Opcion de envio seleccionada: " + (listaFormulario.First().OpcionEnvio == 1 ? "Lo pasamos a buscar" : "Coordina con nosotros") + ".";
                

                
                message.Body = message.Body + "<br><br>Mensaje INTERNO.";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Formulario enviado<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string EnviarMailNuevoPublicacion()
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                //message.To.Add("andresvinik@gmail.com");
                string mailsTo = ObtenerConfiguracion("MAILEnvioNuevaPublicacion");
                message.To.Add(mailsTo);
                message.Subject = "MLA - AVISO!! Nueva Publicacion ingresada";
                message.IsBodyHtml = true;

                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'><img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Se ha creado una nueva publicacion: <a href='http://www.mejorloalquilo.com/Home/ListarProductosPendientesAprobacion'>Ver lista de publicaciones pendientes</a>";

                message.Body = message.Body + "<br><br>Mensaje INTERNO.";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Aviso de nueva publicacion enviado<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string EnviarMailPublicacionAprobada(Productos prod, string idProd)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                //message.To.Add("andresvinik@gmail.com");
                //string mailsTo = ObtenerConfiguracion("MAILEnvioNuevaPublicacion");
                message.To.Add(prod.Usuario.Email);
                message.Subject = "MLA - ¡Felicitaciones, tu publicación ha sido APROBADA!";
                message.IsBodyHtml = true;

                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'>";
                message.Body = message.Body + "<img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + prod.Usuario.Nombre + ", tu publicaci&oacute;n ha sido aprobada, puedes verla aqu&iacute;: <a href='http://www.mejorloalquilo.com/Home/Details/" + idProd + "'>" + prod.Nombre + "</a>";

                message.Body = message.Body + "<br><br>Equipo de MejorLoAlquilo.";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Aviso de publicacion aprobada<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public string EnviarMailPublicacionRechazada(Productos prod, string idProd, string msjRechazo)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mejorloalquilo.com");
            string msg = string.Empty;
            try
            {
                // "postmaster@mejorloalquilo.com"
                //MailAddress fromAddress = new MailAddress(ObtenerConfiguracion("MAILDESDE"), "MejorLoAlquilo");
                MailAddress fromAddress = new MailAddress("info@mejorloalquilo.com", "MejorLoAlquilo");
                message.From = fromAddress;
                //message.To.Add("andresvinik@gmail.com");
                //string mailsTo = ObtenerConfiguracion("MAILEnvioNuevaPublicacion");
                message.To.Add(prod.Usuario.Email);
                message.Subject = "MLA - Tu publicación ha sido RECHAZADA - Verifica los datos ingresados";
                message.IsBodyHtml = true;

                //string IdUsuarioEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(usuario.IdUsuario.ToString())).Replace("%", "%25");

                message.Body = "<a href='http://www.mejorloalquilo.com'>";
                message.Body = message.Body + "<img src='http://www.mejorloalquilo.com/assets/images/logo.png' style='width: 200px;' /></a><br><br>Hola " + prod.Usuario.Nombre + ", tu publicaci&oacute;n ha sido RECHAZADA por el siguiente motivo: '" + msjRechazo + "'.<br><br>Puedes verla aqu&iacute;: <a href='http://www.mejorloalquilo.com/Home/ViewUser'>Mis publicaciones</a>";

                message.Body = message.Body + "<br><br>Equipo de MejorLoAlquilo.";

                smtpClient.Timeout = 1000000;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@mejorloalquilo.com", "info1234*");

                smtpClient.Send(message);
                msg = "Aviso de publicacion rechazada<BR>";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;

        }

        public ActionResult ConfirmarNuevoUsuario(string id)
        {
            Busqueda();

            int idUsu = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            
            var estadoactivo = from eu in db.EstadosUsuario
                               where eu.IdEstadoUsuarioFijo == 1
                               select eu.IdEstadoUsuario;

            Usuarios usuario = db.Usuarios.Find(idUsu);
            usuario.IdEstadoUsuario = estadoactivo.First(); //LO PASO A ACTIVO

            db.Entry(usuario).State = EntityState.Modified;

            var usuarioOriginal = db.Usuarios.AsNoTracking().Where(u => u.IdUsuario == idUsu).FirstOrDefault();

            GrabarLog(2, usuario.IdUsuario, "Usuarios", db.Entry(usuarioOriginal), db.Entry(usuario));

            db.SaveChanges();

            //FALTA LA CONFIRMACION, cambiar el usuario a estado Activo!!


            return View(usuario); // RedirectToAction("ViewUser");
        }

        public ActionResult CambiarPassword(string id)
        {
            Busqueda();

            /*
            int idUsu = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));

            var estadoactivo = from eu in db.EstadosUsuario
                               where eu.IdEstadoUsuarioFijo == 1
                               select eu.IdEstadoUsuario;

            Usuarios usuario = db.Usuarios.Find(idUsu);
            usuario.IdEstadoUsuario = estadoactivo.First(); //LO PASO A ACTIVO

            db.Entry(usuario).State = EntityState.Modified;

            var usuarioOriginal = db.Usuarios.AsNoTracking().Where(u => u.IdUsuario == idUsu).FirstOrDefault();

            GrabarLog(2, usuario.IdUsuario, "Usuarios", db.Entry(usuarioOriginal), db.Entry(usuario));

            db.SaveChanges();
            */
            //FALTA LA CONFIRMACION, cambiar el usuario a estado Activo!!


            return View(); // RedirectToAction("ViewUser");
        }

        public ActionResult OlvidoPassword()
        {
            Busqueda();

            return View();
        }

        [HttpPost]
        public ActionResult OlvidoPassword(string email)
        {
            Busqueda();

            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.AsNoTracking().Where(u => u.Email == email).FirstOrDefault();
                //Usuarios user = 
                EnviarMailCambioPassword(usuario);
            }

            string message = "¡Falta poco!";
            string messageDetalle = "Se ha enviado un correo a " + email + " para que recuperes tu contraseña.";

            //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
            return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });

            //return View(email);
        }

        public ActionResult RecuperarPassword(string id)
        {
            Busqueda();

            int idUsuario = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Usuarios usuario = db.Usuarios.Find(idUsuario);


            return View(usuario);
        }

        [HttpPost]
        public ActionResult RecuperarPassword(Usuarios usuario, string IdUsuarioEncrypt)
        {
            Busqueda();

            int id = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdUsuarioEncrypt)));
            usuario.IdUsuario = id;

            var usuarioOriginal = db.Usuarios.AsNoTracking().Where(u => u.IdUsuario == usuario.IdUsuario).FirstOrDefault();
           
            //usuarioOriginal.Password = usuario.Password;
            //usuarioOriginal.ConfirmPassword = usuario.ConfirmPassword;
            //usuarios.IdEstadoUsuario = usuarioOriginal.IdEstadoUsuario;

            //if (ModelState.IsValid)
            {
                usuarioOriginal.Password = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usuario.Password));
                usuarioOriginal.ConfirmPassword = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usuario.ConfirmPassword));


                db.Entry(usuarioOriginal).State = EntityState.Modified;

                //GrabarLog(2, usuario.IdUsuario, "Usuarios", db.Entry(usuarioOriginal), db.Entry(usuario));

                db.SaveChanges();
            }

            string message = "¡Felicitaciones!";
            string messageDetalle = "Tu contraseña ha sido modificada con éxito.";

            //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
            return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });

            //return View(usuario);
        }

        //OLVIDO USUARIO
        public ActionResult OlvidoUser()
        {
            Busqueda();

            return View();
        }

        [HttpPost]
        public ActionResult OlvidoUser(string email)
        {
            Busqueda();

            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.AsNoTracking().Where(u => u.Email == email).FirstOrDefault();
                //Usuarios user = 
                EnviarMailOlvidoUser(usuario);
            }

            string message = "¡Falta poco!";
            string messageDetalle = "Se ha enviado un correo a " + email + " para que recuperes tu usuario.";

            //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
            return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });

            //return View(email);
        }

        public ActionResult LandingPage(int? id)
        {
            Busqueda();


            LogVisitas lv = new LogVisitas();
            lv.Fecha = DateTime.Now;
            lv.IP = DevolverIPCliente();
            if (id != null)
            {
                lv.IdCampaign = id.Value;
            }
            db.LogsVisitas.Add(lv);
            db.SaveChanges();

            


            ViewBag.ListaDias = DevolverDias(0); //listaDias; //.ToList()ñ new SelectList(listaDias);
            ViewBag.ListaMeses = DevolverMeses(0); // listaMeses;
            ViewBag.ListaAnios = DevolverAnios(0); // listaAnios;

            //SelectList deptos = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", new SelectListItem { Text = "Depto", Value = "0", Selected = true });
            //List<SelectListItem> dep = new List<SelectListItem>(db.Departamentos.ToList());

            //SelectList sl =    new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", new SelectListItem { Text = "Depto", Value = "0", Selected = true });
            //var algo = new 
            //SelectList sl2 = new SelectList(new SelectListItem { Text = "Depto", Value = "0" });
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");


            return View();
        }

        public ActionResult LandingPageOpciones(int? id)
        {
            Busqueda();


            LogVisitas lv = new LogVisitas();
            lv.Fecha = DateTime.Now;
            lv.IP = DevolverIPCliente();
            if (id != null)
            {
                lv.IdCampaign = id.Value;
            }
            db.LogsVisitas.Add(lv);
            db.SaveChanges();




            ViewBag.ListaDias = DevolverDias(0); //listaDias; //.ToList()ñ new SelectList(listaDias);
            ViewBag.ListaMeses = DevolverMeses(0); // listaMeses;
            ViewBag.ListaAnios = DevolverAnios(0); // listaAnios;

            //SelectList deptos = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", new SelectListItem { Text = "Depto", Value = "0", Selected = true });
            //List<SelectListItem> dep = new List<SelectListItem>(db.Departamentos.ToList());

            //SelectList sl =    new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", new SelectListItem { Text = "Depto", Value = "0", Selected = true });
            //var algo = new 
            //SelectList sl2 = new SelectList(new SelectListItem { Text = "Depto", Value = "0" });
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");


            return View();
        }
       
        public ActionResult Faq()
        {
            Busqueda();

            return View();
        }


        public ActionResult Recomendaciones()
        {
            Busqueda();

            return View();
        }

        public ActionResult QuienesSomos()
        {
            Busqueda();

            return View();
        }

        public ActionResult Conocenos()
        {
            Busqueda();

            return View();
        }

        public ActionResult Suscripcion()
        {
            Busqueda();

            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");


            ViewBag.ListaDias = DevolverDias(0);
            ViewBag.ListaMeses = DevolverMeses(0);
            ViewBag.ListaAnios = DevolverAnios(0);

            return View();
        }

        [HttpPost]
        public ActionResult CreateSuscriptor2(Suscriptor suscriptor, int ListaDias, int ListaMeses, int ListaAnios)
        {

            Busqueda();

            ModelState.Clear();

            

            if (String.IsNullOrEmpty(suscriptor.Nombre))
            {
                ModelState.AddModelError("Nombre", "Nombre es un campo requerido.");
            }

            if (!String.IsNullOrEmpty(suscriptor.Email))
            {
                if (suscriptor.Email.Contains("@") && suscriptor.Email.Contains("."))
                {
            
                }
                else
                {
                    ModelState.AddModelError("Email", "El Email ingresado no tiene el formato correcto.");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email es un campo requerido.");
            }

            if (ListaMeses == 0 || ListaDias == 0 || ListaAnios == 0)
            {
                ModelState.AddModelError("FechaNacimiento", "Fecha de nacimiento es un campo requerido.");
            }

            if (suscriptor.IdDepartamento == 0)
            {
                ModelState.AddModelError("IdDepartamento", "Departamento es un campo requerido.");
            }
            else
            {
                if (suscriptor.IdDepartamento == 10 && suscriptor.IdBarrio == null)
                {
                    ModelState.AddModelError("IdBarrio", "Barrio es un campo requerido.");
                }
            }

            if (ModelState.IsValid)
            {

                
                suscriptor.FechaSuscripto = DateTime.Now;


                db.Suscriptores.Add(suscriptor);
                //EntityState estado = db.Entry(usuarios).State;
                suscriptor.FechaNacimiento = new DateTime(ListaAnios, ListaMeses, ListaDias);
                
                db.SaveChanges();

                //GrabarLog(1, suscriptor.IdSuscriptor, "Suscriptors", null, db.Entry(suscriptor));


                //SUBO LA IMAGEN

                EnviarMailSuscriptor(suscriptor);

                string message = "¡Gracias por suscribirte!";
                string messageDetalle = "Te hemos enviado un correo electrónico, ahora podrás informarte más sobre MejorLoAlquilo.";

                
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }
           
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", suscriptor.IdDepartamento);
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", suscriptor.IdBarrio);


            ViewBag.ListaDias = DevolverDias(ListaDias);
            ViewBag.ListaMeses = DevolverMeses(ListaMeses);
            ViewBag.ListaAnios = DevolverAnios(ListaAnios);

            return View("Suscripcion", suscriptor);
            
        }

        [HttpPost]
        public ActionResult CreateSuscriptor3(Suscriptor suscriptor)
        {

            Busqueda();

            ModelState.Clear();


            /*
            if (String.IsNullOrEmpty(suscriptor.Nombre))
            {
                ModelState.AddModelError("Nombre", "Nombre es un campo requerido.");
            }*/

            if (!String.IsNullOrEmpty(suscriptor.Email))
            {
                if (suscriptor.Email.Contains("@") && suscriptor.Email.Contains("."))
                {

                }
                else
                {
                    ModelState.AddModelError("Email", "El Email ingresado no tiene el formato correcto.");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email es un campo requerido.");
            }
            /*
            if (ListaMeses == 0 || ListaDias == 0 || ListaAnios == 0)
            {
                ModelState.AddModelError("FechaNacimiento", "Fecha de nacimiento es un campo requerido.");
            }
            
            if (suscriptor.IdDepartamento == 0)
            {
                ModelState.AddModelError("IdDepartamento", "Departamento es un campo requerido.");
            }
            else
            {
                if (suscriptor.IdDepartamento == 10 && suscriptor.IdBarrio == null)
                {
                    ModelState.AddModelError("IdBarrio", "Barrio es un campo requerido.");
                }
            }
            */
            if (ModelState.IsValid)
            {
                suscriptor.Nombre = "SUSCRIPCION DESDE EL FOOTER";
                suscriptor.IdBarrio = 1002;
                suscriptor.IdDepartamento = 10;
                suscriptor.FechaNacimiento = DateTime.Now;
                suscriptor.FechaSuscripto = DateTime.Now;


                db.Suscriptores.Add(suscriptor);
                //EntityState estado = db.Entry(usuarios).State;
                //suscriptor.FechaNacimiento = new DateTime(ListaAnios, ListaMeses, ListaDias);

                db.SaveChanges();

                //GrabarLog(1, suscriptor.IdSuscriptor, "Suscriptors", null, db.Entry(suscriptor));


                //SUBO LA IMAGEN

                EnviarMailSuscriptor(suscriptor);

                string message = "¡Gracias por suscribirte!";
                string messageDetalle = "Te hemos enviado un correo electrónico, ahora podrás informarte más sobre MejorLoAlquilo.";


                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }

            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", suscriptor.IdDepartamento);
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", suscriptor.IdBarrio);

            /*
            ViewBag.ListaDias = DevolverDias(ListaDias);
            ViewBag.ListaMeses = DevolverMeses(ListaMeses);
            ViewBag.ListaAnios = DevolverAnios(ListaAnios);
            */
            return View("Index", suscriptor);

        }

        [HttpPost]
        public ActionResult CreateSuscriptor(Suscriptor suscriptor, int ListaDias, int ListaMeses, int ListaAnios)
        {

            Busqueda();

            ModelState.Clear();

            
            if (String.IsNullOrEmpty(suscriptor.Nombre))
            {
                ModelState.AddModelError("Nombre", "Nombre es un campo requerido.");
            }
            
            if (!String.IsNullOrEmpty(suscriptor.Email))
            {
                if (suscriptor.Email.Contains("@") && suscriptor.Email.Contains("."))
                {
                    //if (existeEmail.Count() != 0)
                    //{
                    //    ModelState.AddModelError("Email", "El Email ingresado ya se encuentra en uso!");
                    //}
                }
                else
                {
                    ModelState.AddModelError("Email", "El Email ingresado no tiene el formato correcto.");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email es un campo requerido.");
            }

            if (ListaMeses == 0 || ListaDias == 0 || ListaAnios == 0)
            {
                ModelState.AddModelError("FechaNacimiento", "Fecha de nacimiento es un campo requerido.");
            }

            if (suscriptor.IdDepartamento == 0)
            {
                ModelState.AddModelError("IdDepartamento", "Departamento es un campo requerido.");
            }
            else
            {
                if (suscriptor.IdDepartamento == 10 && suscriptor.IdBarrio == null)
                {
                    ModelState.AddModelError("IdBarrio", "Barrio es un campo requerido.");
                }
            }

            /*
            if (existeUser.Count() != 0)
            {
                ModelState.AddModelError("User", "El Usuario ingresado ya se encuentra en uso!");
            }

            if (!usuarios.AceptoTerminosYCondiciones)
            {
                ModelState.AddModelError("AceptoTerminosYCondiciones", "Debes aceptar los términos y condiciones.");
            }
            */
            if (ModelState.IsValid)
            {

                /*
                var estadopend = from eu in db.EstadosUsuario
                                 where eu.IdEstadoUsuarioFijo == 2
                                 select eu.IdEstadoUsuario;
                */
                //usuarios.IdBarrio = 2;
                //usuarios.IdEstadoUsuario = estadopend.First(); //PENDIENTE

                //DATOS FIJOS POR AHORA!!
                //usuarios.IdPais = 1;
                //usuarios.IdCiudad = 1;
                suscriptor.FechaSuscripto = DateTime.Now;
                suscriptor.FechaNacimiento = new DateTime(ListaAnios, ListaMeses, ListaDias);
                
                db.Suscriptores.Add(suscriptor);
                //EntityState estado = db.Entry(usuarios).State;

                //Usuarios usu = new Usuarios();
                //usu.Nombre = suscriptor.Nombre;
                //usu.Email = suscriptor.Email;
                //usu.User = suscriptor.Email;
                //usu.IdDepartamento = suscriptor.IdDepartamento;
                //usu.IdBarrio = suscriptor.IdBarrio;
                //db.Usuarios.Add(usu);

                db.SaveChanges();

                //GrabarLog(1, suscriptor.IdSuscriptor, "Suscriptors", null, db.Entry(suscriptor));


                //SUBO LA IMAGEN
                
                EnviarMailSuscriptor(suscriptor);

                string message = "¡Gracias por suscribirte!";
                string messageDetalle = "Te hemos enviado un correo electrónico, ahora podrás informarte más sobre MejorLoAlquilo.";

                //return ErrorPage(message, messageDetalle); // RedirectToAction("ErrorPage");
                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });
            }
            /*
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");
            */
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1", suscriptor.IdDepartamento);
            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1", suscriptor.IdBarrio);

            

            ViewBag.ListaDias = DevolverDias(ListaDias);
            ViewBag.ListaMeses = DevolverMeses(ListaMeses);
            ViewBag.ListaAnios = DevolverAnios(ListaAnios);

            return View("LandingPage",suscriptor);// RedirectToAction("LandingPage");
            //return View(suscriptor);
        }

        
        public ActionResult PruebaJSON()
        {
            
            return Json(new { p1 = "bar", p2 = "Blech" });
        }

        #region LOGS

        //accion, fecha, usuario, tabla, objeto
        public void GrabarLog(int accion, int idUsuario, string tabla, DbEntityEntry original, DbEntityEntry actual/*, DbPropertyValues valoresAnteriores, DbPropertyValues valoresActuales, DbEntityEntry entry, EntityState estado*/)
        {
            DbPropertyValues valores;
            if (accion == 3) //CUANDO ES DELETE
            {
                valores = original.OriginalValues;
            }
            else
            {
                valores = actual.CurrentValues;
            }
            
            //GRABO CABEZAL
            LogModificacionesCabezal lmc = new LogModificacionesCabezal();
            lmc.Accion = accion;
            lmc.Fecha = DateTime.Now;
            lmc.IdUsuario = idUsuario;
            lmc.Tabla = tabla;
            db.LogsModificacionesCabezales.Add(lmc);
            db.SaveChanges();

            //switch (estado)
            //{
            //    case EntityState.Added:
            //        break;
            //    case EntityState.Modified:
            //        break;
            //    case EntityState.Deleted:
            //        break;

            //}

            //GRABO DETALLE
            LogModificacionesDetalle lmd;

            switch(accion)
            {
                case 1: //ALTA
                    //foreach(object o in objeto                    
                    foreach(string campo in valores.PropertyNames)
                    {
                        lmd = new LogModificacionesDetalle();
                        lmd.CampoModificado = campo;
                        lmd.IdLogModificacionesCabezal = lmc.IdLogModificacionesCabezal;
                        lmd.ValorAnterior = null;
                        lmd.ValorActual = ((actual.Property(campo).CurrentValue == null) ? null : actual.Property(campo).CurrentValue.ToString());
                        db.LogsModificacionesDetalles.Add(lmd);
                    }
                    break;

                case 2: //MODIFICACION
                    foreach (string campo in valores.PropertyNames)
                    {
                        if (original.Property(campo).CurrentValue != null && actual.Property(campo).CurrentValue != null)
                        {
                            if ((original.Property(campo).CurrentValue == null && actual.Property(campo).CurrentValue != null) ||
                                (original.Property(campo).CurrentValue != null && actual.Property(campo).CurrentValue == null) ||
                                !(original.Property(campo).CurrentValue.ToString().Equals(actual.Property(campo).CurrentValue.ToString())))
                            {
                                lmd = new LogModificacionesDetalle();
                                lmd.CampoModificado = campo;
                                lmd.IdLogModificacionesCabezal = lmc.IdLogModificacionesCabezal;
                                lmd.ValorAnterior = ((original.Property(campo).CurrentValue == null) ? null : original.Property(campo).CurrentValue.ToString());
                                lmd.ValorActual = ((actual.Property(campo).CurrentValue == null) ? null : actual.Property(campo).CurrentValue.ToString());

                                db.LogsModificacionesDetalles.Add(lmd);
                            }
                        }
                    }
                    break;

                case 3: //BAJA
                    foreach (string campo in valores.PropertyNames)
                    {
                        lmd = new LogModificacionesDetalle();
                        lmd.CampoModificado = campo;
                        lmd.IdLogModificacionesCabezal = lmc.IdLogModificacionesCabezal;
                        lmd.ValorAnterior = ((original.Property(campo).CurrentValue == null) ? null : original.Property(campo).CurrentValue.ToString());
                        lmd.ValorActual = null;
                        db.LogsModificacionesDetalles.Add(lmd);
                    }
                    break;
            }

            db.SaveChanges();
        }

        public void GrabarLogAccesoLogin(int idUsuario)
        {
            LogAccesos la = new LogAccesos();
            la.FechaLogout = null;
            la.FechaLogin = DateTime.Now;
            la.IdUsuario = idUsuario;
            la.IP = DevolverIPCliente();
            db.LogsAccesos.Add(la);
            db.SaveChanges();
        }

        public void GrabarLogAccesoLogout(int idUsuario)
        {
            string ip = DevolverIPCliente();

            var logAcceso = from la in db.LogsAccesos
                            where (la.IdUsuario == idUsuario && la.FechaLogout == null && la.IP == ip)
                            orderby la.FechaLogin descending
                            select la;

            if (logAcceso.Count() > 0)
            {
                LogAccesos log = logAcceso.FirstOrDefault();
                log.FechaLogout = DateTime.Now;

                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
            }
            else 
            {
                //ya estaba logueado o algo se rompio y no pudo desloguearse
                LogAccesos laLogout = new LogAccesos();
                laLogout.FechaLogin = null;
                laLogout.FechaLogout = DateTime.Now;
                laLogout.IdUsuario = idUsuario;
                laLogout.IP = DevolverIPCliente();
                db.LogsAccesos.Add(laLogout);
                db.SaveChanges();
            }
        }

        public void GrabarLogNavegacion(int accion, int idUsuario, string tabla, DbEntityEntry original, DbEntityEntry actual/*, DbPropertyValues valoresAnteriores, DbPropertyValues valoresActuales, DbEntityEntry entry, EntityState estado*/)
        {
        }

        public void GrabarLogBusqueda(string textoBusqueda, int idCategoria)
        {
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            LogBusquedas lb = new LogBusquedas();
            lb.FechaBusqueda = DateTime.Now;
            lb.IP = DevolverIPCliente();
            if (usuario.Count() > 0)
            {
                lb.IdUsuario = usuario.FirstOrDefault();
            }

            if (!String.IsNullOrEmpty(textoBusqueda))
            {
                lb.TextoBusqueda = textoBusqueda;
            }

            if (idCategoria != 0)
            {
                lb.IdCategoriaProducto = idCategoria;
            }

            db.LogsBusquedas.Add(lb);
            db.SaveChanges();
        }

        public void GrabarLogVisitasProductos(Productos prod)
        {
            var usuario = from u in db.Usuarios
                          where u.User == User.Identity.Name
                          select u.IdUsuario;

            string ipVisistante = DevolverIPCliente();
            bool bPrimeraVisita = false;
            
            var visita = from v in db.LogsVisitasProductos
                         where v.IP == ipVisistante && v.IdProducto == prod.IdProducto
                         select v;

            bPrimeraVisita = (visita.Count() == 0);

            if(bPrimeraVisita)
            {
                LogVisitasProductos lvp = new LogVisitasProductos();
                lvp.Fecha = DateTime.Now;
                lvp.IdProducto = prod.IdProducto;
                if (usuario.Count() > 0)
                {
                    lvp.IdUsuario = usuario.First();
                }
                lvp.IP = ipVisistante;
                db.LogsVisitasProductos.Add(lvp);
                db.SaveChanges();

                //SUMO AL CONTADOR
                prod.CantVisitas = prod.CantVisitas + 1;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        public SelectList DevolverDias(int dia)
        {
            List<SelectListItem> listaDias = new List<SelectListItem>();
            //List<int> listaDias = new List<int>();

            listaDias.Add(new SelectListItem { Text = "Día", Value = "0" });
            for (int d = 1; d <= 31; d++)            {
                listaDias.Add(new SelectListItem { Text = d.ToString(), Value = d.ToString() });
                //  listaDias.Add(d);
            }

            return new SelectList(listaDias, "Value", "Text", dia); ;
        }

        public SelectList DevolverMeses(int mes)
        {


            List<SelectListItem> listaMeses = new List<SelectListItem>();
            listaMeses.Add(new SelectListItem { Text = "Mes", Value = "0" });

            for (int m = 1; m <= 12; m++)
            {
                listaMeses.Add(new SelectListItem { Text = m.ToString(), Value = m.ToString() });
            }

            return new SelectList(listaMeses, "Value", "Text", mes); ;
        }

        public SelectList /*List<SelectListItem>*/ DevolverAnios(int anio)
        {
            List<SelectListItem> listaAnios = new List<SelectListItem>();
            listaAnios.Add(new SelectListItem { Text = "Año", Value = "0" });
            
            for (int a = (DateTime.Today.Year - 100); a <= (DateTime.Today.Year - 18); a++)
            {
                listaAnios.Add(new SelectListItem { Text = a.ToString(), Value = a.ToString() });
            }

            return new SelectList(listaAnios,"Value","Text",anio); // new SelectList(listaAnios).ToList();
        }


        public string DevolverIPCliente()
        {
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
            {
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipaddress;
        }

        public void ExpresionRegularParaPassword()
        {
            //Shown below is the regular expression for password strength with n number of digits, upper case, special characters and at least 12 characters in length.
            string regex = @"(?=^.{12,25}$)(?=(?:.*?\d){2})(?=.*[a-z])(?=(?:.*?[A-Z]){2})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$";

            /*
                EXPLICACION
             
                (?=^.{12,25}$) -- password length range from 12 to 25, the numbers are adjustable 
 
                (?=(?:.*?[!@#$%*()_+^&}{:;?.]){1}) -- at least 1 special characters (!@#$%*()_+^&}{:;?.}) , the number is adjustable 
 
                (?=(?:.*?\d){2}) -- at least 2 digits, the number is adjustable 
 
                (?=.*[a-z]) -- characters a-z 
 
                (?=(?:.*?[A-Z]){2}) -- at least 2 upper case characters, the number is adjustable 
             */

        }

        public Image DevolverThumb(Image img)
        {

            /*cuando es mas larga horizontalmente que verticalmente queremos q el alto sea de 200
             * cuand es mas larga vert que hor queremos que el ancho sea 320
             */

            //alto = 200

            decimal r0 = Convert.ToDecimal(img.Width) / Convert.ToDecimal(img.Height);
            decimal r = 246 / 186; //pasar a conf gral!!!

            Size s = new Size();
            //if(r0 > r)
            if (r0 > 1) //es mas ancho que alto
            {
                s.Height = 200;
                s.Width = (200 * img.Width) / img.Height;

                //s.Width = 246;
                //s.Height = (246 * img.Height) / img.Width;
                //img.setAttribute("width", "246");
            }
            else //es mas alto que ancho
            {
                //s.Height = 186;
                //s.Width = (186 * img.Width) / img.Height;
                //img.setAttribute("height", "186");
                s.Width = 330;
                s.Height = (330 * img.Height) / img.Width;
            }

            return img.GetThumbnailImage(s.Width, s.Height, null, System.IntPtr.Zero);
        }

        public ActionResult ProbarEncriptacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProbarEncriptacion(string txtTexto, string txtTexto2, string txtRuta)
        {

            //ProcesoRemoveDiacritics();

            //EncriptarPasswordUsuarios();

            string txtTextoEncrypt = ""; // HttpUtility.UrlEncode(Funciones.Encrypt(txtTexto));
            string txtTextoEncrypt2 = "";

            if (!String.IsNullOrEmpty(txtTexto))
            {
                txtTextoEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(txtTexto));
                txtTextoEncrypt2 = MvcApplication4.Controllers.Funciones.Encrypt(txtTexto);
            }

            string txtTextoDesencrypt = "";

            if (!String.IsNullOrEmpty(txtTexto2))
            {
                txtTextoDesencrypt = Funciones.Decrypt(HttpUtility.UrlDecode(txtTexto2));
            }

            string ruta = Server.MapPath(txtRuta);

            string ruta2 = Url.Content(txtRuta);

            try 
            { 
                Image img1 = Image.FromFile(ruta);
                ViewBag.ErrorRuta1 = "RUTA 1 OK";
            } 
            catch(FileNotFoundException e1)
            {
                ViewBag.ErrorRuta1 = "NO SE ENCONTRO RUTA 1";
            }

            try
            {
                Image img2 = Image.FromFile(ruta2);
                ViewBag.ErrorRuta2 = "RUTA 2 OK";
            }
            catch (FileNotFoundException e2)
            {
                ViewBag.ErrorRuta2 = "NO SE ENCONTRO RUTA 2";
            }

            ViewBag.TextoEncriptado = txtTextoEncrypt;
            
            ViewBag.TextoDesencriptado = txtTextoDesencrypt;
            
            ViewBag.TextoEncriptado2 = txtTextoEncrypt2;

            ViewBag.Ruta = ruta;


            ViewBag.Ruta2 = ruta2;

            return View();
        }

        //[HttpPost]
        public void EncriptarPasswordUsuarios()
        {

            //ProcesoRemoveDiacritics();

            var usuario = from u in db.Usuarios
                          where u.IdUsuario >= 2031
                          select u;

            foreach(Usuarios usu in usuario.ToList())
            {
                usu.Password = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usu.Password));
                usu.ConfirmPassword = HttpUtility.UrlEncode(MvcApplication4.Controllers.Funciones.Encrypt(usu.ConfirmPassword));
                db.Entry(usu).State = EntityState.Modified;
                //try
                //{

                    db.SaveChanges();
                //}
                //catch(Exception e)
                //{
                  //  string msj = e.Message;
                //}
            }

            

            //string txtTextoEncrypt = HttpUtility.UrlEncode(Funciones.Encrypt(txtTexto));

            //string txtTextoEncrypt2 = MvcApplication4.Controllers.Funciones.Encrypt(txtTexto);

            //ViewBag.TextoEncriptado = txtTextoEncrypt;

            //ViewBag.TextoEncriptado2 = txtTextoEncrypt2;

//            return View();
        }


        public ViewResult ListarCantidadSuscriptoresPorDia()
        {
            //var cantpordia = from lv in db.LogsVisitas
            //                 orderby lv.Fecha descending
            //                 group lv by lv.Fecha.Value.Date into d
            //                 select new { FechaRet = d.Key, Cant = d.Count() };


            string sql = "select CAST(fecha as date) AS fecha, count(*) as cant from logvisitas group by CAST(fecha as date) order by CAST(fecha as date) DESC";
            var args = new DbParameter[] { /*new SqlParameter { ParameterName = "Major", Value = "Masters" }*/ };
            var spd = db.Database.SqlQuery(typeof(SuscriptoresPorDia),sql, args);

            List<SuscriptoresPorDia> listaSD = spd.Cast<SuscriptoresPorDia>().ToList();
            
            //foreach(var item in cantpordia.ToList())
            //{
            //    SuscriptoresPorDia sd = new SuscriptoresPorDia();
            //    sd.cant = item.Cant;
            //    sd.fecha = item.FechaRet;

            //    listaSD.Add(sd);
            //}

            ViewBag.ListaLogVisitas = listaSD;
            return View();
        }

        public string ObtenerConfiguracion(string parametro)
        {
            var conf = from c in db.Configuraciones
                          where c.Parametro == parametro
                          select c.Valor;
            
            string ret = conf.First();

            return ret;
        }

        #region REPORTES

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ReporteVisitasPorArticulo()
        {
            
            Busqueda();
            
            return View(db.Productos.OrderByDescending(p => p.CantVisitas).ToList());
        }

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ReporteSuscritosPorAnio()
        {
            
            Busqueda();
            return View(); //db.Suscriptores.Where(s => s.FechaNacimiento.Year >= anioDesde && s.FechaNacimiento.Year <= anioHasta).OrderByDescending(s => s.IdSuscriptor).ToList());
        }
        
        [HttpPost]
        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ReporteSuscritosPorAnioResultado(int anioDesde, int anioHasta)
        {

            Busqueda();
            
            ViewBag.Desde = anioDesde;
            ViewBag.Hasta = anioHasta;
            ViewBag.CantidadVisitas = db.Suscriptores.Where(s => s.FechaNacimiento.Year >= anioDesde && s.FechaNacimiento.Year <= anioHasta).OrderByDescending(s => s.IdSuscriptor).Count();
            return View(db.Suscriptores.Where(s => s.FechaNacimiento.Year >= anioDesde && s.FechaNacimiento.Year <= anioHasta).OrderByDescending(s => s.IdSuscriptor).ToList());
        }

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ReporteConsultasRealizadas()
        {
            
            Busqueda();

            var sol = from cp in db.ComentariosProducto
                      join lmd in db.LogsModificacionesDetalles on cp.IdComentariosProducto.ToString() equals lmd.ValorActual
                      join lmc in db.LogsModificacionesCabezales on lmd.IdLogModificacionesCabezal equals lmc.IdLogModificacionesCabezal
                      where lmc.Accion == 1 && lmc.Tabla == "ComentariosProductoes" && lmd.CampoModificado == "IdComentariosProducto"
                      orderby cp.FechaComentario descending
                      select new
                      {
                          cp.Producto.Nombre,
                          Propietario = cp.Producto.Usuario.Nombre + " " + cp.Producto.Usuario.Apellido + " [" + cp.Producto.Usuario.User + "]",
                          Consultante = cp.Usuario.Nombre + " " + cp.Usuario.Apellido + " [" + cp.Usuario.User + "]",
                          lmc.Fecha,
                          Consulta = cp.Comentario,
                          EmailProp = cp.Producto.Usuario.Email,
                          EmailCons = cp.Usuario.Email,
                          cp.EsRespuesta
                      };

            List<ReporteConsultaView> ret = new List<ReporteConsultaView>();
            foreach (var i in sol.ToList())
            {
                ReporteConsultaView item = new ReporteConsultaView { NombreProducto = i.Nombre, PropietarioProducto = i.Propietario, UsuarioConsultante = i.Consultante, FechaRealizado = i.Fecha, Consulta = i.Consulta, EmailPropietario = i.EmailProp, EmailConsultante = i.EmailCons, EsRespuesta = i.EsRespuesta };
                ret.Add(item);
            }
            //List<ReporteSolicitudView> ret = sol.ToList<ReporteSolicitudView>();
            //IQueryable list = sol;
            //return View(sol.ToList<ReporteSolicitudView>());
            return View(ret/* db.Alquilers.OrderByDescending(a => a.IdAlquiler).ToList()*/);

            //return View(db.ComentariosProducto.OrderByDescending(c => c.IdComentariosProducto).ToList());
        }

        [Authorize(Users = "dimapego,pepe,MLAAdmin")]
        public ActionResult ReporteSolicitudesRealizadas()
        {
            
            Busqueda();

            var sol = from a in db.Alquilers
                      join lmd in db.LogsModificacionesDetalles on a.IdAlquiler.ToString() equals lmd.ValorActual
                      join lmc in db.LogsModificacionesCabezales on lmd.IdLogModificacionesCabezal equals lmc.IdLogModificacionesCabezal
                      where lmc.Accion == 1 && lmc.Tabla == "Alquileres"
                      orderby lmc.Fecha descending
                      select new
                      {
                          a.FechaDesde,
                          a.FechaHasta,
                          a.Producto.Nombre,
                          Propietario = a.Producto.Usuario.User,
                          Arrendatario = a.UsuarioArrendatario.User,
                          lmc.Fecha,
                          Estado = a.EstadoAlquiler.DescripcionI1,
                          EmailProp = a.Producto.Usuario.Email,
                            EmailArren = a.UsuarioArrendatario.Email
                      };

            List<ReporteSolicitudView> ret = new List<ReporteSolicitudView>();
            foreach (var i in sol.ToList())
            {
                ReporteSolicitudView item = new ReporteSolicitudView { FechaDesde = i.FechaDesde, FechaHasta = i.FechaHasta, ArrendatarioProducto = i.Arrendatario, Estado = i.Estado, FechaRealizado = i.Fecha, NombreProducto = i.Nombre, PropietarioProducto = i.Propietario, EmailPropietario = i.EmailProp, EmailArrendatario = i.EmailArren};
                ret.Add(item);
            }
            //List<ReporteSolicitudView> ret = sol.ToList<ReporteSolicitudView>();
            //IQueryable list = sol;
            //return View(sol.ToList<ReporteSolicitudView>());
            return View(ret/* db.Alquilers.OrderByDescending(a => a.IdAlquiler).ToList()*/);
        }

        #endregion

        public int DevolverCantidadConsultasSinLeer()
        {
            //Consultas para articulos donde el duenho es el usuario logueado
            var cons = from c in db.ComentariosProducto
                       where c.Leido == false && c.IdComentariosProductoOrigen == null && c.Producto.Usuario.User == User.Identity.Name
                       select c;

            return cons.Count();
        }

        public int DevolverCantidadSolicitudesPendientes()
        {
            //Consultas para articulos donde el duenho es el usuario logueado
            var solpend = from a in db.Alquilers
                       where a.IdEstadoAlquiler == 1 && a.Producto.Usuario.User == User.Identity.Name && a.Leido == false
                       select a;

            return solpend.Count();
        }

        public int DevolverCantidadReservasAprobadas()
        {
            /*
             1	Pendiente
            2	Rechazado
            3	Confirmado
             */
            //reservas aprobadas para articulos donde el arrendatario es el usuario logueado
            var solapro = from a in db.Alquilers
                          where a.IdEstadoAlquiler == 3 && a.UsuarioArrendatario.User == User.Identity.Name && a.Leido == false
                          select a;

            return solapro.Count();
        }

        public int DevolverCantidadReservasRechazadas()
        {
            /*
             1	Pendiente
            2	Rechazado
            3	Confirmado
             */
            //reservas rechazadas para articulos donde el arrendatario es el usuario logueado
            var solrech = from a in db.Alquilers
                          where a.IdEstadoAlquiler == 2 && a.UsuarioArrendatario.User == User.Identity.Name && a.Leido == false
                          select a;

            return solrech.Count();
        }

        public void MarcarConsultasComoLeidas()
        {
            var cons = from c in db.ComentariosProducto
                       where c.Leido == false && c.IdComentariosProductoOrigen == null && c.Producto.Usuario.User == User.Identity.Name
                       select c;

            foreach (ComentariosProducto cp in cons)
            {
                cp.Leido = true;
                db.Entry(cp).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        

        public void MarcarSolicitudesComoLeidas(int idEstadoAlquiler)
        {
            /* AGRGAR PROPIEDAD LEIDO o VISTO en Alquileres
            var cons = from c in db.ComentariosProducto
                       where c.Leido == false && c.IdComentariosProductoOrigen == null && c.Producto.Usuario.User == User.Identity.Name
                       select c;

            foreach (ComentariosProducto cp in cons)
            {
                cp.Leido = true;
                db.Entry(cp).State = EntityState.Modified;
            }

            db.SaveChanges();
             */ 
        }


        public int DevolverCantidadRespuestasSinLeer()
        {
            //Respuestas donde el usuario logueado es el usuario del comentario ORIGEN.
            var cons = from c in db.ComentariosProducto
                       where c.Leido == false && c.ComentarioProductoOrigen.Usuario.User == User.Identity.Name
                       select c;

            return cons.Count();
        }

        public void MarcarRespuestasComoLeidas()
        {
            var cons = from c in db.ComentariosProducto
                       where c.Leido == false && c.ComentarioProductoOrigen.Usuario.User == User.Identity.Name
                       select c;

            foreach (ComentariosProducto cp in cons)
            {
                cp.Leido = true;
                db.Entry(cp).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public void MarcarSolicitudesComoLeidas()
        {
            var sol = from a in db.Alquilers
                       where a.Leido == false && a.Producto.Usuario.User == User.Identity.Name && a.IdEstadoAlquiler == 1
                       select a;

            foreach (Alquileres alq in sol)
            {
                alq.Leido = true;
                db.Entry(alq).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public void MarcarReservasAprobadasComoLeidas()
        {
            var sol = from a in db.Alquilers
                      where a.Leido == false && a.UsuarioArrendatario.User == User.Identity.Name && a.IdEstadoAlquiler == 3
                      select a;

            foreach (Alquileres alq in sol)
            {
                alq.Leido = true;
                db.Entry(alq).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public void MarcarReservasRechazadasComoLeidas()
        {
            var sol = from a in db.Alquilers
                      where a.Leido == false && a.UsuarioArrendatario.User == User.Identity.Name && a.IdEstadoAlquiler == 2
                      select a;

            foreach (Alquileres alq in sol)
            {
                alq.Leido = true;
                db.Entry(alq).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public void SEARCH()
        {
            string searchName = "foo";

            string SQL = "Select ID, LastName, FirstName, WebSite, TimeStamp From dbo.Author Where Contains (LastName, {0})";

            //IEnumerable<Productos> authors = db.ExecuteQuery(Of Author)(SQL, searchName)
        }

        //Transforms the culture of a letter to its equivalent representation in the 0-127 ascii table, such as the letter 'é' is substituted by an 'e'
        public string RemoveDiacritics(string s)
        {
            string normalizedString = null;
            StringBuilder stringBuilder = new StringBuilder();
            normalizedString = s.Normalize(NormalizationForm.FormD);
            int i = 0;
            char c = '\0';

            for (i = 0; i <= normalizedString.Length - 1; i++)
            {
                c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().ToLower();
        }

        [HttpPost]
        public void ProcesoRemoveDiacritics()
        {
            foreach(Productos p in db.Productos.AsNoTracking())
            {
                p.NombreBusqueda = RemoveDiacritics(p.Nombre);
                p.DescripcionBusqueda = RemoveDiacritics(p.Descripcion);

                db.Entry(p).State = EntityState.Modified;
                
                
            }

            db.SaveChanges();
        }


        public ActionResult EvaluarArrendador(string id)
        {
            Busqueda();

            int idAlquiler = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Alquileres alquiler = db.Alquilers.Find(idAlquiler);

            //ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            //ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            return View(alquiler);
        }

        [HttpPost]
        public ActionResult EvaluarArrendador(Alquileres alq, string calif, int[] score, string IdAlquilerEncrypt)
        {
            Busqueda();

            int id = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdAlquilerEncrypt)));
            //alquiler.IdAlquiler = id;
            Alquileres alquiler = db.Alquilers.AsNoTracking().Where(a => a.IdAlquiler == id).FirstOrDefault();

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                //ARRENDADOR
                alquiler.CalificacionCuidadoArrendador = score[0];
                alquiler.CalificacionComunicacionArrendador = score[1];
                alquiler.CalificacionCumplimientoNormasArrendador = score[2];
                
                alquiler.CalificacionPositivaArrendatario = (calif == "1");
                //alquiler.ComentarioArrendatario = Comentario;
                alquiler.ComentarioExperienciaArrendador = alq.ComentarioExperienciaArrendador;
                alquiler.ComentarioPrivadoArrendador = alq.ComentarioPrivadoArrendador;

                db.Entry(alquiler).State = EntityState.Modified;
                
                db.SaveChanges();
            }

            return RedirectToAction("ViewAlquileresDelUsuario");


        }

        public ActionResult EvaluarArrendatario(string id)
        {
            Busqueda();
            

            int idAlquiler = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Alquileres alquiler = db.Alquilers.Find(idAlquiler);

            return View(alquiler);
        }

        [HttpPost]
        public ActionResult EvaluarArrendatario(Alquileres alq, string calif, int [] score, string IdAlquilerEncrypt)
        {
            Busqueda();

            int id = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdAlquilerEncrypt)));
            //alquiler.IdAlquiler = id;
            Alquileres alquiler = db.Alquilers.AsNoTracking().Where(a => a.IdAlquiler == id).FirstOrDefault();


            ModelState.Clear();

            if (ModelState.IsValid)
            {
                //ARRENDATARIO
                alquiler.CalificacionCuidadoArrendatario = score[0];
                alquiler.CalificacionComunicacionArrendatario = score[1];
                alquiler.CalificacionCumplimientoNormasArrendatario = score[2];
                
                alquiler.CalificacionPositivaArrendatario = (calif == "1");
                //alquiler.ComentarioArrendatario = Comentario;
                alquiler.ComentarioExperienciaArrendatario = alq.ComentarioExperienciaArrendatario;
                alquiler.ComentarioPrivadoArrendatario = alq.ComentarioPrivadoArrendatario;

                db.Entry(alquiler).State = EntityState.Modified;
                /*
                Calificacion c = new Calificacion();
                c.IdAlquiler = id;
                c.CalificacionValor = score;
                c.Comentario = Comentario;
                

                db.Calificaciones.Add(c);
                 */
                db.SaveChanges();
            }

            return RedirectToAction("ViewAlquileresPorUsuario");

            
        }

        [HttpPost]
        public ActionResult CalificarAlquiler(string IdAlquilerEncrypt, int score, string Comentario, string calif)
        {
            Busqueda();
            ModelState.Clear();


            int id = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(IdAlquilerEncrypt)));
            
            var alquiler = db.Alquilers.AsNoTracking().Where(a => a.IdAlquiler == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                //ARRENDATARIO
                alquiler.CalificacionArrendatario = score;
                alquiler.CalificacionPositivaArrendatario = (calif == "1");
                alquiler.ComentarioArrendatario = Comentario;

                db.Entry(alquiler).State = EntityState.Modified;
                /*
                Calificacion c = new Calificacion();
                c.IdAlquiler = id;
                c.CalificacionValor = score;
                c.Comentario = Comentario;
                

                db.Calificaciones.Add(c);
                 */
                db.SaveChanges();
            }

            return RedirectToAction("ViewAlquileresDelUsuario");
            

        }

        public ActionResult VerEvaluacionArrendador(string id)
        {
            Busqueda();


            int idAlquiler = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Alquileres alquiler = db.Alquilers.Find(idAlquiler);

            return View(alquiler);
        }

        public ActionResult VerEvaluacionArrendatario(string id)
        {
            Busqueda();


            int idAlquiler = Convert.ToInt32(Funciones.Decrypt(HttpUtility.UrlDecode(id)));
            Alquileres alquiler = db.Alquilers.Find(idAlquiler);

            return View(alquiler);
        }

        
        public ActionResult BannerServicioPremium()
        {
            Busqueda();

            return View();
        }

        [Authorize]
        public ActionResult ServicioPremium()
        {
            Busqueda();

            //ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true).OrderBy(c => c.DescripcionI1), "IdCategoriaProducto", "DescripcionI1");
            
            List<SelectListItem> estado = new List<SelectListItem>();
            estado.Add(new SelectListItem { Text = "Nuevo", Value = "1" });
            estado.Add(new SelectListItem { Text = "Usado", Value = "2" });

            List<SelectListItem> vidautil = new List<SelectListItem>();
            vidautil.Add(new SelectListItem { Text = "1 año", Value = "1" });
            vidautil.Add(new SelectListItem { Text = "2 años", Value = "2" });
            vidautil.Add(new SelectListItem { Text = "Mas de 2 años", Value = "3" });
            

            ViewBag.Estado = new SelectList(estado, "Value","Text");
            ViewBag.TiempoVidaUtil = new SelectList(vidautil, "Value", "Text");
            //ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo");
            //ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1");
            //ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            return View();
        }

        [HttpPost]
        public ActionResult ServicioPremium(ICollection<int> IdCategoriaProducto, ICollection<string> Marca, ICollection<string> Descripcion, ICollection<int> Cantidad, ICollection<int> Estado, ICollection<decimal> PrecioEstimadoActual, ICollection<decimal> PrecioCompra, ICollection<int> TiempoVidaUtil, int envio)
        {
            Busqueda();

            ModelState.Clear();

            if (ModelState.IsValid)
            {

                var usuario = from u in db.Usuarios
                              where u.User == User.Identity.Name
                              select u.IdUsuario;

                List<Formulario> listaFormulario = new List<Formulario>();

                for (int i = 0; i < IdCategoriaProducto.Count(); i++)
                {
                    Formulario f = new Formulario();

                    f.Cantidad = Cantidad.ToArray()[i];
                    f.Descripcion = Descripcion.ToArray()[i];
                    f.Marca = Marca.ToArray()[i];
                    f.PrecioCompra = PrecioCompra.ToArray()[i];
                    f.PrecioEstimadoActual = PrecioEstimadoActual.ToArray()[i];
                    f.TiempoVidaUtil = TiempoVidaUtil.ToArray()[i];
                    f.IdCategoriaProducto = IdCategoriaProducto.ToArray()[i];
                    f.Estado = Estado.ToArray()[i];
                    f.IdUsuario = usuario.First();

                    f.OpcionEnvio = envio;

                    listaFormulario.Add(f);

                    db.Formularios.Add(f);
                    db.SaveChanges();
                }

                EnviarMailNuevoFormulario(listaFormulario);

                string message = "¡Gracias por tu aporte!";
                string messageDetalle = "El formulario ha sido enviado para que nuestro equipo evalúe la información ingresada, te estaremos contactando a la brevedad.";


                return RedirectToAction("ErrorPage", new { m1 = message, m2 = messageDetalle });

            }

            //ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true).OrderBy(c => c.DescripcionI1), "IdCategoriaProducto", "DescripcionI1");

            List<SelectListItem> estado = new List<SelectListItem>();
            estado.Add(new SelectListItem { Text = "Nuevo", Value = "1" });
            estado.Add(new SelectListItem { Text = "Usado", Value = "2" });



            ViewBag.Estado = new SelectList(estado, "Value", "Text");
            //ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo");
            //ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1");
            //ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            return View();
        }

        public bool ValidarDatosPermitidos(string texto)
        {

            if (String.IsNullOrEmpty(texto))
            {
                return true;
            }

            //Regex regEx = new Regex(@"^[a-zA-Z'.]{1,40}$");
            //regEx.IsMatch(texto);
            
            //VALIDO NUMEROS (DE TELEFONO, DE DIRECCION, ETC) ...non negative int
            /*if (Regex.IsMatch(texto, @"^\d+$"))
            {
                return false;
            }*/

            //List<string> palabrasReservadas = new List<string>();
            string[] palabrasReservadas = ObtenerConfiguracion("PALABRAS_RESERVADAS").Split(';');
            
            //FALTA AGERGAR LAS PALABRAS a la lista
            
            foreach (string pal in palabrasReservadas)
            {
                if (texto.ToUpper().Contains(pal.ToUpper()))
                {
                    return false;
                }
            }

            return true;
        }



        //RESPONSIVE

        public ActionResult IndexRESP()
        {
            return View();
        }

        public ActionResult SendSMS()
        {
            Busqueda();
            // Find your Account Sid and Auth Token at twilio.com/user/account
            string AccountSid = "ACefeff6e4af493bda816a410099e0d430";

            string AccountSidTest = "ACd716ad7bc888de9414371d06b572fcc2";

            string AuthToken = "a77954720996fc86ef3e79279a13a448";

            string AuthTokenTest = "26babd61d0ded0040ebd4fdc139420c1";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage(
                "+19283623170", "+59894170358",
                "SMS de prueba",
                new string[] { "https://c1.staticflickr.com/3/2899/14341091933_1e92e62d12_b.jpg" }
            );

            return View();
        }



        //REFRESHHHHHHHH

        public ActionResult SeleccionModo()
        {
            //Busqueda();
            
            return View();
        }

        public ActionResult ModoBasico()
        {
            //Busqueda();

            return View("Index2");
        }

        public ActionResult ModoAvanzado()
        {
            Busqueda();
            //DEBERIA LLEVAR A LA PAGINA ORIGINAL

            return View("Index");
        }

        public ActionResult Index2()
        {
            //Busqueda();

            return View();
        }

        public ActionResult BuscadorSimple()
        {
            Busqueda();

            return View("Index");
        }

        [Authorize]
        public ActionResult CreateProductPasoAPaso()
        {
            Busqueda();

            ViewBag.IdBarrio = new SelectList(db.Barrios.OrderBy(b => b.DescripcionI1), "IdBarrio", "DescripcionI1");
            ViewBag.IdCategoriaProducto = new SelectList(db.CategoriaProductoes.Where(cp => cp.Visible == true).OrderBy(c => c.DescripcionI1), "IdCategoriaProducto", "DescripcionI1");
            ViewBag.IdMoneda = new SelectList(db.Monedas, "IdMoneda", "Simbolo");
            ViewBag.IdTipoProducto = new SelectList(db.TiposProductos.OrderBy(tp => tp.DescripcionI1), "IdTipoProducto", "DescripcionI1");
            ViewBag.IdDepartamento = new SelectList(db.Departamentos.OrderBy(d => d.DescripcionI1), "IdDepartamento", "DescripcionI1");

            return View();
        }
    }


    
    
    public class ReporteSolicitudView
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string NombreProducto { get; set; }
        public string PropietarioProducto { get; set; }
        public string ArrendatarioProducto { get; set; }
        public DateTime FechaRealizado { get; set; }
        public string Estado { get; set; }
        public string EmailPropietario { get; set; }
        public string EmailArrendatario { get; set; }
    }

    public class ReporteConsultaView
    {
        public string NombreProducto { get; set; }
        public string PropietarioProducto { get; set; }
        public string UsuarioConsultante { get; set; }
        public DateTime FechaRealizado { get; set; }
        public string Consulta { get; set; }
        public string EmailPropietario { get; set; }
        public string EmailConsultante { get; set; }
        public bool EsRespuesta { get; set; }
    }

    public class PconU {
        public Productos p;
        public Usuarios u;
    }

    public class EmailJob : IJob
    {
        public MvcApplication4.Models.Usuarios UsuarioDestino { get; set; }
        public string EmailTo { get; set; }

        public EmailJob(Usuarios usuario)
        {
            //EmailTo = emailTo;
            UsuarioDestino = usuario;
        }

        public void Execute(IJobExecutionContext context)
        {
            HomeController.EnviarMailPRUEBA();

            JobKey key = context.JobDetail.Key;
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string emailTo = dataMap.GetString("emailTo");
            string nombreTo = dataMap.GetString("nombreTo");

            HomeController.EnviarMailRecordatorioConsultasSinResponder(emailTo,nombreTo);
            
            
            //using (var message = new MailMessage("user@gmail.com", "user@live.co.uk"))
            //{
            //    message.Subject = "Test";
            //    message.Body = "Test at " + DateTime.Now;
            //    using (SmtpClient client = new SmtpClient
            //    {
            //        EnableSsl = true,
            //        Host = "smtp.gmail.com",
            //        Port = 587,
            //        Credentials = new NetworkCredential("user@gmail.com", "password")
            //    })
            //    {
            //        client.Send(message);
            //    }
            //}
        }
    }

    public class JobScheduler
    {
        public static void Start(Usuarios usuario)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            
            //EmailJob ej = new EmailJob(new Usuarios());

            IJobDetail job = JobBuilder.Create<EmailJob>()
                .WithIdentity("PRUEBA1")
                .UsingJobData("emailTo",usuario.Email)
                .UsingJobData("nombreTo", usuario.Nombre)
                .Build();
                

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule
                (s => s.RepeatForever()
                    .WithIntervalInHours(2)
                    )
                /*.WithDailyTimeIntervalSchedule
                  (s =>
                      
                     s.WithIntervalInHours(2)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )*/
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }

    

    //namespace ServerNotifications.Web.Domain.Twilio
    
    //public interface IRestClient
    //{
    //    Message SendMessage(string from, string to, string body, params string[] mediaUrl);
    //}

    //public class RestClient : IRestClient
    //{   
    //    private readonly TwilioRestClient _client;
    //    private MLAContext db = new MLAContext();

    //    public string ObtenerConfiguracion(string parametro)
    //    {
    //        var conf = from c in db.Configuraciones
    //                   where c.Parametro == parametro
    //                   select c.Valor;

    //        string ret = conf.First();

    //        return ret;
    //    }

    //    public RestClient()
    //    {
    //        _client = new TwilioRestClient(Credentials.TwilioAccountSid, Credentials.TwilioAuthToken);
    //    }

    //    public RestClient(TwilioRestClient client)
    //    {
    //        _client = client;
    //    }

    //    public Message SendMessage(string from, string to, string body, params string[] mediaUrl)
    //    {
    //        return _client.SendMessage(from, to, body, mediaUrl);
    //    }
    //}
    
}
