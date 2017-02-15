using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Jinn.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Jinn/Teams
        public ActionResult Index()
        {
            return View();
        }
    }
}