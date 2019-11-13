let hrefHandler = {
    init: function (topWindow: Window) {
        const urlLoginPage = "Login/Index";
        const href = window.location.href;
        if (topWindow.location.href.indexOf(urlLoginPage) <= 0 &&
            href.indexOf(urlLoginPage) > 0) {
            topWindow.location.href = href;
        }
    }
};

$(function () {
    hrefHandler.init(window.top);
});