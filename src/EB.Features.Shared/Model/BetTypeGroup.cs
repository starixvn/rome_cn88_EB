using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Shared.Model
{
    public class BetTypeGroup
    {
        public BetTypeGroup()
        {
        }

        internal BetTypeGroup(int sportTypeGroup, int id, Func<int, string> getBetTypeDisplayName)
        {
            SportTypeGroup = sportTypeGroup;
            Id = id;
            DisplayName = getBetTypeDisplayName(id);
        }

        public int SportTypeGroup { get; }

        public int Id { get; }

        public string DisplayName { get; }
    }
}
