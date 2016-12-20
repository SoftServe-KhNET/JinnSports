using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.BLL.Dtos;

namespace JinnSports.WEB.Controllers
{
    public class TeamDetailsController : Controller
    {
        private readonly ITeamDetailsService teamDetailsService;

        public TeamDetailsController()
        {
            this.teamDetailsService = new TeamDetailsService();
        }

        // GET: TeamDetails
        public ActionResult Index()
        {
            return this.View();
        }
        // GET: TeamDetails
        public ActionResult Details()
        {
            string url = string.Format("/TeamDetails/LoadResults?id={0}", this.Request.QueryString["id"]);
            return this.View((object)url);
        }

        [HttpPost]
        public ActionResult LoadResults(int? id = 6)
        {
            string draw = this.Request.Form.GetValues("draw").FirstOrDefault();
            string start = this.Request.Form.GetValues("start").FirstOrDefault();
            string length = this.Request.Form.GetValues("length").FirstOrDefault();
            //string id = this.Request.Form.GetValues("id").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int teamId = id != null ? Convert.ToInt32(id) : 0;
            int recordsTotal = this.teamDetailsService.Count(id);

            List<ResultDto> results = this.teamDetailsService.GetResults(teamId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

           /* foreach (ResultDto result in results)
            {
                re.Results = null;
            }*/

            return this.Json(
                new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = results
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}