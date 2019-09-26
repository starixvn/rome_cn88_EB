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
    }
}
