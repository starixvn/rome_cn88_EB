namespace EB.Features.Site.Reports.Request.HouseWinLoss
{
    using System;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Utilities;
    using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class CountHouseWinLossQuery : ReportBaseQuery, IRequest<CountHouseWinLossViewModel>
    {
        public CountHouseWinLossQuery(UserSession currentSession)
            : base(currentSession)
        {
            UserSelectedLiveStatusId = LiveStatusConstants.AllMarkets;
            Date = SystemTime.Today;
            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today;
            IsTest = true;
        }

        public int HeadlineId { get; set; }

        public string UserSelectedSportGroupIds { get; set; }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public UserLevel Role => Enumeration.FromValue<UserLevel>(RoleId);

        public int UserSelectedViewer { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public int UserSelectedTicketStatusId { get; set; } = 1;

        public DateTime Date { get; set; }

        public bool IsTest { get; set; }

        public bool IsHistoricReport { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}