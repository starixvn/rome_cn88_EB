namespace EB.Features.Site.Main.Requests
{
    using EB.Features.Site.Main.ViewModel;
    using EB.Features.Site.Shared;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class ChangePasswordQuery : BaseQuery, IRequest<ChangePasswordViewModel>
    {
        public ChangePasswordQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public string CurrentPassword { get; set; }

        public override bool IsValid()
            => !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrEmpty(CurrentPassword);
    }
}