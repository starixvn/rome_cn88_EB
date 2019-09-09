namespace EB.Features.Site.Reports.Request
{
    using System;
    using System.Collections.Generic;
    using EB.Features.Reports.Extension;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Utilities;
    using EB.Features.Site.Reports.ViewModel;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class MatchWinLossQuery : ReportBaseQuery, IRequest<MatchWinLossReportViewModel>
    {
        public MatchWinLossQuery(UserSession currentSession)
            : base(currentSession)
        {
            Date = SystemTime.Today;
            UserSelectedLiveStatusId = LiveStatusConstants.AllMarkets;
            IsTest = true;
        }

        public IEnumerable<int> SelectedSportGroupIds
            => UserSelectedSportGroupIds.ToListInt();

        public string UserSelectedSportGroupIds { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public DateTime Date { get; set; }

        public bool IsTest { get; set; }
    }
}