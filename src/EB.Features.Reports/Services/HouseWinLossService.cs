using EB.Features.Reports.Criteria;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Shared.Services;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return dynamicRepository.Fetch<HouseWinLossModel>(new HouseWinLossCriteria(param));
        }

        public IEnumerable<HouseWinLossModel> Get(HouseWinLossOldParam param)
        {
            param.ArchiveDate = reportDateService.GetArchiveDate();
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();

            return dynamicRepository.Fetch<HouseWinLossModel>(new HouseWinLossCriteria(param));
        }
    }
}
