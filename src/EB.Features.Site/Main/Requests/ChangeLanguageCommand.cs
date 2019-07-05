namespace EB.Features.Site.Main.Requests
{
    using MediatR;

    public class ChangeLanguageCommand : IRequest<string>
    {
        public ChangeLanguageCommand(string languageCode)
        {
            LanguageCode = languageCode;
        }

        public string LanguageCode { get; }        
    }
}
