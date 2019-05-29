namespace EB.Features.Site.Reports.Request.HouseWinLoss
{
    using Fanex.NAP.Core.Web.Session;
    using MediatR;
    using ViewModel;

    public class HouseWinLossExcelQuery: HouseWinLossQuery, IRequest<ExcelFileViewModel>
    {
        public HouseWinLossExcelQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}