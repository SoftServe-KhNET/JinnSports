using JinnSports.BLL.Dtos;
using JinnSports.BLL.Filters;
using JinnSports.BLL.Interfaces;
using JinnSports.WEB.Areas.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinnSports.WEB.Areas.Mvc.Controllers
{
    public class TeamController : Controller
    {
        private const int PAGESIZE = 10;

        private readonly ITeamService teamService;

        private readonly ITeamDetailsService teamDetailsService;

        public TeamController(ITeamService teamService, ITeamDetailsService teamDetailsService)
        {
            this.teamService = teamService;
            this.teamDetailsService = teamDetailsService;
        }

        // GET: Mvc/Team
        public ActionResult Index(TeamFilter filter)
        {
            int recordsTotal = this.teamService.Count(filter);            

            IEnumerable<TeamDto> teams = this.teamService.GetTeams(filter);

            PageInfo pageInfo = new PageInfo(recordsTotal, filter.Page, filter.PageSize);

            TeamViewModel teamViewModel = new TeamViewModel()
            {
                ActionName = "Index",
                ControllerName = "Team",
                PageInfo = pageInfo,
                TeamDtos = teams,
                Filter = filter
            };

            return this.View(teamViewModel);
        }

        [HttpPost]
        public ActionResult FilterTeam(TeamFilter filter)
        {
            filter.Page = 1;
            return RedirectToAction("Index", filter);
        }


        /*
        public ActionResult Details(int id, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            int teamResultsCount = this.teamDetailsService.Count(id);

            PageInfo pageInfo = new PageInfo(teamResultsCount, page, PAGESIZE);

            TeamDto team = this.teamService.GetTeamById(id);
            IEnumerable<ResultDto> results = 
                this.teamDetailsService.GetResults(id, (page - 1) * PAGESIZE, PAGESIZE);

            TeamResultsDto teamResults = new TeamResultsDto { Team = team, Results = results };

            if (teamResults != null)
            {
                return this.View(new TeamDetailsViewModel()
                {
                    ActionName = "Details",
                    ControllerName = "Team",
                    PageInfo = pageInfo,
                    TeamResultDto = teamResults
                });
            }
            else
            {
                return this.View();
            }
        }*/
    }
}