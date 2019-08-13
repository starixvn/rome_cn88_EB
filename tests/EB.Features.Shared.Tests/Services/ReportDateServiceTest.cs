namespace EB.Features.Shared.Tests.Services
{
    using System;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    public class ReportDateServiceTest
    {
        private ICacheService fanexCacheService;
        private IDynamicRepository dataAccess;
        private IReportDateService service;

        [SetUp]
        public void SetUp()
        {
            fanexCacheService = Substitute.For<ICacheService>();
            dataAccess = Substitute.For<IDynamicRepository>();
            service = new ReportDateServiceImpl(dataAccess, fanexCacheService);
        }

        [Test]
        public void GetArchiveDate_Always_CallFanexCachingService()
        {
            // Arrange
            var key = "EB.Features.Shared.Services.ReportDateServiceImpl.ArchiveDate";

            // Action
            service.GetArchiveDate();

            // Assert
            fanexCacheService.Received().GetOrSet(key, Arg.Any<Func<DateTime>>(), Arg.Any<CacheItemOptions>());
        }

        [Test]
        public void GetBalanceUpDay_Always_ReturnCorrectBalanceUpDay()
        {
            // Arrange
            var key = "EB.Features.Shared.Services.ReportDateServiceImpl.BalanceUpDayAndMoveBetsDay";
            var day = new DateTime(2019, 2, 2);
            var balanceUpDay = new BalanceUpDayAndMoveBetsDay(day, DateTime.MaxValue);
            fanexCacheService.GetOrSet(key, Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(balanceUpDay);

            // Action
            var act = service.GetBalanceUpDay();

            // Assert
            Assert.AreEqual(day, act);
        }

        [Test]
        public void GetMoveBetsDay_Always_ReturnCorrectMoveBetsDay()
        {
            // Arrange
            var key = "EB.Features.Shared.Services.ReportDateServiceImpl.BalanceUpDayAndMoveBetsDay";
            var day = new DateTime(2019, 2, 2);
            var balanceUpDay = new BalanceUpDayAndMoveBetsDay(DateTime.MaxValue, day);
            fanexCacheService.GetOrSet(key, Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(balanceUpDay);

            // Action
            var act = service.GetMoveBetsDay();

            // Assert
            Assert.AreEqual(day, act);
        }
    }
}
