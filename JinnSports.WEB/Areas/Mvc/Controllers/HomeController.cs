using JinnSports.BLL.Interfaces;
using System.Web.Mvc;
using System.Web.UI;

namespace JinnSports.WEB.Areas.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService newsService;

        private readonly IEventService eventService;

        public HomeController(INewsService newsService, IEventService eventService)
        {
            this.newsService = newsService;
            this.eventService = eventService;
        }

        // GET: Mvc/Home
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            return this.View(this.eventService.GetMainPageInfo());
        }
    }
}