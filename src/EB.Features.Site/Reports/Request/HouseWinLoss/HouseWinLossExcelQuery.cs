namespace EB.Features.Site.Reports.Request.HouseWinLoss
{
    using EB.Features.Site.Shared.Models;
    using MediatR;
    using ViewModel;

    public class HouseWinLossExcelQuery: HouseWinLossQuery, IRequest<ExcelFileViewModel>
    {
        public HouseWinLossExcelQuery(UserSession currentSession)
            : base(currentSession)
        {
        }
    }
}