namespace EB.Features.Main.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using EB.Features.Main.Resources;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Service.Interface;

    public interface IAuthService
    {
        string Login(string userName, string password);

        User GetUserInfo(string userName);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserService userService;
        private readonly IGroupService groupService;

        public AuthService(IUserService userService, IGroupService groupService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        public User GetUserInfo(string userName)
            => userService.Get(userName.Trim());

        public string Login(string userName, string password)
        {
            var message = string.Empty;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                var user = userService.Get(userName.Trim());

                if (user != null)
                {
                    var hashPassword = GetHash(user.Password);

                    if (password == hashPassword)
                    {
                        if (!CheckUserSitePermission(user))
                        {
                            message = SiteResources.You_have_no_authorization;
                        }                        
                    }
                    else
                    {
                        message = SiteResources.Invalid_Username_or_Password;
                    }
                }
                else
                {
                    message = SiteResources.Invalid_Username_or_Password;
                }
            }
            else
            {
                message = SiteResources.Invalidinput;
            }

            return message;
        }

        private bool CheckUserSitePermission(User user)
        {
            var group = user.Group == null ? null : groupService.Get(user.Group.Id);
            var siteLevelList = userService.GetSiteLevel(user.Id, NapSettings.Settings.Environment);
            var menuList = userService.GetMenuItem(user.Id, NapSettings.Settings.SiteId);

            // gets all sites of logging-in user
            return (group == null || group.IsActive) &&
                   siteLevelList.Any(x => x.Site.Id == NapSettings.Settings.SiteId && x.IsActive &&
                   menuList.Any());
        }

        private static string GetHash(string input)
        {
            var encrypted = string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var hashAlgorithm = new SHA256Managed();
                var hashData = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLowerInvariant();
            }

            return encrypted;
        }
    }
}
