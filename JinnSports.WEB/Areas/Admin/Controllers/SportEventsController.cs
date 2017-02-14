using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;
using JinnSports.BLL.Interfaces;
using DTO.JSON;
using System.Web.Configuration;

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
        [HttpPost]
        public IHttpActionResult CreateEvents(List<SportEventDTO> events)
        {
            IEnumerable<string> values = new List<string>();
            bool serviceCode = Request.Headers.TryGetValues(WebConfigurationManager.AppSettings["ApiKey"], out values);

            if (!serviceCode)
            {
                return this.BadRequest();
            }

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
