using System.Collections.Generic;

namespace EB.Features.Reports.Constants
{
    public static class ConjunctionConstant
    {
        public const int Or = 1;
        public const int And = 0;

        public static IDictionary<int, string> List { get; } = new Dictionary<int, string>
        {
            [And] = nameof(And),
            [Or] = nameof(Or)          
        };
    }
}
