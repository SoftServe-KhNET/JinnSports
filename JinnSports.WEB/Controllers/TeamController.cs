using System.Collections.Generic;
using System.Web.Mvc;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using System.Linq;
using JinnSports.BLL.Dtos;
using System;

namespace JinnSports.WEB.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;

        public TeamController()
        {
            this.teamService = new TeamService();
        }

        // GET: Team
        public ActionResult Index()
        {
            return this.View();
        }

        // GET: Team
        public ActionResult Details(int id)
        {            
            return this.View();
        }

        [HttpPost]
        public ActionResult LoadTeams()
        {
            string draw = this.Request.Form.GetValues("draw").FirstOrDefault();
            string start = this.Request.Form.GetValues("start").FirstOrDefault();
            string length = this.Request.Form.GetValues("length").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = this.teamService.Count();

            List<TeamDto> teams = this.teamService.GetAllTeams()
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            foreach (TeamDto team in teams)
            {
                team.Results = null;
            }

            return this.Json(
                new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = teams
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}
