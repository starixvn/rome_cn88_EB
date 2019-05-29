using System.Collections.Generic;
using System.Linq;
using EB.Features.Main.Model;
using NapMenuItem = Fanex.NAP.Core.Models.MenuItem;

namespace EB.Features.Main.Services
{
    public interface IMenuService
    {
        IEnumerable<MenuItem> GetMenu(IEnumerable<NapMenuItem> napMenuItems);
    }

    public class MenuService : IMenuService
    {
        public IEnumerable<MenuItem> GetMenu(IEnumerable<NapMenuItem> napMenuItems)
        {
            var menuItems = new List<MenuItem>();
            foreach (var napMenuItem in napMenuItems.Where(i => i.Parent == null))
            {
                var menuItem = new MenuItem(napMenuItem);
                var napSubMenuItems = napMenuItems.Where(i => i.Parent != null && i.Parent.Id == napMenuItem.Id);                
                menuItem.SubMenus = napSubMenuItems.Select(i => new MenuItem(i)).ToList();

                menuItems.Add(menuItem);
            }

            return menuItems;
        }
    }
}
