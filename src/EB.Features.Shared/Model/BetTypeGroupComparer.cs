using System.Collections.Generic;

namespace EB.Features.Shared.Model
{
    public class BetTypeGroupComparer : IEqualityComparer<BetTypeGroup>
    {
        public bool Equals(BetTypeGroup x, BetTypeGroup y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(BetTypeGroup obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
