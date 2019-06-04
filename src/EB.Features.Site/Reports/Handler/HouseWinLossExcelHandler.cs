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
using EB.Features.Reports.Extension;

namespace EB.Features.Site.Reports
{
    public class HouseWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<HouseWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IHouseWinLossService houseWinLossService;
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
            this.houseWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(HouseWinLossExcelQuery message)
        {
            var reportData = houseWinLossService.Get(new HouseWinLossParam
            {
                IsSettled = message.UserSelectedTicketStatusId == TicketStatusConstants.Settled,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                FromDate = message.Date,
                ToDate = message.Date
            });
            var totalRow = reportData.Sum();

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

        internal static Mapper<HouseWinLossModel> GetGrandRowMapper(decimal totalStock)
        {
            var mapper = new Mapper<HouseWinLossModel>();

            mapper
               .Map(A, betTypeWinLoss => string.Empty)
               .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
               .Map(C, betTypeWinLoss => betTypeWinLoss.Stake)
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
            => $"Wager Report - {query.FromDate.ToShortDate()}";        
    }
}