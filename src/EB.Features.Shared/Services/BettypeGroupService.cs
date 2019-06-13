namespace EB.Features.Shared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Shared.Criteria;
    using EB.Features.Shared.Model;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface IBettypeGroupService
    {
        IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(int sportGroupId = 0);

        IEnumerable<BetTypeGroup> GetBetTypeGroupsBySportbook(IEnumerable<int> sportGroupIds);
    }

    public class BettypeGroupServiceImpl : IBettypeGroupService
    {
        private const string BetTypesCacheKey = "EB.Features.Shared.Services.Bettypes";
        private const int CachedDay = 1;

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheRefService cacheRefService;

        public BettypeGroupServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<BetTypeGroup> GetBetTypeGroupsBySportbook(IEnumerable<int> sportGroupIds)
            => GetBetTypeGroups().Where(b => sportGroupIds.Contains(b.SportTypeGroup));

        public IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(int sportGroupId = 0)
            => sportGroupId == 0
                ? GetBetTypeGroups()
                : GetBetTypeGroups().Where(bettype => bettype.SportTypeGroup == sportGroupId);

        private IEnumerable<BetTypeGroup> GetBetTypeGroups()
            => ConfiguredCacheService.GetOrSet(
                BetTypesCacheKey,
                () =>
                {
                    var betTypes = dynamicRepository.Fetch<BetTypeGroup>(new GetBetTypeGroupBySportCriteria());

                    return betTypes.Select(bettype => new BetTypeGroup(bettype.SportTypeGroup, bettype.Id, cacheRefService.GetBetTypeName));
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CachedDay)));
    }
}
