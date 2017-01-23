using System.Web.Mvc;
using System.Web.UI;
using JinnSports.BLL.Interfaces;
using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using System.Linq;

namespace JinnSports.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService newsService;

        private IEventService eventService;

        public HomeController(INewsService newsService, IEventService eventService)
        {
            this.newsService = newsService;
            this.eventService = eventService;
        }

        //[OutputCache(Duration = 600, Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            return this.View(this.eventService.GetMainPageInfo());
        }
    }
}