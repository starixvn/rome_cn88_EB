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
        MatchWinLossViewModel Get(MatchWinLossParam param);
    }

    public class MatchWinLossService : IMatchWinLossService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IReportDateService reportDateService;
        private readonly ICacheRefService cacheRefService;

        public MatchWinLossService(
            IDynamicRepository dynamicRepository,
            IReportDateService reportDateService,
            ICacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
        }

        public MatchWinLossViewModel Get(MatchWinLossParam param)
        {
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();

            var reportData = dynamicRepository.Fetch<MatchWinLossModel>(new MatchWinLossCriteria(param));
            BuildViewModel(reportData);

            return new MatchWinLossViewModel(reportData.OrderBy(i => $"{i.LeagueNameByEnglish}_{i.MatchId}"));
        }

        private void BuildViewModel(IEnumerable<MatchWinLossModel> reportData)
        {
            foreach (var row in reportData)
            {
                row.BetTypeGroupName = cacheRefService.GetBetTypeName(row.BetTypeGroup);

                if (row.BetTypeGroup == BetTypeIdConstants.SystemParlay)
                {
                    row.LeagueName = AppResources.Parlay;
                }
                else
                {
                    row.EventInfo = row.BetTypeGroup == BetTypeIdConstants.Outright
                        ? string.Empty
                        : $"{cacheRefService.GetTeamName(row.HomeId)} v {cacheRefService.GetTeamName(row.AwayId)}";
                    row.LeagueName = cacheRefService.GetLeagueName(row.LeagueId);
                    row.LeagueNameByEnglish = cacheRefService.GetLeagueName(row.LeagueId, Language.English);
                }
            }
        }

    }
}
