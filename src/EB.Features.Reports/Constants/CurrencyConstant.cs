using System.Collections.Generic;

namespace EB.Features.Reports.Constants
{
    public static class CurrencyConstant
    {
        public const int CY = 81;
        public const int UUS = 20;

        public static IDictionary<int, string> List { get; } = new Dictionary<int, string>
        {
            [CY] = nameof(CY),
            [UUS] = nameof(UUS)
        };
    }
}
