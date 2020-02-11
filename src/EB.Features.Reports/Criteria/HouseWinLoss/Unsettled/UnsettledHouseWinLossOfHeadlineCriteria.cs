namespace EB.Features.Reports.Criteria
{
    using EB.Features.Reports.Params;

    public class UnsettledHouseWinLossOfHeadlineCriteria : HouseWinLossBaseCriteria
    {
        public UnsettledHouseWinLossOfHeadlineCriteria(HouseWinLossParam param) : base(param)
        {
        }

        public override string GetSettingKey()
            => IsCountUser ? "CS_EB_Rpt_Outstanding_Headline_CustCount" : "CS_EB_Rpt_Outstanding_Headline";
    }
}
