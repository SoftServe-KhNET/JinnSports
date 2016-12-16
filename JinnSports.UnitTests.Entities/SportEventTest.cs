using JinnSports.Entities.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JinnSports.UnitTests.Entities
{
    [TestFixture]
    public class SportEventTest
    {
        private SportType sportType;
        private Result res1, res2, res3;
        private SportEvent sportEvent, sportEvent1, sportEvent2, sportEvent3;

        [SetUp]
        public void Init()
        {
            sportType = new SportType { Id = 0, Name = "Football" };

            Team team1 = new Team { Id = 0, Name = "Team_1", SportType = sportType };
            Team team2 = new Team { Id = 1, Name = "Team_2", SportType = sportType };
            Team team3 = new Team { Id = 2, Name = "Team_3", SportType = sportType };

            res1 = new Result { Id = 0, Team = team1, Score = 3 };
            res2 = new Result { Id = 1, Team = team2, Score = 1 };
            res3 = new Result { Id = 2, Team = team3, Score = 5 };

            sportEvent1 = new SportEvent { Id = 0 };
            sportEvent1.Results = new List<Result>();

            sportEvent2 = new SportEvent { Id = 1 };
            sportEvent2.Results = new List<Result>();

            sportEvent3 = new SportEvent { Id = 2 };
            sportEvent3.Results = new List<Result>();

            DateTime date = new DateTime(2016, 11, 21, 16, 30, 0);

            sportEvent1.Date = date;
            sportEvent2.Date = date;
            sportEvent3.Date = date;

            sportEvent1.SportType = sportType;
            sportEvent2.SportType = sportType;
            sportEvent3.SportType = sportType;

            sportEvent1.Results.Add(res1);
            sportEvent1.Results.Add(res2);
            sportEvent2.Results.Add(res1);
            sportEvent2.Results.Add(res2);

            sportEvent3.Results.Add(res1);
            sportEvent3.Results.Add(res3);
        }

        [Test]
        public void CreateAndAddResults()
        {
            Assert.DoesNotThrow(() => 
            {
                sportEvent = new SportEvent();
                sportEvent.Results = new List<Result>();

                sportEvent.Id = 0;
                sportEvent.Date = new DateTime(2016, 11, 21, 16, 30, 0);
                sportEvent.SportType = sportType;
                sportEvent.Results.Add(res1);
                sportEvent.Results.Add(res2);
            });
        }

        [Test]
        public void CheckHashIsCorrect()
        {
            sportEvent = new SportEvent();
            sportEvent.Results = new List<Result>();

            DateTime date = new DateTime(2016, 11, 21, 16, 30, 0);

            sportEvent.Date = date;
            sportEvent.SportType = sportType;
            sportEvent.Results.Add(res1);
            sportEvent.Results.Add(res2);

            int hashCheck = date.GetHashCode() ^ sportType.Name.GetHashCode() ^ res1.Team.Name.GetHashCode() ^ res2.Team.Name.GetHashCode();
            int hashCheckReverce = date.GetHashCode() ^ sportType.Name.GetHashCode() ^ res2.Team.Name.GetHashCode() ^ res1.Team.Name.GetHashCode();

            Assert.IsTrue(hashCheck == sportEvent.GetHashCode() && hashCheckReverce == sportEvent.GetHashCode());
        }

        [Test]
        public void CheckHashCodeSameInstances()
        {
            Assert.IsTrue(sportEvent1.GetHashCode() == sportEvent2.GetHashCode());
        }

        [Test]
        public void CheckHashCodeNotSameInstances()
        {
            Assert.IsFalse(sportEvent1.GetHashCode() == sportEvent3.GetHashCode());
        }

        [Test]
        public void CheckEquals()
        {
            Assert.IsTrue(sportEvent1.Equals(sportEvent2) && sportEvent2.Equals(sportEvent1));
        }

        [Test]
        public void CheckNotEquals()
        {
            Assert.IsFalse(sportEvent1.Equals(sportEvent3) && sportEvent3.Equals(sportEvent1));
        }

        [Test]
        public void CheckSetContainsDifferent()
        {
            ICollection<SportEvent> set = new HashSet<SportEvent>();
            set.Add(sportEvent1);
            set.Add(sportEvent3);

            Assert.IsTrue(set.Contains(sportEvent1) && set.Contains(sportEvent3) && (set.Count == 2));
        }

        [Test]
        public void CheckSetNotContainsSame()
        {
            ICollection<SportEvent> set = new HashSet<SportEvent>();
            set.Add(sportEvent1);
            set.Add(sportEvent2);
            
            Assert.IsTrue(set.Contains(sportEvent1) && (set.Count == 1));
        }
    }
}
