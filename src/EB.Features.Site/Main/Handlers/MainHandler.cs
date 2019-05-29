using System.Collections.Generic;
using System.Threading.Tasks;
using EB.Features.Main.Model;
using EB.Features.Main.Services;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Main.ViewModel;
using MediatR;

namespace EB.Features.Site.Main.Handlers
{
    public class MainHandler :
        IAsyncRequestHandler<IndexQuery, IndexViewModel>,
        IAsyncRequestHandler<MenuBarQuery, IEnumerable<MenuItem>>
    {
        private readonly IMenuService siteMenuService;

        public MainHandler(IMenuService siteMenuService)
        {
            this.siteMenuService = siteMenuService;
        }

        public async Task<IEnumerable<MenuItem>> Handle(MenuBarQuery query)
        {
            var menuItems = siteMenuService.GetMenu(query.MenuList);

            return menuItems;
        }

        public async Task<IndexViewModel> Handle(IndexQuery query)
        {
            return new IndexViewModel(query.currentSession);
        }
    }
}