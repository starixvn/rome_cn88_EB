namespace EB.Features.Site.Main.Handlers
{
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

    public class MainHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<MenuBarQuery, IEnumerable<MenuItem>>
    {
        private readonly IMenuService siteMenuService;
        private readonly IBrowserHelper browserHelper;
        private readonly Func<string, string> encodeDataFunction;

        public MainHandler(IMenuService siteMenuService, IBrowserHelper browserHelper)
            : this(siteMenuService, browserHelper, null)
        {
        }

        internal MainHandler(
            IMenuService siteMenuService,
            IBrowserHelper browserHelper,
            Func<string, string> encodeDataFunction)
        {
            this.siteMenuService = siteMenuService;
            this.browserHelper = browserHelper;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
        }

        public IEnumerable<MenuItem> Handle(MenuBarQuery message)
            => siteMenuService.GetMenu(message.CurrentSession.UserId);

        public IndexViewModel Handle(IndexQuery message)
            => new IndexViewModel(message.CurrentSession, browserHelper, encodeDataFunction, message);
    }
}