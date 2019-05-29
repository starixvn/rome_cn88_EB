using EB.Features.Shared.Model;
using EB.Features.Site.Reports.ViewModel;
using Fanex.NAP.Core.Web.Attribute;
using MediatR;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EB.Features.Site.Shared
{
    [NapSuperAuth]
    public class BaseController : Controller
    {
        protected const string NoInformationPageViewPath = "~/Shared/Views/NoInformation.cshtml";
        private const string InvalidExcelFileName = "Invalid";

        protected readonly IMediator mediator;

        public BaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> ViewAsync<TResponse>(IRequest<TResponse> message, string viewPath)
             => (message as BaseQuery).IsValid() ?
                View(viewPath, await mediator.Send(message).ConfigureAwait(false)) : View(NoInformationPageViewPath);

        protected ActionResult JsonData<TResponse>(
           IRequest<TResponse> message,
           JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            if (!(message as BaseQuery).IsValid())
            {
                return Json(new Result((int)HttpStatusCode.Forbidden, Resource.You_have_no_authorization));
            }

            return Json(mediator.Send(message), jsonRequestBehavior);
        }

        protected async Task<ActionResult> JsonDataAsync<TResponse>(
            IRequest<TResponse> message,
            JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            if (!(message as BaseQuery).IsValid())
            {
                return Json(new Result((int)HttpStatusCode.Forbidden, Resource.You_have_no_authorization));
            }

            return Json(await mediator.Send(message).ConfigureAwait(false), jsonRequestBehavior);
        }

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var baseQuery = message as BaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
            }

            return GetExcelContent(await mediator.Send(message));
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }
}