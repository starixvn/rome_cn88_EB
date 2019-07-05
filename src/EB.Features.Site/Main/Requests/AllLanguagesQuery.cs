namespace EB.Features.Site.Main.Requests
{
    using EB.Features.Site.Main.ViewModel;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
    }
}
