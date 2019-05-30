using System;
using System.Collections.Generic;
using System.Linq;
using NapMenuItem = Fanex.NAP.Core.Models.MenuItem;

namespace EB.Features.Main.Model
{
    public class MenuItem : IMenuItem
    {
        private readonly Func<string> getDisplayName;

        public MenuItem() : this(null, null)
        {
        }

        public MenuItem(NapMenuItem napMenuItem, Func<string> getDisplayName = null)
        {
            this.getDisplayName = getDisplayName;
            Key = napMenuItem.Id.ToString();
            ResourceKey = napMenuItem.Name;
            Link = napMenuItem.Url;
        }

        public string Key { get; set; }

        public string ResourceKey { get; set; }

        public virtual string Link { get; set; }

        public List<MenuItem> SubMenus { get; set; }

        public string DisplayName
            => getDisplayName != null ? getDisplayName() : MainResource.ResourceManager.GetString(ResourceKey);

        public bool ShowSubMenus => SubMenus != null && SubMenus.Any();

        public IEnumerable<MenuItem> VisibileSubMenus => SubMenus;
    }
}
