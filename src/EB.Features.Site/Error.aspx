<%@ Page Language="C#" AutoEventWireup="false" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="EB.Features.Site.Shared" %>
<%@ Import Namespace="EB.Features.Site.Shared.Models" %>
<%
    var um = new ONEbook.UM.Services.Impl.MaintenanceService();
    if (!um.ByPassUM && um.IsUnderMaintenance())
    {
        Response.Redirect("~/Login/Index", true);
    }
%>
<html>
    <head>
        <title>Error</title>
        <%=StaticResources.RenderStyles("~/assets/common/bundles/site-cn88/error-page") %>
        <%=StaticResources.RenderStyles("~/assets/main/bundles/error-page") %>
    </head>
    <body>
         <%
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UserSessionImpl.Current.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        %>
        <div class="page-content">
            <span class="icon-cancel-circular"></span>
            <div class="txt"><% =EB.Features.Shared.Resources.AppResources.SystemErrorMessage %></div>
        </div>
    </body>
</html>