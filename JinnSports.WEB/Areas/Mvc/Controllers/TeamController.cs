using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;

        private readonly ITeamDetailsService teamDetailsService;

        public TeamController(ITeamService teamService, ITeamDetailsService teamDetailsService)
        {
            this.teamService = teamService;
            this.teamDetailsService = teamDetailsService;
        }

        // GET: Mvc/Team
        public ActionResult Index()
        {
            int recordsTotal = this.teamService.Count();
            IEnumerable<TeamDto> teams = this.teamService.GetAllTeams(0, recordsTotal);
            if (teams != null)
            {
                return this.View(teams);
            }
            else
            {
                return this.View();
            }
        }
        public ActionResult Details(int id)
        {
            int teamResultsCount = this.teamDetailsService.Count(id);

            TeamDto team = this.teamService.GetTeamById(id);
            IEnumerable<ResultDto> results = this.teamDetailsService.GetResults(id, 0, teamResultsCount);

            TeamResultsDto teamResults = new TeamResultsDto { Team = team, Results = results };

            if (teamResults != null)
            {
                return this.View(teamResults);
            }
            else
            {
                return this.View();
            }
        }
    }
}