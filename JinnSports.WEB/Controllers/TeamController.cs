using System.Collections.Generic;
using System.Web.Mvc;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.WEB.Mappers;
using JinnSports.WEB.Models;

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
            IEnumerable<TeamDto> teams = this.teamService.GetAllTeams();

            List<TeamViewModel> teamViewModels = new List<TeamViewModel>();
            foreach (var team in teams)
            {
                teamViewModels.Add(team.MapToTeamViewModel());
            }

            return this.View(teamViewModels);
        }

        // GET: Team
        public ActionResult Details(int id)
        {
            TeamDetailsDto team = this.teamService.GetTeamDetailsById(id);

            TeamDetailsViewModel teamDetailsViewModel = new TeamDetailsViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Results = new List<ResultDetailsViewModel>()
            };

            foreach (var teamResult in team.Results)
            {
                teamDetailsViewModel.Results.Add(teamResult.MapToResultDetailsViewModel());
            }

            return this.View(teamDetailsViewModel);
        }
    }
}
