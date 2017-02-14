using System;
using JinnSports.BLL.Service;
using JinnSports.Entities.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.DAL.Repositories;
using System.Linq;
using System.Collections;
using System.Transactions;
using JinnSports.BLL.Extentions;
using AutoMapper;
using JinnSports.DAL.EFContext;
using JinnSports.WEB;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class TeamDetailsServiceTests
    {
        private TeamDetailsService teamDetailsService;

        private SportsContext databaseSportsContext;

        private TransactionScope databaseTransaction;

        private List<SportType> sportTypes;
        private List<Team> teams;
        private List<Result> results;
        private List<SportEvent> sportEvents;
        private IEnumerable<List<ResultDto>> resultsDtoCollection;
        private List<ResultDto> resultsDto;
        private List<Result> testResults;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            this.databaseSportsContext = new SportsContext("SportsContext");

            // Clear tables
            this.databaseSportsContext.TeamNames.RemoveRange(
                this.databaseSportsContext.TeamNames);
            this.databaseSportsContext.Results.RemoveRange(
                this.databaseSportsContext.Results);
            this.databaseSportsContext.SportEvents.RemoveRange(
                this.databaseSportsContext.SportEvents);
            this.databaseSportsContext.Teams.RemoveRange(
                this.databaseSportsContext.Teams);
            this.databaseSportsContext.SportTypes.RemoveRange(
                this.databaseSportsContext.SportTypes);

            this.databaseSportsContext.SaveChanges();

            AutoMapperConfiguration.Configure();

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[SportTypes] ON;
                INSERT INTO [dbo].[SportTypes] ([Id], [Name])
                VALUES
                (1, 'Football'),
                (2, 'Basketball'),
                (3, 'Tennis'),
                (4, 'Snooker');               
                SET IDENTITY_INSERT [dbo].[SportTypes] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[Teams] ON;
                INSERT INTO [dbo].[Teams] ([Id], [Name], [SportType_Id])
                VALUES
                (1, 'Manchester United', 1),
                (2, 'Milano', 1),
                (3, 'Manchester City', 1),
                (4, 'Chelsea', 1),
                (5, 'Bayern', 1),
                (6, 'Chicago Bulls', 2),
                (7, 'Los Angeles Lakers', 2),
                (8, 'Phoenix Suns', 2);               
                SET IDENTITY_INSERT [dbo].[Teams] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[SportEvents] ON;
                INSERT INTO [dbo].[SportEvents] ([Id], [Date], [SportType_Id])
                VALUES
                (1, '20161119 17:00', 1),
                (2, '20161028 17:00', 1),
                (3, '20161017 18:00', 1),
                (4, '20161103 16:00', 1),
                (5, '20161105 16:00', 2),
                (6, '20161129 16:00', 2),
                (7, '20161115 16:00', 2);               
                SET IDENTITY_INSERT [dbo].[SportEvents] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[Results] ON;
                INSERT INTO [dbo].[Results] ([Id], [Score], [SportEvent_Id], [Team_Id])
                VALUES
                (1, 2, 1, 1),
                (2, 1, 2, 2),
                (3, 3, 4, 2),
                (4, 1, 1, 3),
                (5, 0, 3, 3),
                (6, 0, 3, 4),
                (7, 2, 4, 4),
                (8, 4, 2, 5),
                (9, 68, 5, 6),
                (10, 52, 6, 6),
                (11, 65, 5, 7),
                (12, 65, 7, 7),
                (13, 64, 7, 8),
                (14, 52, 6, 8);               
                SET IDENTITY_INSERT [dbo].[Results] OFF;");

            this.databaseSportsContext.SaveChanges();

            this.teamDetailsService = new TeamDetailsService(
                new EFUnitOfWork(this.databaseSportsContext));
        }
        
        [SetUp]
        public void InitTestEntities()
        {
            this.sportTypes = new List<SportType>();
            this.teams = new List<Team>();
            this.results = new List<Result>();
            this.sportEvents = new List<SportEvent>();

            this.testResults = new List<Result>();
            this.resultsDto = new List<ResultDto>();
            this.resultsDtoCollection = new List<List<ResultDto>>();

            AutoMapperConfiguration.Configure();
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
            Team lalakers = new Team()
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

            SportEvent chicagoBulls_vs_LA_event = new SportEvent()
            {
                Id = 1,
                Date = new DateTime(2016, 11, 5, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };
            SportEvent chicagoBulls_vs_Suns_event = new SportEvent()
            {
                Id = 2,
                Date = new DateTime(2016, 11, 29, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };
            SportEvent la_vs_Suns_event = new SportEvent()
            {
                Id = 3,
                Date = new DateTime(2016, 11, 15, 16, 0, 0),
                SportType = basketball,
                Results = new List<Result>()
            };

            SportEvent mu_vs_MC_event = new SportEvent()
            {
                Id = 5,
                Date = new DateTime(2016, 11, 19, 17, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };

            SportEvent bayern_vs_Milano_event = new SportEvent()
            {
                Id = 6,
                Date = new DateTime(2016, 10, 28, 17, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            Result bayern_vs_Milano = new Result()
            {
                Id = 8,
                Team = bayern,
                Score = 4,
                SportEvent = bayern_vs_Milano_event
            };
            Result milano_vs_Bayern = new Result()
            {
                Id = 2,
                Team = milano,
                Score = 1,
                SportEvent = bayern_vs_Milano_event
            };


            SportEvent chelsea_vs_MC_event = new SportEvent()
            {
                Id = 7,
                Date = new DateTime(2016, 10, 17, 18, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            Result chelsea_vs_MC = new Result()
            {
                Id = 6,
                Team = chelsea,
                Score = 0,
                SportEvent = chelsea_vs_MC_event
            };
            Result mc_vs_Chelsea = new Result()
            {
                Id = 5,
                Team = mc,
                Score = 0,
                SportEvent = chelsea_vs_MC_event
            };

            SportEvent chelsea_vs_Milano_event = new SportEvent()
            {
                Id = 8,
                Date = new System.DateTime(2016, 11, 3, 16, 0, 0),
                SportType = football,
                Results = new List<Result>()
            };
            Result chelsea_vs_Milano = new Result()
            {
                Id = 7,
                Team = chelsea,
                Score = 2,
                SportEvent = chelsea_vs_Milano_event
            };
            Result milano_vs_Chelsea = new Result()
            {
                Id = 3,
                Team = milano,
                Score = 3,
                SportEvent = chelsea_vs_Milano_event
            };

            // Init Results
            Result ch_vs_LA = new Result()
            {
                Id = 9,
                Score = 68,
                Team = chicagoBulls,
                SportEvent = chicagoBulls_vs_LA_event
            };
            Result la_vs_Ch = new Result()
            {
                Id = 11,
                Score = 65,
                Team = lalakers,
                SportEvent = chicagoBulls_vs_LA_event
            };

            Result la_vs_Ph = new Result()
            {
                Id = 12,
                Score = 65,
                Team = lalakers,
                SportEvent = la_vs_Suns_event
            };
            Result ph_vs_LA = new Result()
            {
                Id = 13,
                Score = 64,
                Team = phoenixSuns,
                SportEvent = la_vs_Suns_event
            };

            Result ch_vs_Ph = new Result()
            {
                Id = 10,
                Score = 52,
                Team = chicagoBulls,
                SportEvent = chicagoBulls_vs_Suns_event
            };
            Result ph_vs_Ch = new Result()
            {
                Id = 14,
                Score = 52,
                Team = phoenixSuns,
                SportEvent = chicagoBulls_vs_Suns_event
            };
            Result mu_vs_MC = new Result()
            {
                Id = 1,
                Score = 2,
                Team = mu,
                SportEvent = mu_vs_MC_event
            };
            Result mc_vs_MU = new Result()
            {
                Id = 4,
                Score = 1,
                Team = mc,
                SportEvent = mu_vs_MC_event
            };

            this.sportTypes.Add(football);
            this.sportTypes.Add(basketball);
            this.sportTypes.Add(tennis);
            this.sportTypes.Add(snooker);


            this.teams.Add(mu);
            this.teams.Add(milano);
            this.teams.Add(mc);
            this.teams.Add(chelsea);
            this.teams.Add(bayern);
            this.teams.Add(chicagoBulls);
            this.teams.Add(lalakers);
            this.teams.Add(phoenixSuns);

            this.sportEvents.Add(chicagoBulls_vs_LA_event);
            this.sportEvents.Add(chicagoBulls_vs_Suns_event);
            this.sportEvents.Add(la_vs_Suns_event);
            this.sportEvents.Add(bayern_vs_Milano_event);
            this.sportEvents.Add(mu_vs_MC_event);
            this.sportEvents.Add(chelsea_vs_Milano_event);
            this.sportEvents.Add(chelsea_vs_MC_event);

            this.results.Add(mu_vs_MC);
            this.results.Add(milano_vs_Bayern);
            this.results.Add(milano_vs_Chelsea);
            this.results.Add(mc_vs_MU);
            this.results.Add(mc_vs_Chelsea);
            this.results.Add(chelsea_vs_MC);
            this.results.Add(chelsea_vs_Milano);
            this.results.Add(bayern_vs_Milano);
            this.results.Add(ch_vs_LA);
            this.results.Add(ch_vs_Ph);
            this.results.Add(la_vs_Ch);
            this.results.Add(la_vs_Ph);
            this.results.Add(ph_vs_LA);
            this.results.Add(ph_vs_Ch);

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
            lalakers.Results.Add(la_vs_Ch);
            lalakers.Results.Add(la_vs_Ph);
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

            //ResultDto formation
            ResultDto chicagoBulls_vs_LA_Result_Dto = new ResultDto()
            {
                Date = new EventDate(chicagoBulls_vs_LA_event.Date).ToString(),
                Id = ch_vs_LA.Id,
                Score = string.Format("{0} : {1}", ch_vs_LA.Score, la_vs_Ch.Score),
                TeamIds = new List<int>()
                {
                    chicagoBulls.Id,
                    lalakers.Id
                },
                TeamNames = new List<string>()
                {
                    chicagoBulls.Name,
                    lalakers.Name
                }
            };
            ResultDto la_vs_ChicagoBulls_Result_Dto = new ResultDto()
            {
                Date = new EventDate(la_vs_Suns_event.Date).ToString(),
                Id = la_vs_Ch.Id,
                Score = string.Format("{0} : {1}", la_vs_Ch.Score, ch_vs_LA.Score),
                TeamIds = new List<int>()
                {
                    lalakers.Id,
                    chicagoBulls.Id
                },
                TeamNames = new List<string>()
                {
                    lalakers.Name,
                    chicagoBulls.Name
                }
            };
            ResultDto suns_vs_ChicagoBulls_Result_Dto = new ResultDto()
            {
                Date = new EventDate(chicagoBulls_vs_Suns_event.Date).ToString(),
                Id = ph_vs_Ch.Id,
                Score = string.Format("{0} : {1}", ph_vs_Ch.Score, ch_vs_Ph.Score),
                TeamIds = new List<int>()
                {
                    phoenixSuns.Id,
                    chicagoBulls.Id
                },
                TeamNames = new List<string>()
                {
                    phoenixSuns.Name,
                    chicagoBulls.Name
                }
            };
            ResultDto chicagoBulls_vs_Suns_Result_Dto = new ResultDto()
            {
                Date = new EventDate(chicagoBulls_vs_Suns_event.Date).ToString(),
                Id = ch_vs_Ph.Id,
                Score = string.Format("{0} : {1}", ch_vs_Ph.Score, ph_vs_Ch.Score),
                TeamIds = new List<int>()
                {
                    chicagoBulls.Id,
                    phoenixSuns.Id
                },
                TeamNames = new List<string>()
                {
                    chicagoBulls.Name,
                    phoenixSuns.Name
                }
            };
            ResultDto suns_vs_LA_Result_Dto = new ResultDto()
            {
                Date = new EventDate(la_vs_Suns_event.Date).ToString(),
                Id = ph_vs_LA.Id,
                Score = string.Format("{0} : {1}", ph_vs_LA.Score, la_vs_Ph.Score),
                TeamIds = new List<int>()
                {
                    phoenixSuns.Id,
                    lalakers.Id
                },
                TeamNames = new List<string>()
                {
                    phoenixSuns.Name,
                    lalakers.Name
                }
            };
            ResultDto la_vs_Suns_Result_Dto = new ResultDto()
            {
                Date = new EventDate(la_vs_Suns_event.Date).ToString(),
                Id = la_vs_Ph.Id,
                Score = string.Format("{0} : {1}", la_vs_Ph.Score, ph_vs_LA.Score),
                TeamIds = new List<int>()
                {
                    lalakers.Id,
                    phoenixSuns.Id
                },
                TeamNames = new List<string>()
                {
                    lalakers.Name,
                    phoenixSuns.Name
                }
            };
            List<ResultDto> ch_Results_dto = new List<ResultDto>()
            {
                chicagoBulls_vs_LA_Result_Dto,
                chicagoBulls_vs_Suns_Result_Dto
            };
            List<ResultDto> la_Results_dto = new List<ResultDto>()
            {
                la_vs_Suns_Result_Dto,
                la_vs_ChicagoBulls_Result_Dto
            };
            List<ResultDto> ph_Results_dto = new List<ResultDto>()
            {
                suns_vs_ChicagoBulls_Result_Dto,
                suns_vs_LA_Result_Dto
            };
            this.resultsDtoCollection = new List<List<ResultDto>>
            {
                ch_Results_dto,
                la_Results_dto,
                ph_Results_dto
            };
            this.resultsDto = new List<ResultDto>()
            {
                chicagoBulls_vs_LA_Result_Dto,
                la_vs_ChicagoBulls_Result_Dto,
                suns_vs_ChicagoBulls_Result_Dto,
                chicagoBulls_vs_Suns_Result_Dto,
                suns_vs_LA_Result_Dto,
                la_vs_Suns_Result_Dto
            };
            this.testResults = new List<Result>()
            {
                ch_vs_LA,
                la_vs_Ch,
                ph_vs_Ch,
                ch_vs_Ph,
                ph_vs_LA,
                la_vs_Ph
            };

            this.databaseTransaction = new TransactionScope(
                TransactionScopeOption.Required, 
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.Serializable
                });
        }

        [TearDown]
        public void Clean()
        {
            this.databaseTransaction.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeClean()
        {
            this.databaseSportsContext.Dispose();
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
        public void CountCheckTeamResultsCount(int teamId, int result)
        {
            int count = this.teamDetailsService.Count(teamId);

            Assert.AreEqual(result, count);
        }

        [Test]
        [TestCase(6, 0)]
        [TestCase(7, 1)]
        [TestCase(8, 2)]
        public void GetResultsCheckResults(int teamId, int element)
        {
            List<ResultDto> resultDtoCollection = new List<ResultDto>();
            List<ResultDto> dtoTest = this.resultsDtoCollection.ElementAt(element);
            ResultDtoComparer dtoComparer = new ResultDtoComparer();
            int skip = 0;
            int take = 10;
            resultDtoCollection = this.teamDetailsService.GetResults(teamId, skip, take).ToList();

            CollectionAssert.AreEqual(resultDtoCollection, dtoTest, dtoComparer);
        }

        [Test]
        public void CheckResultToResultDtoMapping()
        {
            ResultDtoComparer dtoComparer = new ResultDtoComparer();

            List<ResultDto> resultsDto = this.resultsDto;
            List<ResultDto> mappedDto = new List<ResultDto>();
            List<Result> testResults = this.testResults;

            foreach (Result result in testResults)
            {
                mappedDto.Add(Mapper.Map<Result, ResultDto>(result));
            }

            CollectionAssert.AreEqual(resultsDto, mappedDto, dtoComparer);
        }

        [Test]
        public void CheckTeamToTeamDtoMapping()
        {
            List<Team> teams = this.teams;
            List<TeamDto> teamsTestDto = new List<TeamDto>();
            List<TeamDto> mappedDto = new List<TeamDto>();
            TeamDtoComparer dtoComparer = new TeamDtoComparer();

            foreach (Team team in teams)
            {
                teamsTestDto.Add(new TeamDto
                {
                    Name = team.Name,
                    Id = team.Id
                });
                mappedDto.Add(Mapper.Map<Team, TeamDto>(team));
            }

            CollectionAssert.AreEqual(teamsTestDto, mappedDto, dtoComparer);
        }

        public class ResultDtoComparer : IComparer, IComparer<ResultDto>
        {
            public int Compare(object x, object y)
            {
                var ldto = x as ResultDto;
                var rdto = y as ResultDto;
                if (ldto == null || rdto == null)
                {
                    throw new InvalidOperationException();
                }
                return this.Compare(ldto, rdto);
            }

            public int Compare(ResultDto ldto, ResultDto rdto)
            {
                if (ldto.Date == null || string.IsNullOrEmpty(ldto.Score) || ldto.Id < 1 ||
                    ldto.TeamIds.ElementAt(0) < 1 || ldto.TeamIds.ElementAt(1) < 1 ||
                    string.IsNullOrEmpty(ldto.TeamNames.ElementAt(0)) ||
                    string.IsNullOrEmpty(ldto.TeamNames.ElementAt(1)))
                {
                    return -1;
                }
                return ldto.Date.CompareTo(rdto.Date) & ldto.Score.CompareTo(rdto.Score) &
                       ldto.Id.CompareTo(rdto.Id) & ldto.TeamIds.ElementAt(0).CompareTo(rdto.TeamIds.ElementAt(0)) &
                       ldto.TeamIds.ElementAt(1).CompareTo(rdto.TeamIds.ElementAt(1)) &
                       ldto.TeamNames.ElementAt(0).CompareTo(rdto.TeamNames.ElementAt(0)) &
                       ldto.TeamNames.ElementAt(1).CompareTo(rdto.TeamNames.ElementAt(1));
            }
        }

        public class TeamDtoComparer : IComparer, IComparer<TeamDto>
        {
            public int Compare(object x, object y)
            {
                var ldto = x as TeamDto;
                var rdto = y as TeamDto;
                if (ldto == null || rdto == null)
                {
                    throw new InvalidOperationException();
                }
                return this.Compare(ldto, rdto);
            }

            public int Compare(TeamDto ldto, TeamDto rdto)
            {
                if (string.IsNullOrEmpty(ldto.Name) || ldto.Id < 1)
                {
                    return -1;
                }
                return ldto.Name.CompareTo(rdto.Name) &
                       ldto.Id.CompareTo(rdto.Id);
            }
        }
    }
}