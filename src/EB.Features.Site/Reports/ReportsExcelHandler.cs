using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
using EB.Features.Site.Shared.ViewModels;
using MediatR;

namespace EB.Features.Site.Reports
{
    public class ReportsExcelHandler : IRequestHandler<MatchWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IMatchWinLossService matchWinLossService;
        private readonly ReportDateService reportDateService;
        private readonly ReportsWorkbook workbook;

        public ReportsExcelHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            IMatchWinLossService matchWinLossService,
            ReportDateService reportDateService,
            ReportsWorkbook workbook)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.matchWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(MatchWinLossExcelQuery message)
        {
            return null;
        }
    }
}