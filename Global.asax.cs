using MvcApplication4.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using Owin;
/*
using System.Web.Optimization;
using ServerNotifications.Web.Domain;
using ServerNotifications.Web.Domain.Twilio;
using ServerNotifications.Web.Models.Repository;
*/

namespace MvcApplication4
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //private const string JobCashAction = "http://localhost:38416/Home/AddJobCache"; 

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Busqueda",
                url: "Productos/{action}/{id}",
                defaults: new
                {
                    controller = "Productos",
                    action = "Buscar",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            /*routes.MapRoute(
                "Buscarrrr", // Route name
                "Productos/Buscar", // URL with parameters
                new { controller = "Productos", action = "Buscar", id = UrlParameter.Optional } // Parameter defaults
            );*/

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=USUARIO-HP\SQLEXPRESS; Integrated Security=True; MultipleActiveResultSets=True");
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=USUARIO-HP\SQLEXPRESS;Initial Catalog=MLA;Integrated Security=SSPI");
            
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //JobScheduler.Start();

                        
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            //string lang = Request.RequestContext.RouteData.Values["culture"].ToString();
            //CultureInfo culture = CultureInfo.InvariantCulture;//if need invariant
            //System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(lang);

            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            /*
            var rd = Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            string currentArea = rd.Values["area"] as string;
            */

            if (Request.Url.AbsolutePath.Contains("EditProduct"))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }

            //if (HttpContext.Current.Cache["jobkey"] == null)
            //{
            //    HttpContext.Current.Cache.Add("jobkey",
            //                    "jobvalue", null,
            //                    DateTime.MaxValue,
            //                    TimeSpan.FromSeconds(15), // set the time interval  
            //                    CacheItemPriority.Default, JobCacheRemoved);
            //}  

        }

        //private static void JobCacheRemoved(string key, object value, CacheItemRemovedReason reason)
        //{
        //    var client = new WebClient();
        //    client.DownloadData(JobCashAction);
        //    ScheduleJob();
        //}
        //private static void ScheduleJob()
        //{
        //    // ExecuteAnyMethod  
        ////}  

        /*
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var message = string.Format("[This is a test] ALERT!" +
                "It appears the server is having issues." +
                "Exception: {0}. Go to: http://newrelic.com for more details.", exception.Message);

            var notifier = new Notifier(new AdministratorsRepository(), new RestClient());
            notifier.SendMessages(message);
        }
        */
    }
}