using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JinnSports.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}