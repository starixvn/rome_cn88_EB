namespace EB.Features.Site.Reports
{
    using System;
    using System.Linq;
    using EB.Features.Reports;
    using EB.Features.Reports.Constants;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared;
    using EB.Features.Shared.Configuration;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Model;
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
        private readonly ISportGroupService sportGroupService;

        public HouseWinLossExcelHandler(
            IHouseWinLossService matchWinLossService,
            IHouseWinLossWorkbook workbook,
            ICacheRefService cacheRefService,
            ISportGroupService sportGroupService)
        {
            this.houseWinLossService = matchWinLossService;
            this.workbook = workbook;
            this.cacheRefService = cacheRefService;
            this.sportGroupService = sportGroupService;
        }

        public ExcelFileViewModel Handle(HouseWinLossExcelQuery message)
        {
            var param = new HouseWinLossParam
            {
                CustId = message.CustId,
                Role = message.Role,
                Viewer = message.UserSelectedViewer,
                IsHistoricReport = message.IsHistoricReport,
                TicketStatusId = message.UserSelectedTicketStatusId,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroups = message.UserSelectedSportGroupIds,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                Date = message.Date
            };
            var reportData = houseWinLossService.Get(param);
            var countUsers = houseWinLossService.CountUsers(param);

            var fileContent = workbook
                .SetData(reportData.Matches, reportData.Total, message.Role.IsSuperLevel())
                .SetRowMappers(
                    GetRowMapper(reportData.Total.HeadlineStock, message.Role, RowTypeConstant.NormalRow),
                    GetRowMapper(reportData.Total.HeadlineStock, message.Role, RowTypeConstant.GrandTotal),
                    GetFooterRowMapper())
                .UseTemplate(GetTemplateInfo(message.Role))
                .Sheet("ReportData")
                .Title(GetReportTitle(message, countUsers))
                .Header(ReportResources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        internal static Mapper<HouseWinLossModel> GetRowMapper(decimal totalStock, UserLevel role, int rowType)
            => role.IsHeadline() ? GetRowMapperHeadline(totalStock, rowType)
            : role.IsSuperLevel() ? GetRowMapperSuper(totalStock, rowType)
            : role.IsMasterLevel() ? GetRowMapperMaster(totalStock, rowType)
            : role.IsAgentLevel() ? GetRowMapperAgent(rowType)
            : GetRowMapperMember(rowType);


        internal static Mapper<HouseWinLossModel> GetRowMapperHeadline(decimal totalStock, int rowType)
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
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberWinLoss / AppSettings.ReduceNumber)
                .Map(F, betTypeWinLoss => betTypeWinLoss.MemberComm / AppSettings.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.MemberIncome / AppSettings.ReduceNumber)
                .Map(H, betTypeWinLoss => betTypeWinLoss.SuperComm / AppSettings.ReduceNumber)
                .Map(I, betTypeWinLoss => betTypeWinLoss.HouseWinLoss / AppSettings.ReduceNumber)
                .Map(J, betTypeWinLoss => betTypeWinLoss.HouseComm / AppSettings.ReduceNumber)
                .Map(K, betTypeWinLoss => betTypeWinLoss.HouseIncome / AppSettings.ReduceNumber)
                .Map(L, betTypeWinLoss => betTypeWinLoss.HouseTurnover / AppSettings.ReduceNumber)
                .Map(M, betTypeWinLoss => betTypeWinLoss.HeadlineResult / AppSettings.ReduceNumber)
                .Map(N, betTypeWinLoss => betTypeWinLoss.RebateRoundDown / AppSettings.ReduceNumber)
                .Map(O, betTypeWinLoss => betTypeWinLoss.HeadlineResultNet / AppSettings.ReduceNumber)
                .Map(P, betTypeWinLoss => betTypeWinLoss.HeadlineStock / AppSettings.ReduceNumber)
                .Map(Q, betTypeWinLoss => (totalStock == 0 ? 0 : betTypeWinLoss.HeadlineStock / totalStock * 100));

            return mapper;
        }

        internal static Mapper<HouseWinLossModel> GetRowMapperSuper(decimal totalStock, int rowType)
        {
            var mapper = new Mapper<HouseWinLossModel>();

            if (rowType == RowTypeConstant.NormalRow)
            {
                mapper
                    .Map(A, betTypeWinLoss => betTypeWinLoss.HeadlineUserName)
                    .Map(B, betTypeWinLoss => betTypeWinLoss.UserName);
            }
            else
            {
                mapper.Map(A, betTypeWinLoss => string.Empty);
                mapper.Map(B, betTypeWinLoss => string.Empty);
            }

            mapper
                .Map(C, betTypeWinLoss => betTypeWinLoss.Wagers)
                .Map(D, betTypeWinLoss => betTypeWinLoss.Stake / AppSettings.ReduceNumber)
                .Map(E, betTypeWinLoss => betTypeWinLoss.ValidStake / AppSettings.ReduceNumber)
                .Map(F, betTypeWinLoss => betTypeWinLoss.MemberIncome / AppSettings.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.Agent / AppSettings.ReduceNumber)
                .Map(H, betTypeWinLoss => betTypeWinLoss.AgentResult / AppSettings.ReduceNumber)
                .Map(I, betTypeWinLoss => betTypeWinLoss.MasterResult / AppSettings.ReduceNumber)
                .Map(J, betTypeWinLoss => betTypeWinLoss.Super / AppSettings.ReduceNumber)
                .Map(K, betTypeWinLoss => betTypeWinLoss.SuperResult / AppSettings.ReduceNumber)
                .Map(L, betTypeWinLoss => betTypeWinLoss.HeadlineIncome / AppSettings.ReduceNumber)
                .Map(M, betTypeWinLoss => betTypeWinLoss.HeadlineResult / AppSettings.ReduceNumber)
                .Map(N, betTypeWinLoss => betTypeWinLoss.HeadlineStock / AppSettings.ReduceNumber)
                .Map(O, betTypeWinLoss => (totalStock == 0 ? 0 : betTypeWinLoss.HeadlineStock / totalStock * 100));

            return mapper;
        }

        internal static Mapper<HouseWinLossModel> GetRowMapperMaster(decimal totalStock, int rowType)
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
                .Map(F, betTypeWinLoss => betTypeWinLoss.Agent / AppSettings.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.AgentResult / AppSettings.ReduceNumber)
                .Map(H, betTypeWinLoss => betTypeWinLoss.MasterResult / AppSettings.ReduceNumber)
                .Map(I, betTypeWinLoss => betTypeWinLoss.Super / AppSettings.ReduceNumber)
                .Map(J, betTypeWinLoss => betTypeWinLoss.SuperResult / AppSettings.ReduceNumber)
                .Map(L, betTypeWinLoss => betTypeWinLoss.SuperStock / AppSettings.ReduceNumber);

            return mapper;
        }

        internal static Mapper<HouseWinLossModel> GetRowMapperAgent(int rowType)
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
                .Map(F, betTypeWinLoss => betTypeWinLoss.Agent / AppSettings.ReduceNumber)
                .Map(G, betTypeWinLoss => betTypeWinLoss.AgentResult / AppSettings.ReduceNumber)
                .Map(H, betTypeWinLoss => betTypeWinLoss.MasterResult / AppSettings.ReduceNumber)
                .Map(I, betTypeWinLoss => betTypeWinLoss.AgentStock / AppSettings.ReduceNumber);

            return mapper;
        }

        internal static Mapper<HouseWinLossModel> GetRowMapperMember(int rowType)
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
                .Map(F, betTypeWinLoss => betTypeWinLoss.AgentResult / AppSettings.ReduceNumber);

            return mapper;
        }

        internal static Mapper<HouseWinLossModel> GetFooterRowMapper()
        {
            var mapper = new Mapper<HouseWinLossModel>();
            mapper.Map(N, data => ReportResources.ProfitMargin);
            mapper.Map(O, data => data.SuperStock != 0 ? data.SuperResult / data.SuperStock : 0);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(UserLevel role)
          => new TemplateInfo(
              GetExcelTemplatePath($"HouseWinLossReport/{role.DisplayName}"),
              noInfomationText: AppResources.NoInformationIsAvailable,
              titleStartIndex: 1,
              headerStartIndex: 2,
              bodyStartIndex: 3,
              bodyEndIndex: 4,
              freezeRowIndex: 3,
              columnStartName: A,
              sheetIndex: 1,
              footerEndIndex: 5);

        internal string GetReportTitle(HouseWinLossExcelQuery param, CountHouseWinLossModel countUsers)
        {
            var title = $"{ReportResources.HouseWinLossReport} {ReportResources.Of} {param.Date.ToShortDate()}";
            if (param.IsHistoricReport)
            {
                title = $"{ReportResources.HouseWinLossReport} {param.FromDate.ToShortDate()} --> {param.ToDate.ToShortDate()}";
            }

            var liveStatus = LiveStatusConstants.LiveStatuses[param.UserSelectedLiveStatusId];
            var ticketStatus = TicketStatusConstants.TicketStatuses[param.UserSelectedTicketStatusId];
            var betType = param.UserSelectedBetTypeId == 0 ? AppResources.AllBetTypes : cacheRefService.GetBetTypeName(param.UserSelectedBetTypeId);
            var sportGroup = GetValidSportGroupNames(sportGroupService.GetSportGroups(), param.SelectedSportGroupIds);

            title = $"{title} - {GetResourcesText(ticketStatus)} - {param.Role.DisplayName} - {sportGroup} - {betType} - {GetResourcesText(liveStatus)} {Environment.NewLine}";
            title += $"{ ReportResources.Headline}: { countUsers.HeadlineCount} | { ReportResources.SMA}: { countUsers.SuperCount}";
            title += $"| { ReportResources.MA}: { countUsers.MasterCount} | { ReportResources.Agent}: { countUsers.AgentCount}";
            title += $"| { AppResources.Member}: { countUsers.MemberCount}";

            return title;
        }

        private static string GetResourcesText(string key)
            => AppResources.ResourceManager.GetString(key);

        internal string GetExcelFileName(HouseWinLossExcelQuery param)
            => param.IsHistoricReport ?
                $"Wager Report - {param.FromDate.ToShortDate()}-{param.ToDate.ToShortDate()} - {param.Role.DisplayName}" :
                $"Wager Report - {param.Date.ToShortDate()} - {param.Role.DisplayName}";
    }
}