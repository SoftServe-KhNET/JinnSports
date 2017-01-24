using JinnSports.BLL.Service;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ParserController : Controller
    {
        // GET: Admin/Parser
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult RunParser()
        {
            ParserService.Run();
            return this.View("Index");
        }

        [HttpPost]
        public ActionResult UpdateProxy()
        {
            ParserService.UpdateProxy();
            return this.View("Index");
        }
    }
}