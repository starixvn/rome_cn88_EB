using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Shared.Constants
{
    public static class BetTypeIdConstants
    {
        public const int Handicap = 1;
        public const int OverUnder = 3;
        public const int CorrectScore = 4;
        public const int OneXTwo = 5;
        public const int Moneyline = 20;
        public const int FirstHandicap = 7;
        public const int FirstOverUnder = 8;
        public const int FirstOneXTwo = 15;
        public const int FirstCorrectScore = 30;
        public const int FirstMoneyline = 21;

        public const int Outright = 10;
        public const int SystemParlay = 29;

        public static List<int> MajorBetType = new List<int> {
            Handicap,
            OverUnder,
            CorrectScore,
            OneXTwo,
            Moneyline,
            FirstCorrectScore,
            FirstHandicap,
            FirstMoneyline,
            FirstOneXTwo,
            FirstOverUnder };
    }
}
