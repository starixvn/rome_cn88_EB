using System;
using System.Linq;
using System.Collections.Generic;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Site.Reports.Handler.PerformanceBonus;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;
using EB.Features.Shared.Services;
using EB.Features.Shared.Model;

namespace EB.Features.Site.Tests.Reports.Handler.PerformanceBonus
{
    public class PerformanceBonusHandlerTest
    {
        private IPerformanceBonusService service;
        private ISportGroupService sportGroupService;
        private AccountingPeriodService periodService;
        private PerformanceBonusHandler handler;
        private UserSession currentSession;

        [SetUp]
        public void SetUp()
        {
            sportGroupService = Substitute.For<ISportGroupService>();
            currentSession = Substitute.For<UserSession>();
            service = Substitute.For<IPerformanceBonusService>();
            periodService = Substitute.For<AccountingPeriodService>();
            handler = new PerformanceBonusHandler(sportGroupService, service, periodService);
        }

        [Test]
        public void Handle_Awlays_CallGetSportGroups()
        {
            // Arrange
            sportGroupService.GetSportGroups()
                .Returns(new List<SportGroup>());
            periodService.GetCurrentPeriod().Returns(new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now));

            // Act
            handler.Handle(new PerformanceBonusQuery(currentSession));

            // Assert
            sportGroupService.Received().GetSportGroups();
        }

        [Test]
        public void Handle_Awlays_CallGetOldAccountingPeriods()
        {
            // Arrange
            periodService.GetCurrentPeriod().Returns(new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now));

            // Act
            handler.Handle(new PerformanceBonusQuery(currentSession));

            // Assert
            periodService.Received().GetOldAccountingPeriods();
        }

        [Test]
        public void Handle_FirstTimesLoading_SetDefaultValue()
        {
            // Arrange
            var currentPeriod = new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now);
            periodService.GetCurrentPeriod().Returns(currentPeriod);

            // Act
            var act = handler.Handle(new PerformanceBonusQuery(currentSession));

            // Assert
            Assert.AreEqual(currentPeriod.Id, act.Query.UserSelectedPeriodId);
            Assert.AreEqual(currentPeriod.FromDate, act.Query.FromDate);
            Assert.AreEqual(currentPeriod.ToDate, act.Query.ToDate);
        }


        [Test]
        public void Handle_SecondTimesLoading_NotChangeInputValue()
        {
            // Arrange
            var currentPeriod = new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now);
            periodService.GetCurrentPeriod().Returns(currentPeriod);
            var query = new PerformanceBonusQuery(currentSession)
            {
                UserSelectedPeriodId = "2_2019",
                FromDate = DateTime.Now.AddDays(-1),
                ToDate = DateTime.Now
            };

            // Act
            var act = handler.Handle(query);

            // Assert
            Assert.AreEqual(query.UserSelectedPeriodId, act.Query.UserSelectedPeriodId);
            Assert.AreEqual(query.FromDate, act.Query.FromDate);
            Assert.AreEqual(query.ToDate, act.Query.ToDate);
        }

        [Test]
        public void Handle_InvalidSuperName_NotCallPerformanceBonusService()
        {
            // Arrange
            service.CheckValidSuper("razor").Returns("razor");
            periodService.GetCurrentPeriod().Returns(new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now));

            // Act
            handler.Handle(new PerformanceBonusQuery(currentSession) { UserNames = "razor" });

            // Assert
            service.DidNotReceive().Get(Arg.Any<PerformanceBonusParam>());
        }

        [Test]
        public void Handle_ValidSuperName_CallPerformanceBonusService()
        {
            // Arrange
            service.CheckValidSuper("razor").Returns(string.Empty);
            periodService.GetCurrentPeriod().Returns(new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now));

            // Act
            handler.Handle(new PerformanceBonusQuery(currentSession) { UserNames = "razor" });

            // Assert
            service.Received().Get(Arg.Any<PerformanceBonusParam>());
        }

        [Test]
        public void Handle_Always_SetCorrectFormatPeriodList()
        {
            // Arrange
            var currentPeriods = new List<AccountingPeriod> { new AccountingPeriod(1, 2019, DateTime.Now.AddDays(-30), DateTime.Now) };
            periodService.GetOldAccountingPeriods().Returns(currentPeriods);
            var query = new PerformanceBonusQuery(currentSession)
            {
                UserSelectedPeriodId = "2_2019",
                FromDate = DateTime.Now.AddDays(-1),
                ToDate = DateTime.Now
            };

            // Act
            var act = handler.Handle(query);

            // Assert
            Assert.AreEqual($"{currentPeriods[0].FromDateStr} ~ {currentPeriods[0].ToDateStr}", act.PeriodSelectionViewModel.Items.First().Text);
            Assert.AreEqual(currentPeriods[0].Id, act.PeriodSelectionViewModel.Items.First().Value);
        }
    }
}
