using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamService
    {
        int Count();

        IEnumerable<TeamDto> GetAllTeams(int skip, int take);

        TeamDto GetTeamById(int teamId);
    }
}
