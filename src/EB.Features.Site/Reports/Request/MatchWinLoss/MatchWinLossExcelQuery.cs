namespace EB.Features.Site.Reports.Request
{
    using EB.Features.Site.Reports.ViewModel;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class MatchWinLossExcelQuery : MatchWinLossQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossExcelQuery(UserSession currentSession)
            : base(currentSession)
        {
        }
    }
}