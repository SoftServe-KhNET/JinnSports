using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamService : IService
    {
        IEnumerable<TeamDto> GetAllTeams();
        TeamDetailsDto GetTeamDetailsById(int id);
    }
}
