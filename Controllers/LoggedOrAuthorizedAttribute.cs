using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication4.Controllers
{
    public class LoggedOrAuthorizedAttribute : AuthorizeAttribute
    {
        public LoggedOrAuthorizedAttribute()
        {
            View = "error";
            Master = String.Empty;
        }

        public String View { get; set; }
        public String Master { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            //CheckIfUserIsAuthenticated(filterContext);
        }

        private void CheckIfUserIsAuthenticated(AuthorizationContext filterContext)
        {
            // If Result is null, we’re OK: the user is authenticated and authorized. 
            if (filterContext.Result == null)
                return;

            // If here, you’re getting an HTTP 401 status code. In particular,
            // filterContext.Result is of HttpUnauthorizedResult type. Check Ajax here. 
            if (filterContext.HttpContext.Request.IsAjaxRequest() && !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // For an Ajax request, just end the request 
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();

                //var routeValues =  new RouteValueDictionary();
                //routeValues["controller"] = "Home";
                //routeValues["action"] = "LogIn";

                //var result = new RedirectToRouteResult(routeValues);//new ViewResult { ViewName = "LogIn" };
                //filterContext.Result = result;
            }

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(View))
                    return;
                var result = new ViewResult { ViewName = View, MasterName = Master };
                filterContext.Result = result;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //base.HandleUnauthorizedRequest(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest() && !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // For an Ajax request, just end the request 
                //filterContext.HttpContext.Response.StatusCode = 401;
                //filterContext.HttpContext.Response.End();

                var routeValues = new RouteValueDictionary();
                routeValues["controller"] = "Home";
                routeValues["action"] = "LogIn";

                var result = new RedirectToRouteResult(routeValues);//new ViewResult { ViewName = "LogIn" };
                filterContext.Result = new RedirectController().RedirectWherever(); //result;
            }

            
        }
    }

    public class RedirectController : Controller
    {
        public ActionResult RedirectWherever()
        {
            return RedirectToAction("LogIn", "Home");
        }
    }
}