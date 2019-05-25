using EB.Features.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Shared.Model
{
    public class SportGroup
    {
        private readonly Func<int, string> getSportGroupDisplayName;

        internal SportGroup(
            int id,
            IEnumerable<int> displaySportIds,
            Func<int, string> getSportGroupDisplayName)
        {
            Id = id;
            DisplaySportIds = displaySportIds;
            this.getSportGroupDisplayName = getSportGroupDisplayName;
        }

        public int Id { get; }

        public IEnumerable<int> DisplaySportIds { get; }

        public string DisplayName => GetDisplayName();

        private string GetDisplayName()
        {
            if (Id == SportGroupConstants.ParlaySportGroupId)
            {
                return "SharedResources.Parlay";
            }

            if (Id == SportGroupConstants.VirtualSportsGroupId)
            {
                return "SharedResources.VirtualSports";
            }

            var stringBuilder = new StringBuilder();
            var length = DisplaySportIds.Count();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append($"{getSportGroupDisplayName(DisplaySportIds.ElementAt(i))}");

                if (i != length - 1)
                {
                    stringBuilder.Append(" / ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
