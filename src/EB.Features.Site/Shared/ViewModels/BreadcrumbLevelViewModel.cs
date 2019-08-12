using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EB.Features.Shared.Model;
using EB.Features.Site.Shared.Helpers;

namespace EB.Features.Site.Shared.ViewModels
{
    public class BreadcrumbLevelViewModel
    {
        public BreadcrumbLevelViewModel(
            IEnumerable<CustInfo> breadcrumbLevel,
            string breadcrumbLevelJson = null)
        {
            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson ?? JsonHelper.ToJsonFunc(breadcrumbLevel);
        }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        public string BuildBreadcrumbLink(CustInfo model, string currentLink)
        {
            var param = new Dictionary<string, object>
            {
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName,
                ["RoleId"] = model.RoleId,
                ["BreadcrumbLevelJson"] = BreadcrumbLevelJson
            };

            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(currentLink, param);
        }

        public IEnumerable<BreadcrumbItem> BuildBreadcrumbLinks(string currentLink)
        {
            if (BreadcrumbLevel.Any())
            {
                return BreadcrumbLevel.Select(custInfo =>
                    new BreadcrumbItem(custInfo.CustName, BuildBreadcrumbLink(custInfo, currentLink)));
            }

            return Enumerable.Empty<BreadcrumbItem>();
        }
    }

    public class BreadcrumbItem
    {
        public BreadcrumbItem(string text, string link)
        {
            Text = text;
            Link = link;
        }

        public string Text { get; }

        public string Link { get; }
    }
}