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
        private IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public ActionResult Index()
        {
            IEnumerable<SportTypeDto> sportTypes = this.eventService.GetSportTypes();
            if (sportTypes != null)
            {
                return this.View(sportTypes);
            }
            else
            {
                return this.HttpNotFound();
            }
        }

    }
}