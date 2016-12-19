using System;
using JinnSports.WEB.Controllers;
using System.Web.Mvc;
using NUnit.Framework;

namespace JinnSports.UnitTests.WEB
{
    [TestFixture]
    public class TeamDetailsTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            TeamDetailsController controller = new TeamDetailsController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void Details()
        {
            // Arrange
            TeamDetailsController controller = new TeamDetailsController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
