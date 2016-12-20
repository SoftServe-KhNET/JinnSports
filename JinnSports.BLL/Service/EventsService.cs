using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Linq;
using AutoMapper;

namespace JinnSports.BLL.Service
{
    public class EventsService : IEventService
    {
        private const string SPORTCONTEXT = "SportsContext";

        private IUnitOfWork dataUnit;

        public int Count(string sport)
        {
            int count;
            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                count = this.dataUnit.GetRepository<SportEvent>()
                    .Get(filter: m => m.SportType.Name == sport)
                    .Count();
            }
            return count;
        }

        public IEnumerable<SportEventDto> GetSportEvents(string sport, int skip, int take)
        {
            IList<SportEventDto> results = new List<SportEventDto>();

            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                // Выбираем результаты заданного вида спорта, сортируя по дате
                IEnumerable<SportEvent> sportEvents =
                    this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => m.SportType.Name == sport,
                        orderBy: s => s.OrderByDescending(x => x.Date),
                        skip: skip,
                        take: take);

                // Формирование SportEventDto из SportEvent при помощи AutoMapper
                foreach (SportEvent sportEvent in sportEvents)
                {
                    results.Add(Mapper.Map<SportEventDto>(sportEvent));
                }
            }

            return results;
        }

        /*
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
                    orderedEvents[sportEvent.SportType.Name].Add(Mapper.Map<SportEvent, SportEventDto>(sportEvent));
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
        */
    }
}
