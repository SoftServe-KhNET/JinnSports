using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using DTO.JSON;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    public class SportEventsController : ApiController
    {
        private IEventService eventService;

        public IHttpActionResult PostEvents(List<SportEventDTO> events)
        {
            this.eventService = new EventsService();

            if (this.eventService.SaveSportEvents(events))
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            } 
        }
    }
}
