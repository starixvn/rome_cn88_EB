namespace EB.Features.Site.Reports.Request
{
    using System;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Utilies;
    using EB.Features.Site.Reports.ViewModel;
    using EB.Features.Site.Shared;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class MatchWinLossQuery : BaseQuery, IRequest<MatchWinLossReportViewModel>
    {
        public MatchWinLossQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
            Date = SystemTime.Today;
            UserSelectedLiveStatusId = LiveStatusConstants.AllMarkets;
            IsTest = true;
        }

        public int UserSelectedSportGroupId { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public DateTime Date { get; set; }

        public bool IsTest { get; set; }
    }
}