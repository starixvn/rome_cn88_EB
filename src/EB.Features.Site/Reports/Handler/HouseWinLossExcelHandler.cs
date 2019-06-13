namespace EB.Features.Site.Reports
{
    using EB.Features.Reports;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class HouseWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<HouseWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly IHouseWinLossService houseWinLossService;
        private readonly IHouseWinLossWorkbook workbook;

        public HouseWinLossExcelHandler(
            IHouseWinLossService matchWinLossService,
            IHouseWinLossWorkbook workbook)
        {
            this.houseWinLossService = matchWinLossService;
            this.workbook = workbook;
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
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake / CommonConstants.ReduceNumber)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake / CommonConstants.ReduceNumber)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome / CommonConstants.ReduceNumber)
                .Map(F, betTypeWinLoss => betTypeWinLoss.SuperComm / CommonConstants.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HouseWinLoss / CommonConstants.ReduceNumber)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HouseComm / CommonConstants.ReduceNumber)
                .Map(I, betTypeWinLoss => betTypeWinLoss.HouseIncome / CommonConstants.ReduceNumber)
                .Map(J, betTypeWinLoss => betTypeWinLoss.HeadlineStock / CommonConstants.ReduceNumber)
                .Map(K, betTypeWinLoss => (totalStock == 0 ? 0 : betTypeWinLoss.HeadlineStock / totalStock * CommonConstants.ReduceNumber));

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

        internal static string GetReportTitle(HouseWinLossExcelQuery param)
        {
            if (param.IsHistoricReport)
            {
                return $"{ReportResources.HouseWinLossReport} {param.FromDate.ToShortDate()} --> {param.ToDate.ToShortDate()}";
            }

            return $"{ReportResources.HouseWinLossReport} {ReportResources.Of} {param.FromDate.ToShortDate()}";
        }

        internal static string GetExcelFileName(HouseWinLossExcelQuery param)
            => param.IsHistoricReport ?
            $"Wager Report - {param.FromDate.ToShortDate()}-{param.ToDate.ToShortDate()}" :
            $"Wager Report - {param.Date.ToShortDate()}";
    }
}