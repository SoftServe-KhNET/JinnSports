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
using JinnSports.BLL.Filters;
using System.ComponentModel;

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

        private void FilterEvents(ref IQueryable<SportEvent> events, SportEventFilter filter)
        {
            if (filter.SportTypeId > 0)
            {
                events = events.Where(e => e.SportType.Id == filter.SportTypeId);
            }

            if (filter.DateFrom.Ticks > 0)
            {
                events = events.Where(e => e.Date >= filter.DateFrom);
            }

            if (filter.DateTo.Ticks > 0)
            {
                events = events.Where(e => e.Date <= filter.DateTo);
            }

            if (!string.IsNullOrEmpty(filter.TeamName))
            {
                events = events.Where(e => e.Results.Any(
                    r => r.Team.Name == filter.TeamName ||
                        r.Team.Names.Any(n => n.Name == filter.TeamName)));
            }
        }

        private void OrderEvents(ref IQueryable<SportEvent> events, SportEventFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SortedField))
            {
                switch (filter.SortedField)
                {
                    case "FirstTeam":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            events = events.OrderBy(
                                e => e.Results.ElementAt(0).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(0).Id)
                                .ThenBy(e => e.Date);
                        }
                        else
                        {
                            events = events.OrderByDescending(
                                e => e.Results.ElementAt(0).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(0).Id)
                                .ThenBy(e => e.Date);
                        }
                        break;

                    case "SecondTeam":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            events = events.OrderBy(
                                e => e.Results.ElementAt(1).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(1).Id)
                                .ThenBy(e => e.Date);
                        }
                        else
                        {
                            events = events.OrderByDescending(
                                e => e.Results.ElementAt(1).Team.Name)
                                .ThenBy(e => e.Results.ElementAt(1).Id)
                                .ThenBy(e => e.Date);
                        }
                        break;

                    case "Date":
                        if (filter.SortDirection == ListSortDirection.Ascending)
                        {
                            events = events.OrderBy(e => e.Date).ThenBy(e => e.Id);
                        }
                        else
                        {
                            events = events.OrderByDescending(e => e.Date).ThenBy(e => e.Id);
                        }
                        break;

                    default:
                        events = events.OrderBy(e => e.Date).ThenBy(e => e.Id);
                        break;
                }
            }
            else
            {
                events = events.OrderBy(e => e.Date).ThenBy(e => e.Id);
            }
        }

        public int Count(SportEventFilter filter)
        {
            int count;
            IQueryable<SportEvent> events = this.dataUnit.GetRepository<SportEvent>().Get();

            if (filter != null)
            {
                this.FilterEvents(ref events, filter);
            }

            count = events.Count();

            return count;
        }

        public IEnumerable<ResultDto> GetSportEvents(SportEventFilter filter)
        {
            IList<ResultDto> results = new List<ResultDto>();
            IQueryable<SportEvent> sportEvents = 
                this.dataUnit.GetRepository<SportEvent>().Get(
                    includeProperties: "Results,SportType,Results.Team");

            if (filter != null)
            {
                this.FilterEvents(ref sportEvents, filter);
                this.OrderEvents(ref sportEvents, filter);                

                if (filter.Page < 1)
                {
                    filter.Page = 1;
                }
                if (filter.PageSize < 1)
                {
                    filter.PageSize = 10;
                }
                
                sportEvents = sportEvents.Skip((filter.Page - 1) * filter.PageSize);
                sportEvents = sportEvents.Take(filter.PageSize);
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

            SportEventFilter filter = new SportEventFilter()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = new DateTime()
            };

            IEnumerable<ResultDto> upcomingEvents = this.GetSportEvents(filter);

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

                // TODO: resolve injection
                this.predictionSender.SendPredictionRequest(); // Check new events and send request to Predictor
            }
            catch (Exception ex)
            {
                Log.Error("Exception when trying to save transferred data to DB", ex);
                return false;
            }
            Log.Info("Transferred data sucessfully saved");
            return true;
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
