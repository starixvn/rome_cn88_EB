using EB.Features.Reports.Model;

namespace EB.Features.Reports.ViewModel
{
    public class MatchWinLossViewModel : MatchWinLossModel
    {
        public MatchWinLossViewModel()
        {

        }

        public MatchWinLossViewModel(MatchWinLossModel model)
            : base(model.LeagueId,
            model.MatchId,
            model.HomeId,
            model.AwayId,
            model.BetTypeGroup,
            model.Wagers,
            model.Stake,
            model.ValidStake,
            model.MemberIncome,
            model.HouseIncome,
            model.HeadlineStock)
        {

        }

        public string BetTypeGroupName { get; set; }

        public string LeagueName { get; set; }

        public string EventInfo { get; set; }

        public MatchWinLossViewModel Add(MatchWinLossViewModel match)
        {
            Wagers += match.Wagers;
            Stake += match.Stake;
            ValidStake += match.ValidStake;
            MemberIncome += match.MemberIncome;
            HouseIncome += match.HouseIncome;
            HeadlineStock += match.HeadlineStock;

            return this;
        }
    }
}
