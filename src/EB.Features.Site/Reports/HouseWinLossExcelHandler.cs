using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Shared.Utilies;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
using EB.Features.Site.Shared.ViewModels;
using MediatR;
using ONEbook.SpreadSheets;
using Resources;
using static ONEbook.SpreadSheets.Column;

namespace EB.Features.Site.Reports
{
    public class HouseWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<HouseWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IHouseWinLossService matchWinLossService;
        private readonly ReportDateService reportDateService;
        private readonly HouseWinLossWorkbook workbook;

        public HouseWinLossExcelHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            IHouseWinLossService matchWinLossService,
            ReportDateService reportDateService,
            HouseWinLossWorkbook workbook)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.matchWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(HouseWinLossExcelQuery message)
        {
            var reportData = matchWinLossService.Get(new HouseWinLossParam
            {
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                FromDate = message.FromDate,
                ToDate = SystemTime.Today
            });
            var totalRow = Sum(reportData);

            var fileContent = workbook
                .SetData(reportData, totalRow)
                .SetRowMappers(
                    GetBodyRowMapper(totalRow.HeadlineStock),
                    GetGrandRowMapper(totalRow.HeadlineStock))
                .UseTemplate(GetTemplateInfo(message))
                .Sheet("ReportData")
                .Title(GetReportTitle(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Footer(totalRow, GetBodyRowMapper(totalRow.HeadlineStock))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        internal static Mapper<HouseWinLossModel> GetBodyRowMapper(decimal totalStock)
        {
            var mapper = new Mapper<HouseWinLossModel>();

            mapper
                .Map(A, betTypeWinLoss => betTypeWinLoss.UserName)
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome)
                .Map(F, betTypeWinLoss => betTypeWinLoss.SuperComm)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HouseWinLoss)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HouseComm)
                .Map(I, betTypeWinLoss => betTypeWinLoss.HouseIncome)                
                .Map(J, betTypeWinLoss => betTypeWinLoss.HeadlineStock)
                .Map(K, betTypeWinLoss => betTypeWinLoss.HeadlineStock / totalStock * 100);

            return mapper;
        }

        internal static Mapper<HouseWinLossModel> GetGrandRowMapper(decimal totalStock)
        {
            var mapper = new Mapper<HouseWinLossModel>();

            mapper
               .Map(A, betTypeWinLoss => string.Empty)
               .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
               .Map(C, betTypeWinLoss => betTypeWinLoss.Stake)
               .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake)
               .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome)
               .Map(F, betTypeWinLoss => betTypeWinLoss.SuperComm)
               .Map(G, betTypeWinLoss => betTypeWinLoss.HouseWinLoss)
               .Map(H, betTypeWinLoss => betTypeWinLoss.HouseComm)
               .Map(I, betTypeWinLoss => betTypeWinLoss.HouseIncome)
               .Map(J, betTypeWinLoss => betTypeWinLoss.HeadlineStock)
               .Map(K, betTypeWinLoss => betTypeWinLoss.HeadlineStock / totalStock * 100);

            return mapper;
        }

        
        private TemplateInfo GetTemplateInfo(HouseWinLossExcelQuery message)
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
            => $"House Win Loss - {query.FromDate.ToShortDate()}";

        public HouseWinLossModel Sum(IEnumerable<HouseWinLossModel> matches)
        {
            var total = new HouseWinLossModel();
            foreach (var match in matches)
            {
                total = total.Add(match);
            }

            return total;
        }
    }
}