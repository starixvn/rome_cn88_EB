using System;
using System.Collections.Generic;
using EB.Features.Shared.Criteria;
using EB.Features.Shared.DataAccesses;
using EB.Features.Shared.Model;
using Fanex.Caching;

namespace EB.Features.Shared.Services
{
    public interface SportGroupService
    {
        IEnumerable<SportGroup> GetSportGroups();
    }

    public class SportGroupServiceImpl : SportGroupService
    {
        private const string SportGroupsCacheKey = "EB.Features.Shared.Services.SportServiceImpl.SportGroups";

        private readonly SportGroupDataAccess sportGroupDataAccess;
        private readonly CacheRefService cacheRefService;

        public SportGroupServiceImpl(
            SportGroupDataAccess sportGroupDataAccess,
            CacheRefService cacheRefService)
        {
            this.sportGroupDataAccess = sportGroupDataAccess;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<SportGroup> GetSportGroups()
            => ConfiguredCacheService.GetOrSet(
                SportGroupsCacheKey,
                () => sportGroupDataAccess.GetSportGroups(new GetSportGroupsCriteria(), cacheRefService.GetSportName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}
