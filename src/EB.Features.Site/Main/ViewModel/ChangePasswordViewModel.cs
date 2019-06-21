namespace EB.Features.Site.Main.ViewModel
{
    using System.Collections.Generic;
    using EB.Features.Shared.Model;
    using EB.Features.Site.Main.Requests;

    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel()
        {

        }

        public ChangePasswordViewModel(ChangePasswordQuery query, Result result)
        {
            Result = result;
            Query = query;
        }

        public ChangePasswordQuery Query { get; }

        public Result Result { get; }
    }
}
