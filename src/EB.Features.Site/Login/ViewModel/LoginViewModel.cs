namespace EB.Features.Site.Login
{
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Models;

    public class LoginViewModel
    {

        public LoginViewModel()
        {
            LanguageName = LanguageConstants.CodeNames[UserSessionImpl.Current.Language];
            LanguageCode = UserSessionImpl.Current.Language;
        }

        public LoginViewModel(string message, User userInfo)
        {
            ErrorMessage = message;
            LanguageName = LanguageConstants.CodeNames[UserSessionImpl.Current.Language];
            LanguageCode = UserSessionImpl.Current.Language;
            UserInfo = userInfo;
        }

        public string ErrorMessage { get; }

        public string LanguageName { get; }

        public string LanguageCode { get; }

        public User UserInfo { get; }
    }
}