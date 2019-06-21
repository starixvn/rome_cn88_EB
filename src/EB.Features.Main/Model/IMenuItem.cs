using System.Collections.Generic;

namespace EB.Features.Main.Model
{
    public interface IMenuItem
    {
        string Key { get; set; }

        string ResourceKey { get; set; }

        string DisplayName { get; }

        string Link { get; set; }

        bool ShowSubMenus { get; }

        List<MenuItem> SubMenus { get; set; }

        IEnumerable<MenuItem> VisibileSubMenus { get; }
    }
}
