using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GovindEventReminderService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces: new string[]
                {
                    "GovindEventReminderService.Controllers"
                }
            );

            routes.MapRoute(
               name: "Admin",

               url: "{controller}/{action}/{id}",
               defaults: new
               { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces: new string[]

               {
                   "GovindEventReminderService.Areas.Admin.Controllers"
                 
               }
               
           );

            routes.MapRoute(
                name: "User",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces: new string[]
                {
                    "GovindEventReminderService.Areas.User.Controllers"
                }

);
        }
    }
}
