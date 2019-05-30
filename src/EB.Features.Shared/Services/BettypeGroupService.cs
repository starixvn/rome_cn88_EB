using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Shared.Criteria;
using EB.Features.Shared.Model;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace EB.Features.Shared.Services
{
    public interface BettypeGroupService
    {
        IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(int sportGroupId = 0);
    }

    public class BettypeGroupServiceImpl : BettypeGroupService
    {
        private const string BetTypesCacheKey = "CN88.Features.Shared.Services.Bettypes";

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;

        public BettypeGroupServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

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
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}
