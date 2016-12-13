using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamService
    {
        IEnumerable<TeamDto> GetAllTeams();
        TeamDto GetTeamById(int id);
    }
}
