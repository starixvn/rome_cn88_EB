namespace EB.Features.Reports.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;
    using EB.Features.Shared.Constants;

    public class MatchWinLossViewModel
    {
        public MatchWinLossViewModel(IEnumerable<MatchWinLossModel> matches)
        {
            Total = matches.Sum();
            SportbookMatches = matches.Where(i => i.BetTypeGroup != BetTypeIdConstants.SystemParlay && !i.IsNonSports);
            SportbookMatchesHasMajorBetType = SportbookMatches
                .Where(i => BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup))
                .OrderBy(i => i.BetTypeGroup);
            SportbookMatchesHasMinorBetType = SportbookMatches
                .Where(i => !BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup) && i.BetTypeGroup != BetTypeIdConstants.Outright);
            SportbookMatchesHasOutright = matches.Where(i => i.BetTypeGroup == BetTypeIdConstants.Outright);

            Parlay = matches.FirstOrDefault(i => i.BetTypeGroup == BetTypeIdConstants.SystemParlay && !i.IsNonSports);
            NonSportMatches = matches.Where(i => i.IsNonSports).OrderBy(i => i.BetTypeGroupNameByEnglish);
        }

        public IEnumerable<MatchWinLossModel> SportbookMatches { get; }

        public IEnumerable<MatchWinLossModel> SportbookMatchesHasMajorBetType { get; }

        public IEnumerable<MatchWinLossModel> SportbookMatchesHasMinorBetType { get; }

        public IEnumerable<MatchWinLossModel> SportbookMatchesHasOutright { get; }

        public IEnumerable<MatchWinLossModel> NonSportMatches { get; }

        public MatchWinLossModel Parlay { get; }

        public MatchWinLossModel Total { get; }

        public bool IsShowTotalRow => SportbookMatches.Any() || Parlay != null || NonSportMatches.Any();
    }
}
