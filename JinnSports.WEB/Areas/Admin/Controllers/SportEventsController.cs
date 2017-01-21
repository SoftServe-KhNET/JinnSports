using System.Collections.Generic;
using System.Web.Http;
using DTO.JSON;
using JinnSports.BLL.Interfaces;
using System.Net.Http;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    public class SportEventsController : ApiController
    {
        private readonly IEventService eventService;
        
        public SportEventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public IHttpActionResult PostEvents(List<SportEventDTO> events)
        {
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
