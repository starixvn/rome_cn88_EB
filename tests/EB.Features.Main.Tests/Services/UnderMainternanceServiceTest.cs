using EB.Features.Main.Services;
using NSubstitute;
using NUnit.Framework;
using ONEbook.UM.Models;
using ONEbook.UM.Services;

namespace EB.Features.Main.Tests.Services
{
    public class UnderMainternanceServiceTest
    {
        private IMaintenanceService onebookMaintenanceService;

        private IUnderMainternanceService underMainternanceService;

        [SetUp]
        public void SetUp()
        {
            onebookMaintenanceService = Substitute.For<IMaintenanceService>();
            underMainternanceService = new UnderMainternanceService(onebookMaintenanceService);
        }

        [Test]
        public void UnderMainternanceInfo_Always_CallOnebookService()
        {
            // Arrange
            var UMInfo = new UnderMaintenanceTime
            {
                ConnectionResult = new ConnectionResult(true, "OK")
            };
            onebookMaintenanceService.GetUnderMaintenanceTime()
                .Returns(UMInfo);

            // Act
            underMainternanceService.UnderMainternanceInfo();

            // Assert
            onebookMaintenanceService.Received().GetUnderMaintenanceTime();
        }


        [Test]
        public void UnderMainternanceInfo_Always_GetInfoFromOnebookService()
        {
            // Arrange
            var UMInfo = new UnderMaintenanceTime
            {
                ConnectionResult = new ConnectionResult(true, "OK"),
                From = "abc",
                To = "xyz",
                IsUnderMaintenanceTime = true
            };
            onebookMaintenanceService.GetUnderMaintenanceTime()
                .Returns(UMInfo);
            onebookMaintenanceService.IsUnderMaintenance().Returns(true);

            // Act
            var act = underMainternanceService.UnderMainternanceInfo();

            // Assert
            Assert.AreEqual(UMInfo.ConnectionResult.IsOk, act.IsUM);
            Assert.AreEqual(UMInfo.From, act.FromTimeText);
            Assert.AreEqual(UMInfo.To, act.ToTimeText);
        }
    }
}
