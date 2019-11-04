namespace EB.Features.Reports.Criteria
{
    using EB.Features.Reports.Params;

    public class SettledHouseWinLossOfHeadlineCriteria : HouseWinLossBaseCriteria
    {

        public SettledHouseWinLossOfHeadlineCriteria(HouseWinLossParam param) : base(param)
        {
        }

        public override string GetSettingKey()
            => "CS_Rpt_EBWinloss_Headline";
    }
}
