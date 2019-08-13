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

        SportGroup GetSportGroupById(int sportGroupId);
    }

    public class SportGroupServiceImpl : ISportGroupService
    {
        private const string SportGroupsCacheKey = "EB.Features.Shared.Services.SportServiceImpl.SportGroups";
        private const int CachedSecond = 30;

        private readonly SportGroupDataAccess sportGroupDataAccess;
        private readonly ICacheRefService cacheRefService;
        private readonly ICacheService fanexCacheService;

        public SportGroupServiceImpl(
            SportGroupDataAccess sportGroupDataAccess,
            ICacheRefService cacheRefService,
            ICacheService fanexCacheService)
        {
            this.sportGroupDataAccess = sportGroupDataAccess;
            this.cacheRefService = cacheRefService;
            this.fanexCacheService = fanexCacheService;
        }

        public SportGroup GetSportGroupById(int sportGroupId)
            => GetSportGroups().SingleOrDefault(i => i.Id == sportGroupId);

        public IEnumerable<SportGroup> GetSportGroups()
            => fanexCacheService.GetOrSet(
                SportGroupsCacheKey,
                () => sportGroupDataAccess.GetSportGroups(
                    new GetSportGroupsCriteria(), 
                    cacheRefService.GetSportName, 
                    cacheRefService.GetSportNameByEnglish),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedSecond)));
    }
}
