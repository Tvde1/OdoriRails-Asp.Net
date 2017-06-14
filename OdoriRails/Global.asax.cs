using OdoriRails.Helpers.DAL;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

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

        void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            string message = "";
            if (exception is DatabaseException)
            {
                message = "Something went wrong connecting to our database. Please try again later.";
            }
            else if (exception is HttpException)
            {
                message = "This page does not exist.";
            }
            else
            {
                message = "An unknown error has occurred.";
            }

            Server.ClearError();
            Response.RedirectToRoute("Default", new { controller = "Login", action = "Error", message });
        }
    }
}