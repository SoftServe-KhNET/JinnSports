using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class TeamServiceTests
    {
        /// <summary>
        /// TeamDto's in database
        /// </summary>
        private List<TeamDto> databaseTeams;

        private ITeamService teamService;

        /// <summary>
        /// TeamDto comparer for comparing collections
        /// </summary>
        private TeamDtoComparer comparer;

        /// <summary>
        /// Init test data
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            this.databaseTeams = new List<TeamDto>();
            this.comparer = new TeamDtoComparer();
            this.teamService = new TeamService(new EFUnitOfWork(new SportsContext("SportsContext")));

            this.databaseTeams.Add(new TeamDto()
            {
                Id = 1,
                Name = "Manchester United"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 2,
                Name = "Milano"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 3,
                Name = "Manchester City"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 4,
                Name = "Chelsea"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 5,
                Name = "Bayern"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 6,
                Name = "Chicago Bulls"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 7,
                Name = "Los Angeles Lakers"
            });
            this.databaseTeams.Add(new TeamDto()
            {
                Id = 8,
                Name = "Phoenix Suns"
            });
        }

        /// <summary>
        /// Check team counts
        /// </summary>
        [Test]
        public void TeamCount()
        {
            int actualCount = this.teamService.Count();
            int expectedCount = this.databaseTeams.Count();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        [TestCase(0, 4)]
        [TestCase(0, 10)]
        [TestCase(4, 2)]
        [TestCase(4, 10)]
        [TestCase(10, 0)]
        public void GetTeams(int skip, int take)
        {
            List<TeamDto> expectedTeams = this.databaseTeams.OrderBy(t => t.Id)
                .Skip(skip)
                .Take(take)
                .ToList();
            List<TeamDto> actualTeams = this.teamService.GetAllTeams(skip, take).ToList();

            CollectionAssert.AreEqual(expectedTeams, actualTeams, this.comparer);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void GetTeamById(int teamId)
        {
            TeamDto expeactedTeam = this.databaseTeams.Where(t => t.Id == teamId).FirstOrDefault();
            TeamDto actualTeam = this.teamService.GetTeamById(teamId);
            if (expeactedTeam == null)
            {
                Assert.AreEqual(expeactedTeam, actualTeam);
            }
            else
            {
                Assert.AreEqual(expeactedTeam.Id, actualTeam.Id);
                Assert.AreEqual(expeactedTeam.Name, actualTeam.Name);
            }
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
