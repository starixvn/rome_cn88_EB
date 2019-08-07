namespace EB.Features.Shared.Services
{
    using System;
    using EB.Features.Shared.Criteria;
    using EB.Features.Shared.Model;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface IReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();

        DateTime GetArchiveDate();
    }

    public class ReportDateServiceImpl : IReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "EB.Features.Shared.Services.ReportDateServiceImpl.BalanceUpDayAndMoveBetsDay";
        private const string ArchiveDateCacheKey = "EB.Features.Shared.Services.ReportDateServiceImpl.ArchiveDate";
        private const int CachedMinute = 5;

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService fanexCacheService;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository,
            ICacheService fanexCacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.fanexCacheService = fanexCacheService;
        }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
            => fanexCacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () => dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CachedMinute)));

        public DateTime GetArchiveDate()
            => fanexCacheService.GetOrSet(
                ArchiveDateCacheKey,
                () => dynamicRepository.Get<DateTime>(new GetArchiveDateCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CachedMinute)));
    }
}
