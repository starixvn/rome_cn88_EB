namespace EB.Features.Site.Reports.Request.HouseWinLoss
{
    using System;
    using System.Collections.Generic;
    using EB.Features.Reports.Extension;
    using EB.Features.Shared.Model;
    using EB.Features.Site.Shared.Models;
    using Features.Shared.Constants;
    using Features.Shared.Utilities;
    using MediatR;
    using ViewModel.HouseWinLoss;

    public class HouseWinLossQuery : ReportBaseQuery, IRequest<HouseWinLossReportViewModel>
    {
        private int headlineId;

        public HouseWinLossQuery(UserSession currentSession)
            : base(currentSession)
        {
            UserSelectedLiveStatusId = LiveStatusConstants.AllMarkets;
            Date = SystemTime.Today;
            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today;
            IsTest = true;
            BreadcrumbLevel = new List<CustInfo>();
        }

        public int HeadlineId { get; set; }

        public IEnumerable<int> SelectedSportGroupIds
            => UserSelectedSportGroupIds.ToListInt();

        public string UserSelectedSportGroupIds { get; set; }

        public int HeadlineId
        {
            get
            {
                return Role.IsSuperLevel() ? CustId : headlineId;
            }
            set
            {
                headlineId = value;
            }
        }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public UserLevel Role => Enumeration.FromValue<UserLevel>(RoleId);

        public int UserSelectedViewer { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public int UserSelectedTicketStatusId { get; set; }

        public DateTime Date { get; set; }

        public bool IsTest { get; set; }

        public bool IsHistoricReport { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; }

        public string CustName { get; set; }
    }
}