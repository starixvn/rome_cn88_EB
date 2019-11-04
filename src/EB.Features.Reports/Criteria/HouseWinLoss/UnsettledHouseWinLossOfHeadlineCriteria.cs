namespace EB.Features.Reports.Criteria
{
    using EB.Features.Reports.Params;

    public class UnsettledHouseWinLossOfHeadlineCriteria : HouseWinLossBaseCriteria
    {
        public UnsettledHouseWinLossOfHeadlineCriteria(HouseWinLossParam param) : base(param)
        {
        }

        public override string GetSettingKey()
            => "CS_Rpt_EBOutstanding_Headline";
    }
}
