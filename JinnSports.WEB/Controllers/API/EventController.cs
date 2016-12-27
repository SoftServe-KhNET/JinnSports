using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace JinnSports.WEB.ApiControllers
{
    public class EventController : ApiController
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public IHttpActionResult LoadEvents(int sportTypeId)
        {
            var a = sportTypeId;

            string draw = this.Request.GetQueryNameValuePairs().Where(x => x.Key == "draw").FirstOrDefault().Value;
            string start = this.Request.GetQueryNameValuePairs().Where(x => x.Key == "start").FirstOrDefault().Value;
            string length = this.Request.GetQueryNameValuePairs().Where(x => x.Key == "length").FirstOrDefault().Value;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = this.eventService.Count(sportTypeId);

            IEnumerable<ResultDto> results = this.eventService
                .GetSportEvents(sportTypeId, skip, pageSize);

            return this.Ok(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = results
            });
        }

        // POST: api/Event
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Event/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Event/5
        public void Delete(int id)
        {
        }
    }
}
