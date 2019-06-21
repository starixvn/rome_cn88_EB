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

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public static IReportDateService Instance { get; set; }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
            => ConfiguredCacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () => dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CachedMinute)));

        public DateTime GetArchiveDate()
            => ConfiguredCacheService.GetOrSet(
                ArchiveDateCacheKey,
                () => dynamicRepository.Get<DateTime>(new GetArchiveDateCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CachedMinute)));
    }
}
