namespace EB.Features.Site.Reports
{
    using EB.Features.Reports;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared;
    using EB.Features.Shared.Configuration;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class HouseWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<HouseWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly IHouseWinLossService houseWinLossService;
        private readonly IHouseWinLossWorkbook workbook;
        private readonly ICacheRefService cacheRefService;

        public HouseWinLossExcelHandler(
            IHouseWinLossService matchWinLossService,
            IHouseWinLossWorkbook workbook,
            ICacheRefService cacheRefService)
        {
            this.houseWinLossService = matchWinLossService;
            this.workbook = workbook;
            this.cacheRefService = cacheRefService;
        }

        public ExcelFileViewModel Handle(HouseWinLossExcelQuery message)
        {
            var reportData = houseWinLossService.Get(new HouseWinLossParam
            {
                IsHistoricReport = message.IsHistoricReport,
                TicketStatusId = message.UserSelectedTicketStatusId,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                Date = message.Date
            });

            var fileContent = workbook
                .SetData(reportData.Matches, reportData.Total)
                .SetRowMappers(
                    GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.NormalRow),
                    GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.GrandTotal))
                .UseTemplate(GetTemplateInfo())
                .Sheet("ReportData")
                .Title(GetReportTitle(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Footer(reportData.Total, GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.GrandTotal))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        internal static Mapper<HouseWinLossModel> GetRowMapper(decimal totalStock, int rowType)
        {
            var mapper = new Mapper<HouseWinLossModel>();

            if (rowType == RowTypeConstant.NormalRow)
            {
                mapper.Map(A, betTypeWinLoss => betTypeWinLoss.UserName);
            }
            else
            {
                mapper.Map(A, betTypeWinLoss => string.Empty);
            }

            mapper
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake / AppSettings.ReduceNumber)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake / AppSettings.ReduceNumber)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome / AppSettings.ReduceNumber)
                .Map(F, betTypeWinLoss => betTypeWinLoss.SuperComm / AppSettings.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HouseWinLoss / AppSettings.ReduceNumber)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HouseComm / AppSettings.ReduceNumber)
                .Map(I, betTypeWinLoss => betTypeWinLoss.HouseIncome / AppSettings.ReduceNumber)
                .Map(J, betTypeWinLoss => betTypeWinLoss.HeadlineStock / AppSettings.ReduceNumber)
                .Map(K, betTypeWinLoss => (totalStock == 0 ? 0 : betTypeWinLoss.HeadlineStock / totalStock * AppSettings.ReduceNumber));

            return mapper;
        }

        private TemplateInfo GetTemplateInfo()
          => new TemplateInfo(
              GetExcelTemplatePath("HouseWinLossReport"),
              noInfomationText: AppResources.NoInformationIsAvailable,
              titleStartIndex: 1,
              headerStartIndex: 2,
              bodyStartIndex: 3,
              bodyEndIndex: 4,
              freezeRowIndex: 3,
              columnStartName: A,
              sheetIndex: 1,
              footerEndIndex: 5);

        internal string GetReportTitle(HouseWinLossExcelQuery param)
        {
            var title = $"{ReportResources.HouseWinLossReport} {ReportResources.Of} {param.FromDate.ToShortDate()}";
            if (param.IsHistoricReport)
            {
                title = $"{ReportResources.HouseWinLossReport} {param.FromDate.ToShortDate()} --> {param.ToDate.ToShortDate()}";
            }

            var liveStatus = LiveStatusConstants.LiveStatuses[param.UserSelectedLiveStatusId];
            var ticketStatus = TicketStatusConstants.TicketStatuses[param.UserSelectedTicketStatusId];
            var betType = param.UserSelectedBetTypeId == 0 ? AppResources.AllBetTypes : cacheRefService.GetBetTypeName(param.UserSelectedBetTypeId);
            var sportType = param.UserSelectedSportGroupId == 0 ? AppResources.AllSports : cacheRefService.GetSportName(param.UserSelectedSportGroupId);

            return $"{title} - {GetResourcesText(ticketStatus)} - {sportType} - {betType} - {GetResourcesText(liveStatus)}";
        }

        private static string GetResourcesText(string key)
            => AppResources.ResourceManager.GetString(key);

        internal string GetExcelFileName(HouseWinLossExcelQuery param)
            => param.IsHistoricReport ?
            $"Wager Report - {param.FromDate.ToShortDate()}-{param.ToDate.ToShortDate()}" :
            $"Wager Report - {param.Date.ToShortDate()}";
    }
}