namespace EB.Features.Site.Reports.ViewModel.HouseWinLoss
{
    using System.Collections.Generic;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Resources;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Resources;

    public class CountHouseWinLossViewModel
    {
        public IEnumerable<ItemModel> Users { get; }

        public CountHouseWinLossViewModel(CountHouseWinLossModel model, UserLevel userLevel)
        {
            var countAllUsers = new Dictionary<UserLevel, ItemModel>
            {
                { UserLevel.Headline, new ItemModel(ReportResources.Headline, model.HeadlineCount.ToString()) },
                { UserLevel.Super, new ItemModel(ReportResources.SMA, model.SuperCount.ToString()) },
                { UserLevel.Master, new ItemModel(ReportResources.MA, model.MasterCount.ToString()) },
                { UserLevel.Agent, new ItemModel(ReportResources.Agent, model.AgentCount.ToString()) },
                { UserLevel.Member, new ItemModel(AppResources.Member, model.MemberCount.ToString()) },
            };

            Users = FilterByUserLevel(countAllUsers, userLevel);
        }

        private static IEnumerable<ItemModel> FilterByUserLevel(Dictionary<UserLevel, ItemModel> countAllUsers, UserLevel userLevel)
        {
            foreach (var item in countAllUsers)
            {
                if (userLevel.IsHeadline() || 
                    (item.Key.IsNotHeadline() && userLevel.IsEqualOrUplineOf(item.Key)))
                {
                    yield return item.Value;
                }
            }
        }
    }
}