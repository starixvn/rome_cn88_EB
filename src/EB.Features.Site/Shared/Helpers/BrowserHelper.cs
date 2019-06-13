using EB.Features.Shared.Model;
using EB.Features.Site.Shared.ViewModels;

namespace EB.Features.Site.Shared.Helpers
{
    public interface IBrowserHelper
    {
        bool IsMobile(bool isOnlyTablet);

        Device GetDevice();

        string GetDeviceId();

        string GetUserAgent();

        bool IsAndroidNativeBrowser(string userAgent);

        BrowserInfo GetBrowserInfo();
    }
}