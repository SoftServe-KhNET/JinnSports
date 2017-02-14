using System.Web.Mvc;

namespace JinnSports.WEB.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return this.View();
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return this.View();
        }

        public ActionResult InternalServerError()
        {
            Response.StatusCode = 500;
            return this.View();
        }
    }
}