using EB.Features.Reports.Model;
using EB.Features.Reports.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Reports.Extension
{
    public static class EnumerableExtensions
    {
        public static MatchWinLossViewModel Sum(this IEnumerable<MatchWinLossViewModel> matches)
        {
            var total = new MatchWinLossViewModel();
            foreach (var match in matches)
            {
                total = total.Add(match);
            }

            return total;
        }

        public static HouseWinLossModel Sum(this IEnumerable<HouseWinLossModel> matches)
        {
            var total = new HouseWinLossModel();
            foreach (var match in matches)
            {
                total = total.Add(match);
            }

            return total;
        }
    }
}
