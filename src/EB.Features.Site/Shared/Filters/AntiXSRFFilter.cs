namespace EB.Features.Site.Shared.Filters
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Features.Shared.Model;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AntiXSRFFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var antiXSRFResult = AntiForgeryValidator.ValidateRequest();

            if (antiXSRFResult.IsSuccess)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                ActionDenied(filterContext, antiXSRFResult);
            }
        }

        private static void ActionDenied(ActionExecutingContext filterContext, Result antiXSRFResult)
        {
            var request = filterContext.HttpContext.Request;

            if (request.IsAjaxRequest())
            {
                var response = filterContext.HttpContext.Response;
                response.Clear();
                response.Write("{ \"message\": \"" + antiXSRFResult.Message + "\" }");

                filterContext.Result = new HttpStatusCodeResult(antiXSRFResult.Code);
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
            }
        }
    }
}