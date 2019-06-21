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
            NormalMatches = matches.Where(i => i.BetTypeGroup != BetTypeIdConstants.SystemParlay);
            NormalMatchesHasMajorBetType = NormalMatches
                .Where(i => BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup))
                .OrderBy(i => i.BetTypeGroup);
            NormalMatchesHasMinorBetType = NormalMatches
                .Where(i => !BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup) && i.BetTypeGroup != BetTypeIdConstants.Outright);
            NormalMatchesHasOutright = matches.Where(i => i.BetTypeGroup == BetTypeIdConstants.Outright);

            Parlay = matches.FirstOrDefault(i => i.BetTypeGroup == BetTypeIdConstants.SystemParlay);            
        }

        public IEnumerable<MatchWinLossModel> NormalMatches { get; }

        public IEnumerable<MatchWinLossModel> NormalMatchesHasMajorBetType { get; }

        public IEnumerable<MatchWinLossModel> NormalMatchesHasMinorBetType { get; }

        public IEnumerable<MatchWinLossModel> NormalMatchesHasOutright { get; }

        public MatchWinLossModel Parlay { get; }

        public MatchWinLossModel Total { get; }

        public bool IsShowTotalRow => NormalMatches.Any() || Parlay != null;
    }
}
