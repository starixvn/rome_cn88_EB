using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EB.Features.Main.Model;
using EB.Features.Main.Services;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Main.ViewModel;
using EB.Features.Site.Shared.Helpers;
using MediatR;
using Sunplus.Agent.Utilities;

namespace EB.Features.Site.Main.Handlers
{
    public class MainHandler :
        IAsyncRequestHandler<IndexQuery, IndexViewModel>,
        IAsyncRequestHandler<MenuBarQuery, IEnumerable<MenuItem>>
    {
        private readonly IMenuService siteMenuService;
        private readonly BrowserHelper browserHelper;
        private readonly Func<string, string> encodeDataFunction;

        public MainHandler(IMenuService siteMenuService, BrowserHelper browserHelper)
            : this(siteMenuService, browserHelper, null)
        {
        }

        internal MainHandler(
            IMenuService siteMenuService,
            BrowserHelper browserHelper,
            Func<string, string> encodeDataFunction)
        {
            this.siteMenuService = siteMenuService;
            this.browserHelper = browserHelper;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
        }

        public async Task<IEnumerable<MenuItem>> Handle(MenuBarQuery query)
            => siteMenuService.GetMenu(query.MenuList);

        public async Task<IndexViewModel> Handle(IndexQuery query)
            => new IndexViewModel(query.CurrentSession, browserHelper, encodeDataFunction, query);
    }
}