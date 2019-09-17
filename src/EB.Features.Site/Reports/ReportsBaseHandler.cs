namespace EB.Features.Site.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using EB.Features.Reports.Extension;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Resources;
    using EB.Features.Site.Shared.ViewModels;

    public class ReportsBaseHandler
    {
        protected internal static IEnumerable<SelectListItem> BuildSportGroupSelectListItems(
            IEnumerable<SportGroup> sportTypes,
            IEnumerable<int> userSelectedSportTypeIds)
            => sportTypes?.Any() == true
                ? sportTypes.Select(product => new SelectListItem
                {
                    Text = product.DisplayName,
                    Value = product.Id.ToString(),
                    Selected = userSelectedSportTypeIds.Contains(product.Id)
                })
                : Enumerable.Empty<SelectListItem>();

        protected internal IEnumerable<SelectListItemExt> BuildBetTypeSelectListItems(
            IEnumerable<BetTypeGroup> betTypes,
            int userSelectedBetTypeId)
        {
            var items = betTypes.Select(betType => new SelectListItemExt
            {
                SportTypeGroup = betType.SportTypeGroup,
                Text = betType.DisplayName,
                Value = betType.Id.ToString(),
                Selected = betType.Id == userSelectedBetTypeId
            }).ToList();

            items.Insert(0, new SelectListItemExt { Text = AppResources.AllBetTypes, Value = "0", Selected = userSelectedBetTypeId == 0 });

            return items;
        }

        protected internal static string GetValidSportGroupIds(IEnumerable<int> allSportIds, string sportGroupIds)
        {
            if (string.IsNullOrEmpty(sportGroupIds))
            {
                return allSportIds.JoinWithComma();
            }

            return sportGroupIds.ToListInt().Where(id => allSportIds.Contains(id)).JoinWithComma();
        }
    }
}