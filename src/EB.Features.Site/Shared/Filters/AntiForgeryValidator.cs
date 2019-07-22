namespace EB.Features.Site.Shared.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using Features.Shared.Model;

    /// <summary>
    /// http://blog.stevensanderson.com/2008/09/01/prevent-cross-site-request-forgery-csrf-using-aspnet-mvcs-antiforgerytoken-helper/
    /// </summary>
    public static class AntiForgeryValidator
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";

        public static Result ValidateRequest()
        {
            try
            {
                var request = HttpContext.Current.Request;
                var formToken = request.Headers[AntiForgeryFormTokenName];

                if (string.IsNullOrWhiteSpace(formToken))
                {
                    return Result.CreateErrorResult(
                        (int)HttpStatusCode.Forbidden,
                        "Request Verification Token does not exist. Please refresh your page. Thanks.");
                }

                var cookieValue = request.Cookies[AntiForgeryConfig.CookieName];

                if (!string.IsNullOrWhiteSpace(cookieValue?.Value))
                {
                    AntiForgery.Validate(cookieValue.Value, formToken);
                }
            }
            catch (Exception)
            {
                return Result.CreateErrorResult(
                    (int)HttpStatusCode.Forbidden,
                    "Validation of Anti-XSRF token failed. Please refresh your page. Thanks.");
            }

            return Result.CreateSuccessResult();
        }
    }
}