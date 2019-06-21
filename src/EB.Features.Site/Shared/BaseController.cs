using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using EB.Features.Main.Resources;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Model;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Shared.Models;
using Fanex.NAP.Core.Web.Attribute;
using MediatR;

namespace EB.Features.Site.Shared
{
    [NapSuperAuth]
    public class BaseController : Controller
    {
        protected const string NoInformationPageViewPath = "~/Shared/Views/NoInformation.cshtml";
        private const string InvalidExcelFileName = "Invalid";
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(LanguageConstants.Default);

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
            if ((message as BaseQuery).IsValid())
            {
                return Json(mediator.Send(message), jsonRequestBehavior);
            }

            return Json(new Result((int)HttpStatusCode.Forbidden, SiteResources.You_have_no_authorization));
        }

        protected async Task<ActionResult> JsonDataAsync<TResponse>(
            IRequest<TResponse> message,
            JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            if ((message as BaseQuery).IsValid())
            {
                return Json(await mediator.Send(message).ConfigureAwait(false), jsonRequestBehavior);
            }

            return Json(new Result((int)HttpStatusCode.Forbidden, SiteResources.You_have_no_authorization));
        }

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
        {
            if (!(message is BaseQuery baseQuery) || baseQuery.IsValid())
            {
                return GetExcelContent(await mediator.Send(message));
            }

            return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var cultureName = UserSessionImpl.Current.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = LanguageConstants.Default;
                UserSessionImpl.Current.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
        => File(viewModel.Content, ExcelFileViewModel.ContentType, viewModel.Name);
    }
}
