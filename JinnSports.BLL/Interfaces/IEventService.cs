using System.Collections.Generic;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        int Count();

        IDictionary<string, List<SportEventDto>> GetSportEvents();

        void SortEventsByDate(IDictionary<string, List<SportEventDto>> orderedEvents);
    }
}
