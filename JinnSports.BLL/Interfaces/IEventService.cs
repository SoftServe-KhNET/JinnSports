using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService : IService
    {
        IList<CompetitionEventDto> GetCEvents();
    }
}
