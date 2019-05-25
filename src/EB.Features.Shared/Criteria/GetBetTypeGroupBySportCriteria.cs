using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Shared.Criteria
{
    public class GetBetTypeGroupBySportCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_BetType_Sel";

        public override bool IsValid() => true;
    }
}
