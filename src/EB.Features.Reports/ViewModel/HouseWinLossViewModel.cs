namespace EB.Features.Reports.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;

    public class HouseWinLossViewModel
    {
        public HouseWinLossViewModel(IEnumerable<HouseWinLossModel> matches)
        {
            Total = matches.Sum();
            Matches = matches;
        }

        public IEnumerable<HouseWinLossModel> Matches { get; }

        public HouseWinLossModel Total { get; }

        public bool IsShowTotalRow => Matches.Any();
    }
}
