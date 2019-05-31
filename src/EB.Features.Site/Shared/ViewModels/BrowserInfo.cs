namespace EB.Features.Site.Shared.ViewModels
{
    public class BrowserInfo
    {
        public BrowserInfo(bool isPhone, bool isTablet)
        {
            IsPhone = isPhone;
            IsTablet = isTablet;
        }

        public bool IsMobile => IsPhone || IsTablet;

        public bool IsPhone { get; }

        public bool IsTablet { get; }
    }
}