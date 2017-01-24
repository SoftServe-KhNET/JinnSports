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
                url: "Mvc/Results/{id}",
                defaults: new { action = "Index", controller = "Event", id = UrlParameter.Optional },
                namespaces: new[] { "JinnSports.WEB.Areas.Mvc.Controllers" });

            context.MapRoute(
                name: "Mvc_Teams",
                url: "Mvc/Teams",
                defaults: new { controller = "Team", action = "Index" },
                namespaces: new[] { "JinnSports.WEB.Areas.Mvc.Controllers" });

            context.MapRoute(
                name: "Mvc_TeamDetails",
                url: "Mvc/TeamDetails/{id}",
                defaults: new { controller = "Team", action = "Details", id = UrlParameter.Optional },
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