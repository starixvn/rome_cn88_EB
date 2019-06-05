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
            decimal houseIncome,
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
            HouseIncome = houseIncome;
            HeadlineStock = headlineStock;
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

        public decimal HouseIncome { get; protected set; }

        public decimal HeadlineStock { get; protected set; }

        public string BetTypeGroupName { get; set; }

        public string LeagueName { get; set; }

        public string EventInfo { get; set; }

        public MatchWinLossModel Add(MatchWinLossModel match)
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
