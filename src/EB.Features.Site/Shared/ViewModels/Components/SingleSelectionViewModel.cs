namespace EB.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;

    public class SingleSelectionViewModel
    {
        public SingleSelectionViewModel(
            IEnumerable<SelectListItemExt> items,
            int userSelectedId)
        {
            Items = items;
            UserSelectedId = userSelectedId;
        }

        public IEnumerable<SelectListItemExt> Items { get; }

        public int UserSelectedId { get; }
    }
}