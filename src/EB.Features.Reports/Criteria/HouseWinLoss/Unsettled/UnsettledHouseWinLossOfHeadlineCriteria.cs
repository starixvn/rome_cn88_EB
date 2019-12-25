namespace EB.Features.Reports.Criteria
{
    using System.Collections;
    using EB.Features.Reports.Params;

    public class UnsettledHouseWinLossOfHeadlineCriteria : HouseWinLossBaseCriteria
    {
        public UnsettledHouseWinLossOfHeadlineCriteria(HouseWinLossParam param) : base(param)
        {
        }

        public override string GetSettingKey()
            => IsCountUser ? "CS_Rpt_EBOutstanding_Headline_CustCount" : "CS_Rpt_EBOutstanding_Headline";
    }
}
