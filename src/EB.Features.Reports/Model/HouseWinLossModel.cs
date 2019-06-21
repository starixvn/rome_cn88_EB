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

        public static HouseWinLossModel operator +(HouseWinLossModel model1, HouseWinLossModel model2)
            => new HouseWinLossModel
            {
                Wagers = model1.Wagers + model2.Wagers,
                Stake = model1.Stake + model2.Stake,
                ValidStake = model1.ValidStake + model2.ValidStake,
                MemberIncome = model1.MemberIncome + model2.MemberIncome,
                SuperComm = model1.SuperComm + model2.SuperComm,
                HouseWinLoss = model1.HouseWinLoss + model2.HouseWinLoss,
                HouseComm = model1.HouseComm + model2.HouseComm,
                HouseIncome = model1.HouseIncome + model2.HouseIncome,
                HeadlineStock = model1.HeadlineStock + model2.HeadlineStock
            };
    }
}
