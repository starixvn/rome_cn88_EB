namespace EB.Features.Site.Reports
{
    using EB.Features.Reports;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class MatchWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<MatchWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly IMatchWinLossService matchWinLossService;
        private readonly IMatchWinLossWorkbook workbook;

        public MatchWinLossExcelHandler(
            IMatchWinLossService matchWinLossService,
            IMatchWinLossWorkbook workbook)
        {
            this.matchWinLossService = matchWinLossService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(MatchWinLossExcelQuery message)
        {
            var reportData = matchWinLossService.Get(new MatchWinLossParam
            {
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                WinLossDate = message.Date
            });
            var fileContent = workbook
                .SetData(reportData)
                .SetRowMappers(
                    GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.NormalRow),
                    GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.SubTotal),
                    GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.GrandTotal))
                .UseTemplate(GetTemplateInfo())
                .Sheet("ReportData")
                .Title(GetReportTitle(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Footer(reportData.Total, GetRowMapper(reportData.Total.HeadlineStock, RowTypeConstant.GrandTotal))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        internal static Mapper<MatchWinLossModel> GetRowMapper(decimal totalStock, int rowType)
        {
            var mapper = new Mapper<MatchWinLossModel>();

            switch (rowType)
            {
                case RowTypeConstant.NormalRow:
                    mapper.Map(A, betTypeWinLoss => betTypeWinLoss.BetTypeGroupName);
                    break;
                case RowTypeConstant.SubTotal:
                    mapper.Map(A, betTypeWinLoss => ReportResources.SubTotal);
                    break;
                default:
                    mapper.Map(A, betTypeWinLoss => ReportResources.GrandTotal);
                    break;
            }

            mapper
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake / CommonConstants.ReduceNumber)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake / CommonConstants.ReduceNumber)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome / CommonConstants.ReduceNumber)
                .Map(F, betTypeWinLoss => betTypeWinLoss.HouseIncome / CommonConstants.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HeadlineStock / CommonConstants.ReduceNumber)
                .Map(H, betTypeWinLoss => (totalStock == 0 ? 0 : betTypeWinLoss.HeadlineStock / totalStock * CommonConstants.ReduceNumber));

            return mapper;
        }

        private TemplateInfo GetTemplateInfo()
          => new TemplateInfo(
              GetExcelTemplatePath("MatchWinLossReport"),
              noInfomationText: AppResources.NoInformationIsAvailable,
              titleStartIndex: 1,
              headerStartIndex: 2,
              bodyStartIndex: 3,
              bodyEndIndex: 8,
              freezeRowIndex: 3,
              columnStartName: A,
              sheetIndex: 1,
              footerEndIndex: 9);

        internal static string GetReportTitle(MatchWinLossExcelQuery query)
            => $"{ReportResources.MatchWinLossReport} {ReportResources.Of} {query.Date.ToShortDate()}";

        internal static string GetExcelFileName(MatchWinLossExcelQuery query)
            => $"Match Win Loss - {query.Date.ToShortDate()}";
    }    
}