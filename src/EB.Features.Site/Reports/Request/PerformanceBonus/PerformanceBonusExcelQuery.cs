namespace EB.Features.Site.Reports.Request
{
    using EB.Features.Site.Reports.ViewModel;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class PerformanceBonusExcelQuery : PerformanceBonusQuery, IRequest<ExcelFileViewModel>
    {
        public PerformanceBonusExcelQuery(UserSession currentSession)
            : base(currentSession)
        {           
        }
    }
}