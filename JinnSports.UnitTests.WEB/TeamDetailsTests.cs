using JinnSports.WEB.Controllers;
using System.Web.Mvc;
using JinnSports.BLL.Service;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using NUnit.Framework;
using JinnSports.WEB;

namespace JinnSports.UnitTests.WEB
{
    [TestFixture]
    public class TeamDetailsTest
    {
        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            AutofacConfig.Configure();
        }
        [Test]
        public void Details([Values(0, 1)] int id)
        {
            // Arrange
            TeamDetailsController controller = new TeamDetailsController(
                new TeamService(new EFUnitOfWork(new SportsContext("SportsContext"))),
                new ChartService(new EFUnitOfWork(new SportsContext("SportsContext"))));

            // Act
            ActionResult result = controller.Details(id);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}