namespace EB.Features.Site.Reports.Request
{
    using EB.Features.Site.Reports.ViewModel;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class MatchWinLossExcelQuery : MatchWinLossQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossExcelQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}