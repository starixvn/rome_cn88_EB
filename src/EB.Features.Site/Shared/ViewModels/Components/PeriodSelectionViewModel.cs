namespace EB.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;

    public class PeriodSelectionViewModel
    {
        public PeriodSelectionViewModel(
            IEnumerable<SelectListItemExt> items,
            string userSelectedId)
        {
            Items = items;
            UserSelectedId = userSelectedId;
        }

        public IEnumerable<SelectListItemExt> Items { get; }

        public string UserSelectedId { get; }
    }
}