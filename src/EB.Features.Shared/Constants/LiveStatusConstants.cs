namespace EB.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class LiveStatusConstants
    {
        public const int PreMatch = 0;
        public const int InPlay = 1;
        public const int AllMarkets = 2;

        public static IDictionary<int, string> LiveStatuses { get; } = new Dictionary<int, string>
        {
            [AllMarkets] = nameof(AllMarkets),
            [PreMatch] = nameof(PreMatch),
            [InPlay] = nameof(InPlay)
        };

        public static string GetLiveStatus(int key)
            => LiveStatuses.ContainsKey(key) ? LiveStatuses[key] : LiveStatuses[AllMarkets];
    }
}
