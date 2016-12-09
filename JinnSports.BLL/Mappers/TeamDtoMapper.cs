using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;

namespace JinnSports.BLL.Mappers
{
    internal static class TeamDtoMapper
    {
        public static TeamDto MapToTeamDto(this Team team)
        {
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name
            };
        }
    }
}
