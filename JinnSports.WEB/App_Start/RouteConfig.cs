using System.Web.Mvc;
using System.Web.Routing;

namespace JinnSports.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "Results",
                 url: "Results",
                 defaults: new { controller = "Event", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "JinnSports.WEB.Controllers" });

            routes.MapRoute(
                name: "Teams",
                url: "Teams",
                defaults: new { controller = "Team", action = "Index" },
                namespaces: new[] { "JinnSports.WEB.Controllers" });

            //MVC presentation part Routing
            routes.MapRoute(
                name: "MainPage",
                url: string.Empty,
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "JinnSports.WEB.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}");
        }
    }
}
