namespace EB.Features.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Criteria;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Services;
    using Fanex.Data.Repository;

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

            var reportData= param.IsSettled?
                dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossCriteria(param)):
                dynamicRepository.Fetch<HouseWinLossModel>(new UnsettledHouseWinLossCriteria(param));

            return reportData.OrderBy(i => i.UserName);
	}
        public IEnumerable<HouseWinLossModel> Get(HouseWinLossOldParam param)
        {
            param.ArchiveDate = reportDateService.GetArchiveDate();
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();

            var reportData = dynamicRepository.Fetch<HouseWinLossModel>(new SettledHouseWinLossCriteria(param));

            return reportData.OrderBy(i => i.UserName);
        }
    }
}
