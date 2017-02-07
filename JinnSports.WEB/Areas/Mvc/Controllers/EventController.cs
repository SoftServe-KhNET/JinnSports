using JinnSports.BLL.Dtos;
using JinnSports.BLL.Dtos.SportType;
using JinnSports.BLL.Filters;
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
        private readonly IEventService eventsService;

        public EventController(IEventService eventsService)
        {
            this.eventsService = eventsService;
        }

        private SportEventViewModel GetSportEvents(SportEventFilter filter)
        {
            IEnumerable<SportTypeDto> sportTypes = this.eventsService.GetSportTypes();
            int recordsTotal = this.eventsService.Count(filter);
            IEnumerable<ResultDto> results = this.eventsService.GetSportEvents(filter);
            PageInfo pageInfo = new PageInfo(recordsTotal, filter.Page, filter.PageSize);

            SportEventViewModel viewModel = new SportEventViewModel()
            {
                PageInfo = pageInfo,
                Filter = filter,
                Results = results,
                SportTypes = sportTypes
            };
            return viewModel;
        }

        // GET: Mvc/Event
        public ActionResult Index(SportEventFilter filter)
        {
            SportEventViewModel viewModel = this.GetSportEvents(filter);
            viewModel.ActionName = "Index";
            viewModel.ControllerName = "Event";
            return this.View(viewModel);
        }

        /*
        [HttpPost]
        public ActionResult PostIndex(string sportTypeSelector, string timeSelector)
        {
            int sportTypeId = 
                !string.IsNullOrEmpty(sportTypeSelector) ? Convert.ToInt32(sportTypeSelector) : 0;
            
            int timeId = 
                !string.IsNullOrEmpty(timeSelector) ? Convert.ToInt32(timeSelector) : 1;

            int recordsTotal = this.eventsService.Count(sportTypeId, timeId - 1);

            return this.RedirectToAction(
                "Index", new { id = sportTypeId, page = 1, time = timeId - 1 });
        }*/
    }
}