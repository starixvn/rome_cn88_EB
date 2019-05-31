using System.Collections.Generic;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Shared.Services;
using Fanex.Data.Repository;

namespace EB.Features.Reports.Services
{
    public interface IHouseWinLossService
    {
        IEnumerable<HouseWinLossModel> Get(HouseWinLossParam param);

        IEnumerable<HouseWinLossModel> Get(HouseWinLossOldParam param);
    }

    public class HouseWinLossService : IHouseWinLossService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;

        public HouseWinLossService(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
        }

        public IEnumerable<HouseWinLossModel> Get(HouseWinLossParam param)
        {            
            param.ArchiveDate = reportDateService.GetArchiveDate();
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();

            return param.IsSettled?
                dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossCriteria(param)):
                dynamicRepository.Fetch<HouseWinLossModel>(new UnsettledHouseWinLossCriteria(param));
	}
        public IEnumerable<HouseWinLossModel> Get(HouseWinLossOldParam param)
        {
            param.ArchiveDate = reportDateService.GetArchiveDate();
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();

            return dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossCriteria(param));
        }
    }
}
