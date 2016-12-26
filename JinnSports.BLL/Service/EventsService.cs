using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.DAL.Repositories;
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
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventsService));

        private const string SPORTCONTEXT = "SportsContext";

        private IUnitOfWork dataUnit;

        public int Count(int sportId)
        {
            int count;
            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                count = this.dataUnit.GetRepository<SportEvent>()
                    .Get(filter: m => m.SportType.Id == sportId)
                    .Count();
            }
            return count;
        }

        public IEnumerable<ResultDto> GetSportEvents(int sportId, int skip, int take)
        {
            IList<ResultDto> results = new List<ResultDto>();

            using (this.dataUnit = new EFUnitOfWork(SPORTCONTEXT))
            {
                // Выбираем результаты заданного вида спорта, сортируя по дате
                IEnumerable<SportEvent> sportEvents =
                    this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => m.SportType.Id == sportId,
                        orderBy: s => s.OrderByDescending(x => x.Date),
                        skip: skip,
                        take: take);

                // Формирование SportEventDto из SportEvent при помощи AutoMapper
                foreach (SportEvent sportEvent in sportEvents)
                {
                    results.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
                }
            }

            return results;
        }

        public bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs)
        {
            try
            {
                dataUnit = new EFUnitOfWork(SPORTCONTEXT);

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
