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

        public string HomeName { get; set; }

        public int AwayId { get; }

        public string AwayName { get; set; }

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

        public static MatchWinLossModel operator +(MatchWinLossModel match1, MatchWinLossModel match2)
            => new MatchWinLossModel
            {
                Wagers = match1.Wagers + match2.Wagers,
                Stake = match1.Stake + match2.Stake,
                ValidStake = match1.ValidStake + match2.ValidStake,
                MemberIncome = match1.MemberIncome + match2.MemberIncome,
                HouseIncome = match1.HouseIncome = match2.HouseIncome,
                HeadlineStock = match1.HeadlineStock + match2.HeadlineStock
            };
    }
}
