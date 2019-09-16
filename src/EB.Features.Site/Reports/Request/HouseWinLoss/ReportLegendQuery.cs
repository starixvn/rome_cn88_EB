namespace EB.Features.Site.Reports.Request.HouseWinLoss
{
    using System.Collections.Generic;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class ReportLegendQuery : BaseQuery, IRequest<IEnumerable<string>>
    {
        public ReportLegendQuery(UserSession currentSession)
           : base(currentSession)
        {

        }
    }
}