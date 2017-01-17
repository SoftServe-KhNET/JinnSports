
using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamDetailsService
    {
        int Count(int teamId);
        IEnumerable<ResultDto> GetResults(int teamId, int skip, int take);
    }
}
