using EB.Features.Shared.Model;

namespace EB.Features.Site.Shared.ViewModels
{
    public class BrowserViewModel
    {
        public bool IsUnsupportedBrowser { get; set; }

        public string DownloadNewBrowserLink { get; set; }

        public UserLevel UserLevel { get; set; }
    }
}