using System.Collections.Generic;
using System.Transactions;
using JinnSports.BLL.Dtos.Charts;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JinnSports.UnitTests.Services
{
    [TestFixture]
    public class ChartServiceTests
    {
        private IChartService chartService;
        
        private SportsContext databaseSportsContext;

        private TransactionScope databaseTransaction;

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

            this.chartService = new ChartService(new EFUnitOfWork(this.databaseSportsContext));

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[SportTypes] ON;
                INSERT INTO [dbo].[SportTypes] ([Id], [Name])
                VALUES
                (1, 'Football');               
                SET IDENTITY_INSERT [dbo].[SportTypes] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[Teams] ON;
                INSERT INTO [dbo].[Teams] ([Id], [Name], [SportType_Id])
                VALUES
                (1, 'Manchester United', 1),
                (2, 'Milano', 1);               
                SET IDENTITY_INSERT [dbo].[Teams] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[TeamNames] ON;
                INSERT INTO [dbo].[TeamNames] ([Id], [Name], [Team_Id])
                VALUES
                (1, 'Manchester United', 1),
                (2, 'Milano', 2);               
                SET IDENTITY_INSERT [dbo].[TeamNames] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[SportEvents] ON;
                INSERT INTO [dbo].[SportEvents] ([Id], [Date], [SportType_Id])
                VALUES
                (1, '20161119 17:00', 1),
                (2, '20161120 17:00', 1),
                (3, '20161121 18:00', 1),
                (4, '20161122 16:00', 1),
                (5, '20161123 16:00', 1);
                SET IDENTITY_INSERT [dbo].[SportEvents] OFF;");

            this.databaseSportsContext.Database.ExecuteSqlCommand(
                @"SET IDENTITY_INSERT [dbo].[Results] ON;
                INSERT INTO [dbo].[Results] ([Id], [Score], [SportEvent_Id], [Team_Id])
                VALUES
                (1, 3, 1, 1),
                (2, 1, 1, 2),
                (3, 5, 2, 1),
                (4, 3, 2, 2),
                (5, 4, 3, 1),
                (6, 2, 3, 2),
                (7, 1, 4, 1),
                (8, 4, 4, 2),
                (9, 0, 5, 1),
                (10, 2, 5, 2);               
                SET IDENTITY_INSERT [dbo].[Results] OFF;");

            this.databaseSportsContext.SaveChanges();

            this.chartService = new ChartService(new EFUnitOfWork(this.databaseSportsContext));
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
        public void Clear()
        {
            this.databaseTransaction.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeClear()
        {
            this.databaseSportsContext.Dispose();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void GetDataTableForTeamCheckCorrectResult(int id)
        {
            GoogleVisualizationDataTable dataTableForFirstTeam = new GoogleVisualizationDataTable();
            dataTableForFirstTeam.AddColumn("Date", "string");
            dataTableForFirstTeam.AddColumn("WinRate", "number");
            dataTableForFirstTeam.AddRow(new List<object> { "19.11.2016", 100 });
            dataTableForFirstTeam.AddRow(new List<object> { "20.11.2016", 100 });
            dataTableForFirstTeam.AddRow(new List<object> { "21.11.2016", 100 });
            dataTableForFirstTeam.AddRow(new List<object> { "22.11.2016", 75 });
            dataTableForFirstTeam.AddRow(new List<object> { "23.11.2016", 60 });

            GoogleVisualizationDataTable dataTableForSecondTeam = new GoogleVisualizationDataTable();
            dataTableForSecondTeam.AddColumn("Date", "string");
            dataTableForSecondTeam.AddColumn("WinRate", "number");
            dataTableForSecondTeam.AddRow(new List<object> { "19.11.2016", 0 });
            dataTableForSecondTeam.AddRow(new List<object> { "20.11.2016", 0 });
            dataTableForSecondTeam.AddRow(new List<object> { "21.11.2016", 0 });
            dataTableForSecondTeam.AddRow(new List<object> { "22.11.2016", 25 });
            dataTableForSecondTeam.AddRow(new List<object> { "23.11.2016", 40 });
            
            var res = this.chartService.GetDataTableForTeam(id);
            string recievedResult = JsonConvert.SerializeObject(res);

            if (id == 1)
            {
                string expectedResult = JsonConvert.SerializeObject(dataTableForFirstTeam);
                Assert.AreEqual(expectedResult, recievedResult);
            }
            else if (id == 2)
            {
                string expectedResult = JsonConvert.SerializeObject(dataTableForSecondTeam);
                Assert.AreEqual(expectedResult, recievedResult);
            }
        }
    }
}