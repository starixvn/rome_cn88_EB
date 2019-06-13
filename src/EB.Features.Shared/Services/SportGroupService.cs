namespace EB.Features.Shared.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using EB.Features.Shared.Criteria;
    using EB.Features.Shared.DataAccesses;
    using EB.Features.Shared.Model;
    using Fanex.Caching;

    public interface ISportGroupService
    {
        IEnumerable<SportGroup> GetSportGroups();

        IEnumerable<SportGroup> GetSportGroupsForSportbook();
    }

    public class SportGroupServiceImpl : ISportGroupService
    {
        private const string SportGroupsCacheKey = "EB.Features.Shared.Services.SportServiceImpl.SportGroups";
        private const int CachedSecond = 30;

        private readonly SportGroupDataAccess sportGroupDataAccess;
        private readonly ICacheRefService cacheRefService;

        public SportGroupServiceImpl(
            SportGroupDataAccess sportGroupDataAccess,
            ICacheRefService cacheRefService)
        {
            this.sportGroupDataAccess = sportGroupDataAccess;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<SportGroup> GetSportGroups()
            => ConfiguredCacheService.GetOrSet(
                SportGroupsCacheKey,
                () => sportGroupDataAccess.GetSportGroups(new GetSportGroupsCriteria(), cacheRefService.GetSportName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedSecond)));

        public IEnumerable<SportGroup> GetSportGroupsForSportbook()
            => GetSportGroups().Where(s => s.ProductId != 0);
    }
}
