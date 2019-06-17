/* File Created: April 14, 2014 */
/* Keep login being a pure javascript page only */
/* global CryptoJS */
'use strict';

var msg, innerMsg, username, password, btnSubmit;

function shake(target, times, duration) {
    for (var i = times, j = 0; i >= 0; i--) {
        setTimeout(target + '.style.left = "-' + i + '0px"', j++ * duration);
        setTimeout(target + '.style.left = "' + i + '0px"', j++ * duration);
    }
}

// Returns the version of Internet Explorer or -1
// (indicating the use of another browser).
function getIeVersion() {
    var version = -1; // Return value assumes failure.

    if (window.navigator.userAgent.indexOf('MSIE') > 0) {
        var usrAgent = navigator.userAgent;
        var reg = new RegExp('MSIE ([0-9]{1,}[.0-9]{0,})');

        if (reg.exec(usrAgent) != null) {
            version = parseFloat(RegExp.$1);
        }
    }

    return version;
}

function checkBrowser() {
    var msieVersion = getIeVersion();

    if (msieVersion !== -1 && msieVersion < 8.0) {
        var warnPnl = document.getElementById('divWarningPanel');

        if (warnPnl.style.display === '' || warnPnl.style.display === 'none') {
            warnPnl.style.display = 'block';
        }
    }
}
function ShowPassword() {
    var type = document.getElementById("txtPassword").getAttribute("type");
    if (type == "password") {
        document.getElementById("txtPassword").setAttribute("type", "text");
    } else {
        document.getElementById("txtPassword").setAttribute("type", "password");
    }

}
window.onload = function () {

    siteInfrastructure.localStorage.clear();
    /* jshint ignore: start */
    setTimeout('window.history.forward()', 0);
    /* jshint ignore: end */

    msg = document.getElementById('divMessage');
    username = document.getElementById('txtUsername');
    password = document.getElementById('txtPassword');
    btnSubmit = document.getElementById('btnSubmit');

    checkBrowser(); // check if the client's browser is supported

    if (msg.innerHTML.length > 0) {
        msg.style.display = 'block';
        shake('msg', 5, 100);
    }
    else {
        msg.style.display = 'none';
    }

    $("#language-ddl li").on("click touchend", function () {
        changeLanguage($(this).find(".flag").data("language-code"));
    });

    var currentLanguage = $("#currentLanguage").val();
    var $flag = $("#flag");
    $flag.removeClass("flag-cn");
    $flag.removeClass("flag-tw");
    $flag.removeClass("flag-en");
    $flag.addClass($(".flag[data-language-code='" + currentLanguage + "']").data("flag"));
};

function changeLanguage(lang) {

    //document.forms[0].submit();
    $.ajax({
        type: "POST",
        url: $("#changeLanguagePath").val(),
        data: { languageCode: lang },
        success: function (result) {
            if (result !== undefined) {
                // Remove www so that subdomain can override LANGUAGE value
                var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace("www.", '');
                $.cookie("LANGUAGE", lang, { expires: 30, path: domain });
                top.location.href = removeBookmarksInUrl(top.location.href);
            }
        }
    });
}

function removeBookmarksInUrl(url) {
    if (typeof url !== 'string') {
        return null;
    }

    var i = url.indexOf("#");
    if (i === -1) {
        return url;
    }

    return url.substr(0, i);
}

document.forms[0].onsubmit = function () {
    msg.style.display = 'none';

    var isValid = true;

    username.value = username.value.trim();

    if (username.value.length === 0) {
        username.className += ' err';
        isValid = false;
        shake('txtUsername', 5, 100);
    } else {
        username.className = username.className.replace('err', '');
    }

    if (password.value.length === 0) {
        password.className += ' err';

        isValid = false;
        shake('txtPassword', 5, 100);
    } else {
        var encryptedPwd = CryptoJS.CfsEnCode(password.value);
        var hashPwd = '';

        password.className = password.className.replace('err', '');
        hashPwd = CryptoJS.SHA256(encryptedPwd);
        document.getElementById('hdnPassword').value = hashPwd;
    }

    return isValid;
};

/* jshint freeze: false */
String.prototype.trim = function () {
    return this.replace(/^\s*/, '').replace(/\s*$/, '');
};
