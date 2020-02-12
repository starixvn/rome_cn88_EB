namespace EB.Features.Reports.Services
{
    using System;
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

        CountHouseWinLossModel CountUsers(HouseWinLossParam param);
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

        public CountHouseWinLossModel CountUsers(HouseWinLossParam param)
        {
            param.IsCountUser = true;
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();
            param.FromDate = param.IsHistoricReport ? param.FromDate : param.Date;
            param.ToDate = param.IsHistoricReport ? param.ToDate : param.Date;

            var criteria = param.Role.IsHeadline() ?
                GetHeadlineCriteriaByTicketStatus(param) :
                GetSMACriteriaByTicketStatus(param);

            return dynamicRepository.Get<CountHouseWinLossModel>(criteria.GetSpParams());
        }

        public HouseWinLossViewModel Get(HouseWinLossParam param)
        {
            param.IsCountUser = false;
            param.MoveBetsDay = reportDateService.GetMoveBetsDay();
            param.FromDate = param.IsHistoricReport ? param.FromDate : param.Date;
            param.ToDate = param.IsHistoricReport ? param.ToDate : param.Date;

            var criteria = param.Role.IsHeadline() ?
                GetHeadlineCriteriaByTicketStatus(param) :
                GetSMACriteriaByTicketStatus(param);

            var reportData = dynamicRepository.Fetch<HouseWinLossModel>(criteria.GetSpParams());

            return new HouseWinLossViewModel(
                reportData.Select(i =>
                {
                    i.Rebate = -1 * i.HeadlineResult * 10 / 100;
                    i.RebateRoundDown = Math.Sign(i.Rebate) * Math.Floor(Math.Abs(i.Rebate / 100)) * 100;
                    return i;
                }).OrderBy(i => i.UserName));
        }

        internal static CriteriaBase GetHeadlineCriteriaByTicketStatus(HouseWinLossParam param)
        {
            switch (param.TicketStatusId)
            {
                case TicketStatusConstants.AbnormalBets:
                    return new AbnormalWinLossOfHeadlineCriteria(param);
                case TicketStatusConstants.ProblemAccounts:
                    return new ProblemAccountWinLossOfHeadlineCriteria(param);
                case TicketStatusConstants.UnSettled:
                    return new UnsettledHouseWinLossOfHeadlineCriteria(param);
                default:
                    return new SettledHouseWinLossOfHeadlineCriteria(param);
            }
        }

        internal static CriteriaBase GetSMACriteriaByTicketStatus(HouseWinLossParam param)
        {
            switch (param.TicketStatusId)
            {
                case TicketStatusConstants.AbnormalBets:
                    return new AbnormalWinLossOfSMACriteria(param);
                case TicketStatusConstants.ProblemAccounts:
                    return new ProblemAccountWinLossOfSMACriteria(param);
                case TicketStatusConstants.UnSettled:
                    return new UnsettledHouseWinLossOfSMACriteria(param);
                default:
                    return new SettledHouseWinLossOfSMACriteria(param);
            }
        }
    }
}
