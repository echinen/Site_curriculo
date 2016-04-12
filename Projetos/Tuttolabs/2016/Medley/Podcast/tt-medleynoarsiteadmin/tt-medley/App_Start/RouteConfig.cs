using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace tt_medley
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    name:"Json",
                    url:"Json/{action}",
                    defaults: new { controller = "Json", action = "Index" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "mpc_podcasts", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultEmail",
                url: "{controller}/{action}/{code}/{email}",
                defaults: new { controller = "mpc_podcasts", action = "Index", code = UrlParameter.Optional, email = UrlParameter.Optional }
            );
        }
    }
}
