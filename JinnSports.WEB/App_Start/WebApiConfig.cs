using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JinnSports.WEB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "ApiTeamDetails",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "TeamDetails", action = "LoadResults", id = RouteParameter.Optional });
        }
    }
}
