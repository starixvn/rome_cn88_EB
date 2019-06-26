namespace EB.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EB.Features.Site.Main.Requests;
    using EB.Features.Site.Main.ViewModel;
    using EB.Features.Site.Shared;
    using MediatR;

    public class UserInfoController : BaseController
    {
        private const string ViewsPath = "~/Main/Views";

        public UserInfoController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult ChangePassword()
            => View($"{ViewsPath}/ChangePassword.cshtml", new ChangePasswordViewModel());

        [HttpPost]
        public async Task<ActionResult> SavePassword(ChangePasswordQuery query)
            => await ViewAsync(query, $"{ViewsPath}/ChangePassword.cshtml");
    }
}
