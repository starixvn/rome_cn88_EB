using EB.Features.Site.Reports.ViewModel;
using Fanex.NAP.Core.Web.Session;
using MediatR;

namespace EB.Features.Site.Reports.Request
{
    public class MatchWinLossExcelQuery : MatchWinLossQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossExcelQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}