using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Filters;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamService
    {
        int Count(TeamFilter filter);

        IEnumerable<TeamDto> GetTeams(TeamFilter filter);

        TeamDto GetTeamById(int teamId);
    }
}
