using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using JinnSports.BLL.Matcher;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using JinnSports.Entities.Entities;
using NUnit.Framework;

namespace JinnSports.UnitTests.BLL
{
    [TestFixture]
    public class NamingMatcherTests
    {
        private SportsContext databaseSportsContext;

        private DbContextTransaction databaseTransaction;

        [OneTimeSetUp]
        public void Init()
        {
            this.databaseSportsContext = new SportsContext("SportsContext");

            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);            

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

            TeamName tn1 = new TeamName { Name = "Шахтер Д" };
            TeamName tn2 = new TeamName { Name = "Динамо Бухарест" };
            TeamName tn3 = new TeamName { Name = "Динамо Киев" };
            TeamName tn4 = new TeamName { Name = "Сент-Этьен" };
            TeamName tn5 = new TeamName { Name = "Заря ЛГ" };
            TeamName tn6 = new TeamName { Name = "Манчестер Юнайтед" };
            TeamName tn7 = new TeamName { Name = "Манчестер С" };
            TeamName tn8 = new TeamName { Name = "Шальке-04" };
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

        [Test]
        public void ResolveNamingPositiveTest()
        {
            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            SportType football = new SportType { Name = "Football" };
            SportType sport = unit.GetRepository<SportType>().Get(x => x.Name.ToUpper() == football.Name.ToUpper()).FirstOrDefault();           

            Team positiveTeam = new Team { Name = "Краснодар ФК", SportType = sport, Names = new List<TeamName>() };  

            NamingMatcher matcher = new NamingMatcher(unit);

            List<Conformity> positiveConformities = matcher.ResolveNaming(positiveTeam);

            Assert.IsNull(positiveConformities);
            TeamName name = new TeamName { Name = positiveTeam.Name };
            Team team = unit.GetRepository<Team>().Get((x) => x.Names.Select(n => n.Name).Contains(name.Name)).FirstOrDefault();                  
            Assert.IsTrue(team.Names.Select(t => t.Name).Contains("Краснодар"));
            Assert.IsTrue(team.Names.Select(t => t.Name).Contains("Краснодар ФК"));
        }

        [Test]
        public void ResolveNamingNegativeTest()
        {
            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            SportType football = new SportType { Name = "Football" };
            SportType sport = unit.GetRepository<SportType>().Get(x => x.Name.ToUpper() == football.Name.ToUpper()).FirstOrDefault();

            Team negativeTeam = new Team { Name = "Металлист", SportType = sport, Names = new List<TeamName>() };

            NamingMatcher matcher = new NamingMatcher(unit);

            List<Conformity> negativeConformities = matcher.ResolveNaming(negativeTeam);

            Assert.IsNull(negativeConformities);
            TeamName name = new TeamName { Name = negativeTeam.Name };
            Team team = team = unit.GetRepository<Team>().Get((x) => x.Names.Select(n => n.Name).Contains(name.Name)).FirstOrDefault();
            Assert.AreEqual(team, negativeTeam);
        }

        [Test]
        public void ResolveNamingNeutralTest()
        {
            IUnitOfWork unit = new EFUnitOfWork(this.databaseSportsContext);

            SportType football = new SportType { Name = "Football" };
            SportType sport = unit.GetRepository<SportType>().Get(x => x.Name.ToUpper() == football.Name.ToUpper()).FirstOrDefault();

            Conformity expConf = new Conformity { InputName = "Ст. Этьен", ExistedName = "Сент-Этьен", IsConfirmed = false };

            Team neutralTeam = new Team { Name = "Ст. Этьен", SportType = sport };

            NamingMatcher matcher = new NamingMatcher(unit);

            List<Conformity> neutralConformities = matcher.ResolveNaming(neutralTeam);

            Conformity newConf = neutralConformities.FirstOrDefault();

            Assert.AreEqual(expConf.InputName, newConf.InputName);
            Assert.AreEqual(expConf.ExistedName, newConf.ExistedName);
        }

        [OneTimeTearDown]
        public void Clean()
        {
            // Pend changes
            this.databaseTransaction.Rollback();
            this.databaseTransaction.Dispose();
        }
    }
}