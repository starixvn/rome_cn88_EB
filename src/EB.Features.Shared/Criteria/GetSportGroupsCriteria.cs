using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Shared.Criteria
{
    public class GetSportGroupsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_SportType_Sel";

        public override bool IsValid() => true;
    }
}
