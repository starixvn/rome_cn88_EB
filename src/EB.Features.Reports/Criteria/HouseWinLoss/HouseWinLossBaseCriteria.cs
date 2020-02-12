namespace EB.Features.Reports.Criteria
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class HouseWinLossBaseCriteria : CriteriaBase
    {
        public HouseWinLossBaseCriteria(HouseWinLossParam param)
        {
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroups = param.SportTypeGroups;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;
            IsCountUser = param.IsCountUser;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string SportTypeGroups { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        [SpParam(Ignored = true)]
        public bool IsCountUser { get; }

        public override bool IsValid()
            => BetTypeGroup >= 0 && IsLive >= 0 && FromDate <= ToDate;

        public override Hashtable GetSpParams()
        {
            var spParams = base.GetSpParams();
            
            spParams.Add("SettingKey", GetSettingKey());

            return spParams;
        }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}
