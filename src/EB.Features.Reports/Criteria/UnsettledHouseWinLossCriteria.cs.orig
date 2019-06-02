using System;
using EB.Features.Reports.Params;
using Fanex.Data;
using Fanex.Data.Repository;

namespace EB.Features.Reports.Criteria
{
    public class UnsettledHouseWinLossCriteria : CriteriaBase
    {
        public UnsettledHouseWinLossCriteria(HouseWinLossParam param)
        {
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroup = param.SportTypeGroup;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;            
        }
        
        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public override bool IsValid()
            => true;

        public override string GetSettingKey()
            => "CS_Rpt_Outstanding_Headline";
    }
}
