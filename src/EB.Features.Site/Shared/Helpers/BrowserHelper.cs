using EB.Features.Shared.Model;
using EB.Features.Site.Shared.ViewModels;

namespace EB.Features.Site.Shared.Helpers
{
    public interface BrowserHelper
    {
        bool IsMobile(bool isOnlyTablet);

        BrowserViewModel CheckSupportedBrowsers();

        Device GetDevice();

        string GetDeviceId();

        string GetUserAgent();

        bool IsAndroidNativeBrowser(string userAgent);

        BrowserInfo GetBrowserInfo();
    }
}