"use strict";

var Sphinx = Sphinx || {};
Sphinx.Resources = (function () {

    var listLanguage = [];

    function Language() {
        this.Id = '';
        this.Username = '';
        this.Flag = '';
        this.Password = '';
        this.Language = '';
        this.Captcha = '';
        this.ErrorMessage = '';
        this.Login = '';
        this.ShowPassword = '';
        this.AutumnTitle = '';
        this.SignIn = '';
        this.errUserName = '';
        this.errValidCode = '';
    }

    var addLanguage = function (lang) {
        listLanguage.push(lang);
    };

    var english = function () {
        var resource = new Language();
        resource.Id = 'en-US';
        resource.Username = 'Username';
        resource.Flag = 'flag flag-en';
        resource.Password = 'Password';
        resource.Language = 'English';
        resource.Captcha = 'Validation';
        resource.ErrorMessage = '';
        resource.Login = 'Login';
        resource.ShowPassword = 'Show password';
        resource.AutumnTitle = 'textTitle_en';
        resource.SignIn = 'SignIn';
        resource.errUserName = 'Please input username and password';
        resource.errValidCode = 'You must input validation code.';
        resource.browserWarning = "Your browser is out of date and might not be working properly with our website. For a better support, we strongly recommend that you upgrade to a newer version";
        resource.updateYourBrowser = "Click here to update your browser";

        return resource;
    };
    addLanguage(english());

    var taiwan = function () {
        var resource = new Language();
        resource.Id = 'zh-TW';
        resource.Username = '用户名';
        resource.Flag = 'flag flag-tw';
        resource.Password = '密碼';
        resource.Language = '繁體';
        resource.Captcha = '驗證';
        resource.ErrorMessage = '';
        resource.Login = '登入';
        resource.ShowPassword = '顯示密碼';
        resource.AutumnTitle = 'textTitle-tw';
        resource.SignIn = 'SignIn';
        resource.errUserName = '請輸入用戶名及密碼';
        resource.errValidCode = '您必须输入正确验证码';
        resource.browserWarning = "您的瀏覽器已過期，可能無法與我們的網站正常操作。為了提供更好的支援，我們強烈建議您升級至新的版本";
        resource.updateYourBrowser = "請點擊這里以升級您的瀏覽器";

        return resource;
    };
    addLanguage(taiwan());
    
    var chineseSimplified = function () {
        var resource = new Language();
        resource.Id = 'zh-Hans';
        resource.Username = '用户名';
        resource.Flag = 'flag flag-cn';
        resource.Password = '密码';
        resource.Language = '简体';
        resource.Captcha = '验证';
        resource.ErrorMessage = '';
        resource.Login = '登入';
        resource.ShowPassword = '显示密码';
        resource.AutumnTitle = 'textTitle-cn';
        resource.SignIn = 'SignIn';
        resource.errUserName = '请输入用户名及密码';
        resource.errValidCode = '您必須輸入正確驗證碼';
        resource.browserWarning = "您的浏览器已过期，可能无法与我们的网站正常操作。为了提供更好的支援，我们强烈建议您升级至新的版本";
        resource.updateYourBrowser = "请点击这里以升级您的浏览器";

        return resource;
    };
    addLanguage(chineseSimplified());

    return {
        init: function () {
        },

        languages: function () {
            return listLanguage;
        },

        getLanguage: function (languageCode) {

            switch (languageCode) {
                case 'en-US':
                    return english();
                
                case 'zh-TW':
                    return taiwan();
              
                case 'zh-Hans':
                    return chineseSimplified();
            }

            return english();
        }
    };
})();