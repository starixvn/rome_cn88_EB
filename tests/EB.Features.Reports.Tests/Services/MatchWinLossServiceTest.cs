using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Services
{
    public class MatchWinLossServiceTest
    {
        private IDynamicRepository dynamicRepository;
        private IReportDateService reportDateService;
        private ICacheRefService cacheRefService;
        private IMatchWinLossService reportService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            reportDateService = Substitute.For<IReportDateService>();
            cacheRefService = Substitute.For<ICacheRefService>();

            reportService = new MatchWinLossService(dynamicRepository, reportDateService, cacheRefService);
        }

        [Test]
        public void Get_AwlaySetMoveBetsDayToParameterVariable()
        {
            // Arrange
            var moveBetsDay = DateTime.Now;
            var param = new MatchWinLossParam
            {
                MoveBetsDay = DateTime.MinValue
            };
            reportDateService.GetMoveBetsDay().Returns(moveBetsDay);

            // Action
            reportService.Get(param);

            // Asserts
            Assert.AreEqual(moveBetsDay, param.MoveBetsDay);
        }


        [Test]
        public void Get_Always_CallDynamicRepository()
        {
            // Arrange


            // Act
            reportService.Get(new MatchWinLossParam());

            // Assert
            dynamicRepository.Received().Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>());
        }


        [Test]
        public void Get_SetBetTypeName_FromRemotingRef()
        {
            // Arrange
            var expectedBettypeName = "soccer";
            var matchs = new List<MatchWinLossModel>
            {
                new MatchWinLossModel(),
                new MatchWinLossModel(),
            };
            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>())
                .Returns(matchs);
            cacheRefService.GetBetTypeName(0).Returns(expectedBettypeName);

            // Act
            var act = reportService.Get(new MatchWinLossParam());

            // Assert
            Assert.AreEqual(2, act.NormalMatches.Count(i => i.BetTypeGroupName == expectedBettypeName));
        }


        [Test]
        public void Get_BettypeIsParlay_SetLeagueNameFromResourceFile()
        {
            // Arrange
            var expectedLeagueName = AppResources.Parlay;
            var matchs = new List<MatchWinLossModel>
            {
                new MatchWinLossModel { BetTypeGroup = BetTypeIdConstants.SystemParlay }
            };
            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>())
                .Returns(matchs);

            // Act
            var act = reportService.Get(new MatchWinLossParam());

            // Assert
            Assert.AreEqual(expectedLeagueName, act.Parlay.LeagueName);
        }

        [Test]
        public void Get_SetHomeName_FromRemotingRef()
        {
            // Arrange
            var expectedHomeName = "MU";
            var matchs = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ HomeId = 1 }
            };
            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>())
                .Returns(matchs);
            cacheRefService.GetTeamName(1).Returns(expectedHomeName);

            // Act
            var act = reportService.Get(new MatchWinLossParam());

            // Assert
            Assert.IsTrue(act.NormalMatches.Any(i => i.HomeName == expectedHomeName));
        }

        [Test]
        public void Get_SetAwayName_FromRemotingRef()
        {
            // Arrange
            var expectedAwayName = "MU";
            var matchs = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ AwayId = 1 }
            };
            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>())
                .Returns(matchs);
            cacheRefService.GetTeamName(1).Returns(expectedAwayName);

            // Act
            var act = reportService.Get(new MatchWinLossParam());

            // Assert
            Assert.IsTrue(act.NormalMatches.Any(i => i.AwayName == expectedAwayName));
        }

        [Test]
        public void Get_SetLeagueName_FromRemotingRef()
        {
            // Arrange
            var expectedAwayName = "English Premier";
            var matchs = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ LeagueId = 1 }
            };
            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>())
                .Returns(matchs);
            cacheRefService.GetLeagueName(1).Returns(expectedAwayName);

            // Act
            var act = reportService.Get(new MatchWinLossParam());

            // Assert
            Assert.IsTrue(act.NormalMatches.Any(i => i.LeagueName == expectedAwayName));
        }


        [Test]
        public void Get_Always_LeagueNameByEnglishIsEnglish()
        {
            // Arrange
            var leagueNameByEnglishIsEnglish = "LeagueName is en-US";
            var leagueNameByEnglishIsTW = "LeagueName is zh-TW";
            var matchs = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ LeagueId = 1 }
            };
            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<MatchWinLossCriteria>())
                .Returns(matchs);
            cacheRefService.GetLeagueName(1, "en-US").Returns(leagueNameByEnglishIsEnglish);
            cacheRefService.GetLeagueName(1).Returns(leagueNameByEnglishIsTW);

            // Act
            var act= reportService.Get(new MatchWinLossParam());

            // Assert
            Assert.IsTrue(act.NormalMatches.Any(i => i.LeagueNameByEnglish == leagueNameByEnglishIsEnglish));
            Assert.IsTrue(act.NormalMatches.Any(i => i.LeagueName == leagueNameByEnglishIsTW));
        }
    }
}
