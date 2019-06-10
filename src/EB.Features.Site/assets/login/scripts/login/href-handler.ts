let hrefHandler = {    
    topWindow: null,
    init: function (topWindow: any) {
        debugger;
        var urlLoginPage = "Login/Index";
        hrefHandler.topWindow = topWindow;
        var href = window.location.href;
        if (hrefHandler.topWindow.location.href.indexOf(urlLoginPage) <= 0 &&
            href.indexOf(urlLoginPage) > 0) {
            hrefHandler.topWindow.location.href = href;
        }
    }
};

$(function () {
    hrefHandler.init(window.top);
});