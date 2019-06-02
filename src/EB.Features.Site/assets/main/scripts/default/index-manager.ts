let indexManager = {
    urls: {
        mainRootPath: null,
        changeLanguage: null,
        getTotalOnlineMemberUrl: null,
        onlineList: null,
        liveChat: null,
        contactUs: null,
        reCheckin: null,
        changePassword: null,
        changeSecurityCode: null,
        onlineMembersPath: null,
        oldReportsPath: null,
        wagerMonitoringConsolePath: null,
        troubleshootingPath: null
    },
    elements: {
        mainMenu: null,
        subMenu: null,
        currentURLCookieName: null,
        subMenuItems: null,
        mainFrame: null,
        menuBar: null,
        onlineUserId: null,
        clearClientData: null,
        isNotHeadLine: 1,
        intervalToGetTotalOnlineMember: 600000,
        header: null,
        content: null
    },
    currentState: {
        mainMenuId: null,
        isOpenPopup: false,
        isClearActiveMenu: false,
        wagerMonitoringWindow: null
    },
    resources: {
        changePassword: null,
        changeSecurityCode: null
    },
    topWindow: null,
    init: function (topWindow: any) {
        indexManager.topWindow = topWindow;

        indexManager.cacheElements();
        indexManager.bindEvents();

        if (indexManager.elements.clearClientData === "1") {
            siteInfrastructure.localStorage.clear();
        }

        indexManager.restoreUrl();
    },
    cacheElements: function () {
        indexManager.elements.isNotHeadLine = parseInt($("#IsNotHeadLine").val());
        indexManager.elements.intervalToGetTotalOnlineMember = parseInt($("#IntervalToGetTotalOnlineMember").val());
        indexManager.urls.mainRootPath = $("#mainRootPath").val();

        indexManager.urls.changeLanguage = indexManager.urls.mainRootPath + "Language/Change";
        indexManager.urls.getTotalOnlineMemberUrl = indexManager.urls.mainRootPath + "Default/GetTotalOnlineMember";
        indexManager.urls.onlineList = $("#OnlineMembersPath").val();
        indexManager.urls.liveChat = indexManager.urls.mainRootPath + "CustomerList/LiveChat";
        indexManager.urls.contactUs = indexManager.urls.mainRootPath + "Help/ContactUs";
        indexManager.urls.reCheckin = indexManager.urls.mainRootPath + "Login/ReCheckin";
        indexManager.urls.changePassword = indexManager.urls.mainRootPath + "Password/ChangePassword";
        indexManager.urls.changeSecurityCode = indexManager.urls.mainRootPath + "SecurityCode/ChangeSecurityCode";
        indexManager.urls.oldReportsPath = $("#OldReportsPath").val();
        indexManager.urls.wagerMonitoringConsolePath = $("#WagerMonitoringConsolePath").val();
        indexManager.urls.troubleshootingPath = $("#TroubleshootingPath").val();

        indexManager.elements.mainMenu = $(".menu-item");
        indexManager.elements.subMenu = $(".sub-menu");
        indexManager.elements.subMenuItems = $(".sub-menu-item");
        indexManager.elements.currentURLCookieName = $("#currentURLCookieName").val();
        indexManager.elements.mainFrame = $("#main");
        indexManager.elements.menuBar = $(".menubar");
        indexManager.elements.onlineUserId = $("#OnlineUserId").val();
        indexManager.elements.clearClientData = $("#ClearClientData").val().toString();
        indexManager.resources.changePassword = $("#ChangePassword").val();
        indexManager.resources.changeSecurityCode = $("#ChangeSecurityCode").val();
        indexManager.elements.header = $("#header");
        indexManager.elements.content = $("#content");
    },
    bindEvents: function () {
        $(".changelanguage-link").on("click touchend", (event: JQueryEventObject) => {
            indexManager.changeLanguage($(event.target).data("value"));
        });
        $(".online-member").on("click touchend", () => {
            indexManager.viewOnlineList();
        });
        $("#lnk-personal-announcement").on("click touchend", function () {
            $.blockUI();
            indexManager.activeMainMenu("menu-ANNOUNCEMENT");
            $("#main").attr("src", $(this).children("a").attr("href"));
        });
        indexManager.elements.mainMenu.on("click touchend", function () {
            $.blockUI();
            const $this = $(this);
            indexManager.mainMenuClick($this.attr("id"));
        });
        indexManager.elements.subMenuItems.on("click touchend", function () {
            $.blockUI();
            const $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
        });

        $("#lnk-new-url").on("click touchend", () => {
            $("#menu-HELP").click();
        });

        $("#lnk-live-chat").on("click touchend", () => {
            window.open(indexManager.urls.liveChat, "LiveChat", "height=600,width=800");
        });
        $("#lnk-contact-us").on("click touchend", () => {
            indexManager.elements.mainMenu.removeClass("active");
            indexManager.elements.subMenu.addClass("display-none");

            indexManager.elements.menuBar.removeClass("has-submenu");
            indexManager.elements.mainFrame.attr("src", indexManager.urls.contactUs);
        });
        $("#menu-back").on("click touchend", () => {
            const history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
            let historyIndex = history ? parseInt(history, 0) : 0;

            if (historyIndex === 0) {
                return;
            }

            const key = indexManager.elements.currentURLCookieName + "_" + historyIndex;
            historyIndex--;

            siteInfrastructure.localStorage.remove(key);
            siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, historyIndex);

            indexManager.restoreUrl();
        });

        $("#lnk-my-setting").on("click touchend", () => {
            indexManager.topWindow.showCN88CancelButton = false;
            indexManager.currentState.isClearActiveMenu = true;
            indexManager.clearAllActiveMenu();
        });

        $("#lnk-change-password").on("click touchend", () => {
            indexManager.topWindow.popupManager.init($, top);
            indexManager.topWindow.popupManager.open({
                title: indexManager.resources.changePassword,
                url: indexManager.urls.changePassword,
                top: 128,
                width: 490,
                height: 500
            });
        });

        $("#lnk-security-code").on("click touchend", () => {
            indexManager.topWindow.popupManager.init($, top);
            indexManager.topWindow.popupManager.open({
                title: indexManager.resources.changeSecurityCode,
                url: indexManager.urls.changeSecurityCode,
                top: 128,
                width: 490,
                height: 420
            });
        });
        $("#lnk-logout").on("click touchend", () => {
            siteInfrastructure.logout();
        });

        $("#sub-menu-OLDREPORT").parent().off("click touchend").on("click touchend", () => {
            $.blockUI();
            indexManager.currentState.isOpenPopup = true;
            const $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
            $.unblockUI();
            indexManager.currentState.isOpenPopup = false;
            indexManager.viewOldReport();
        });

        $("#sub-menu-WAGERMONITORINGCONSOLE").parent().off("click touchend").on("click touchend", () => {
            $.blockUI();
            indexManager.currentState.isOpenPopup = true;
            const $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
            $.unblockUI();
            indexManager.currentState.isOpenPopup = false;
            indexManager.viewWagerMonitoringConsole();
        });

        $("#sub-menu-TROUBLESHOOTING").parent().off("click touchend").on("click touchend", () => {
            $.blockUI();
            indexManager.currentState.isOpenPopup = true;
            const $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
            $.unblockUI();
            indexManager.currentState.isOpenPopup = false;
            indexManager.viewTroubleshooting();
        });

        $("#main").on("load", () => {
            top.$.unblockUI();
            const currentUrl = top.main.location.href;
            const activeMainMenu = $("#main-menu-items .menu-item.active");
            const activeMainMenuId = activeMainMenu.length ? activeMainMenu.attr("id") : null;

            const activeSubmenu = activeMainMenuId != null ?
                $(".sub-menu[data-main-menu='" + activeMainMenuId + "'] .sub-menu-item.active > a") : null;

            indexManager.addHistory({
                mainMenu: activeMainMenuId,
                subMenu: activeSubmenu && activeSubmenu.length ? activeSubmenu.attr("id") : null,
                url: currentUrl,
                isClearActiveMenu: indexManager.currentState.isClearActiveMenu
            });

            indexManager.currentState.isClearActiveMenu = false;
            indexManager.calculateMainFrameHeight();
        });

        setTimeout(indexManager.reCheckin, 30000);
        if (indexManager.elements.isNotHeadLine === 1) {
            setTimeout(indexManager.GetTotalOnlineMember, indexManager.elements.intervalToGetTotalOnlineMember);
        }
    },
    changeLanguage: (lang: string) => {
        $.ajax({
            type: "POST",
            url: indexManager.urls.changeLanguage,
            data: { languageCode: lang },
            success: function (result: any) {
                if (result !== undefined) {
                    // remove www so that subdomain can override LANGUAGE value
                    const domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, "").replace("www.", "");
                    $.cookie("LANGUAGE", lang, { expires: 30, path: domain });
                    indexManager.reloadPage();
                }
            }
        });
    },

    GetTotalOnlineMember: function () {
        $.ajax({
            type: "GET",
            url: indexManager.urls.getTotalOnlineMemberUrl,
            success: function (result: string) {
                $("#numberOnlineMember").html(result);
            },
            complete: function () {
                setTimeout(indexManager.GetTotalOnlineMember, indexManager.elements.intervalToGetTotalOnlineMember);
            }
        });
    },

    reloadPage: () => {
        top.location.href = indexManager.removeBookmarksInUrl(top.location.href);
    },
    removeBookmarksInUrl: (url: any) => {
        if (typeof url !== "string") {
            return null;
        }

        let i = url.indexOf("#");
        if (i === -1) {
            return url;
        }

        return url.substr(0, i);
    },
    viewOnlineList: () => {
        window.open(indexManager.urls.onlineList, "OnlineList", "height=600,width=800");
    },
    viewOldReport: () => {
        window.open(indexManager.urls.oldReportsPath, "OldReport", "height=750,width=960");
    },
    viewTroubleshooting: () => {
        window.open(indexManager.urls.troubleshootingPath, "OnlineList", "height=700,width=900");
    },
    viewWagerMonitoringConsole: () => {
        $.unblockUI();
        if (indexManager.currentState.wagerMonitoringWindow == null || indexManager.currentState.wagerMonitoringWindow.closed) {
            indexManager.currentState.wagerMonitoringWindow = window.open(
                indexManager.urls.wagerMonitoringConsolePath,
                "WagerMonitoringConsole",
                "height=800,width=1280");
        } else {
            indexManager.currentState.wagerMonitoringWindow.focus();
        }
    },
    mainMenuClick: (id: string) => {
        if (id === "menu-back") {
            return;
        }

        indexManager.elements.mainMenu.removeClass("active");
        let $this = $("#" + id);

        $this.addClass("active");

        indexManager.currentState.mainMenuId = id;
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");

        let subMenu = $(".sub-menu[data-main-menu='" + id + "']");
        if (subMenu && subMenu.length) {
            subMenu.removeClass("display-none");
            indexManager.elements.menuBar.addClass("has-submenu");
            subMenu.find(".sub-menu-item >.redirect-item:first")[0].click();
        } else {
            (<HTMLIFrameElement>$("#main")[0]).src = (<HTMLLinkElement>$this[0].childNodes[1]).href;
        }
    },
    subMenuClick: (id: string) => {
        indexManager.elements.subMenuItems.removeClass("active");
        let $this = $("#" + id);

        $this.parent().addClass("active");
        if (!indexManager.currentState.isOpenPopup) {
            (<HTMLIFrameElement>$("#main")[0]).src = (<HTMLLinkElement>$this[0]).href;
        }
    },
    addHistory: (newEvent: any) => {
        let history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
        let historyIndex = history ? parseInt(history, 0) : 0;
        historyIndex++;
        if (historyIndex === 21) {
            siteInfrastructure.localStorage.shiftStorage(indexManager.elements.currentURLCookieName, newEvent, historyIndex);
            return;
        }

        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, historyIndex);
        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName + "_" + historyIndex, newEvent);
    },
    restoreUrl: () => {
        let history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
        let historyIndex = history ? parseInt(history, 0) : 0;

        if (historyIndex === 0) {
            indexManager.elements.mainMenu.find("a")[1].click();
            return;
        }

        let key = indexManager.elements.currentURLCookieName + "_" + historyIndex;
        let currentPage = siteInfrastructure.localStorage.get(key);
        historyIndex--;

        siteInfrastructure.localStorage.remove(key);
        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, historyIndex);
        if (currentPage.isClearActiveMenu) {
            top.showCN88CancelButton = !indexManager.isMySettingsPage(currentPage.url);
            indexManager.currentState.isClearActiveMenu = true;
            $("#main").attr("src", currentPage.url);
            return;
        }

        if (currentPage.mainMenu == null) {
            indexManager.elements.mainMenu.find("a")[1].click();
            return;
        }

        indexManager.elements.mainMenu.removeClass("active");
        $("#" + currentPage.mainMenu).addClass("active");

        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");

        indexManager.elements.subMenuItems.removeClass("active");

        let subMenu = $(".sub-menu[data-main-menu='" + currentPage.mainMenu + "']");
        if (subMenu && subMenu.length) {
            subMenu.removeClass("display-none");
            indexManager.elements.menuBar.addClass("has-submenu");
        }

        if (currentPage.subMenu != null) {
            $("#" + currentPage.subMenu).parent().addClass("active");
        }

        $("#main").attr("src", currentPage.url);
    },
    reCheckin: function () {
        $.ajax({
            type: "POST",
            url: indexManager.urls.reCheckin,
            data: { onid: indexManager.elements.onlineUserId },
            success: (isKickout: any) => {
                try {
                    if (isKickout) {
                        alert("Kick out!");
                        siteInfrastructure.logout();
                    }
                }
                catch (e) {
                    if (typeof (isKickout) === "string"
                        && isKickout.indexOf("Oops... you are here because of a system error") > 0) {
                        siteInfrastructure.logout();
                        alert((<HTMLInputElement>document.getElementById("errorMessage")).value);
                    }
                }
            },
            error: (xhr) => {
                if (xhr.status === 500) {
                    siteInfrastructure.logout();
                    alert((<HTMLInputElement>document.getElementById("errorMessage")).value);
                } else if (xhr.status === 401 || xhr.status === 403) {
                    siteInfrastructure.logout();
                }
            },
            complete: () => {
                setTimeout(indexManager.reCheckin, 30000);
            }
        });
    },
    activeMainMenu: (id: string) => {
        indexManager.elements.mainMenu.removeClass("active");
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");

        indexManager.currentState.mainMenuId = id;
        $("#" + id).addClass("active");
    },
    closePopupAndReloadMainIframe: () => {
        setTimeout(() => {
            indexManager.topWindow.popupManager.close();
            indexManager.elements.mainFrame[0].contentWindow.location.reload(true);
        }, 1000 * 3);
    },
    activeSubMenu: (id: string, mainMenuId: string) => {
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.addClass("has-submenu");
        $(".sub-menu[data-main-menu='" + mainMenuId + "']").removeClass("display-none");

        $("#" + id).parent().addClass("active");
    },
    calculateMainFrameHeight: () => {
        const height = $(window).height()
            - indexManager.elements.header.height()
            - indexManager.elements.menuBar.height();
        indexManager.elements.content.height(height);
    },
    clearAllActiveMenu: () => {
        indexManager.elements.mainMenu.removeClass("active");
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");
    },
    isMySettingsPage: (currentUrl: string) => {
        return currentUrl.toLowerCase().indexOf("cn88customersetting/viewcn88customersetting/mysetting") > -1;
    }
};

$(function () {
    indexManager.init(window.top);
});