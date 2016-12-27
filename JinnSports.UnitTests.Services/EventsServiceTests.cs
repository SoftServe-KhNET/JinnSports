using AutoMapper;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Extentions;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using JinnSports.Entities.Entities;
using JinnSports.WEB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JinnSports.UnitTests.Services.TeamDetailsServiceTests;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class EventsServiceTests
    {
        private IEventService eventService;

        /// <summary>
        /// Expected sport events in database
        /// </summary>
        private List<SportEvent> databaseSportEvents;

        private TeamDetailsServiceTests.ResultDtoComparer comparer;

        [OneTimeSetUp]
        public void Init()
        {
            this.eventService = new EventsService(new EFUnitOfWork(new SportsContext("SportsContext")));
            this.comparer = new TeamDetailsServiceTests.ResultDtoComparer();
            this.databaseSportEvents = new List<SportEvent>();

            AutoMapperConfiguration.Configure();

            // --- Init sport types ---
            SportType football = new SportType()
            {
                Id = 1,
                Name = "Football",
            };
            SportType basketball = new SportType()
            {
                Id = 2,
                Name = "Basketball"
            };
            SportType tennis = new SportType()
            {
                Id = 3,
                Name = "Hockey"
            };

            // ---- Init teams ----
            // Football teams
            Team mu = new Team()
            {
                Id = 1,
                Name = "Manchester United",
                SportType = football,
                Results = new List<Result>()
            };
            Team milano = new Team()
            {
                Id = 2,
                Name = "Milano",
                SportType = football,
                Results = new List<Result>()
            };
            Team mc = new Team()
            {
                Id = 3,
                Name = "Manchester City",
                SportType = football,
                Results = new List<Result>()
            };
            Team chelsea = new Team()
            {
                Id = 4,
                Name = "Chelsea",
                SportType = football,
                Results = new List<Result>()
            };
            Team bayern = new Team()
            {
                Id = 5,
                Name = "Bayern",
                SportType = football,
                Results = new List<Result>()
            };

            // Basketball teams
            Team chicagoBulls = new Team()
            {
                Id = 6,
                Name = "Chicago Bulls",
                SportType = basketball,
                Results = new List<Result>()
            };
            Team laLakers = new Team()
            {
                Id = 7,
                Name = "Los Angeles Lakers",
                SportType = basketball,
                Results = new List<Result>()
            };
            Team phoenixSuns = new Team()
            {
                Id = 8,
                Name = "Phoenix Suns",
                SportType = basketball,
                Results = new List<Result>()
            };

            //    --- Init Events --- 
            SportEvent mu_vs_MC_event = new SportEvent()
            {
                Id = 1,
                Date = new DateTime(2016, 11, 19, 17, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };

            SportEvent bayern_vs_Milano_event = new SportEvent()
            {
                Id = 2,
                Date = new DateTime(2016, 10, 28, 17, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            SportEvent chelsea_vs_MC_event = new SportEvent()
            {
                Id = 3,
                Date = new DateTime(2016, 10, 17, 18, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            SportEvent chelsea_vs_Milano_event = new SportEvent()
            {
                Id = 4,
                Date = new System.DateTime(2016, 11, 3, 16, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            SportEvent chicagoBulls_vs_LA_event = new SportEvent()
            {
                Id = 5,
                Date = new DateTime(2016, 11, 5, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };
            SportEvent chicagoBulls_vs_Suns_event = new SportEvent()
            {
                Id = 6,
                Date = new DateTime(2016, 11, 29, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };
            SportEvent la_vs_Suns_event = new SportEvent()
            {
                Id = 7,
                Date = new DateTime(2016, 11, 15, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };

            // --- Init results ---
            Result mu_vs_MC = new Result()
            {
                Id = 1,
                Score = 2,
                Team = mu,
                SportEvent = mu_vs_MC_event
            };
            Result milano_vs_Bayern = new Result()
            {
                Id = 2,
                Team = milano,
                Score = 1,
                SportEvent = bayern_vs_Milano_event
            };
            Result milano_vs_Chelsea = new Result()
            {
                Id = 3,
                Team = milano,
                Score = 3,
                SportEvent = chelsea_vs_Milano_event
            };
            Result mc_vs_MU = new Result()
            {
                Id = 4,
                Score = 1,
                Team = mc,
                SportEvent = mu_vs_MC_event
            };
            Result mc_vs_Chelsea = new Result()
            {
                Id = 5,
                Team = mc,
                Score = 0,
                SportEvent = chelsea_vs_MC_event
            };
            Result chelsea_vs_MC = new Result()
            {
                Id = 6,
                Team = chelsea,
                Score = 0,
                SportEvent = chelsea_vs_MC_event
            };
            Result chelsea_vs_Milano = new Result()
            {
                Id = 7,
                Team = chelsea,
                Score = 2,
                SportEvent = chelsea_vs_Milano_event
            };
            Result bayern_vs_Milano = new Result()
            {
                Id = 8,
                Team = bayern,
                Score = 4,
                SportEvent = bayern_vs_Milano_event
            };
            Result ch_vs_LA = new Result()
            {
                Id = 9,
                Score = 68,
                Team = chicagoBulls,
                SportEvent = chicagoBulls_vs_LA_event
            };
            Result ch_vs_Ph = new Result()
            {
                Id = 10,
                Score = 52,
                Team = chicagoBulls,
                SportEvent = chicagoBulls_vs_Suns_event
            };
            Result la_vs_Ch = new Result()
            {
                Id = 11,
                Score = 65,
                Team = laLakers,
                SportEvent = chicagoBulls_vs_LA_event
            };

            Result la_vs_Ph = new Result()
            {
                Id = 12,
                Score = 65,
                Team = laLakers,
                SportEvent = la_vs_Suns_event
            };
            Result ph_vs_LA = new Result()
            {
                Id = 13,
                Score = 64,
                Team = phoenixSuns,
                SportEvent = la_vs_Suns_event
            };
            Result ph_vs_Ch = new Result()
            {
                Id = 14,
                Score = 52,
                Team = phoenixSuns,
                SportEvent = chicagoBulls_vs_Suns_event
            };

            mu.Results.Add(mu_vs_MC);
            milano.Results.Add(milano_vs_Bayern);
            milano.Results.Add(milano_vs_Chelsea);
            mc.Results.Add(mc_vs_Chelsea);
            mc.Results.Add(mc_vs_MU);
            chelsea.Results.Add(chelsea_vs_MC);
            chelsea.Results.Add(chelsea_vs_Milano);
            bayern.Results.Add(bayern_vs_Milano);
            chicagoBulls.Results.Add(ch_vs_LA);
            chicagoBulls.Results.Add(ch_vs_Ph);
            laLakers.Results.Add(la_vs_Ch);
            laLakers.Results.Add(la_vs_Ph);
            phoenixSuns.Results.Add(ph_vs_Ch);
            phoenixSuns.Results.Add(ph_vs_LA);

            chicagoBulls_vs_LA_event.Results.Add(ch_vs_LA);
            chicagoBulls_vs_LA_event.Results.Add(la_vs_Ch);
            la_vs_Suns_event.Results.Add(la_vs_Ph);
            la_vs_Suns_event.Results.Add(ph_vs_LA);
            chicagoBulls_vs_Suns_event.Results.Add(ch_vs_Ph);
            chicagoBulls_vs_Suns_event.Results.Add(ph_vs_Ch);
            bayern_vs_Milano_event.Results.Add(bayern_vs_Milano);
            bayern_vs_Milano_event.Results.Add(milano_vs_Bayern);
            mu_vs_MC_event.Results.Add(mu_vs_MC);
            mu_vs_MC_event.Results.Add(mc_vs_MU);
            chelsea_vs_Milano_event.Results.Add(chelsea_vs_Milano);
            chelsea_vs_Milano_event.Results.Add(milano_vs_Chelsea);
            chelsea_vs_MC_event.Results.Add(chelsea_vs_MC);
            chelsea_vs_MC_event.Results.Add(mc_vs_Chelsea);

            this.databaseSportEvents.Add(chicagoBulls_vs_LA_event);
            this.databaseSportEvents.Add(chicagoBulls_vs_Suns_event);
            this.databaseSportEvents.Add(la_vs_Suns_event);
            this.databaseSportEvents.Add(bayern_vs_Milano_event);
            this.databaseSportEvents.Add(mu_vs_MC_event);
            this.databaseSportEvents.Add(chelsea_vs_Milano_event);
            this.databaseSportEvents.Add(chelsea_vs_MC_event);
        }

        [OneTimeTearDown]
        public void Clean()
        {

        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void SportEventsCount(int sportId)
        {
            int expectedCount = this.databaseSportEvents
                .Where(e => e.SportType.Id == sportId)
                .Count();
            int actualCount = this.eventService.Count(sportId);
        }

        [Test]
        [TestCase(1, 0, 10)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 10, 10)]
        [TestCase(2, 0, 10)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 10, 10)]
        [TestCase(3, 0, 10)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 10, 10)]
        [TestCase(4, 0, 10)]
        [TestCase(4, 1, 1)]
        [TestCase(4, 10, 10)]
        public void GetSportEvents(int sportId, int skip, int take)
        {
            List<SportEvent> expectedSportEvents = this.databaseSportEvents
                .Where(e => e.SportType.Id == sportId)
                .OrderByDescending(e => e.Date)
                .Skip(skip)
                .Take(take)
                .ToList();
            List<ResultDto> expectedResultDtos = new List<ResultDto>();
            foreach (SportEvent item in expectedSportEvents)
            {
                expectedResultDtos
                    .Add(Mapper.Map<SportEvent, ResultDto>(item));
            }

            List<ResultDto> actualResultDtos = this.eventService
                .GetSportEvents(sportId, skip, take).ToList();

            Assert.AreEqual(expectedResultDtos.Count, actualResultDtos.Count);
            for (int i = 0; i < expectedResultDtos.Count; i++)
            {
                Assert.AreEqual(expectedResultDtos[i].Id, actualResultDtos[i].Id);
                Assert.AreEqual(expectedResultDtos[i].Date, actualResultDtos[i].Date);
                Assert.AreEqual(expectedResultDtos[i].TeamIds.Count(), 
                    actualResultDtos[i].TeamIds.Count());
                Assert.AreEqual(expectedResultDtos[i].TeamNames.Count(), 
                    actualResultDtos[i].TeamIds.Count());                

                for (int j = 0; j < expectedResultDtos[i].TeamIds.Count(); j++)
                {
                    bool existTeamId = 
                        expectedResultDtos[i].TeamIds.Contains(actualResultDtos[i].TeamIds.ElementAt(j));
                    Assert.AreEqual(true, existTeamId);

                    bool existTeamName =
                        expectedResultDtos[i].TeamNames.Contains(actualResultDtos[i].TeamNames.ElementAt(j));
                    Assert.AreEqual(true, existTeamName);

                    /*
                    bool idComparing =
                        expectedResultDtos[i].TeamIds.ElementAt(0) == actualResultDtos[i].TeamIds.ElementAt(0)
                        && expectedResultDtos[i].TeamIds.ElementAt(1) == actualResultDtos[i].TeamIds.ElementAt(1)
                        ||
                        expectedResultDtos[i].TeamIds.ElementAt(0) == actualResultDtos[i].TeamIds.ElementAt(1)
                        && expectedResultDtos[i].TeamIds.ElementAt(1) == actualResultDtos[i].TeamIds.ElementAt(0);
                    Assert.AreEqual(true, idComparing);

                    bool nameComparing = expectedResultDtos[i].TeamNames.ElementAt(0) == actualResultDtos[i].TeamNames.ElementAt(0)
                        && expectedResultDtos[i].TeamNames.ElementAt(1) == actualResultDtos[i].TeamNames.ElementAt(1)
                        ||
                        expectedResultDtos[i].TeamNames.ElementAt(0) == actualResultDtos[i].TeamNames.ElementAt(1)
                        && expectedResultDtos[i].TeamNames.ElementAt(1) == actualResultDtos[i].TeamNames.ElementAt(0);
                    Assert.AreEqual(true, nameComparing);  
                    */                  
                }
            }
        }

        [Test]
        public void MapSportEvent()
        {
            List<ResultDto> expectedResultDtos = new List<ResultDto>();
            List<ResultDto> actualResultDtos = new List<ResultDto>();            
            foreach(SportEvent sportEvent in this.databaseSportEvents)
            {
                ResultDto current = new ResultDto();
                current.Id = sportEvent.Id;
                current.Date = new EventDate(sportEvent.Date).ToString();

                current.Score = string.Format(
                            "{0} : {1}",
                            sportEvent.Results.ElementAt(0).Score,
                            sportEvent.Results.ElementAt(1).Score);

                current.TeamIds = sportEvent.Results.Select(x => x.Team.Id);
                current.TeamNames = sportEvent.Results.Select(x => x.Team.Name);

                expectedResultDtos.Add(current);
                actualResultDtos.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
            }

            CollectionAssert.AreEqual(expectedResultDtos, actualResultDtos, new ResultDtoComparer());
        }
    }
}
