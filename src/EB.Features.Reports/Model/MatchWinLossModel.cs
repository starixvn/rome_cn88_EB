using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Reports.Model
{
    public class MatchWinLossModel
    {
        public MatchWinLossModel()
        {

        }

        public MatchWinLossModel(int leagueId, 
            int matchId, 
            int homeId, 
            int awayId, 
            int bettype, 
            int wager, 
            decimal stake, 
            decimal validStake,
            decimal memberIncome, 
            decimal headlineResult, 
            decimal headlineStock)
        {
            LeagueId = leagueId;
            MatchId = matchId;
            HomeId = homeId;
            AwayId = awayId;
            BetTypeGroup = bettype;
            Wagers = wager;
            Stake = stake;
            ValidStake = validStake;
            MemberIncome = memberIncome;
            HeadlineResult = headlineResult;
            HeadlineStock = headlineResult;
        }
        public int LeagueId { get; }

        public int MatchId { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public int BetTypeGroup { get; }

        public int Wagers { get; protected set; }

        public decimal Stake { get; protected set; }

        public decimal ValidStake { get; protected set; }

        public decimal MemberIncome { get; protected set; }

        public decimal HeadlineResult { get; protected set; }

        public decimal HeadlineStock { get; protected set; }
    }
}
