namespace EB.Features.AccountManagement.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppResources;
    using Criteria.SubAccount;
    using Fanex.Data.Repository;
    using Models.SubAccount;
    using Params.SubAccount;
    using Shared.Model;
    using Sunplus.Agent.Model.Entities;

    public interface SubAccountService
    {
        Task<SubAccountItem> GetSuperSubAccountAsync(string usernameOrNickname);

        Task<Result> UpdateSubAccountAsync(UpdateSubAccountParam param);
    }

    public class SubAccountServiceImpl : SubAccountService
    {
        private readonly IDynamicRepository dynamicRepository;

        public SubAccountServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<SubAccountItem> GetSuperSubAccountAsync(string usernameOrNickname)
        {
            ICriteria criteria = new GetSuperSubAccountCriteria(usernameOrNickname);
            var subAccountItem = await dynamicRepository.GetAsync<SubAccountItem>(criteria);

            return subAccountItem ?? new SubAccountItem();
        }

        public async Task<Result> UpdateSubAccountAsync(UpdateSubAccountParam param)
        {
            var subAccountItem = await GetSuperSubAccountAsync(param.Username);

            return subAccountItem.CustId == param.CustId && subAccountItem.SubAccId == param.SubAccId
                ? await UpdateSubAccountAndKickoutAsync(param, subAccountItem)
                : Result.CreateErrorResult(message: SubAccountResource.InvalidSMASubAccount);
        }

        private async Task<Result> UpdateSubAccountAndKickoutAsync(
            UpdateSubAccountParam param,
            SubAccountItem subAccountItem)
        {
            param.Name = subAccountItem.FirstName;
            param.PageAccess = string.Join(string.Empty, FilterWithValidPermissions(param.PageAccess));
            var result = await UpdateAsync(new UpdateSubAccountCommand(param));

            // Kickout when updating Permissions.
            if (result.IsSuccess && subAccountItem.PageAccess != param.PageAccess)
            {
                await KickoutSubAccountAsync(param.CustId, param.SubAccId);
            }

            return result;
        }

        private static IEnumerable<string> FilterWithValidPermissions(string pageAccess)
        {
            if (string.IsNullOrWhiteSpace(pageAccess))
            {
                yield break;
            }

            var pageAccessObject = (PageAccess)pageAccess;

            if (pageAccessObject.ViewTotalBets)
            {
                yield return PageAccessType.ViewTotalBets;
            }

            if (pageAccessObject.ViewReports)
            {
                yield return PageAccessType.ViewReports;
            }

            if (pageAccessObject.UpdateAccount)
            {
                yield return PageAccessType.ViewMemberInfo;
                yield return PageAccessType.UpdateAccount;
            }
            else if (pageAccessObject.ViewMemberInfo)
            {
                yield return PageAccessType.ViewMemberInfo;
            }
        }

        private async Task<Result> UpdateAsync(UpdateSubAccountCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return new Result(
                command.ErrCode,
                command.ErrCode == 0 ? SubAccountResource.UpdateSuccessfully : command.ErrMsg);
        }

        private async Task KickoutSubAccountAsync(int custId, int subAccountId)
            => await dynamicRepository.ExecuteAsync(new KickoutSubAccountCommand(custId, subAccountId));
    }
}
