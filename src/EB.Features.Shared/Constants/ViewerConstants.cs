namespace EB.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class ViewerConstants
    {
        public const int Headline = 0;
        public const int SMA = 1;

        public static IDictionary<int, string> Viewers { get; } = new Dictionary<int, string>
        {
            [Headline] = nameof(Headline),
            [SMA] = nameof(SMA)
        };
    }
}
