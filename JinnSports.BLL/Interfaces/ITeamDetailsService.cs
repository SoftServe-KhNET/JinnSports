
using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;
using JinnSports.BLL.Filters;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamDetailsService
    {
        int Count(TeamDetailsFilter filter);
        IEnumerable<ResultDto> GetResults(TeamDetailsFilter filter);
    }
}
