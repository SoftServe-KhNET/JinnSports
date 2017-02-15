using JinnSports.BLL;
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
        public ActionResult Index(int page = 1, int id = 0, TimeSelector timeSelector = TimeSelector.All)
        {
            int recordsTotal = this.sportTypeService.Count(id, timeSelector);

            if (page < 1)
            {
                page = 1;
            }

            PageInfo pageInfo = new PageInfo(recordsTotal, page, PAGESIZE);
            SportTypeSelectDto sportTypeModel = this.sportTypeService.GetSportTypes(
                id,
                timeSelector,
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
    }
}