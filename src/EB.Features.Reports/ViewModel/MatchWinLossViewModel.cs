using EB.Features.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            model.HeadlineResult,
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
            HeadlineResult += match.HeadlineResult;
            HeadlineStock += match.HeadlineStock;

            return this;
        }
    }
}
