using System.Web.Mvc;
using System.Web.Routing;

namespace OdoriRails
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "SchoonmaakReparatie",
                "mainmenu/{action}/{id}",
                new{controller = "SRMainMenu", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}
