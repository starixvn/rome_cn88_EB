namespace EB.Features.Site.Reports
{
    using System.Linq;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Resources;
    using static ONEbook.SpreadSheets.Column;

    public class HouseWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<HouseWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly IHouseWinLossService houseWinLossService;
        private readonly HouseWinLossWorkbook workbook;

        public HouseWinLossExcelHandler(
            IHouseWinLossService matchWinLossService,
            HouseWinLossWorkbook workbook)
        {
            this.houseWinLossService = matchWinLossService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(HouseWinLossExcelQuery message)
        {
            var reportData = houseWinLossService.Get(new HouseWinLossParam
            {
                TicketStatusId = message.UserSelectedTicketStatusId,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                FromDate = message.Date,
                ToDate = message.Date
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

            switch (rowType)
            {
                case RowTypeConstant.NormalRow:
                    mapper.Map(A, betTypeWinLoss => betTypeWinLoss.UserName);
                    break;
                default:
                    mapper.Map(A, betTypeWinLoss => string.Empty);
                    break;
            }

            mapper
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake / 100)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake / 100)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome / 100)
                .Map(F, betTypeWinLoss => betTypeWinLoss.SuperComm / 100)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HouseWinLoss / 100)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HouseComm / 100)
                .Map(I, betTypeWinLoss => betTypeWinLoss.HouseIncome / 100)
                .Map(J, betTypeWinLoss => betTypeWinLoss.HeadlineStock / 100)
                .Map(K, betTypeWinLoss => (totalStock == 0 ? 0 : betTypeWinLoss.HeadlineStock / totalStock * 100));

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

        private string GetReportTitle(HouseWinLossExcelQuery query)
            => $"{ReportResources.HouseWinLossReport} {ReportResources.Of} {query.FromDate.ToShortDate()}";

        private string GetExcelFileName(HouseWinLossExcelQuery query)
            => $"Wager Report - {query.FromDate.ToShortDate()}";        
    }
}