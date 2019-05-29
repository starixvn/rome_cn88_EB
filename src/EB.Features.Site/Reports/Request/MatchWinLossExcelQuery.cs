using EB.Features.Shared.Constants;
using EB.Features.Shared.Utilies;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Shared;
using Fanex.NAP.Core.Web.Session;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EB.Features.Site.Reports.Request
{
    public class MatchWinLossExcelQuery : MatchWinLossQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossExcelQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}