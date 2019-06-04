namespace EB.Features.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Criteria;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Services;
    using Fanex.Data.Repository;

    public interface IMatchWinLossService
    {
        IEnumerable<MatchWinLossViewModel> Get(MatchWinLossParam param);
    }

    public class MatchWinLossService : IMatchWinLossService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;

        public MatchWinLossService(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<MatchWinLossViewModel> Get(MatchWinLossParam param)
        {
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();

            var reportData = dynamicRepository.Fetch<MatchWinLossModel>(new MatchWinLossCriteria(param));
            var matchWinLoss = BuildViewModel(reportData);

            return matchWinLoss.OrderBy(i => $"{i.LeagueId}_{i.MatchId}_{i.BetTypeGroup}");
        }

        private IEnumerable<MatchWinLossViewModel> BuildViewModel(IEnumerable<MatchWinLossModel> reportData)
        {
            var matches = new List<MatchWinLossViewModel>();
            foreach (var row in reportData)
            {
                var match = new MatchWinLossViewModel(row);
                match.BetTypeGroupName = cacheRefService.GetBetTypeName(row.BetTypeGroup);

                if (match.BetTypeGroup == BetTypeIdConstants.SystemParlay)
                {
                    match.LeagueName = AppResources.Parlay;
                }
                else
                {
                    match.EventInfo = match.BetTypeGroup == BetTypeIdConstants.Outright
                        ? string.Empty
                        : $"{cacheRefService.GetTeamName(match.HomeId)} v {cacheRefService.GetTeamName(match.AwayId)}";
                    match.LeagueName = cacheRefService.GetLeagueName(row.LeagueId);
                }

                matches.Add(match);
            }

            return matches;
        }

    }
}
