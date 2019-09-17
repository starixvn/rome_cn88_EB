using System;

namespace EB.Features.Reports.Model
{
    public class HouseWinLossModel
    {
        public HouseWinLossModel()
        {

        }

        public int HeadlineId { get; }

        public string HeadlineUserName { get; }

        public int CustId { get; }

        public string UserName { get; internal set; }

        public int Wagers { get; internal set; }

        public decimal Stake { get; internal set; }

        public decimal ValidStake { get; internal set; }

        public decimal MemberWinLoss { get; internal set; }

        public decimal MemberComm { get; internal set; }

        public decimal MemberIncome { get; internal set; }

        public decimal SuperComm { get; internal set; }

        public decimal HouseWinLoss { get; internal set; }

        public decimal HouseComm { get; internal set; }

        public decimal HouseTurnover { get; internal set; }

        public decimal HouseIncome { get; internal set; }

        public decimal HeadlineResult { get; internal set; }

        public decimal HeadlineIncome { get; internal set; }

        public decimal RebateRoundDown { get; internal set; }

        public decimal Rebate { get; internal set; }


        public decimal HeadlineResultNet
        {
            get
            {
                return HeadlineResult + RebateRoundDown;
            }

            internal set
            {
                
            }
        }

        public decimal HeadlineStock { get; internal set; }

        public decimal Agent { get; internal set; }

        public decimal AgentResult { get; internal set; }

        public decimal AgentStock { get; internal set; }

        public decimal MasterResult { get; internal set; }

        public decimal Super { get; internal set; }

        public decimal SuperResult { get; internal set; }

        public decimal SuperStock { get; internal set; }
        public static HouseWinLossModel operator +(HouseWinLossModel model1, HouseWinLossModel model2)
            => new HouseWinLossModel
            {
                HeadlineResultNet = model1.HeadlineResultNet + model2.HeadlineResultNet,
                Wagers = model1.Wagers + model2.Wagers,
                Stake = model1.Stake + model2.Stake,
                ValidStake = model1.ValidStake + model2.ValidStake,
                MemberIncome = model1.MemberIncome + model2.MemberIncome,
                SuperComm = model1.SuperComm + model2.SuperComm,
                HouseWinLoss = model1.HouseWinLoss + model2.HouseWinLoss,
                HouseComm = model1.HouseComm + model2.HouseComm,
                HouseIncome = model1.HouseIncome + model2.HouseIncome,
                HeadlineStock = model1.HeadlineStock + model2.HeadlineStock,
                MemberWinLoss = model1.MemberWinLoss + model2.MemberWinLoss,
                HouseTurnover = model1.HouseTurnover + model2.HouseTurnover,
                MemberComm = model1.MemberComm + model2.MemberComm,
                HeadlineResult = model1.HeadlineResult + model2.HeadlineResult,
                Agent = model1.Agent + model2.Agent,
                AgentResult = model1.AgentResult + model2.AgentResult,
                AgentStock = model1.AgentStock + model2.AgentStock,
                MasterResult = model1.MasterResult + model2.MasterResult,
                Super = model1.Super + model2.Super,
                SuperResult = model1.SuperResult + model2.SuperResult,
                SuperStock = model1.SuperStock + model2.SuperStock,
                HeadlineIncome = model1.HeadlineIncome + model2.HeadlineIncome,
                Rebate = model1.Rebate + model2.Rebate,
                RebateRoundDown = model1.RebateRoundDown + model2.RebateRoundDown
            };
    }
}
