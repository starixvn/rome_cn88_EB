using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Model;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.Models;
using Fanex.NAP.Core.Web.Session;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.Handler
{

    public class MatchWinLossHandlerTest
    {
        private ISportGroupService sportGroupService;
        private IBettypeGroupService betTypeService;
        private IMatchWinLossService matchWinLossService;
        private IReportDateService reportDateService;
        private ICacheRefService cacheRefService;
        private MatchWinLossHandler hander;
        private UserSession currentSession;

        [SetUp]
        public void Setup()
        {
            sportGroupService = Substitute.For<ISportGroupService>();
            betTypeService = Substitute.For<IBettypeGroupService>();
            matchWinLossService = Substitute.For<IMatchWinLossService>();
            reportDateService = Substitute.For<IReportDateService>();
            cacheRefService = Substitute.For<ICacheRefService>();
            currentSession = Substitute.For<UserSession>();
            hander = new MatchWinLossHandler(sportGroupService, betTypeService, matchWinLossService, reportDateService);
        }


        [Test]
        public void Handler_Always_CallSportGroups()
        {
            // Arrange


            // Act
            hander.Handle(new MatchWinLossQuery(currentSession));

            // Assert
            sportGroupService.Received().GetSportGroups();
        }


        [Test]
        public void Handler_AlwaysGetAllBetType_BySportGroupId()
        {
            // Arrange
            var expectedGroupId = 1;
            var sportGroup = new List<SportGroup>
            {
                new SportGroup(expectedGroupId, new List<int>(), cacheRefService.GetSportName, cacheRefService.GetSportNameByEnglish)
            };

            sportGroupService.GetSportGroups()
                .Returns(sportGroup);

            // Act
            hander.Handle(new MatchWinLossQuery(currentSession));

            // Assert
            betTypeService.Received().GetBetTypeGroupsBySport(Arg.Is<IEnumerable<int>>(i => i.Contains(expectedGroupId)));
        }


        [Test]
        public void Handle_IsFirstTimeLoading_CallGetBetTypeGroupsBySportOneTimes()
        {
            // Arrange
            var query = new MatchWinLossQuery(currentSession)
            {
                UserSelectedSportGroupId = 0
            };

            // Act
            hander.Handle(query);

            // Assert
            betTypeService.Received(1).GetBetTypeGroupsBySport(Arg.Any<IEnumerable<int>>());
        }


        [Test]
        public void Handle_IsSecondTimeLoading_CallGetBetTypeGroupsBySportTwoTimes()
        {
            // Arrange
            var expectedGroupId = 1;            
            var query = new MatchWinLossQuery(currentSession)
            {
                UserSelectedSportGroupId = expectedGroupId
            };

            // Act
            hander.Handle(query);

            // Assert
            betTypeService.Received(1).GetBetTypeGroupsBySport(Arg.Any<IEnumerable<int>>());
            betTypeService.Received(1).GetBetTypeGroupsBySport(Arg.Any<int>());
        }


        [Test]
        public void Handle_Always_CallGetOfMatchWinLossService()
        {
            // Arrange
            var query = new MatchWinLossQuery(currentSession);

            // Act
            hander.Handle(query);

            // Assert
            matchWinLossService.Received().Get(Arg.Any<MatchWinLossParam>());
        }


        [Test]
        public void Handler_Always_CallBalanceUpDayService()
        {
            // Arrange
            var query = new MatchWinLossQuery(currentSession);

            // Act
            hander.Handle(query);

            // Assert
            reportDateService.Received().GetBalanceUpDay();
        }
    }
}
