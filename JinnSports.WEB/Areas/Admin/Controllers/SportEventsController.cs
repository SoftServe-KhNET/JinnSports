using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;
using JinnSports.BLL.Interfaces;
using DTO.JSON;

namespace JinnSports.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class SportEventsController : ApiController
    {
        private readonly IEventService eventService;
        
        public SportEventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [AllowAnonymous]
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
