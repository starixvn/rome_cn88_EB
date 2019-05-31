using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
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
    public class MatchWinLossExcelHandler : ExcelBaseHandler, IRequestHandler<MatchWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IMatchWinLossService matchWinLossService;
        private readonly ReportDateService reportDateService;
        private readonly MatchWinLossWorkbook workbook;

        public MatchWinLossExcelHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            IMatchWinLossService matchWinLossService,
            ReportDateService reportDateService,
            MatchWinLossWorkbook workbook)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.matchWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
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
            var totalRow = Sum(reportData);
            var noParlayMatchWinLossDetail = reportData.Where(i => i.BetTypeGroup != BetTypeIdConstants.SystemParlay);
            var parlay = reportData.FirstOrDefault(i => i.BetTypeGroup == BetTypeIdConstants.SystemParlay);

            var fileContent = workbook
                .SetData(noParlayMatchWinLossDetail, parlay, totalRow)
                .SetRowMappers(
                    GetBodyRowMapper(totalRow.HeadlineStock),
                    GetSubRowMapper(totalRow.HeadlineStock),
                    GetGrandRowMapper(totalRow.HeadlineStock))
                .UseTemplate(GetTemplateInfo(message))
                .Sheet("ReportData")
                .Title(GetReportTitle(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Footer(totalRow, GetBodyRowMapper(totalRow.HeadlineStock))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        internal static Mapper<MatchWinLossViewModel> GetBodyRowMapper(decimal totalStock)
        {
            var mapper = new Mapper<MatchWinLossViewModel>();

            mapper
                .Map(A, betTypeWinLoss => betTypeWinLoss.BetTypeGroupName)
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome)
                .Map(F, betTypeWinLoss => betTypeWinLoss.HouseIncome)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HeadlineStock)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HeadlineStock / totalStock * 100);

            return mapper;
        }

        internal static Mapper<MatchWinLossViewModel> GetSubRowMapper(decimal totalStock)
        {
            var mapper = new Mapper<MatchWinLossViewModel>();

            mapper
                .Map(A, betTypeWinLoss => ReportResources.SubTotal)
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome)
                .Map(F, betTypeWinLoss => betTypeWinLoss.HouseIncome)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HeadlineStock)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HeadlineStock / totalStock * 100);

            return mapper;
        }

        internal static Mapper<MatchWinLossViewModel> GetGrandRowMapper(decimal totalStock)
        {
            var mapper = new Mapper<MatchWinLossViewModel>();

            mapper
                .Map(A, betTypeWinLoss => ReportResources.GrandTotal)
                .Map(B, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(C, betTypeWinLoss => betTypeWinLoss.Stake)
                .Map(D, betTypeWinLoss => betTypeWinLoss.ValidStake)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberIncome)
                .Map(F, betTypeWinLoss => betTypeWinLoss.HouseIncome)
                .Map(G, betTypeWinLoss => betTypeWinLoss.HeadlineStock)
                .Map(H, betTypeWinLoss => betTypeWinLoss.HeadlineStock / totalStock * 100);

            return mapper;
        }
        private TemplateInfo GetTemplateInfo(MatchWinLossExcelQuery message)
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

        private string GetReportTitle(MatchWinLossExcelQuery query)
            => $"{ReportResources.MatchWinLossReport} {ReportResources.Of} {query.Date.ToShortDate()}";

        private string GetExcelFileName(MatchWinLossExcelQuery query)
            => $"Match Win Loss - {query.Date.ToShortDate()}";

        public MatchWinLossViewModel Sum(IEnumerable<MatchWinLossViewModel> matchs)
        {
            var total = new MatchWinLossViewModel();
            foreach (var match in matchs)
            {
                total = total.Add(match);
            }

            return total;
        }
    }
}