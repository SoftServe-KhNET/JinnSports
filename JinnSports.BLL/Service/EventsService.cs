using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Matcher;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Linq;
using AutoMapper;
using DTO.JSON;
using System;
using log4net;
using JinnSports.Entities.Entities.Temp;
using JinnSports.BLL.Dtos.SportType;

namespace JinnSports.BLL.Service
{
    public class EventsService : IEventService
    {
        private const string SPORTCONTEXT = "SportsContext";

        private static readonly ILog Log = LogManager.GetLogger(typeof(EventsService));

        private readonly IUnitOfWork dataUnit;

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
                .Count(m => m.SportType.Id == sportTypeId);
            }
            else
            {
                count = this.dataUnit.GetRepository<SportEvent>()
                .Count();
            }
            return count;
        }

        public IEnumerable<ResultDto> GetSportEvents(int sportTypeId, int time, int skip, int take)
        {
            IList<ResultDto> results = new List<ResultDto>();

            IEnumerable<SportEvent> sportEvents;
            if (sportTypeId != 0)
            {
                sportEvents =
                    this.dataUnit.GetRepository<SportEvent>().Get(
                    filter: m => m.SportType.Id == sportTypeId,
                    includeProperties: "Results,SportType,Results.Team",
                    orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                    skip: skip,
                    take: take);
            }
            else
            {
                sportEvents =
                    this.dataUnit.GetRepository<SportEvent>().Get(
                    includeProperties: "Results,SportType,Results.Team",
                    orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                    skip: skip,
                    take: take);
            }
            if (time != 0)
            {
                sportEvents = sportEvents.Where(m => Math.Sign(DateTime.Compare(m.Date, DateTime.Now)) == time).Select(m => m);
            }
            foreach (SportEvent sportEvent in sportEvents)
            {
                results.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
            }
            return results;
        }

        public IEnumerable<SportTypeDto> GetSportTypes()
        {
            IList<SportTypeDto> sportTypeDto = new List<SportTypeDto>();

            IEnumerable<SportType> sportTypes =
                this.dataUnit.GetRepository<SportType>().Get();

            foreach (SportType sportType in sportTypes)
            {
                sportTypeDto.Add(Mapper.Map<SportType, SportTypeDto>(sportType));
            }
            return sportTypeDto;
        }

        public bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs)
        {
            Log.Info("Writing transferred data...");
            using (this.dataUnit)
            {
                try
                {
                    NamingMatcher matcher = new NamingMatcher(this.dataUnit);

                    IEnumerable<SportType> sportTypes = this.dataUnit.GetRepository<SportType>().Get();
                    IEnumerable<SportEvent> existingEvents = this.dataUnit.GetRepository<SportEvent>().Get();

                    foreach (SportEventDTO eventDTO in eventDTOs)
                    {
                        SportType sportType = sportTypes.FirstOrDefault(st => st.Name == eventDTO.SportType)
                                                ?? new SportType { Name = eventDTO.SportType };

                        bool conflictExist = false;
                        List<Result> results = new List<Result>();
                        List<TempResult> tempResults = new List<TempResult>();

                        foreach (ResultDTO resultDTO in eventDTO.Results)
                        {
                            Team team = new Team { Name = resultDTO.TeamName, SportType = sportType };
                            List<Conformity> conformities = matcher.ResolveNaming(team);

                            if (conformities == null)
                            {
                                team = this.dataUnit.GetRepository<Team>().Get((x) => x.Names.Contains(new TeamName { Name = team.Name })).FirstOrDefault();
                                Result result = new Result { Team = team, Score = resultDTO.Score ?? -1, IsHome = resultDTO.IsHome };
                                results.Add(result);
                            }
                            else
                            {
                                conflictExist = true;

                                TempResult result = new TempResult { Team = team, Score = resultDTO.Score };

                                foreach (Conformity conformity in conformities)
                                {
                                    result.Conformities.Add(conformity);
                                }
                                conformities.Clear();
                                tempResults.Add(result);
                            }
                        }

                        if (conflictExist)
                        {
                            TempSportEvent tempSportEvent = new TempSportEvent { SportType = sportType, Date = this.ConvertAndTrimDate(eventDTO.Date) };
                            foreach (TempResult tempResult in tempResults)
                            {
                                tempSportEvent.TempResults.Add(tempResult);
                            }
                            foreach (Result result in results)
                            {
                                TempResult tempRes = new TempResult { Team = result.Team, Score = result.Score, IsHome = result.IsHome };
                                tempSportEvent.TempResults.Add(tempRes);
                            }
                            this.dataUnit.GetRepository<TempSportEvent>().Insert(tempSportEvent);
                        }
                        else
                        {
                            SportEvent sportEvent = new SportEvent { SportType = sportType, Date = this.ConvertAndTrimDate(eventDTO.Date) };
                            foreach (Result result in results)
                            {
                                sportEvent.Results.Add(result);
                            }
                            if (!existingEvents.Contains(sportEvent))
                            {
                                this.dataUnit.GetRepository<SportEvent>().Insert(sportEvent);
                            }
                        }
                    }
                    this.dataUnit.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.Error("Exception when trying to save transferred data to DB", ex);
                    return false;
                }
            }
            Log.Info("Transferred data sucessfully saved");
            return true;
        }

        private DateTime ConvertAndTrimDate(long dateTicks)
        {
            DateTime temp = new DateTime(dateTicks);
            return new DateTime(temp.Year, temp.Month, temp.Day);
        }
    }
}
