namespace EB.Features.Reports.Extension
{
    using System.Collections.Generic;
    using EB.Features.Reports.Model;

    public static class EnumerableExtensions
    {
        public static PerformanceBonusModel Sum(this IEnumerable<PerformanceBonusModel> models)
        {            
            var total = new PerformanceBonusModel();
            foreach (var model in models)
            {
                total += model;
            }

            return total;
        }

        public static MatchWinLossModel Sum(this IEnumerable<MatchWinLossModel> matches)
        {
            var total = new MatchWinLossModel();
            foreach (var match in matches)
            {
                total += match;
            }

            return total;
        }

        public static HouseWinLossModel Sum(this IEnumerable<HouseWinLossModel> matches)
        {
            var total = new HouseWinLossModel();
            foreach (var match in matches)
            {
                total += match;
            }

            return total;
        }

        public static string JoinWithComma<T>(this IEnumerable<T> source) => string.Join(",", source);
    }
}
