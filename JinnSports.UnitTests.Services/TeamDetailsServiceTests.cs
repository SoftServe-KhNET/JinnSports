using System;
using JinnSports.BLL.Service;
using JinnSports.Entities.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using System.Linq;
using System.Collections;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class TeamDetailsServiceTests
    {

        List<SportType> sportTypes;
        List<Team> teams;
        List<Result> results;
        List<SportEvent> sportEvents;
        IEnumerable<List<ResultDto>> resultsDto;
        public class ResultDtoComparer : IComparer, IComparer<ResultDto>
        {
            public int Compare(object x, object y)
            {
                var ldto = x as ResultDto;
                var rdto = y as ResultDto;
                if (ldto == null || rdto == null) throw new InvalidOperationException();
                return Compare(ldto, rdto);
            }

            public int Compare(ResultDto ldto, ResultDto rdto)
            {
                if (ldto.Date == null || string.IsNullOrEmpty(ldto.Score) || ldto.Id < 1 || ldto.TeamFirstId < 1 || ldto.TeamSecondId < 1 ||
                string.IsNullOrEmpty(ldto.TeamFirst) || string.IsNullOrEmpty(ldto.TeamSecond))
                {
                    return -1;
                }

                return ((ldto.Date.CompareTo(rdto.Date)) & (ldto.Score.CompareTo(rdto.Score)) &
                    (ldto.Id.CompareTo(rdto.Id)) & (ldto.TeamFirstId.CompareTo(rdto.TeamFirstId)) &
                    (ldto.TeamSecondId.CompareTo(rdto.TeamSecondId)) & (ldto.TeamFirst.CompareTo(rdto.TeamFirst)) &
                    (ldto.TeamSecond.CompareTo(rdto.TeamSecond)));
            }
        }
        [SetUp]
       public void Init()
        {
            this.sportTypes = new List<SportType>();
            this.teams = new List<Team>();
            this.results = new List<Result>();
            this.sportEvents = new List<SportEvent>();
            this.resultsDto = new List<List<ResultDto>>();

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
                Name = "Tennis"
            };

            SportType snooker = new SportType()
            {
                Id = 4,
                Name = "Snooker"
            };
                       
            // ---- Init teams ----

            // Football teams
            Team MU = new Team()
            {
                Id = 1,
                Name = "Manchester United",
                SportType = football,
                Results = new List<Result>()
            };
            Team Milano = new Team()
            {
                Id = 2,
                Name = "Milano",
                SportType = football,
                Results = new List<Result>()
            };
            Team MC = new Team()
            {
                Id = 3,
                Name = "Manchester City",
                SportType = football,
                Results = new List<Result>()
            };
            Team Chelsea = new Team()
            {
                Id = 4,
                Name = "Chelsea",
                SportType = football,
                Results = new List<Result>()
            };
            Team Bayern = new Team()
            {
                Id = 5,
                Name = "Bayern",
                SportType = football,
                Results = new List<Result>()
            };

            // Basketball teams
            Team ChicagoBulls = new Team()
            {
                Id = 6,
                Name = "Chicago Bulls",
                SportType = basketball,
                Results = new List<Result>()
            };
            Team LALakers = new Team()
            {
                Id = 7,
                Name = "Los Angeles Lakers",
                SportType = basketball,
                Results = new List<Result>()
            };
            Team PhoenixSuns = new Team()
            {
                Id = 8,
                Name = "Phoenix Suns",
                SportType = basketball,
                Results = new List<Result>()
            };
            //    --- Init Events --- 

            SportEvent ChicagoBulls_vs_LA_event = new SportEvent()
            {
                Id = 1,
                Date = new DateTime(2016, 11, 5, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };
            SportEvent ChicagoBulls_vs_Suns_event = new SportEvent()
            {
                Id = 2,
                Date = new DateTime(2016, 11, 29, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };
            SportEvent LA_vs_Suns_event = new SportEvent()
            {
                Id = 3,
                Date = new DateTime(2016, 11, 15, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };

            SportEvent MU_vs_MC_event = new SportEvent()
            {
                Id = 5,
                Date = new DateTime(2016, 11, 19, 17, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };

            SportEvent Bayern_vs_Milano_event = new SportEvent()
            {
                Id = 6,
                Date = new DateTime(2016, 10, 28, 17, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            Result Bayern_vs_Milano = new Result()
            {
                Id = 8,
                Team = Bayern,
                Score = 4,
                SportEvent = Bayern_vs_Milano_event
            };
            Result Milano_vs_Bayern = new Result()
            {
                Id = 2,
                Team = Milano,
                Score = 1,
                SportEvent = Bayern_vs_Milano_event
            };


            SportEvent Chelsea_vs_MC_event = new SportEvent()
            {
                Id = 7,
                Date = new DateTime(2016, 10, 17, 18, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            Result Chelsea_vs_MC = new Result()
            {
                Id = 6,
                Team = Chelsea,
                Score = 0,
                SportEvent = Chelsea_vs_MC_event
            };
            Result MC_vs_Chelsea = new Result()
            {
                Id = 5,
                Team = MC,
                Score = 0,
                SportEvent = Chelsea_vs_MC_event
            };

            SportEvent Chelsea_vs_Milano_event = new SportEvent()
            {
                Id = 8,
                Date = new System.DateTime(2016, 11, 3, 16, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            Result Chelsea_vs_Milano = new Result()
            {
                Id = 7,
                Team = Chelsea,
                Score = 2,
                SportEvent = Chelsea_vs_Milano_event
            };
            Result Milano_vs_Chelsea = new Result()
            {
                Id = 3,
                Team = Milano,
                Score = 3,
                SportEvent = Chelsea_vs_Milano_event
            };

            // Init Results
            Result Ch_vs_LA = new Result()
            {
                Id = 9,
                Score = 68,
                Team = ChicagoBulls,
                SportEvent = ChicagoBulls_vs_LA_event
            };
            Result LA_vs_Ch = new Result()
            {
                Id = 11,
                Score = 65,
                Team = LALakers,
                SportEvent = ChicagoBulls_vs_LA_event
            };

            Result LA_vs_Ph = new Result()
            {
                Id = 12,
                Score = 65,
                Team = LALakers,
                SportEvent = LA_vs_Suns_event
            };
            Result Ph_vs_LA = new Result()
            {
                Id = 13,
                Score = 64,
                Team = PhoenixSuns,
                SportEvent = LA_vs_Suns_event
            };

            Result Ch_vs_Ph = new Result()
            {
                Id = 10,
                Score = 52,
                Team = ChicagoBulls,
                SportEvent = ChicagoBulls_vs_Suns_event
            };
            Result Ph_vs_Ch = new Result()
            {
                Id = 14,
                Score = 52,
                Team = PhoenixSuns,
                SportEvent = ChicagoBulls_vs_Suns_event
            };
            Result MU_vs_MC = new Result()
            {
                Id = 1,
                Score = 2,
                Team = MU,
                SportEvent = MU_vs_MC_event
            };
            Result MC_vs_MU = new Result()
            {
                Id = 4,
                Score = 1,
                Team = MC,
                SportEvent = MU_vs_MC_event
            };


           // test entities formation

            sportTypes.Add(football);
            sportTypes.Add(basketball);
            sportTypes.Add(tennis);
            sportTypes.Add(snooker);


            teams.Add(MU);
            teams.Add(Milano);
            teams.Add(MC);
            teams.Add(Chelsea);
            teams.Add(Bayern);
            teams.Add(ChicagoBulls);
            teams.Add(LALakers);
            teams.Add(PhoenixSuns);

            sportEvents.Add(ChicagoBulls_vs_LA_event);
            sportEvents.Add(ChicagoBulls_vs_Suns_event);
            sportEvents.Add(LA_vs_Suns_event);
            sportEvents.Add(Bayern_vs_Milano_event);
            sportEvents.Add(MU_vs_MC_event);
            sportEvents.Add(Chelsea_vs_Milano_event);
            sportEvents.Add(Chelsea_vs_MC_event);

            /*results.Add(Ch_vs_LA);
            results.Add(LA_vs_Ch);
            results.Add(Ch_vs_Ph);
            results.Add(Ph_vs_Ch);
            results.Add(LA_vs_Ph);
            results.Add(Ph_vs_LA);
            results.Add(MU_vs_MC);
            results.Add(MC_vs_MU);
            results.Add(Bayern_vs_Milano);
            results.Add(Milano_vs_Bayern);
            results.Add(Chelsea_vs_MC);
            results.Add(MC_vs_Chelsea);
            results.Add(Chelsea_vs_Milano);
            results.Add(Milano_vs_Chelsea); OLD ID Initialization*/

            results.Add(MU_vs_MC);
            results.Add(Milano_vs_Bayern);
            results.Add(Milano_vs_Chelsea);
            results.Add(MC_vs_MU);
            results.Add(MC_vs_Chelsea);
            results.Add(Chelsea_vs_MC);
            results.Add(Chelsea_vs_Milano);
            results.Add(Bayern_vs_Milano);
            results.Add(Ch_vs_LA);
            results.Add(Ch_vs_Ph);
            results.Add(LA_vs_Ch);
            results.Add(LA_vs_Ph);
            results.Add(Ph_vs_LA);
            results.Add(Ph_vs_Ch);

            MU.Results.Add(MU_vs_MC);
            Milano.Results.Add(Milano_vs_Bayern);
            Milano.Results.Add(Milano_vs_Chelsea);
            MC.Results.Add(MC_vs_Chelsea);
            MC.Results.Add(MC_vs_MU);
            Chelsea.Results.Add(Chelsea_vs_MC);
            Chelsea.Results.Add(Chelsea_vs_Milano);
            Bayern.Results.Add(Bayern_vs_Milano);
            ChicagoBulls.Results.Add(Ch_vs_LA);
            ChicagoBulls.Results.Add(Ch_vs_Ph);
            LALakers.Results.Add(LA_vs_Ch);
            LALakers.Results.Add(LA_vs_Ph);
            PhoenixSuns.Results.Add(Ph_vs_Ch);
            PhoenixSuns.Results.Add(Ph_vs_LA);

            ChicagoBulls_vs_LA_event.Results.Add(Ch_vs_LA);
            ChicagoBulls_vs_LA_event.Results.Add(LA_vs_Ch);
            LA_vs_Suns_event.Results.Add(LA_vs_Ph);
            LA_vs_Suns_event.Results.Add(Ph_vs_LA);
            ChicagoBulls_vs_Suns_event.Results.Add(Ch_vs_Ph);
            ChicagoBulls_vs_Suns_event.Results.Add(Ph_vs_Ch);
            Bayern_vs_Milano_event.Results.Add(Bayern_vs_Milano);
            Bayern_vs_Milano_event.Results.Add(Milano_vs_Bayern);
            MU_vs_MC_event.Results.Add(MU_vs_MC);
            MU_vs_MC_event.Results.Add(MC_vs_MU);
            Chelsea_vs_Milano_event.Results.Add(Chelsea_vs_Milano);
            Chelsea_vs_Milano_event.Results.Add(Milano_vs_Chelsea);
            Chelsea_vs_MC_event.Results.Add(Chelsea_vs_MC);
            Chelsea_vs_MC_event.Results.Add(MC_vs_Chelsea);
            
            //ResultDto formation
            ResultDto ChicagoBulls_vs_LA_Result_Dto = new ResultDto()
            {
                Date = ChicagoBulls_vs_LA_event.Date,
                Id = Ch_vs_LA.Id,
                Score = String.Format("{0} : {1}", Ch_vs_LA.Score, LA_vs_Ch.Score),
                TeamFirstId = ChicagoBulls.Id,
                TeamSecondId = LALakers.Id,
                TeamFirst = ChicagoBulls.Name,
                TeamSecond = LALakers.Name
            };
            ResultDto LA_vs_ChicagoBulls_Result_Dto = new ResultDto()
            {
                Date = LA_vs_Suns_event.Date,
                Id = LA_vs_Ch.Id,
                Score = String.Format("{0} : {1}", LA_vs_Ch.Score, Ch_vs_LA.Score),
                TeamFirstId = LALakers.Id,
                TeamSecondId = ChicagoBulls.Id,
                TeamFirst = LALakers.Name,
                TeamSecond = ChicagoBulls.Name
            };
            ResultDto Suns_vs_ChicagoBulls_Result_Dto = new ResultDto()
            {
                Date = ChicagoBulls_vs_Suns_event.Date,
                Id = Ph_vs_Ch.Id,
                Score = String.Format("{0} : {1}", Ph_vs_Ch.Score, Ch_vs_Ph.Score),
                TeamFirstId = PhoenixSuns.Id,
                TeamSecondId = ChicagoBulls.Id,
                TeamFirst = PhoenixSuns.Name,
                TeamSecond = ChicagoBulls.Name
            };
            ResultDto ChicagoBulls_vs_Suns_Result_Dto = new ResultDto()
            {
                Date = ChicagoBulls_vs_Suns_event.Date,
                Id = Ch_vs_Ph.Id,
                Score = String.Format("{0} : {1}", Ch_vs_Ph.Score, Ph_vs_Ch.Score),
                TeamFirstId = ChicagoBulls.Id,
                TeamSecondId = PhoenixSuns.Id,
                TeamFirst = ChicagoBulls.Name,
                TeamSecond = PhoenixSuns.Name
            };
            ResultDto Suns_vs_LA_Result_Dto = new ResultDto()
            {
                Date = LA_vs_Suns_event.Date,
                Id = Ph_vs_LA.Id,
                Score = String.Format("{0} : {1}", Ph_vs_LA.Score, LA_vs_Ph.Score),
                TeamFirstId = PhoenixSuns.Id,
                TeamSecondId = LALakers.Id,
                TeamFirst = PhoenixSuns.Name,
                TeamSecond = LALakers.Name
            };
            ResultDto LA_vs_Suns_Result_Dto = new ResultDto()
            {
                Date = LA_vs_Suns_event.Date,
                Id = LA_vs_Ph.Id,
                Score = String.Format("{0} : {1}", LA_vs_Ph.Score, Ph_vs_LA.Score),
                TeamFirstId = LALakers.Id,
                TeamSecondId = PhoenixSuns.Id,
                TeamFirst = LALakers.Name,
                TeamSecond = PhoenixSuns.Name
            };
            List<ResultDto> Ch_Results_dto = new List<ResultDto>()
            {
                ChicagoBulls_vs_LA_Result_Dto,
                ChicagoBulls_vs_Suns_Result_Dto
            };
            List<ResultDto> LA_Results_dto = new List<ResultDto>()
            {
                LA_vs_ChicagoBulls_Result_Dto,
                LA_vs_Suns_Result_Dto
            };
            List<ResultDto> Ph_Results_dto = new List<ResultDto>()
            {
                Suns_vs_ChicagoBulls_Result_Dto,
                Suns_vs_LA_Result_Dto
            };
            resultsDto = new List<List<ResultDto>>
            {
                Ch_Results_dto,
                LA_Results_dto,
                Ph_Results_dto
            };
        }
        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 2)]
        [TestCase(5, 1)]
        [TestCase(6, 2)]
        [TestCase(7, 2)]
        [TestCase(8, 2)]
        public void Count(int teamId, int result)
        {

            TeamDetailsService teamDelailsService = new TeamDetailsService();
            int count;

            count = teamDelailsService.Count(teamId);

            Assert.AreEqual(result, count); 
        }

        [Test]
        [TestCase(6, 0)]
        [TestCase(7, 1)]
        [TestCase(8, 2)]
        public void GetResults(int teamId, int element)
        {
            TeamDetailsService teamDetailsService = new TeamDetailsService();
            List<ResultDto> resultDtoCollection = new List<ResultDto>();
            List<ResultDto> dtoTest = this.resultsDto.ElementAt(element);
            ResultDtoComparer dtoComparer = new ResultDtoComparer();
            
            resultDtoCollection = teamDetailsService.GetResults(teamId).ToList();

            //Assert.IsTrue(resultDtoCollection.Equals(dtoTest));
            CollectionAssert.AreEqual(resultDtoCollection, dtoTest, dtoComparer);
        }
    }
}
