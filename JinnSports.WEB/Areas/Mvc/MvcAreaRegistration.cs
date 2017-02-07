using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc
{
    public class MvcAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mvc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            
            context.MapRoute(
                name: "Mvc_Results",
                url: "Mvc/Results",
                defaults: new { action = "Index", controller = "Event" },
                namespaces: new[] { "JinnSports.WEB.Areas.Mvc.Controllers" });

            context.MapRoute(
                name: "Mvc_Teams",
                url: "Mvc/Teams",
                defaults: new { controller = "Team", action = "Index" },
                namespaces: new[] { "JinnSports.WEB.Areas.Mvc.Controllers" });

            context.MapRoute(
                name: "Mvc_TeamDetails",
                url: "Mvc/TeamDetails",
                defaults: new { controller = "Team", action = "Details" },
                namespaces: new[] { "JinnSports.WEB.Areas.Mvc.Controllers" });
            
            context.MapRoute(
                "Mvc_Default",
                "Mvc/{controller}/{action}",
                new { controller = "Home", action = "Index" },
                namespaces: new[] { "JinnSports.WEB.Areas.Mvc.Controllers" });

            context.MapRoute(
               "Mvc_Select",
               "Mvc/{controller}/{action}/{id}",
               new { action = "SportTypeSelect", id = UrlParameter.Optional });
        }
    }
}