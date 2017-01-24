using JinnSports.BLL.Dtos;
using JinnSports.BLL.Dtos.SportType;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.WEB.Areas.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc.Controllers
{
    public class EventController : Controller
    {
        private const int PAGESIZE = 10;

        private readonly ISportTypeService sportTypeService;

        public EventController(ISportTypeService sportTypeService)
        {
            this.sportTypeService = sportTypeService;
        }

        // GET: Mvc/Event
        public ActionResult Index(int page = 1, int id = 0, int time = 0)
        {
            int recordsTotal = this.sportTypeService.Count(id, time);

            if (page < 1)
            {
                page = 1;
            }

            PageInfo pageInfo = new PageInfo(recordsTotal, page, PAGESIZE);
            SportTypeSelectDto sportTypeModel = this.sportTypeService.GetSportTypes(
                id, 
                time,
                (page - 1) * PAGESIZE, 
                PAGESIZE);


            if (sportTypeModel != null)
            {
                return this.View(new SportEventViewModel()
                {
                     PageInfo = pageInfo,
                     SportTypeSelectDto = sportTypeModel,
                     ActionName = "Index",
                     ControllerName = "Event"
                });
            }
            else
            {
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult PostIndex(string sportTypeSelector, string timeSelector)
        {
            int sportTypeId = 
                !string.IsNullOrEmpty(sportTypeSelector) ? Convert.ToInt32(sportTypeSelector) : 0;
            
            int timeId = 
                !string.IsNullOrEmpty(timeSelector) ? Convert.ToInt32(timeSelector) : 1;

            int recordsTotal = this.sportTypeService.Count(sportTypeId, timeId - 1);

            return this.RedirectToAction(
                "Index", new { id = sportTypeId, page = 1, time = timeId - 1 });
        }
    }
}