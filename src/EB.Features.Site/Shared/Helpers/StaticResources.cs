
namespace EB.Features.Site.Shared
{
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;

    public static class StaticResources
    {
        private static string staticResourcesVersion;

        private static string minText = ".min";

        public static void Init(string version)
        {
            if (IsDebugMode())
            {
                minText = string.Empty;
            }

            staticResourcesVersion = version;
        }

        private static bool IsDebugMode()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

            return compilationSection.Debug;
        }

        private static IHtmlString RenderStyle(params string[] paths)
        {
            var styleHtml = string.Join(string.Empty, paths.Select(path => $"<link href=\"{ path }\" rel=\"stylesheet\" type=\"text/css\">"));

            return new HtmlString(styleHtml);
        }

        private static IHtmlString RenderScript(params string[] paths)
        {
            var scriptHtml = string.Join(string.Empty, paths.Select(path => $"<script src=\"{ path }\" type=\"text/javascript\"></script>"));

            return new HtmlString(scriptHtml);
        }

        public static IHtmlString RenderScripts(params string[] paths)
        {
            var scriptHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.js?v={staticResourcesVersion}")));

            return RenderScript(scriptHtml);
        }

        public static IHtmlString RenderStyles(params string[] paths)
        {
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyle(styleHtml);
        }
    }
}