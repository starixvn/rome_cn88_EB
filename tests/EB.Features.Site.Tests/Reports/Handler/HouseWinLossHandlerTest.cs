using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Params;
using EB.Features.Reports.Resources;
using EB.Features.Reports.Services;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Shared.Models;
using Fanex.NAP.Core.Web.Session;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.Handler
{
    public class HouseWinLossHandlerTest
    {
        private ISportGroupService sportGroupService;
        private IBettypeGroupService betTypeService;
        private IHouseWinLossService houseWinLossService;
        private ICacheRefService cacheRefService;
        private HouseWinLossHandler hander;
        private UserSession currentSession;

        [SetUp]
        public void Setup()
        {
            sportGroupService = Substitute.For<ISportGroupService>();
            betTypeService = Substitute.For<IBettypeGroupService>();
            houseWinLossService = Substitute.For<IHouseWinLossService>();
            cacheRefService = Substitute.For<ICacheRefService>();
            currentSession = Substitute.For<UserSession>();
            hander = new HouseWinLossHandler(sportGroupService, betTypeService, houseWinLossService, currentSession);
        }

        [Test]
        public void HandleReportLegendQuery_Always_ReturnExpectedList()
        {
            // Arrange


            // Act
            var act = hander.Handle(new ReportLegendQuery(currentSession)).ToList();

            // Assert
            Assert.AreEqual(2, act.Count);
            Assert.AreEqual(ReportResources.InaccurateDataLegend, act[0]);
            Assert.AreEqual(ReportResources.CompareAccuratedDataLegend, act[1]);
        }

        [Test]
        public void Handle_Always_CallGetSportGroups()
        {
            // Arrange


            // Act
            hander.Handle(new HouseWinLossQuery(currentSession));

            // Assert
            sportGroupService.Received().GetSportGroups();
        }


        [Test]
        public void Handle_Always_CallGetBetTypeGroupsBySport()
        {
            // Arrange


            // Act
            hander.Handle(new HouseWinLossQuery(currentSession));

            // Assert
            betTypeService.Received().GetBetTypeGroupsBySport();
        }

        [Test]
        public void Handle_Always_CallGetOfHouseWinLossService()
        {
            // Arrange


            // Act
            hander.Handle(new HouseWinLossQuery(currentSession));

            // Assert
            houseWinLossService.Received().Get(Arg.Any<HouseWinLossParam>());
        }

        [Test]
        public void HandleCountHouseWinLossQuery_Always_CallGetSportGroups()
        {
            // Arrange


            // Act
            hander.Handle(new CountHouseWinLossQuery(currentSession));

            // Assert
            sportGroupService.Received().GetSportGroups();
        }       

        [Test]
        public void HandleCountHouseWinLossQuery_Always_CallGetOfHouseWinLossService()
        {
            // Arrange


            // Act
            hander.Handle(new CountHouseWinLossQuery(currentSession));

            // Assert
            houseWinLossService.Received().CountUsers(Arg.Any<HouseWinLossParam>());
        }
    }
}
