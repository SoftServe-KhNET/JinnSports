
using System.Collections.Generic;
using JinnSports.BLL.Dtos;


namespace JinnSports.BLL.Interfaces
{
    public interface ITeamDetailsService
    {
        int Count(int? teamId);
        IEnumerable<ResultDto> GetResults(int teamId);
    }
}
