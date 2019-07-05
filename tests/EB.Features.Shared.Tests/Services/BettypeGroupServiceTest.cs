namespace EB.Features.Shared.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    public class BettypeGroupServiceTest
    {
        private ICacheService fanexCacheService;
        private IDynamicRepository dynamicRepository;
        private IBettypeGroupService bettypeGroupService;
        private ICacheRefService cacheRefService;

        [SetUp]
        public void SetUp()
        {
            fanexCacheService = Substitute.For<ICacheService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheRefService = Substitute.For<ICacheRefService>();

            bettypeGroupService = new BettypeGroupServiceImpl(dynamicRepository, cacheRefService, fanexCacheService);
        }

        [Test]
        public void GetBetTypeGroupsBySport_SportGroupIdIsZero_ReturnAll()
        {
            // Arrange
            var key = "EB.Features.Shared.Services.Bettypes";
            var bettypeGroup = new List<BetTypeGroup>
            {
                new BetTypeGroup(1,1,cacheRefService.GetBetTypeName),
                new BetTypeGroup(2,2,cacheRefService.GetBetTypeName),
                new BetTypeGroup(3,3,cacheRefService.GetBetTypeName),
            };
            fanexCacheService.GetOrSet(key, Arg.Any<Func<IEnumerable<BetTypeGroup>>>(), Arg.Any<CacheItemOptions>())
                .Returns(bettypeGroup);

            // Action
            var act = bettypeGroupService.GetBetTypeGroupsBySport(0);

            // Assert
            Assert.AreEqual(bettypeGroup.Count, act.Count());
        }

        [Test]
        public void GetBetTypeGroupsBySport_ReturnExpectedBettypeBySportTypeGroup()
        {
            // Arrange
            var sportTypeGroup = 1;
            var key = "EB.Features.Shared.Services.Bettypes";
            var bettypeGroup = new List<BetTypeGroup>
            {
                new BetTypeGroup(sportTypeGroup,1,cacheRefService.GetBetTypeName),
                new BetTypeGroup(sportTypeGroup,2,cacheRefService.GetBetTypeName),
                new BetTypeGroup(3,3,cacheRefService.GetBetTypeName),
            };
            fanexCacheService.GetOrSet(key, Arg.Any<Func<IEnumerable<BetTypeGroup>>>(), Arg.Any<CacheItemOptions>())
                .Returns(bettypeGroup);

            // Action
            var act = bettypeGroupService.GetBetTypeGroupsBySport(sportTypeGroup);

            // Assert
            Assert.AreEqual(2, act.Count());
        }

        [Test]
        public void GetBetTypeGroupsBySport_ReturnExpectedBettypeBySportTypeGroups()
        {
            // Arrange
            var sportTypeGroup = new List<int> { 1, 2 };
            var key = "EB.Features.Shared.Services.Bettypes";
            var bettypeGroup = new List<BetTypeGroup>
            {
                new BetTypeGroup(1,1,cacheRefService.GetBetTypeName),
                new BetTypeGroup(2,2,cacheRefService.GetBetTypeName),
                new BetTypeGroup(3,3,cacheRefService.GetBetTypeName),
            };
            fanexCacheService.GetOrSet(key, Arg.Any<Func<IEnumerable<BetTypeGroup>>>(), Arg.Any<CacheItemOptions>())
                .Returns(bettypeGroup);

            // Action
            var act = bettypeGroupService.GetBetTypeGroupsBySport(sportTypeGroup);

            // Assert
            Assert.AreEqual(2, act.Count());
        }
    }
}
