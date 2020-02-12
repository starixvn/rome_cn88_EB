namespace EB.Features.Site.Reports
{
    using System.Linq;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Resources;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class PerformanceBonusExcelHandler : ExcelBaseHandler, IRequestHandler<PerformanceBonusExcelQuery, ExcelFileViewModel>
    {
        private readonly IPerformanceBonusService service;
        private readonly IPerformanceBonusWorkbook workbook;
        private readonly ISportGroupService sportGroupService;

        public PerformanceBonusExcelHandler(
            IPerformanceBonusService matchWinLossService,
            IPerformanceBonusWorkbook workbook,
            ISportGroupService sportGroupService)
        {
            this.service = matchWinLossService;
            this.workbook = workbook;
            this.sportGroupService = sportGroupService;
        }

        public ExcelFileViewModel Handle(PerformanceBonusExcelQuery message)
        {
            var reportData = service.Get(new PerformanceBonusParam
            {
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                ConjunctionId = message.UserSelectedConjunctionId,
                CurrencyId = message.UserSelectedCurrencyId,
                HeadlineStock = message.HeadlineStock,
                UserNames = message.UserNames,
                DividendRate = message.DividendRate,
                SporttypeGroups = message.UserSelectedSportGroupIds
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
                .Map(D, i => i.Dividend)
                .Map(E, i => i.Bonus)
                .Map(F, i => i.BonusRoundDown);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo()
          => new TemplateInfo(
              GetExcelTemplatePath("PerformanceBonusReport"),
              noInfomationText: AppResources.NoInformationIsAvailable,
              titleStartIndex: 1,
              headerStartIndex: 2,
              bodyStartIndex: 3,
              bodyEndIndex: 4,
              freezeRowIndex: 3,
              columnStartName: A,
              sheetIndex: 1,
              footerEndIndex: 5);

        internal string GetReportTitle(PerformanceBonusExcelQuery query)
        {
            var currency = $"{ AppResources.Currency }: { CurrencyConstant.List[query.UserSelectedCurrencyId]}";
            var bonusRate = $"{ReportResources.DividendRate}: {query.DividendRate}%";
            var date = $"{query.FromDate.ToShortDate()} --> {query.ToDate.ToShortDate()}";
            var sportGroup = GetValidSportGroupNames(sportGroupService.GetSportGroups(), query.SelectedSportGroupIds);

            return $"{ReportResources.PerformanceBonusTitle} - {date} - {bonusRate} - {currency} - {AppResources.Sport}: {sportGroup}";
        }


        internal static string GetExcelFileName(PerformanceBonusExcelQuery query)
            => $"Performance Bonus Calculator - {query.FromDate.ToShortDate()}-{query.ToDate.ToShortDate()}";
    }
}