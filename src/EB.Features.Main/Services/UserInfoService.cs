namespace EB.Features.Main.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using EB.Features.Main.Params;
    using EB.Features.Main.Resources;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Resources;
    using Fanex.Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Service.Interface;

    public interface IUserInfoService
    {
        Result ChangePassword(ChangePasswordParam param);

        User GetUserInfo(string userName);
    }

    public class UserInfoService : IUserInfoService
    {
        private readonly IUserService userService;
        private readonly ISecurityService securityService;

        public UserInfoService(IUserService userService,
            ISecurityService securityService)
        {
            this.userService = userService;
            this.securityService = securityService;
        }

        public User GetUserInfo(string userName)
            => userService.Get(userName.Trim());

        public Result ChangePassword(ChangePasswordParam param)
        {
            var resultMessage = string.Empty;
            var user = userService.Get(param.UserId);
            var hashPassword = GetHash(user.Password);
            if (hashPassword == param.OldPassword)
            {
                var password = Cryptography.CfsEnCode(param.NewPassword);

                var resultUpdate = securityService.UpdatePassword(NapSettings.Settings.SiteId, user.Id, password);
                if (!resultUpdate)
                {
                    resultMessage = AppResources.SystemErrorMessage;
                }
            }
            else
            {
                resultMessage = MainResource.WrongOldPasswordMessage;
            }

            return string.IsNullOrEmpty(resultMessage) ?
                Result.CreateSuccessResult(MainResource.PasswordChangedSuccessfully) :
                Result.CreateErrorResult(resultMessage);
        }

        public static string GetHash(string password)
        {
            var encrypted = string.Empty;
            var _hashAlgorithm = new SHA256Managed();

            if (!string.IsNullOrWhiteSpace(password))
            {
                var hashData = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

                encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLowerInvariant();
            }

            return encrypted;
        }
    }
}
