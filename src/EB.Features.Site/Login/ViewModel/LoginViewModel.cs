namespace EB.Features.Site.Login
{
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Models;

    public class LoginViewModel
    {

        public LoginViewModel(string languageCode)
        {
            LanguageCode = languageCode;
            LanguageName = LanguageConstants.GetLanguageName(languageCode);
        }

        public LoginViewModel(string message, User userInfo, string languageCode) : this(languageCode)
        {
            ErrorMessage = message;
            UserInfo = userInfo;
        }

        public string ErrorMessage { get; }

        public string LanguageName { get; }

        public string LanguageCode { get; }

        public User UserInfo { get; }
    }
}