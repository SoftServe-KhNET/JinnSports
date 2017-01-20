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
using System.Data;
using System.Data.Entity;
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

        private TeamDetailsServiceTests.ResultDtoComparer comparer;

        private SportsContext databaseSportsContext;

        private DbContextTransaction databaseTransaction;

        [OneTimeSetUp]
        public void Init()
        {
            this.databaseSportsContext = new SportsContext("SportsContext");

            // Other transactions can't update and insert data
            this.databaseTransaction = this.databaseSportsContext
                .Database.BeginTransaction(IsolationLevel.ReadUncommitted);

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
            this.comparer = new TeamDetailsServiceTests.ResultDtoComparer();            

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
        }

        [OneTimeTearDown]
        public void Clean()
        {
            // Pend changes
            this.databaseTransaction.Rollback();
            this.databaseTransaction.Dispose();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]                
        public void CountCheckEventsExist(int sportId)
        {
            Assert.AreNotEqual(this.databaseSportsContext.SportEvents.Count(), 0);

            int expectedCount = this.databaseSportsContext
                .SportEvents.Where(e => e.SportType.Id == sportId).Count();
            Assert.Greater(expectedCount, 0);
                           
            int actualCount = this.eventService.Count(sportId);

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestCase(-1)]
        [TestCase(4)]
        [TestCase(9999)]
        public void CountCheckEventsNotExist(int sportId)
        {
            int expectedCount = this.databaseSportsContext
                .SportEvents.Where(e => e.SportType.Id == sportId).Count();
            Assert.AreEqual(expectedCount, 0);

            int actualCount = this.eventService.Count(sportId);
            Assert.AreEqual(actualCount, 0);
        }

        [Test]
        [TestCase(1, 0, 10)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 2, 1)]        
        [TestCase(2, 0, 10)]
        [TestCase(2, 1, 1)]        
        public void GetSportEventsCheckEventsExist(int sportId, int skip, int take)
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
                .GetSportEvents(sportId, skip, take).ToList();

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
        [TestCase(1, 10, 10)]
        [TestCase(2, 10, 10)]
        [TestCase(3, 0, 10)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 10, 10)]
        [TestCase(4, 0, 10)]
        [TestCase(4, 1, 1)]
        [TestCase(4, 10, 10)]
        public void GetSportEventsCheckEventsNotExist(int sportId, int skip, int take)
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
                .GetSportEvents(sportId, skip, take).ToList();
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
    }
}
