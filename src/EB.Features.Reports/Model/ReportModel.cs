namespace EB.Features.Reports.Model
{
    public class ReportModel
    {
        public ReportModel()
        {

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
