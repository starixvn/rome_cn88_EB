namespace EB.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class TicketStatusConstants
    {
        public const int Settled = 1;
        public const int UnSettled = 3;
        public const int AbnormalBets = 7;
        public const int ProblemAccounts = 5;

        public static IDictionary<int, string> TicketStatuses { get; } = new Dictionary<int, string>
        {
            [Settled] = nameof(Settled),
            [UnSettled] = nameof(UnSettled),
            [AbnormalBets] = nameof(AbnormalBets),
            [ProblemAccounts] = nameof(ProblemAccounts),
        };

        public static string GetTicketStatus(int key)
            => TicketStatuses.ContainsKey(key) ? TicketStatuses[key] : TicketStatuses[Settled];
    }
}
