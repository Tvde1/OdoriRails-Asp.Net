using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OdoriRails
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "Api Default",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
