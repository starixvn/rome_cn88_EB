namespace EB.Features.Reports.Model
{
    public class MatchWinLossModel
    {
        public MatchWinLossModel()
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

        public string BetTypeGroupName { get; set; }

        public string LeagueName { get; set; }

        public string LeagueNameByEnglish { get; set; }

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
