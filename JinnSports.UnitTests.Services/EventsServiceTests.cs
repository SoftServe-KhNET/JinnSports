using AutoMapper;
using DTO.JSON;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Extentions;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using JinnSports.Entities.Entities.Temp;
using JinnSports.WEB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using static JinnSports.UnitTests.Services.TeamDetailsServiceTests;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class EventsServiceTests
    {
        private IEventService eventService;

        private TeamDetailsServiceTests.ResultDtoComparer comparer;

        private SportsContext databaseSportsContext;

        private TransactionScope databaseTransaction;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            this.databaseSportsContext = new SportsContext("SportsContext");

            //// Other transactions can't update and insert data
            //this.databaseTransaction = this.databaseSportsContext
            //    .Database.BeginTransaction(IsolationLevel.Serializable);

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

            this.eventService = new EventsService(new EFUnitOfWork(this.databaseSportsContext));
            this.comparer = new ResultDtoComparer();

            AutoMapperConfiguration.Configure();

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[SportTypes] ON;
                INSERT INTO [dbo].[SportTypes] ([Id], [Name])
                VALUES
                (1, 'Football'),
                (2, 'Basketball'),
                (3, 'Hockey');               
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
                @"SET IDENTITY_INSERT [dbo].[TeamNames] ON;
                INSERT INTO [dbo].[TeamNames] ([Id], [Name], [Team_Id])
                VALUES
                (1, 'Manchester United', 1),
                (2, 'Milano', 2),
                (3, 'Manchester City', 3),
                (4, 'Chelsea', 4),
                (5, 'Bayern', 5),
                (6, 'Chicago Bulls', 6),
                (7, 'Los Angeles Lakers', 7),
                (8, 'Phoenix Suns', 8);               
                SET IDENTITY_INSERT [dbo].[TeamNames] OFF;");

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

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            TeamName tn1 = new TeamName { Name = "Шахтер Д" };
            TeamName tn2 = new TeamName { Name = "Динамо Бухарест" };
            TeamName tn3 = new TeamName { Name = "Динамо Киев" };
            TeamName tn4 = new TeamName { Name = "Сент-Этьен" };
            TeamName tn5 = new TeamName { Name = "Заря ЛГ" };
            TeamName tn6 = new TeamName { Name = "Манчестер Юнайтед" };
            TeamName tn7 = new TeamName { Name = "Манчестер С" };
            TeamName tn8 = new TeamName { Name = "Маккаби Т. А." };
            TeamName tn9 = new TeamName { Name = "Краснодар" };

            SportType sport = new SportType { Name = "Football" };

            Team t1 = new Team { Name = tn1.Name, Names = new List<TeamName> { tn1 }, SportType = sport };
            Team t2 = new Team { Name = tn2.Name, Names = new List<TeamName> { tn2 }, SportType = sport };
            Team t3 = new Team { Name = tn3.Name, Names = new List<TeamName> { tn3 }, SportType = sport };
            Team t4 = new Team { Name = tn4.Name, Names = new List<TeamName> { tn4 }, SportType = sport };
            Team t5 = new Team { Name = tn5.Name, Names = new List<TeamName> { tn5 }, SportType = sport };
            Team t6 = new Team { Name = tn6.Name, Names = new List<TeamName> { tn6 }, SportType = sport };
            Team t7 = new Team { Name = tn7.Name, Names = new List<TeamName> { tn7 }, SportType = sport };
            Team t8 = new Team { Name = tn8.Name, Names = new List<TeamName> { tn8 }, SportType = sport };
            Team t9 = new Team { Name = tn9.Name, Names = new List<TeamName> { tn9 }, SportType = sport };

            SportEvent event1 = new SportEvent { SportType = sport, Date = DateTime.Now };
            SportEvent event2 = new SportEvent { SportType = sport, Date = DateTime.Now };
            SportEvent event3 = new SportEvent { SportType = sport, Date = DateTime.Now };
            SportEvent event4 = new SportEvent { SportType = sport, Date = DateTime.Now };
            SportEvent event5 = new SportEvent { SportType = sport, Date = DateTime.Now };

            Result result1 = new Result { SportEvent = event1, Team = t1, IsHome = true, Score = 1 };
            Result result2 = new Result { SportEvent = event1, Team = t2, IsHome = false, Score = 0 };
            Result result3 = new Result { SportEvent = event2, Team = t3, IsHome = true, Score = 1 };
            Result result4 = new Result { SportEvent = event2, Team = t4, IsHome = false, Score = 1 };
            Result result5 = new Result { SportEvent = event3, Team = t5, IsHome = true, Score = 3 };
            Result result6 = new Result { SportEvent = event3, Team = t6, IsHome = false, Score = 2 };
            Result result7 = new Result { SportEvent = event4, Team = t7, IsHome = true, Score = 0 };
            Result result8 = new Result { SportEvent = event4, Team = t8, IsHome = false, Score = 4 };
            Result result9 = new Result { SportEvent = event5, Team = t9, IsHome = false, Score = 0 };
            Result result10 = new Result { SportEvent = event5, Team = t1, IsHome = true, Score = 0 };

            unit.GetRepository<Result>().Insert(result1);
            unit.GetRepository<Result>().Insert(result2);
            unit.GetRepository<Result>().Insert(result3);
            unit.GetRepository<Result>().Insert(result4);
            unit.GetRepository<Result>().Insert(result5);
            unit.GetRepository<Result>().Insert(result6);
            unit.GetRepository<Result>().Insert(result7);
            unit.GetRepository<Result>().Insert(result8);
            unit.GetRepository<Result>().Insert(result9);
            unit.GetRepository<Result>().Insert(result10);

            unit.SaveChanges();
        }

        [SetUp]
        public void Init()
        {
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
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        public void CountCheckEventsExist(int sportId, int time)
        {
            Assert.AreNotEqual(this.databaseSportsContext.SportEvents.Count(), 0);

            int expectedCount = this.databaseSportsContext
                .SportEvents.Where(e => e.SportType.Id == sportId).Count();
            Assert.Greater(expectedCount, 0);

            int actualCount = this.eventService.Count(sportId, time);

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestCase(-1, 0)]
        [TestCase(4, 0)]
        [TestCase(9999, 0)]
        public void CountCheckEventsNotExist(int sportId, int time)
        {
            int expectedCount = this.databaseSportsContext
                .SportEvents.Where(e => e.SportType.Id == sportId).Count();
            Assert.AreEqual(expectedCount, 0);

            int actualCount = this.eventService.Count(sportId, time);
            Assert.AreEqual(actualCount, 0);
        }

        [Test]
        [TestCase(1, 0, 0, 10)]
        [TestCase(1, 0, 1, 3)]
        [TestCase(1, 0, 2, 1)]
        [TestCase(2, 0, 0, 10)]
        [TestCase(2, 0, 1, 1)]
        public void GetSportEventsCheckEventsExist(int sportId, int time, int skip, int take)
        {
            // Get SportEvents from datavase directly and check, that they are exist
            List<SportEvent> expectedSportEvents = this.databaseSportsContext.SportEvents
                .Where(e => e.SportType.Id == sportId)
                .OrderByDescending(e => e.Date).ThenByDescending(e => e.Id)
                .Skip(skip)
                .Take(take)
                .ToList();
            Assert.Greater(expectedSportEvents.Count, 0);

            // Map SportEvent to ResultDto
            List<ResultDto> expectedResultDtos = new List<ResultDto>();
            foreach (SportEvent item in expectedSportEvents)
            {
                expectedResultDtos
                    .Add(Mapper.Map<SportEvent, ResultDto>(item));
            }

            // Get SportEvents through EventsService
            List<ResultDto> actualResultDtos = this.eventService
                .GetSportEvents(sportId, time, skip, take).ToList();

            Assert.AreEqual(expectedResultDtos.Count, actualResultDtos.Count);
            for (int i = 0; i < expectedResultDtos.Count; i++)
            {
                Assert.AreEqual(expectedResultDtos[i].Id, actualResultDtos[i].Id);
                Assert.AreEqual(expectedResultDtos[i].Date, actualResultDtos[i].Date);
                Assert.AreEqual(
                    expectedResultDtos[i].TeamIds.Count(),
                    actualResultDtos[i].TeamIds.Count());
                Assert.AreEqual(
                    expectedResultDtos[i].TeamNames.Count(),
                    actualResultDtos[i].TeamIds.Count());

                for (int j = 0; j < expectedResultDtos[i].TeamIds.Count(); j++)
                {
                    Assert.AreEqual(
                        expectedResultDtos[i].TeamIds.ElementAt(j),
                        actualResultDtos[i].TeamIds.ElementAt(j));

                    Assert.AreEqual(
                        expectedResultDtos[i].TeamNames.ElementAt(j),
                        actualResultDtos[i].TeamNames.ElementAt(j));
                }
            }
        }

        [Test]
        [TestCase(1, 0, 10, 10)]
        [TestCase(2, 0, 10, 10)]
        [TestCase(3, 0, 0, 10)]
        [TestCase(3, 0, 1, 1)]
        [TestCase(3, 0, 10, 10)]
        [TestCase(4, 0, 0, 10)]
        [TestCase(4, 0, 1, 1)]
        [TestCase(4, 0, 10, 10)]
        public void GetSportEventsCheckEventsNotExist(int sportId, int time, int skip, int take)
        {
            // Get SportEvents from database directly and check, that they are not exist
            List<SportEvent> expectedSportEvents = this.databaseSportsContext.SportEvents
                .Where(e => e.SportType.Id == sportId)
                .OrderByDescending(e => e.Date)
                .ThenByDescending(e => e.Id)
                .Skip(skip)
                .Take(take)
                .ToList();
            Assert.AreEqual(0, expectedSportEvents.Count);

            // Get SportEvents from EventsService
            List<ResultDto> actualResultDtos = this.eventService
                .GetSportEvents(sportId, time, skip, take).ToList();
            Assert.AreEqual(expectedSportEvents.Count, actualResultDtos.Count);
        }

        [Test]
        public void CheckSportEventToResultDtoMapping()
        {
            List<ResultDto> expectedResultDtos = new List<ResultDto>();
            List<ResultDto> actualResultDtos = new List<ResultDto>();

            // SportEvents in database
            List<SportEvent> sportEvents = this.databaseSportsContext.SportEvents
                .Where(e => e.SportType.Id == 1)
                .OrderByDescending(e => e.Date)
                .ThenByDescending(e => e.Id)
                .Skip(0)
                .Take(10)
                .ToList();

            foreach (SportEvent sportEvent in sportEvents)
            {
                // Manual map SportEvent to ResultDto
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
                // Automapper 
                actualResultDtos.Add(Mapper.Map<SportEvent, ResultDto>(sportEvent));
            }

            CollectionAssert.AreEqual(expectedResultDtos, actualResultDtos, this.comparer);
        }

        [Test]
        public void ExistedTeamsSportEventsSaving()
        {
            ResultDTO r1 = new ResultDTO { TeamName = "Краснодар ФК", IsHome = true, Score = 1 };
            ResultDTO r2 = new ResultDTO { TeamName = "Шахтер Донецк", IsHome = false, Score = 1 };
            SportEventDTO ev = new SportEventDTO
                { Date = DateTime.Now.Ticks, SportType = "Football", Results = new List<ResultDTO> { r1, r2 } };
            List<SportEventDTO> events = new List<SportEventDTO>();
            events.Add(ev);

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            this.eventService.SaveSportEvents(events);

            Team team1 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Краснодар").Select(tn => tn.Team).FirstOrDefault();
            Team team2 =
                unit.GetRepository<TeamName>()
                    .Get(tn => tn.Name == "Краснодар ФК")
                    .Select(tn => tn.Team)
                    .FirstOrDefault();
            Assert.AreEqual(team1, team2);

            Team team3 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Шахтер Д").Select(tn => tn.Team).FirstOrDefault();
            Team team4 =
                unit.GetRepository<TeamName>()
                    .Get(tn => tn.Name == "Шахтер Донецк")
                    .Select(tn => tn.Team)
                    .FirstOrDefault();
            Assert.AreEqual(team3, team4);

            Result savedResult1 = unit.GetRepository<Result>().Get(r => r.Team.Id == team1.Id).FirstOrDefault();
            Assert.IsNotNull(savedResult1);

            Result savedResult2 = unit.GetRepository<Result>().Get(r => r.Team.Id == team2.Id).FirstOrDefault();
            Assert.IsNotNull(savedResult2);

            Result savedResult3 = unit.GetRepository<Result>().Get(r => r.Team.Id == team3.Id).FirstOrDefault();
            Assert.IsNotNull(savedResult2);

            Result savedResult4 = unit.GetRepository<Result>().Get(r => r.Team.Id == team4.Id).FirstOrDefault();
            Assert.IsNotNull(savedResult2);

            SportEvent savedEvent1 = unit.GetRepository<Result>()
                .Get(filter: r => r.SportEvent.Id == savedResult1.SportEvent.Id)
                .Select(r => r.SportEvent).FirstOrDefault();
            Assert.IsTrue(savedEvent1.Results.Contains(savedResult2));

            SportEvent savedEvent2 = unit.GetRepository<Result>()
                .Get(filter: r => r.SportEvent.Id == savedResult3.SportEvent.Id)
                .Select(r => r.SportEvent).FirstOrDefault();
            Assert.IsTrue(savedEvent2.Results.Contains(savedResult4));

            IEnumerable<TempResult> tempResults = unit.GetRepository<TempResult>().Get();
            Assert.IsEmpty(tempResults);

            IEnumerable<TempSportEvent> tempEvents = unit.GetRepository<TempSportEvent>().Get();
            Assert.IsEmpty(tempEvents);

            IEnumerable<Conformity> conformities = unit.GetRepository<Conformity>().Get();
            Assert.IsEmpty(conformities);
        }

        [Test]
        public void NullTeamsSportEventsSaving()
        {
            ResultDTO r1 = new ResultDTO { TeamName = "Ворскла", IsHome = false, Score = 0 };
            ResultDTO r2 = new ResultDTO { TeamName = "Металлист", IsHome = true, Score = 4 };
            SportEventDTO ev = new SportEventDTO
                { Date = DateTime.Now.Ticks, SportType = "Football", Results = new List<ResultDTO> { r1, r2 } };
            List<SportEventDTO> events = new List<SportEventDTO>();
            events.Add(ev);

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            this.eventService.SaveSportEvents(events);

            Team team1 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Металлист").Select(tn => tn.Team).FirstOrDefault();
            Assert.IsNotNull(team1);

            Team team2 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Ворскла").Select(tn => tn.Team).FirstOrDefault();
            Assert.IsNotNull(team2);

            Result savedResult1 = unit.GetRepository<Result>().Get(r => r.Team.Id == team1.Id).FirstOrDefault();
            Assert.IsNotNull(savedResult1);

            Result savedResult2 = unit.GetRepository<Result>().Get(r => r.Team.Id == team2.Id).FirstOrDefault();
            Assert.IsNotNull(savedResult2);

            SportEvent savedEvent1 = unit.GetRepository<Result>()
                .Get(filter: r => r.SportEvent.Id == savedResult1.SportEvent.Id)
                .Select(r => r.SportEvent).FirstOrDefault();
            Assert.IsTrue(savedEvent1.Results.Contains(savedResult2));

            IEnumerable<TempResult> tempResults = unit.GetRepository<TempResult>().Get();
            Assert.IsEmpty(tempResults);

            IEnumerable<TempSportEvent> tempEvents = unit.GetRepository<TempSportEvent>().Get();
            Assert.IsEmpty(tempEvents);

            IEnumerable<Conformity> conformities = unit.GetRepository<Conformity>().Get();
            Assert.IsEmpty(conformities);
        }

        [Test]
        public void BothConformitiesSportEventsSaving()
        {
            ResultDTO r1 = new ResultDTO { TeamName = "Маккаби Тель-Авив", IsHome = true, Score = 2 };
            ResultDTO r2 = new ResultDTO { TeamName = "Ст. Этьен", IsHome = false, Score = 1 };
            SportEventDTO ev = new SportEventDTO
                { Date = DateTime.Now.Ticks, SportType = "Football", Results = new List<ResultDTO> { r1, r2 } };
            List<SportEventDTO> events = new List<SportEventDTO>();
            events.Add(ev);

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            this.eventService.SaveSportEvents(events);

            Team team1 =
                unit.GetRepository<TeamName>()
                    .Get(tn => tn.Name == "Маккаби Тель-Авив")
                    .Select(tn => tn.Team)
                    .FirstOrDefault();
            Assert.IsNull(team1);

            Team team2 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Ст. Этьен").Select(tn => tn.Team).FirstOrDefault();
            Assert.IsNull(team2);

            IEnumerable<SportEvent> simpleEvents = unit.GetRepository<SportEvent>().Get(e => e.Results.Count() == 0);
            Assert.IsTrue(simpleEvents.Count() == 0);

            Conformity conf1 =
                unit.GetRepository<Conformity>().Get(c => c.InputName == "Маккаби Тель-Авив").FirstOrDefault();
            Assert.IsNotNull(conf1);

            Conformity conf2 = unit.GetRepository<Conformity>().Get(c => c.InputName == "Ст. Этьен").FirstOrDefault();
            Assert.IsNotNull(conf2);

            IEnumerable<TempResult> tempResults = unit.GetRepository<TempResult>().Get();
            Assert.IsTrue(tempResults.Count() == 2);

            IEnumerable<TempSportEvent> tempEvents = unit.GetRepository<TempSportEvent>().Get();
            Assert.IsTrue(tempEvents.Count() == 1);
        }

        [Test]
        public void OneConformitySportEventsSaving()
        {
            ResultDTO r1 = new ResultDTO { TeamName = "Динамо Киев", IsHome = true, Score = 3 };
            ResultDTO r2 = new ResultDTO { TeamName = "Ст. Этьен", IsHome = false, Score = 1 };
            SportEventDTO ev = new SportEventDTO
                { Date = DateTime.Now.Ticks, SportType = "Football", Results = new List<ResultDTO> { r1, r2 } };
            List<SportEventDTO> events = new List<SportEventDTO>();
            events.Add(ev);

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            this.eventService.SaveSportEvents(events);

            Team team1 =
                unit.GetRepository<TeamName>()
                    .Get(tn => tn.Name == "Динамо Киев")
                    .Select(tn => tn.Team)
                    .FirstOrDefault();
            Assert.IsNotNull(team1);

            Team team2 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Ст. Этьен").Select(tn => tn.Team).FirstOrDefault();
            Assert.IsNull(team2);

            IEnumerable<SportEvent> emptyEvents = unit.GetRepository<SportEvent>().Get(e => e.Results.Count() == 0);
            Assert.IsTrue(emptyEvents.Count() == 0);

            IEnumerable<SportEvent> oneResultEvents = unit.GetRepository<SportEvent>().Get(e => e.Results.Count() == 1);
            Assert.IsTrue(oneResultEvents.Count() == 0);

            Conformity conf2 = unit.GetRepository<Conformity>().Get(c => c.InputName == "Ст. Этьен").FirstOrDefault();
            Assert.IsTrue(conf2.TempResult != null);
            Assert.IsTrue(conf2.TempResult.Team == null);

            IEnumerable<TempResult> tempResults = unit.GetRepository<TempResult>().Get();
            Assert.IsTrue(tempResults.Count() == 2);

            TempResult tempResult1 = unit.GetRepository<TempResult>().Get(r => r.Team != null).FirstOrDefault();
            Assert.IsTrue(tempResult1.Team.Name == "Динамо Киев");

            Assert.IsTrue(conf2.TempResult.TempSportEvent.Id == tempResult1.TempSportEvent.Id);
        }

        [Test]
        public void DoubleConformitySportEventsSaving() //multiple conformities with the same inputName
        {
            ResultDTO r1 = new ResultDTO { TeamName = "Динамо", IsHome = true, Score = 0 };
            ResultDTO r2 = new ResultDTO { TeamName = "Манчестер Юнайтед", IsHome = false, Score = 1 };
            SportEventDTO ev1 = new SportEventDTO
                { Date = DateTime.Now.Ticks, SportType = "Football", Results = new List<ResultDTO> { r1, r2 } };
            List<SportEventDTO> events = new List<SportEventDTO>();
            events.Add(ev1);

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            this.eventService.SaveSportEvents(events);

            Team team1 =
                unit.GetRepository<TeamName>().Get(tn => tn.Name == "Динамо").Select(tn => tn.Team).FirstOrDefault();
            Assert.IsNull(team1);

            Team team2 =
                unit.GetRepository<TeamName>()
                    .Get(tn => tn.Name == "Манчестер Юнайтед")
                    .Select(tn => tn.Team)
                    .FirstOrDefault();
            Assert.IsNotNull(team2);

            IEnumerable<SportEvent> simpleEvents = unit.GetRepository<SportEvent>().Get(e => e.Results.Count() == 0);
            Assert.IsTrue(simpleEvents.Count() == 0);

            IEnumerable<Conformity> confs = unit.GetRepository<Conformity>().Get(c => c.InputName == "Динамо");
            Assert.IsTrue(confs.Count() == 2);

            IEnumerable<TempResult> tempResults = unit.GetRepository<TempResult>().Get();
            Assert.IsTrue(tempResults.Count() == 2);

            IEnumerable<TempSportEvent> tempEvents = unit.GetRepository<TempSportEvent>().Get();
            Assert.IsTrue(tempEvents.Count() == 1);
        }
    }
}