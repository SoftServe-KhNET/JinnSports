using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using System.Transactions;
using JinnSports.Entities.Entities;
using JinnSports.WEB;
using AutoMapper;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class TeamServiceTests
    {
        private ITeamService teamService;

        private SportsContext databaseSportsContext;

        private TransactionScope databaseTransaction;

        /// <summary>
        /// TeamDto comparer for comparing collections
        /// </summary>
        private TeamDtoComparer comparer;

        /// <summary>
        /// Init test data
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeInit()
        { 
            this.comparer = new TeamDtoComparer();
            AutoMapperConfiguration.Configure();

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

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[SportTypes] ON;
                INSERT INTO [dbo].[SportTypes] ([Id], [Name])
                VALUES
                (1, 'Football'),
                (2, 'Basketball');               
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

            this.databaseSportsContext.SaveChanges();
            this.teamService = new TeamService(new EFUnitOfWork(this.databaseSportsContext));
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

        /// <summary>
        /// Check team counts
        /// </summary>
        [Test]
        public void CountCheckTeamsExist()
        {
            int expectedCount = this.databaseSportsContext.Teams.Count();
            Assert.Greater(expectedCount, 0);

            int actualCount = this.teamService.Count();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        [TestCase(0, 4)]
        [TestCase(0, 10)]
        [TestCase(4, 2)]
        [TestCase(4, 10)]        
        public void GetAllTeamsCheckTeamsExist(int skip, int take)
        {
            // Get teams from database and check, that they are exist
            IQueryable<Team> teams = this.databaseSportsContext.Teams
                .OrderBy(t => t.Id)
                .Skip(skip)
                .Take(take);
            List<TeamDto> expectedTeams = new List<TeamDto>();
            foreach (Team team in teams)
            {
                expectedTeams.Add(Mapper.Map<Team, TeamDto>(team));
            } 
            Assert.Greater(expectedTeams.Count, 0);

            List<TeamDto> actualTeams = this.teamService.GetAllTeams(skip, take).ToList();

            CollectionAssert.AreEqual(expectedTeams, actualTeams, this.comparer);
        }

        [Test]
        [TestCase(100, 0)]
        public void GetAllTeamsCheckTeamsNotExist(int skip, int take)
        {
            // Get teams from database and check, that we don't have them
            int expectedCount = this.databaseSportsContext
                .Teams.OrderBy(t => t.Id)
                .Skip(skip)
                .Take(take)
                .Count();
            Assert.AreEqual(expectedCount, 0);

            List<TeamDto> actualTeams = this.teamService.GetAllTeams(skip, take).ToList();

            Assert.AreEqual(0, actualTeams.Count);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]        
        public void GetTeamByIdCheckTeamExist(int teamId)
        {
            Team databaseTeam = this.databaseSportsContext.Teams.Where(t => t.Id == teamId).First();
            Assert.IsNotNull(databaseTeam);

            TeamDto expectedTeam = new TeamDto()
            {
                Id = databaseTeam.Id,
                Name = databaseTeam.Name
            };

            TeamDto actualTeam = this.teamService.GetTeamById(teamId);

            Assert.AreEqual(expectedTeam.Id, actualTeam.Id);
            Assert.AreEqual(expectedTeam.Name, actualTeam.Name);           
        }

        [Test]
        [TestCase(-1)]
        public void GetTeamByIdCheckTeamNotExist(int teamId)
        {
            Team databaseTeam = this.databaseSportsContext.Teams.Where(t => t.Id == teamId).FirstOrDefault();
            Assert.IsNull(databaseTeam);

            TeamDto actualTeam = this.teamService.GetTeamById(teamId);

            Assert.IsNull(actualTeam);
        }


        /// <summary>
        /// Comparer for comparing TeamDto collections 
        /// </summary>
        public class TeamDtoComparer : IComparer, IComparer<TeamDto>
        {
            public int Compare(object x, object y)
            {
                TeamDto left = x as TeamDto;
                TeamDto right = y as TeamDto;
                if (left == null || right == null)
                {
                    throw new InvalidOperationException();
                }
                return this.Compare(left, right);
            }

            public int Compare(TeamDto x, TeamDto y)
            {
                if (x.Id == y.Id && x.Name == y.Name)
                {
                    return 0;
                }
                if (x.Id < y.Id)
                {
                    return -1;
                }
                return 1;
            }
        }
    }
}
