namespace EB.Features.Reports.Services
{
    using System.Linq;
    using EB.Features.Reports.Criteria;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Services;
    using Fanex.Data.Repository;

    public interface IHouseWinLossService
    {
        HouseWinLossViewModel Get(HouseWinLossParam param);
    }

    public class HouseWinLossService : IHouseWinLossService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IReportDateService reportDateService;

        public HouseWinLossService(
            IDynamicRepository dynamicRepository,
            IReportDateService reportDateService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
        }

        public HouseWinLossViewModel Get(HouseWinLossParam param)
        {
            param.ArchiveDate = reportDateService.GetArchiveDate();
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();
            param.FromDate = param.IsHistoricReport ? param.FromDate : param.Date;
            param.ToDate = param.IsHistoricReport ? param.ToDate : param.Date;

            var reportData = IsSettled(param) ?
                dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossCriteria(param)) :
                dynamicRepository.Fetch<HouseWinLossModel>(new UnsettledHouseWinLossCriteria(param));

            return new HouseWinLossViewModel(reportData.OrderBy(i => i.UserName));
        }

        private static bool IsSettled(HouseWinLossParam param)
            => param.IsHistoricReport || param.TicketStatusId == TicketStatusConstants.Settled;
    }
}
