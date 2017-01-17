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
            this.sportType = new SportType { Id = 0, Name = "Football" };

            Team team1 = new Team { Id = 0, Name = "Team_1", SportType = this.sportType };
            Team team2 = new Team { Id = 1, Name = "Team_2", SportType = this.sportType };
            Team team3 = new Team { Id = 2, Name = "Team_3", SportType = this.sportType };

            this.res1 = new Result { Id = 0, Team = team1, Score = 3 };
            this.res2 = new Result { Id = 1, Team = team2, Score = 1 };
            this.res3 = new Result { Id = 2, Team = team3, Score = 5 };

            this.sportEvent1 = new SportEvent { Id = 0 };
            this.sportEvent1.Results = new List<Result>();

            this.sportEvent2 = new SportEvent { Id = 1 };
            this.sportEvent2.Results = new List<Result>();

            this.sportEvent3 = new SportEvent { Id = 2 };
            this.sportEvent3.Results = new List<Result>();

            DateTime date = new DateTime(2016, 11, 21, 16, 30, 0);

            this.sportEvent1.Date = date;
            this.sportEvent2.Date = date;
            this.sportEvent3.Date = date;

            this.sportEvent1.SportType = this.sportType;
            this.sportEvent2.SportType = this.sportType;
            this.sportEvent3.SportType = this.sportType;

            this.sportEvent1.Results.Add(this.res1);
            this.sportEvent1.Results.Add(this.res2);
            this.sportEvent2.Results.Add(this.res1);
            this.sportEvent2.Results.Add(this.res2);

            this.sportEvent3.Results.Add(this.res1);
            this.sportEvent3.Results.Add(this.res3);
        }

        [Test]
        public void CreateAndAddResultsCheckExceptionNotThrow()
        {
            Assert.DoesNotThrow(() =>
            {
                this.sportEvent = new SportEvent();
                this.sportEvent.Results = new List<Result>();

                this.sportEvent.Id = 0;
                this.sportEvent.Date = new DateTime(2016, 11, 21, 16, 30, 0);
                this.sportEvent.SportType = this.sportType;
                this.sportEvent.Results.Add(this.res1);
                this.sportEvent.Results.Add(this.res2);
            });
        }

        [Test]
        public void GetHashCodeCheckHashIsCorrect()
        {
            this.sportEvent = new SportEvent();
            this.sportEvent.Results = new List<Result>();

            DateTime date = new DateTime(2016, 11, 21, 16, 30, 0);

            this.sportEvent.Date = date;
            this.sportEvent.SportType = this.sportType;
            this.sportEvent.Results.Add(this.res1);
            this.sportEvent.Results.Add(this.res2);

            int hashCheck = date.GetHashCode() ^ this.sportType.Name.GetHashCode() ^ this.res1.Team.Name.GetHashCode() ^ this.res2.Team.Name.GetHashCode();
            int hashCheckReverce = date.GetHashCode() ^ this.sportType.Name.GetHashCode() ^ this.res2.Team.Name.GetHashCode() ^ this.res1.Team.Name.GetHashCode();

            Assert.IsTrue(hashCheck == this.sportEvent.GetHashCode() && hashCheckReverce == this.sportEvent.GetHashCode());
        }

        [Test]
        public void GetHashCodeCheckHashCodeSameInstances()
        {
            Assert.IsTrue(this.sportEvent1.GetHashCode() == this.sportEvent2.GetHashCode());
        }

        [Test]
        public void GetHashCodeCheckHashCodeDifferentInstances()
        {
            Assert.IsFalse(this.sportEvent1.GetHashCode() == this.sportEvent3.GetHashCode());
        }

        [Test]
        public void CheckEquals()
        {
            Assert.IsTrue(this.sportEvent1.Equals(this.sportEvent2));
        }

        [Test]
        public void CheckNotEquals()
        {
            Assert.IsFalse(this.sportEvent1.Equals(this.sportEvent3));
        }

        [Test]
        public void CheckContractEquals()
        {
            Assert.IsTrue(this.sportEvent1.Equals(this.sportEvent2) && this.sportEvent2.Equals(this.sportEvent1) 
                          && (this.sportEvent1.GetHashCode() == this.sportEvent2.GetHashCode()));
        }

        [Test]
        public void CheckContractNotEquals()
        {
            Assert.IsFalse(this.sportEvent1.Equals(this.sportEvent3) || this.sportEvent3.Equals(this.sportEvent1)
                          || (this.sportEvent1.GetHashCode() == this.sportEvent3.GetHashCode()));
        }

        [Test]
        public void CheckNullArgument()
        {
            SportEvent nullEvent = null;
            Assert.IsFalse(this.sportEvent1.Equals(nullEvent) || this.sportEvent1.Equals((object)nullEvent));
        }

        [Test]
        public void CheckNullFields()
        {
            SportEvent nullDateEvent = new SportEvent();
            nullDateEvent.SportType = this.sportType;
            nullDateEvent.Results = new List<Result>();
            nullDateEvent.Results.Add(this.res1);
            nullDateEvent.Results.Add(this.res2);

            SportEvent nullSportTypeEvent = new SportEvent();
            nullSportTypeEvent.Date = new DateTime(2016, 11, 21, 16, 30, 0);
            nullSportTypeEvent.Results = new List<Result>();
            nullSportTypeEvent.Results.Add(this.res1);
            nullSportTypeEvent.Results.Add(this.res2);

            SportEvent nullResultsEvent = new SportEvent();
            nullResultsEvent.Date = new DateTime(2016, 11, 21, 16, 30, 0);
            nullResultsEvent.SportType = this.sportType;

            Assert.IsFalse(this.sportEvent1.Equals(nullDateEvent) || nullDateEvent.Equals(this.sportEvent1)
                           || this.sportEvent1.Equals(nullSportTypeEvent) || nullSportTypeEvent.Equals(this.sportEvent1)
                           || this.sportEvent1.Equals(nullResultsEvent) || nullResultsEvent.Equals(this.sportEvent1));
        }

        [Test]
        public void CheckNullResults()
        {
            SportEvent nullResultsEvent = new SportEvent();
            nullResultsEvent.Date = new DateTime(2016, 11, 21, 16, 30, 0);
            nullResultsEvent.SportType = this.sportType;
            nullResultsEvent.Results = new List<Result>();
            nullResultsEvent.Results.Add(this.res1);
            nullResultsEvent.Results.Add(null);

            Assert.IsFalse(this.sportEvent1.Equals(nullResultsEvent) || nullResultsEvent.Equals(this.sportEvent1));
        }

        [Test]
        public void CheckSetContainsDifferent()
        {
            ICollection<SportEvent> set = new HashSet<SportEvent>();
            set.Add(this.sportEvent1);
            set.Add(this.sportEvent3);

            Assert.IsTrue(set.Contains(this.sportEvent1) && set.Contains(this.sportEvent3) && (set.Count == 2));
        }

        [Test]
        public void CheckSetNotContainsSame()
        {
            ICollection<SportEvent> set = new HashSet<SportEvent>();
            set.Add(this.sportEvent1);
            set.Add(this.sportEvent2);
            
            Assert.IsTrue(set.Contains(this.sportEvent1) && (set.Count == 1));
        }
    }
}
