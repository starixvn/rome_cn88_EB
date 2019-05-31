namespace EB.Features.Reports.Model
{
    public class ReportModel
    {
        public ReportModel()
        {

        }

        public ReportModel(int leagueId,
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
            HeadlineStock = HouseIncome;
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
    }
}
