namespace EB.Features.Reports.Model
{
    public class HouseWinLossModel
    {
        public HouseWinLossModel()
        {

        }

        public int CustId { get; }

        public string UserName { get; }

        public int Wager { get; }

        public decimal Stake { get; }

        public decimal ValidStake { get; }

        public decimal MemberIncome { get; }

        public decimal SuperComm { get; }

        public decimal HouseWinLoss { get; }

        public decimal HouseComm { get; }

        public decimal HouseIncome { get; }

        public decimal HeadlineStock { get; }
    }
}
