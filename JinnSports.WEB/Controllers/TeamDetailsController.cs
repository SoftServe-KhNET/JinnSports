using System.Web.Mvc;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Dtos;

namespace JinnSports.WEB.Controllers
{
    public class TeamDetailsController : Controller
    {
        private readonly ITeamService teamService;

        public TeamDetailsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public ActionResult Details(int id = 0)
        {
            TeamDto team = this.teamService.GetTeamById(id);
            if (team != null)
            {
                return this.View(team);
            }
            else
            {
                return this.HttpNotFound();
            }
        }
    }
}