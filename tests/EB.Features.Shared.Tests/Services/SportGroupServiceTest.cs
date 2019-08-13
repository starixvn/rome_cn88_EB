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

        [Test]
        public void GetSportGroupById_Return_CorrectGroup()
        {
            // Arrange
            var func = Substitute.For<Func<int, string>>();
            var groups = new List<SportGroup>
            {
                new SportGroup(1, new List<int>{1}, func, func),
                new SportGroup(2, new List<int>{2}, func, func)
            };
            fanexCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportGroup>>>(), Arg.Any<CacheItemOptions>())
                .Returns(groups);

            // Act
            var act = sportGroupService.GetSportGroupById(1);

            // Assert
            Assert.AreEqual(1, act.Id);
        }

        [Test]
        public void GetSportGroupById_NotFoundSportGroup_ReturnNull()
        {
            // Arrange
            var func = Substitute.For<Func<int, string>>();
            var groups = new List<SportGroup>
            {
                new SportGroup(1, new List<int>{1}, func, func),
                new SportGroup(2, new List<int>{2}, func, func)
            };
            fanexCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportGroup>>>(), Arg.Any<CacheItemOptions>())
                .Returns(groups);

            // Act
            var act = sportGroupService.GetSportGroupById(3);

            // Assert
            Assert.IsNull(act);
        }
    }
}
