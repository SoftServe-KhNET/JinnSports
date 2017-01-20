using JinnSports.BLL.Dtos;
using JinnSports.BLL.Dtos.SportType;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc.Controllers
{
    public class EventController : Controller
    {
        private readonly ISportTypeService sportTypeService;

        public EventController(ISportTypeService sportTypeService)
        {
            this.sportTypeService = sportTypeService;
        }

        // GET: Mvc/Event
        public ActionResult Index(int id = 0, int time = 0)
        {
            int recordsTotal = this.sportTypeService.Count(id);

            SportTypeSelectDto sportTypeModel = this.sportTypeService.GetSportTypes(id, time);

            if (sportTypeModel != null)
            {
                return this.View(sportTypeModel);
            }
            else
            {
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult PostIndex()
        {
            string requestedId = Request["sportTypeSelector"];
            int sportTypeId = !string.IsNullOrEmpty(requestedId) ? Convert.ToInt32(requestedId) : 0;

            string requestedTime = Request["timeSelector"];
            int timeId = !string.IsNullOrEmpty(requestedTime) ? Convert.ToInt32(requestedTime) : 1;

            return this.RedirectToAction("Index", new { id = sportTypeId, time = timeId - 1 });
        }
    }
}