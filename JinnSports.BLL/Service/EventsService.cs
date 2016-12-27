using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Linq;
using AutoMapper;
using DTO.JSON;
using System;
using log4net;

namespace JinnSports.BLL.Service
{
    public class EventsService : IEventService
    {
        private readonly IUnitOfWork dataUnit;

        private static readonly ILog Log = LogManager.GetLogger(typeof(EventsService));

        public EventsService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
        }

        public int Count(int sportTypeId)
        {
            int count;
                if (sportTypeId != 0)
                {
                    count = this.dataUnit.GetRepository<SportEvent>()
                    .Get(filter: m => m.SportType.Id == sportTypeId)
                    .Count();
                }
                else
                {
                    count = this.dataUnit.GetRepository<SportEvent>()
                    .Get().Count();
                }
            return count;
        }

        public IEnumerable<ResultDto> GetSportEvents(int sportTypeId, int skip, int take)
        {
            IList<ResultDto> results = new List<ResultDto>();

                IEnumerable<SportEvent> sportEvents;
                if (sportTypeId != 0)
                {
                    // Выбираем результаты заданного вида спорта, сортируя по дате
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => m.SportType.Id == sportTypeId,
                        orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip,
                        take: take);
                }
                else
                {
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip,
                        take: take);
                }
                // Формирование SportEventDto из SportEvent при помощи AutoMapper
                foreach (SportEvent sportEvent in sportEvents)
                {
                    results.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
                }
            return results;
        }

        public IEnumerable<SportTypeDto> GetSportTypes()
        {
            IList<SportTypeDto> sportTypeDto = new List<SportTypeDto>();
          
                // Выбираем результаты заданного вида спорта, сортируя по дате
                IEnumerable<SportType> sportTypes =
                    this.dataUnit.GetRepository<SportType>().Get();

                // Формирование SportEventDto из SportEvent при помощи AutoMapper
                foreach (SportType sportType in sportTypes)
                {
                    sportTypeDto.Add(Mapper.Map<SportType, SportTypeDto>(sportType));
                }
            return sportTypeDto;
        }

        public bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs)
        {
            try
            {
                IEnumerable<Team> teams = dataUnit.GetRepository<Team>().Get();
                IEnumerable<SportType> sportTypes = dataUnit.GetRepository<SportType>().Get();

                foreach (SportEventDTO eventDTO in eventDTOs)
                {
                    SportType sportType = sportTypes.FirstOrDefault(st => st.Name == eventDTO.SportType) 
                                            ?? new SportType() { Name = eventDTO.SportType };

                    SportEvent sportEvent = new SportEvent() { SportType = sportType, Date = new DateTime(eventDTO.Date) };

                    foreach (ResultDTO resultDTO in eventDTO.Results)
                    {
                        Team team = teams.FirstOrDefault(t => t.Name == resultDTO.TeamName) 
                                        ?? new Team() { Name = resultDTO.TeamName, SportType = sportType };

                        AddResult(sportEvent, team, resultDTO.Score);
                    }
                    dataUnit.GetRepository<SportEvent>().Insert(sportEvent);
                }
                dataUnit.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error("Exception when trying to save SportEvents to DB", ex);
                return false;
            }
            finally
            {
                if (dataUnit != null)
                {
                    dataUnit.Dispose();
                }
            }
            return true;
        }

        private void AddResult(SportEvent sportEvent, Team team, int? score)
        {
            // TODO change nullable convertion for incoming events
            Result res = new Result() { SportEvent = sportEvent, Team = team, Score = score ?? -1 };
            if (sportEvent.Results == null)
            {
                sportEvent.Results = new List<Result>();
            }
            sportEvent.Results.Add(res);
        }

    }
}
