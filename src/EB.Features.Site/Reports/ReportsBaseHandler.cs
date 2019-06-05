namespace EB.Features.Site.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Resources;
    using EB.Features.Site.Shared.ViewModels;

    public class ReportsBaseHandler
    {
        public ReportsBaseHandler()
        {
        }

        protected internal IEnumerable<SelectListItemExt> BuildSportGroupSelectListItems(
            IEnumerable<SportGroup> sportGroups,
            int userSelectedSportGroupId)
        {
            var items = sportGroups.Select(sportGroup => new SelectListItemExt
            {
                Text = sportGroup.DisplayName,
                Value = sportGroup.Id.ToString(),
                Selected = sportGroup.Id == userSelectedSportGroupId
            }).ToList();

            items.Insert(0, new SelectListItemExt { Text = AppResources.AllSports, Value = "0", Selected = userSelectedSportGroupId == 0 });

            return items;
        }

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
    }
}