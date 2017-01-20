using System.Web.Mvc;
using System.Web.UI;
using JinnSports.BLL.Interfaces;

namespace JinnSports.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService newsService;

        public HomeController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            var news = this.newsService.GetLastNews();

            return this.View(news);
        }
    }
}