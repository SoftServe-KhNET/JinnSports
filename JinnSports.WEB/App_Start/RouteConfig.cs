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
                 defaults: new { controller = "Event", action = "Index" });


            routes.MapRoute(
                name: "Teams",
                url: "Teams",
                defaults: new { controller = "Team", action = "Index" });

            routes.MapRoute(
                name: "TeamDetails",
                url: "Team/{id}",
                defaults: new { controller = "Team", action = "Details", id = UrlParameter.Optional });


            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
