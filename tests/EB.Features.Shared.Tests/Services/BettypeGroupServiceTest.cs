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
        private const string BetTypesCacheKey = "EB.Features.Shared.Services.Bettypes";
        private ICacheService fanexCacheService;
        private IDynamicRepository dynamicRepository;
        private IBettypeGroupService bettypeGroupService;
        private ICacheRefService cacheRefService;
        private List<BetTypeGroup> bettypeGroups;

        [SetUp]
        public void SetUp()
        {
            fanexCacheService = Substitute.For<ICacheService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheRefService = Substitute.For<ICacheRefService>();
            bettypeGroups = new List<BetTypeGroup>
            {
                new BetTypeGroup(1,1,cacheRefService.GetBetTypeName),
                new BetTypeGroup(2,2,cacheRefService.GetBetTypeName),
                new BetTypeGroup(1,3,cacheRefService.GetBetTypeName),
            };
            bettypeGroupService = new BettypeGroupServiceImpl(dynamicRepository, cacheRefService, fanexCacheService);
            fanexCacheService.GetOrSet(BetTypesCacheKey, Arg.Any<Func<List<BetTypeGroup>>>(), Arg.Any<CacheItemOptions>())
                .Returns(bettypeGroups);
        }

        [Test]
        public void GetBetTypeGroupsBySport_SportGroupIdIs0_ReturnAll()
        {
            // Arrange
            

            // Act
            var act = bettypeGroupService.GetBetTypeGroupsBySport(0);

            // Assert
            Assert.AreEqual(3, act.Count());
        }

        [Test]
        public void GetBetTypeGroupsBySport_Exist_ReturnExistList()
        {
            // Arrange
            

            // Act
            var act = bettypeGroupService.GetBetTypeGroupsBySport(1);

            // Assert
            Assert.AreEqual(2, act.Count());
        }

        [Test]
        public void GetBetTypeGroupsBySport_NotExistId_ReturnEmpty()
        {
            // Arrange


            // Act
            var act = bettypeGroupService.GetBetTypeGroupsBySport(new List<int> { 999 });

            // Assert
            Assert.IsEmpty(act);
        }

        [Test]
        public void GetBetTypeGroupsBySport_ExistId_ReturnExistList()
        {
            // Arrange


            // Act
            var act = bettypeGroupService.GetBetTypeGroupsBySport(new List<int> { 1 });

            // Assert
            Assert.AreEqual(2, act.Count());
        }
    }
}
