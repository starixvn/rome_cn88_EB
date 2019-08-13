namespace EB.Features.Shared.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Resources;

    public class SportGroup
    {
        private readonly Func<int, string> getSportGroupDisplayName;

        private readonly Func<int, string> getSportGroupDisplayNameEnglish;

        public SportGroup()
        {

        }

        internal SportGroup(
            int id,
            IEnumerable<int> displaySportIds,
            Func<int, string> getSportGroupDisplayName,
            Func<int, string> getSportGroupDisplayNameEnglish)
        {
            Id = id;
            DisplaySportIds = displaySportIds;
            this.getSportGroupDisplayName = getSportGroupDisplayName;
            this.getSportGroupDisplayNameEnglish = getSportGroupDisplayNameEnglish;
        }

        public int Id { get; }


        public IEnumerable<int> DisplaySportIds { get; }

        public string DisplayName => GetDisplayName(getSportGroupDisplayName);

        public string DisplayNameByEnglish => GetDisplayName(getSportGroupDisplayNameEnglish);

        private string GetDisplayName(Func<int, string> functionToGetDisplayName)
        {
            if (Id == SportGroupConstants.ParlaySportGroupId)
            {
                return AppResources.Parlay;
            }

            if (Id == SportGroupConstants.VirtualSportsGroupId)
            {
                return AppResources.VirtualSports;
            }

            var stringBuilder = new StringBuilder();
            var length = DisplaySportIds.Count();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append($"{functionToGetDisplayName(DisplaySportIds.ElementAt(i))}");

                if (i != length - 1)
                {
                    stringBuilder.Append(" / ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
