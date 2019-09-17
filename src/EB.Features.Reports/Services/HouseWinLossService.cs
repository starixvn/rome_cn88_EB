namespace EB.Features.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Criteria;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Model;
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

            IEnumerable<HouseWinLossModel> reportData;
            if (param.Role.IsHeadline())
            {
                reportData = IsSettled(param) ?
                   dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossOfHeadlineCriteria(param)) :
                   dynamicRepository.Fetch<HouseWinLossModel>(new UnsettledHouseWinLossOfHeadlineCriteria(param));
            }
            else
            {
                reportData = IsSettled(param) ?
                dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossOfSMACriteria(param)) :
                dynamicRepository.Fetch<HouseWinLossModel>(new UnsettledHouseWinLossOfSMACriteria(param));
            }

            return new HouseWinLossViewModel(reportData.OrderBy(i => i.UserName));
        }

        private static bool IsSettled(HouseWinLossParam param)
            => param.IsHistoricReport || param.TicketStatusId == TicketStatusConstants.Settled;

    }
}
