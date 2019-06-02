namespace EB.Features.Reports.Model
{
    public class HouseWinLossModel
    {
        public HouseWinLossModel()
        {

        }

        public int CustId { get; }

        public string UserName { get; }

        public int Wagers { get; private set; }

        public decimal Stake { get; private set; }

        public decimal ValidStake { get; private set; }

        public decimal MemberIncome { get; private set; }

        public decimal SuperComm { get; private set; }

        public decimal HouseWinLoss { get; private set; }

        public decimal HouseComm { get; private set; }

        public decimal HouseIncome { get; private set; }

        public decimal HeadlineStock { get; private set; }

        public HouseWinLossModel Add(HouseWinLossModel model)
        {
            Wagers += model.Wagers;
            Stake += model.Stake;
            ValidStake += model.ValidStake;
            MemberIncome += model.MemberIncome;
            SuperComm += model.SuperComm;
            HouseWinLoss += model.HouseWinLoss;
            HouseComm += model.HouseComm;
            HouseIncome += model.HouseIncome;
            HeadlineStock += model.HeadlineStock;

            return this;
        }
    }
}
