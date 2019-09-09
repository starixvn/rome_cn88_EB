namespace EB.Features.Main.Services
{
    using System.Linq;
    using EB.Features.Main.Criteria.InternalLogin;
    using EB.Features.Main.Resources;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Service.Interface;

    public interface IAuthService
    {
        string ValidateLogin(string userName, string password, string loginIp);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserService userService;
        private readonly IGroupService groupService;
        private readonly IDynamicRepository dynamiceRepository;

        public AuthService(IUserService userService, IGroupService groupService, IDynamicRepository dynamiceRepository)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.dynamiceRepository = dynamiceRepository;
        }

        public string ValidateLogin(string userName, string password, string loginIp)
        {
            var message = string.Empty;
            if (!string.IsNullOrEmpty(userName) 
                && !string.IsNullOrEmpty(password) 
                && CheckIPOffice(userName, loginIp))
            {
                var user = userService.Get(userName.Trim());

                if (user != null)
                {
                    var hashPassword = UserInfoService.GetHash(user.Password);

                    if (password == hashPassword)
                    {
                        if (!CheckUserSitePermission(user))
                        {
                            message = MainResource.You_have_no_authorization;
                        }
                    }
                    else
                    {
                        message = MainResource.Invalid_Username_or_Password;
                    }
                }
                else
                {
                    message = MainResource.Invalid_Username_or_Password;
                }
            }
            else
            {
                message = MainResource.Invalidinput;
            }

            return message;
        }

        private bool CheckIPOffice(string userName, string ip)
        {
            var criteria = new CheckIPOfficeCommand(userName, ip);
            dynamiceRepository.Execute(criteria);

            return criteria.IsAccess;
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
    }
}
