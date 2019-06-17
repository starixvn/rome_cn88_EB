namespace EB.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class TicketStatusConstants
    {
        public const int Settled = 0;
        public const int UnSettled = 1;

        public static IDictionary<int, string> TicketStatuses { get; } = new Dictionary<int, string>
        {
            [Settled] = nameof(Settled),
            [UnSettled] = nameof(UnSettled)
        };
    }
}
