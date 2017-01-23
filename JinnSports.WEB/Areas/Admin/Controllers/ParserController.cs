using JinnSports.BLL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    public class ParserController : Controller
    {
        // GET: Admin/Parser
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RunParser()
        {
            ParserService.Run();
            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateProxy()
        {
            ParserService.UpdateProxy();
            return View("Index");
        }
    }
}