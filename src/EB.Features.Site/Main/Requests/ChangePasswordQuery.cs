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

        public string ConfirmedNewPassword { get; set; }

        public string OldPassword { get; set; }

        public override bool IsValid()
            => !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmedNewPassword) && !string.IsNullOrEmpty(OldPassword);
    }
}