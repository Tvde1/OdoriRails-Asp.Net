using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using OdoriRails.Helpers.DAL;

namespace OdoriRails
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Response.Clear();

            var message = "";
            if (exception is DatabaseException)
                message = "Something went wrong connecting to our database. Please try again later.";
            else if (exception is HttpException)
                message = "This page does not exist.";
            else
                message = "An unknown error has occurred.";

            Server.ClearError();
            Response.RedirectToRoute("Default", new {controller = "Login", action = "Error", message});
        }
    }
}