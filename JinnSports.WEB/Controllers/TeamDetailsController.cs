using System.Web.Mvc;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Dtos;

namespace JinnSports.WEB.Controllers
{
    public class TeamDetailsController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IChartService chartService;

        public TeamDetailsController(ITeamService teamService, IChartService chartService)
        {
            this.teamService = teamService;
            this.chartService = chartService;
        }

        public ActionResult Details(int id = 0)
        {
            TeamDto team = this.teamService.GetTeamById(id);

            if (team != null)
            {
                var data = this.chartService.GetDataTableForTeam(team.Id);

                return this.View(new TeamDetailsDto
                {
                    TeamDto = team,
                    WinRateDataTable = data
                });
            }

            return this.HttpNotFound();
        }
    }
}