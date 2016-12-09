using JinnSports.BLL.Dtos;
using JinnSports.WEB.Models;

namespace JinnSports.WEB.Mappers
{
    internal static class TeamMapper
    {
        public static TeamViewModel MapToTeamViewModel(this TeamDto team)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name
            };
        }
    }
}