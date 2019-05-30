using System;
using EB.Features.Shared.Criteria;
using EB.Features.Shared.Model;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace EB.Features.Shared.Services
{
    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();

        DateTime GetArchiveDate();
    }

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "CN88.Features.Shared.Services.ReportDateServiceImpl.BalanceUpDayAndMoveBetsDay";
        private const string ArchiveDateCacheKey = "CN88.Features.Shared.Services.ReportDateServiceImpl.ArchiveDate";

        private readonly IDynamicRepository dynamicRepository;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public static ReportDateService Instance { get; set; }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
            => ConfiguredCacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () => dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        public DateTime GetArchiveDate()
            => ConfiguredCacheService.GetOrSet(
                ArchiveDateCacheKey,
                () => dynamicRepository.Get<DateTime>(new GetArchiveDateCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
    }
}
