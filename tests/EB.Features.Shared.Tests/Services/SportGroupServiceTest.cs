namespace EB.Features.Shared.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Shared.Criteria;
    using EB.Features.Shared.DataAccesses;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Services;
    using Fanex.Caching;
    using NSubstitute;
    using NUnit.Framework;

    public class SportGroupServiceTest
    {
        private ICacheService fanexCacheService;
        private SportGroupDataAccess sportGroupDataAccess;
        private ISportGroupService sportGroupService;
        private ICacheRefService cacheRefService;

        [SetUp]
        public void SetUp()
        {
            fanexCacheService = Substitute.For<ICacheService>();
            sportGroupDataAccess = Substitute.For<SportGroupDataAccess>();
            cacheRefService = Substitute.For<ICacheRefService>();
            sportGroupService = new SportGroupServiceImpl(sportGroupDataAccess, cacheRefService, fanexCacheService);
        }

        [Test]
        public void GetSportGroups_Always_CallFanexCachingService()
        {
            // Arrange
            var key = "EB.Features.Shared.Services.SportServiceImpl.SportGroups";

            // Action
            sportGroupService.GetSportGroups();

            // Assert
            fanexCacheService.Received().GetOrSet(key, Arg.Any<Func<IEnumerable<SportGroup>>>(), Arg.Any<CacheItemOptions>());
        }        
    }
}
