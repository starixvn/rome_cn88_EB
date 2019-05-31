using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace EB.Features.Site.Shared
{
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

        public static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
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

        private static IHtmlString RenderStyleWithMedia(int maxDeviceWidth, params string[] paths)
        {
            var styleHtml = string.Join(string.Empty, paths.Select(path => $"<link href=\"{ path }\" rel=\"stylesheet\" type=\"text/css\" media=\"screen and (max-device-width: {maxDeviceWidth}px)\">"));

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

        public static IHtmlString RenderStylesWithMedia(int maxDeviceWidth, params string[] paths)
        {
            var styleHtml = string.Join(
                string.Empty,
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.css?v={staticResourcesVersion}")));

            return RenderStyleWithMedia(maxDeviceWidth, styleHtml);
        }

        public static class AssetScripts
        {
            public static IHtmlString Render(params string[] paths)
            {
                return RenderScript(paths.Select(path => $"{path}{minText}.js?v={staticResourcesVersion}").ToArray());
            }
        }

        public static class AssetStyles
        {
            public static IHtmlString Render(params string[] paths)
            {
                return RenderStyle(paths.Select(path => $"{path}{minText}.css?v={staticResourcesVersion}").ToArray());
            }
        }
    }
}