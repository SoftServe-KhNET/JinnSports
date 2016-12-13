using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        IDictionary<string, List<SportEventDto>> GetSportEvents();
        void SortEventsByDate(IDictionary<string, List<SportEventDto>> orderedEvents);
    }
}
