namespace EB.Features.Site.Reports
{
    using System.Linq;
    using EB.Features.Reports;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class PerformanceBonusExcelHandler : ExcelBaseHandler, IRequestHandler<PerformanceBonusExcelQuery, ExcelFileViewModel>
    {
        private readonly IPerformanceBonusService matchWinLossService;
        private readonly IPerformanceBonusWorkbook workbook;

        public PerformanceBonusExcelHandler(
            IPerformanceBonusService matchWinLossService,
            IPerformanceBonusWorkbook workbook)
        {
            this.matchWinLossService = matchWinLossService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(PerformanceBonusExcelQuery message)
        {
            var reportData = matchWinLossService.Get(new PerformanceBonusParam
            {
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                ConjunctionId = message.UserSelectedConjunctionId,
                CurrencyId = message.UserSelectedCurrencyId,
                HeadlineStock = message.HeadlineStock,
                UserNames = message.UserNames,
                BonusRate = message.BonusRate
            });
            var fileContent = workbook
                .SetData(reportData)
                .SetRowMappers(
                    GetRowMapper(RowTypeConstant.NormalRow),
                    GetRowMapper(RowTypeConstant.GrandTotal))
                .UseTemplate(GetTemplateInfo())
                .Sheet("ReportData")
                .Title(GetReportTitle(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Footer(reportData.Sum(), GetRowMapper(RowTypeConstant.GrandTotal))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        private static Mapper<PerformanceBonusModel> GetRowMapper(int rowType)
        {
            var mapper = new Mapper<PerformanceBonusModel>();

            if (rowType == RowTypeConstant.NormalRow)
            {
                mapper.Map(A, i => i.UserName);
                mapper.Map(B, i => i.HeadlineUserName);
            }
            else
            {
                mapper.Map(A, i => ReportResources.Total);
                mapper.Map(B, i => string.Empty);
            }

            mapper
                .Map(C, i => i.HeadlineStock)
                .Map(D, i => i.PerformanceBonus)
                .Map(E, i => i.PerformanceBonus1035)
                .Map(F, i => i.RoundDown);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo()
          => new TemplateInfo(
              GetExcelTemplatePath("PerformanceBonusReport"),
              noInfomationText: AppResources.NoInformationIsAvailable,
              titleStartIndex: 1,
              headerStartIndex: 2,
              bodyStartIndex: 4,
              bodyEndIndex: 5,
              freezeRowIndex: 4,
              columnStartName: A,
              sheetIndex: 1,
              footerEndIndex: 6);

        internal static string GetReportTitle(PerformanceBonusExcelQuery query)
        {
            var currency = $"{ AppResources.Currency }: { CurrencyConstant.List[query.UserSelectedCurrencyId]}";
            var bonusRate = $"{ReportResources.BonusRate}: {query.BonusRate}%";
            var date = $"{query.FromDate.ToShortDate()} --> {query.ToDate.ToShortDate()}";

            return $"{ReportResources.PerformanceBonusTitle} - {date} - {bonusRate} - {currency}";
        }


        internal static string GetExcelFileName(PerformanceBonusExcelQuery query)
            => $"Performance Bonus Calculator - {query.FromDate.ToShortDate()}-{query.ToDate.ToShortDate()}";
    }
}