using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Model;
using EB.Features.Reports.Services;
using Fanex.Caching;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Services
{
    public class AccountingPeriodServiceTest
    {
        private const string AccountingPeriodsCacheKey = "EB.Features.Reports.Services.AccountingPeriodServiceImpl.AccountingPeriods";
        private IDynamicRepository dynamicRepository;
        private ICacheService fanexCacheService;
        private AccountingPeriodService service;

        [SetUp]
        public void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            fanexCacheService = Substitute.For<ICacheService>();
            service = new AccountingPeriodServiceImpl(fanexCacheService, dynamicRepository);
        }

        [Test]
        public void GetCurrentPeriod_NotExistCurrentPeriod_ReturnNull()
        {
            // Arrange
            var period = new AccountingPeriod(1, 2018, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-1));
            fanexCacheService.GetOrSet(AccountingPeriodsCacheKey, Arg.Any<Func<IEnumerable<AccountingPeriod>>>())
                .Returns(new List<AccountingPeriod> { period });

            // Act
            var act = service.GetCurrentPeriod();

            // Assert
            Assert.IsNull(act);
        }

        [Test]
        public void GetCurrentPeriod_ExistCurrentPeriod_ReturnCurrentPeriod()
        {
            // Arrange
            var oldPeriod = new AccountingPeriod(1, DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-1));
            var futurePeriod = new AccountingPeriod(1, DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(1), DateTime.Now.AddYears(1));
            var currentPeriod = new AccountingPeriod(1, DateTime.Now.Year, DateTime.Now.AddDays(-10), DateTime.Now);
            fanexCacheService.GetOrSet(AccountingPeriodsCacheKey, Arg.Any<Func<IEnumerable<AccountingPeriod>>>(), Arg.Any<CacheItemOptions>())
                .Returns(new List<AccountingPeriod> { oldPeriod, currentPeriod, futurePeriod });

            // Act
            var act = service.GetCurrentPeriod();

            // Assert
            Assert.AreEqual($"{DateTime.Now.Year}_1", act.Id);
        }

        [Test]
        public void GetOldAccountingPeriods_Always_NotExistFuturePeriod()
        {
            // Arrange
            var oldPeriod = new AccountingPeriod(1, DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-1));
            var futurePeriod = new AccountingPeriod(1, DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(1), DateTime.Now.AddYears(1));
            var currentPeriod = new AccountingPeriod(1, DateTime.Now.Year, DateTime.Now.AddDays(-10), DateTime.Now);
            fanexCacheService.GetOrSet(AccountingPeriodsCacheKey, Arg.Any<Func<IEnumerable<AccountingPeriod>>>(), Arg.Any<CacheItemOptions>())
                .Returns(new List<AccountingPeriod> { oldPeriod, currentPeriod, futurePeriod });

            // Act
            var act = service.GetOldAccountingPeriods().ToList();

            // Assert
            Assert.AreEqual(2, act.Count);
            Assert.IsFalse(act.Any(i => i.Id == $"{DateTime.Now.AddYears(1)}_1"));
        }

        [Test]
        public void GetOldAccountingPeriods_Always_ExistCurrentPeriod()
        {
            // Arrange
            var oldPeriod = new AccountingPeriod(1, DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-1));
            var futurePeriod = new AccountingPeriod(1, DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(1), DateTime.Now.AddYears(1));
            var currentPeriod = new AccountingPeriod(1, DateTime.Now.Year, DateTime.Now.AddDays(-10), DateTime.Now);
            fanexCacheService.GetOrSet(AccountingPeriodsCacheKey, Arg.Any<Func<IEnumerable<AccountingPeriod>>>(), Arg.Any<CacheItemOptions>())
                .Returns(new List<AccountingPeriod> { oldPeriod, currentPeriod, futurePeriod });

            // Act
            var act = service.GetOldAccountingPeriods().ToList();

            // Assert
            Assert.AreEqual(2, act.Count);
            Assert.AreEqual($"{DateTime.Now.Year}_1", act[0].Id);
        }

        [Test]
        public void GetOldAccountingPeriods_Always_ExistOldPeriod()
        {
            // Arrange
            var oldPeriod = new AccountingPeriod(1, DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-1));
            var futurePeriod = new AccountingPeriod(1, DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(1), DateTime.Now.AddYears(1));
            var currentPeriod = new AccountingPeriod(1, DateTime.Now.Year, DateTime.Now.AddDays(-10), DateTime.Now);
            fanexCacheService.GetOrSet(AccountingPeriodsCacheKey, Arg.Any<Func<IEnumerable<AccountingPeriod>>>(), Arg.Any<CacheItemOptions>())
                .Returns(new List<AccountingPeriod> { oldPeriod, currentPeriod, futurePeriod });

            // Act
            var act = service.GetOldAccountingPeriods().ToList();

            // Assert
            Assert.AreEqual(2, act.Count);
            Assert.AreEqual($"{DateTime.Now.AddYears(-1).Year}_1", act[1].Id);
        }

        [Test]
        public void GetOldAccountingPeriods_Always_OrderDescByFromDate()
        {
            // Arrange
            var oldPeriod = new AccountingPeriod(1, DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-1));
            var futurePeriod = new AccountingPeriod(1, DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(1), DateTime.Now.AddYears(1));
            var currentPeriod = new AccountingPeriod(1, DateTime.Now.Year, DateTime.Now.AddDays(-10), DateTime.Now);
            fanexCacheService.GetOrSet(AccountingPeriodsCacheKey, Arg.Any<Func<IEnumerable<AccountingPeriod>>>(), Arg.Any<CacheItemOptions>())
                .Returns(new List<AccountingPeriod> { oldPeriod, currentPeriod, futurePeriod });

            // Act
            var act = service.GetOldAccountingPeriods().ToList();

            // Assert
            Assert.AreEqual(2, act.Count);
            Assert.AreEqual($"{DateTime.Now.Year}_1", act[0].Id);
            Assert.AreEqual($"{DateTime.Now.AddYears(-1).Year}_1", act[1].Id);
        }
    }
}
