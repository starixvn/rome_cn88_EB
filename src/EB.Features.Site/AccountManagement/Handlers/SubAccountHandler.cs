namespace EB.Features.Site.AccountManagement.Handlers
{
    using System.Threading.Tasks;
    using AccountManagement.Requests.SubAccount;
    using EB.Features.Shared.Constants;
    using Features.AccountManagement.Models.SubAccount;
    using Features.AccountManagement.Params.SubAccount;
    using Features.AccountManagement.Services;
    using Features.Shared.Model;
    using MediatR;

    public class SubAccountHandler :
        IAsyncRequestHandler<SearchQuery, SubAccountItem>,
        IAsyncRequestHandler<UpdateCommand, Result>
    {
        private readonly SubAccountService subAccountService;

        public SubAccountHandler(SubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<SubAccountItem> Handle(SearchQuery message)
            => await subAccountService.GetSuperSubAccountAsync(message.Name);

        public async Task<Result> Handle(UpdateCommand message)
            => await subAccountService.UpdateSubAccountAsync(ToUpdateSubAccountParam(message));

        private static UpdateSubAccountParam ToUpdateSubAccountParam(UpdateCommand message)
            => new UpdateSubAccountParam(
                message.CustId,
                message.SubAccId,
                message.Username,
                message.PageAccess,
                LanguageConstants.GetLanguageCode(message.Language),
                message.IP,
                message.ActorId);
    }
}