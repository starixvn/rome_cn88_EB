using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Criteria.PerformanceBonus;
using EB.Features.Reports.Model;
using EB.Features.Shared.Utilities;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace EB.Features.Reports.Services
{
    public interface AccountingPeriodService
    {
        AccountingPeriod GetCurrentPeriod();

        IEnumerable<AccountingPeriod> GetOldAccountingPeriods();

        IEnumerable<AccountingPeriod> GetAccountingPeriods();
    }

    public class AccountingPeriodServiceImpl : AccountingPeriodService
    {
        private const string AccountingPeriodsCacheKey = "EB.Features.Reports.Services.AccountingPeriodServiceImpl.AccountingPeriods";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService fanexCacheService;

        public AccountingPeriodServiceImpl(
            ICacheService fanexCacheService,
            IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.fanexCacheService = fanexCacheService;
        }

        public AccountingPeriod GetCurrentPeriod()
            => GetAccountingPeriods().FirstOrDefault(p => p.IsCurrentPeriod);

        public IEnumerable<AccountingPeriod> GetOldAccountingPeriods()
        {
            var periods = GetAccountingPeriods().ToList();

            var currentPeriod = periods.FirstOrDefault(p => p.IsCurrentPeriod);
            var currentIndex = periods.IndexOf(currentPeriod);

            return periods
                .Where(p => periods.IndexOf(p) <= currentIndex)
                .OrderByDescending(p => p.FromDate);
        }

        public IEnumerable<AccountingPeriod> GetAccountingPeriods()
            => fanexCacheService.GetOrSet(
                AccountingPeriodsCacheKey,
                () => dynamicRepository
                .Fetch<AccountingPeriod>(new GetAccountingPeriodsCriteria())
                .Where(i => i.ToDate >= SystemTime.MinDate),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}
