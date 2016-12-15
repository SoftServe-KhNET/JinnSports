using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Linq;

namespace JinnSports.BLL.Service
{
    public class EventsService : IEventService
    {
        private const string SPORTCONTEXT = "SportsContext";

        private IUnitOfWork dataUnit;

        public int Count()
        {
            int count;
            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                count = this.dataUnit.GetRepository<SportEvent>().Count();
            }
            return count;
        }

        public IDictionary<string, List<SportEventDto>> GetSportEvents()
        {
            IDictionary<string, List<SportEventDto>> orderedEvents = new Dictionary<string, List<SportEventDto>>();
            
            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                IEnumerable<SportEvent> sportEvents = this.dataUnit.GetRepository<SportEvent>().Get();
                IEnumerable<SportType> sportTypes = this.dataUnit.GetRepository<SportType>().Get();

                foreach (SportType sportType in sportTypes)
                {
                    orderedEvents.Add(sportType.Name, new List<SportEventDto>());
                }

                foreach (SportEvent sportEvent in sportEvents)
                {
                    SportEventDto eventDto = new SportEventDto { Date = sportEvent.Date };

                    foreach (Result result in sportEvent.Results)
                    {
                        eventDto.Results.Add(result.Team.Name, result.Score);
                    }

                    orderedEvents[sportEvent.SportType.Name].Add(eventDto);        
                }
            }

            return orderedEvents;
        }

        public void SortEventsByDate(IDictionary<string, List<SportEventDto>> orderedEvents)
        {
            ICollection<List<SportEventDto>> eventsLists = orderedEvents.Values;
            foreach (List<SportEventDto> eventsList in eventsLists)
            {
                eventsList.OrderBy(e => e.Date);
            }
        }
    }
}
