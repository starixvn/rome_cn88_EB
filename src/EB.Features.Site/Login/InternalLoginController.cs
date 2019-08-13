namespace EB.Features.Site.Login
{
    using System.Web.Mvc;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class InternalLoginController : BaseController
    {
        public InternalLoginController(IMediator mediator, UserSession userSession)
            : base(mediator, userSession)
        {
        }

        [HttpGet]
        public ActionResult Index(InternalLoginQuery message)
        {
            var viewModel = mediator.Send(message).ConfigureAwait(false).GetAwaiter().GetResult();

            return Redirect(viewModel);
        }
    }
}
