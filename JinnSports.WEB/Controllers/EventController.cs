using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.DAL.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JinnSports.WEB.Controllers
{
    public class EventController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}