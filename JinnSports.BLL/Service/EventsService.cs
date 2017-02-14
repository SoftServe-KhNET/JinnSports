using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Matcher;
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
        
        private readonly PredictoionSender predictionSender;

        public EventsService(IUnitOfWork unitOfWork)
        {
            this.dataUnit = unitOfWork;
            this.predictionSender = new PredictoionSender(this.dataUnit);
        }

        public int Count(int sportTypeId, int time)
        {
            int count;
            if (sportTypeId != 0)
            {
                IEnumerable<SportEvent> sportEvents = this.dataUnit.GetRepository<SportEvent>().Get(filter: m => m.SportType.Id == sportTypeId);
                if (time != 0)
                {
                    count = sportEvents.Count(m => DateTime.Compare(m.Date, DateTime.UtcNow) == time);
                }
                else
                {
                    count = sportEvents.Count();
                }
            }
            else
            {
                IEnumerable<SportEvent> sportEvents = this.dataUnit.GetRepository<SportEvent>().Get();
                if (time != 0)
                {

                    count = sportEvents.Count(m => DateTime.Compare(m.Date, DateTime.UtcNow) == time);
                }
                else
                {
                    count = sportEvents.Count();
                }
            }
            return count;
        }

        public IEnumerable<ResultDto> GetSportEvents(int sportTypeId, int time, int skip, int take)
        {
            IList<ResultDto> results = new List<ResultDto>();

            IEnumerable<SportEvent> sportEvents;
            if (sportTypeId != 0)
            {
                if (time != 0)
                {
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => m.SportType.Id == sportTypeId && DateTime.Compare(m.Date, DateTime.UtcNow) == time,
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => time == 1 ? s.OrderBy(x => x.Date).ThenByDescending(x => x.Id) : s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip,
                        take: take);
                }
                else
                {
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => m.SportType.Id == sportTypeId,
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
                        skip: skip,
                        take: take);
                }
            }
            else
            {
                if (time != 0)
                {
                    sportEvents =
                        this.dataUnit.GetRepository<SportEvent>().Get(
                        filter: m => DateTime.Compare(m.Date, DateTime.UtcNow) == time,
                        includeProperties: "Results,SportType,Results.Team",
                        orderBy: s => time == 1 ? s.OrderBy(x => x.Date).ThenByDescending(x => x.Id) : s.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id),
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

        public MainPageDto GetMainPageInfo()
        {
            INewsService newsService = new NewsService();
            var news = newsService.GetLastNews();

            IEnumerable<ResultDto> upcomingEvents = this.GetSportEvents(0, 1, 0, 0);

            return new MainPageDto() { News = news, UpcomingEvents = upcomingEvents };
        }

        public bool SaveSportEvents(ICollection<SportEventDTO> eventDTOs)
        {
            Log.Info("Writing transferred data...");
            try
            {
                NamingMatcher matcher = new NamingMatcher(this.dataUnit);

                IEnumerable<SportType> sportTypes = this.dataUnit.GetRepository<SportType>().Get();

                foreach (SportEventDTO eventDTO in eventDTOs)
                {
                    SportType sportType = sportTypes.FirstOrDefault(st => st.Name == eventDTO.SportType)
                                            ?? new SportType { Name = eventDTO.SportType };

                    SportEvent sportEvent = new SportEvent
                    { SportType = sportType, Date = this.ConvertAndTrimDate(eventDTO.Date), Results = new List<Result>() };
                    TempSportEvent tempEvent = new TempSportEvent()
                    { SportType = sportType, Date = this.ConvertAndTrimDate(eventDTO.Date), TempResults = new List<TempResult>() };

                    foreach (ResultDTO resultDTO in eventDTO.Results)
                    {
                        Team team = new Team
                        {
                            Name = resultDTO.TeamName,
                            SportType = sportType,
                            Names = new List<TeamName> { new TeamName { Name = resultDTO.TeamName } }
                        };

                        List<Conformity> conformities = matcher.ResolveNaming(team);

                        if (conformities == null)
                        {
                            team = this.dataUnit.GetRepository<TeamName>()
                            .Get((x) => x.Name == team.Name).Select(x => x.Team).FirstOrDefault();
                            
                            Result result = new Result { Team = team, Score = resultDTO.Score ?? -1, IsHome = resultDTO.IsHome };
                            sportEvent.Results.Add(result);
                        }
                        else
                        {
                            TempResult result = new TempResult
                            {
                                Score = resultDTO.Score ?? -1,
                                Conformities = new List<Conformity>(),
                                IsHome = resultDTO.IsHome
                            };

                            if (team.Names.FirstOrDefault().Id != 0)
                            {
                                result.Team = team;
                            }

                            foreach (Conformity conformity in conformities)
                            {
                                result.Conformities.Add(conformity);
                            }
                            conformities.Clear();
                            tempEvent.TempResults.Add(result);
                        }
                    }

                    this.Save(tempEvent, sportEvent);
                }
                this.dataUnit.SaveChanges();

                this.predictionSender.SendPredictionRequest();
            }
            catch (Exception ex)
            {
                Log.Error("Exception when trying to save transferred data to DB", ex);
                return false;
            }
            Log.Info("Transferred data sucessfully saved");
            return true;
        }

        public void RunPredictions()
        {
            this.predictionSender.SendPredictionRequest();
        }

        private void Save(TempSportEvent tempEvent, SportEvent sportEvent)
        {
            if (tempEvent.TempResults.Count() != 0)
            {
                foreach (Result result in sportEvent.Results)
                {
                    TempResult tempRes = new TempResult { Team = result.Team, Score = result.Score, IsHome = result.IsHome };
                    tempEvent.TempResults.Add(tempRes);
                }
                this.dataUnit.GetRepository<TempSportEvent>().Insert(tempEvent);
            }
            else
            {
                IEnumerable<SportEvent> existingEvent = this.dataUnit.GetRepository<SportEvent>().Get();
                if (!existingEvent.Contains(sportEvent))
                {
                    this.dataUnit.GetRepository<SportEvent>().Insert(sportEvent);
                }
            }
        }

        private DateTime ConvertAndTrimDate(long dateTicks)
        {
            DateTime temp = new DateTime(dateTicks);
            return new DateTime(temp.Year, temp.Month, temp.Day, temp.Hour, temp.Minute, temp.Second);
        }
    }
}
