namespace EB.Features.Site.Main.Handlers
{
    using EB.Features.Main.Params;
    using EB.Features.Main.Services;
    using EB.Features.Site.Main.Requests;
    using EB.Features.Site.Main.ViewModel;
    using MediatR;

    public class UserInfoHandler :
        IRequestHandler<ChangePasswordQuery, ChangePasswordViewModel>
    {
        private readonly IUserInfoService userInfoService;

        public UserInfoHandler(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }


        public ChangePasswordViewModel Handle(ChangePasswordQuery message)
        {
            var updatePasswordMesssage = userInfoService.ChangePassword(
                new ChangePasswordParam
                {
                    UserId = message.CurrentSession.UserId,
                    NewPassword = message.NewPassword,
                    OldPassword = message.OldPassword,
                    ConfirmedNewPassword = message.ConfirmedNewPassword
                });

            return new ChangePasswordViewModel(message, updatePasswordMesssage);
        }
    }
}