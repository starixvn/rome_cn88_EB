using EB.Features.Site.Shared;
using EB.Features.Site.Shared.Models;

namespace EB.Features.Site.Reports.Request
{
    public class ReportBaseQuery : BaseQuery
    {
        public ReportBaseQuery(UserSession currentSession)
            : base(currentSession)
        {
        }        
    }
}