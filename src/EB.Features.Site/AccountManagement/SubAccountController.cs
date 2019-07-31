namespace EB.Features.Site.AccountManagement
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AccountManagement.Requests.SubAccount;
    using EB.Features.Site.Shared.Filters;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Features.Shared.Constants;
    using MediatR;
    using Shared;

    public class SubAccountController : BaseController
    {
        public SubAccountController(IMediator mediator, UserSession userSession)
            : base(mediator, userSession)
        {
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.SubAccount, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => View("~/AccountManagement/Views/SubAccount/Index.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.SubAccount, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> Search(SearchQuery message)
            => await JsonDataAsync(message);

        [HttpPost, AntiXSRFFilter]
        [NapAuth(FunctionKey = SiteFunctionKeys.SubAccount, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> Update(UpdateCommand message)
        {
            message.Language = message.Language ?? UserSessionImpl.Current.Language;

            return await JsonDataAsync(message);
        }
    }
}