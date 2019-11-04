namespace EB.Features.Reports.Criteria
{
    using EB.Features.Reports.Params;

    public class SettledCountHeadlineHouseWLCriteria : HouseWinLossBaseCriteria
    {

        public SettledCountHeadlineHouseWLCriteria(HouseWinLossParam param) : base(param)
        {
        }

        public override string GetSettingKey()
            => "CS_EB_Rpt_Winloss_Headline_CustCount";
    }
}
