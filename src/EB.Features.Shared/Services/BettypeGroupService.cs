namespace EB.Features.Shared.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Shared.Criteria;
    using EB.Features.Shared.Model;
    using Fanex.Data.Repository;

    public interface IBettypeGroupService
    {
        IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(int sportGroupId = 0);

        IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(IEnumerable<int> sportGroupIds);
    }

    public class BettypeGroupServiceImpl : IBettypeGroupService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheRefService cacheRefService;

        public BettypeGroupServiceImpl(IDynamicRepository dynamicRepository, ICacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(IEnumerable<int> sportGroupIds)
        {
            var betTypes = GetBetTypeGroups()
                .Where(b => sportGroupIds.Contains(b.SportTypeGroup));

            return betTypes ?? betTypes.GroupBy(i => i.Id).Select(i => i.First());
        }


        public IEnumerable<BetTypeGroup> GetBetTypeGroupsBySport(int sportGroupId = 0)
            => sportGroupId == 0
                ? GetBetTypeGroups()
                : GetBetTypeGroups().Where(bettype => bettype.SportTypeGroup == sportGroupId);

        private IEnumerable<BetTypeGroup> GetBetTypeGroups()
            => dynamicRepository.Fetch<BetTypeGroup>(new GetBetTypeGroupBySportCriteria())
            .Select(bettype => new BetTypeGroup(bettype.SportTypeGroup, bettype.Id, cacheRefService.GetBetTypeName))
            .OrderBy(i => i.SportTypeGroup)
            .ThenBy(i => i.Id)
            .ToList();
    }
}
