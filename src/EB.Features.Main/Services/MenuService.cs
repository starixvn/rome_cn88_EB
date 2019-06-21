using System.Collections.Generic;
using System.Linq;
using EB.Features.Main.Model;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Core.Service.Interface;
using NapMenuItem = Fanex.NAP.Core.Models.MenuItem;

namespace EB.Features.Main.Services
{
    public interface IMenuService
    {
        IEnumerable<MenuItem> GetMenu(int userId);

        IEnumerable<MenuItem> GetMenu(IEnumerable<NapMenuItem> napMenuItems);
    }

    public class MenuService : IMenuService
    {
        private readonly IUserService userService;

        public MenuService(IUserService userService)
        {
            this.userService = userService;
        }

        public IEnumerable<MenuItem> GetMenu(int userId)
        {
            var MenuItemList = userService.GetMenuItem(
                userId,
                NapSettings.Settings.SiteId,
                NapSettings.Settings.Environment,
                true);

            return GetMenu(MenuItemList);
        }

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
