using System.Collections.Generic;
using System.Web.Mvc;

namespace EB.Features.Site.Shared.ViewModels.Components
{
    public class SportGroupMultiSelectionViewModel
    {
        public SportGroupMultiSelectionViewModel(
            IEnumerable<SelectListItem> sportGroups,
            string userSelectedSportTypeIds,
            string language)
        {
            SportGroups = sportGroups;
            UserSelectedSportGroupIds = userSelectedSportTypeIds;
            Language = language;
        }

        public IEnumerable<SelectListItem> SportGroups { get; }

        public string Language { get; }

        public string UserSelectedSportGroupIds { get; }
    }
}