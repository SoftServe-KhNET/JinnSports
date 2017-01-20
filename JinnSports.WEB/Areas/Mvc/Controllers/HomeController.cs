using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc.Controllers
{
    public class HomeController : Controller
    {
        // GET: Mvc/Home
        public ActionResult Index()
        {
            return this.View();
        }
    }
}