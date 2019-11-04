namespace EB.Features.Reports.Criteria
{
    using EB.Features.Reports.Params;

    public class UnsettledCountHeadlineHouseWLCriteria : HouseWinLossBaseCriteria
    {
        public UnsettledCountHeadlineHouseWLCriteria(HouseWinLossParam param) : base(param)
        {
        }

        public override string GetSettingKey()
            => "CS_EB_Rpt_Outstanding_Headline_CustCount";
    }
}
