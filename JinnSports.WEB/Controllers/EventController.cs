using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JinnSports.WEB.Controllers
{
    public class EventController : Controller
    {
        private const string FOOTBALL = "Football";

        private readonly IEventService eventService;

        public EventController()
        {
            this.eventService = new EventsService();
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult LoadEvents()
        {
            string draw = this.Request.Form.GetValues("draw").FirstOrDefault();
            string start = this.Request.Form.GetValues("start").FirstOrDefault();
            string length = this.Request.Form.GetValues("length").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = this.eventService.Count(FOOTBALL);

            IEnumerable<SportEventDto> events = this.eventService
                .GetSportEvents(FOOTBALL, skip, pageSize);

            return this.Json(
                new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = events
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}