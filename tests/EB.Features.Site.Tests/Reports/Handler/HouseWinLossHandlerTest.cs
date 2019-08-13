using EB.Features.Reports.Params;
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
        public void Handle_Always_CallGetBetTypeGroupBySportId()
        {
            // Arrange
            var query = new HouseWinLossQuery(currentSession)
            {
                UserSelectedSportGroupId = 1
            };

            // Act
            hander.Handle(query);

            // Assert
            betTypeService.Received().GetBetTypeGroupsBySport(Arg.Is<int>(i => i == 1));
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
    }
}
