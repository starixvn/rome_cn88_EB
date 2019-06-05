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
            MatchesHasNoParlay = matches.Where(i => i.BetTypeGroup != BetTypeIdConstants.SystemParlay);
            NoParlayMatchesHasMajorBetType = MatchesHasNoParlay
                .Where(i => BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup))
                .OrderBy(i => i.BetTypeGroup);
            NoParlayMatchesHasMinorBetType = MatchesHasNoParlay
                .Where(i => !BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup));
            Parlay = matches.FirstOrDefault(i => i.BetTypeGroup == BetTypeIdConstants.SystemParlay);
        }

        public IEnumerable<MatchWinLossModel> MatchesHasNoParlay { get; }

        public IEnumerable<MatchWinLossModel> NoParlayMatchesHasMajorBetType { get; }

        public IEnumerable<MatchWinLossModel> NoParlayMatchesHasMinorBetType { get; }

        public MatchWinLossModel Parlay { get; }

        public MatchWinLossModel Total { get; }

        public bool IsShowTotalRow => MatchesHasNoParlay.Any() || Parlay != null;
    }
}
