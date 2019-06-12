let hrefHandler = {    
    init: function (topWindow: any) {
        let urlLoginPage = "Login/Index";
        let href = window.location.href;
        if (topWindow.location.href.indexOf(urlLoginPage) <= 0 &&
            href.indexOf(urlLoginPage) > 0) {
            topWindow.location.href = href;
        }
    }
};

$(function () {
    hrefHandler.init(window.top);
});