namespace EB.Features.Site.Login
{
    using System.Web.Mvc;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared;
    using MediatR;

    public class InternalLoginController : BaseController
    {
        public InternalLoginController(IMediator mediator)
            : base(mediator)
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
