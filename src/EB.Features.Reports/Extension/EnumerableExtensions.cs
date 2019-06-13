namespace EB.Features.Reports.Extension
{
    using System.Collections.Generic;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.ViewModel;

    public static class EnumerableExtensions
    {
        public static MatchWinLossModel Sum(this IEnumerable<MatchWinLossModel> matches)
        {
            var total = new MatchWinLossModel();
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

        public static string JoinWithComma<T>(this IEnumerable<T> source) => string.Join(",", source);
    }
}
