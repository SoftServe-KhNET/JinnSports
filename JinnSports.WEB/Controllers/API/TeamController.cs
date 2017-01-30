using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace JinnSports.WEB.ApiControllers
{
    public class TeamController : ApiController
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        // GET: api/Team/5
        [HttpGet]
        public IHttpActionResult LoadTeams()
        {
            string draw = this.Request.GetQueryNameValuePairs().Where(x => x.Key == "draw").FirstOrDefault().Value;
            string start = this.Request.GetQueryNameValuePairs().Where(x => x.Key == "start").FirstOrDefault().Value;
            string length = this.Request.GetQueryNameValuePairs().Where(x => x.Key == "length").FirstOrDefault().Value;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = this.teamService.Count();

            IEnumerable<TeamDto> teams = this.teamService.GetTeams(skip, pageSize);

            return this.Ok(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = teams
            });
        }

        // POST: api/Team
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Team/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Team/5
        public void Delete(int id)
        {
        }
    }
}
