using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ST1109348
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //name: "DeleteFarmer",
            //url: "{Employee}/{Delete}/{id}",
            //defaults: new { controller = "Employee", action = "Delete", id = "" }
            //);

        }
    }
}
