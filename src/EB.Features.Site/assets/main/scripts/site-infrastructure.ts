interface JQuery {
    stickyTableHeaders(): void;
}

interface Window {
    cn88SuccessfulUpdatingCallback(): void;
    cn88CancelCallback(): void;
    showCN88CancelButton: boolean;
    cn88SaveAndNewCallback(): void;
    showCN88SaveAndNewButton: boolean,
    $: JQueryStatic;
    main: Window;
    siteInfrastructure: SiteInfrastructure;
    indexManager: IndexManager;
    popupManager: PopupManager;
    CryptoJS: Cryptor;
}

top.showCN88CancelButton = true;
top.showCN88SaveAndNewButton = true;

class Result {
    static warningCode = -1000;

    Code: number;
    Message: string;
    IsSuccessful: boolean;
    IsFailed: boolean;
    IsWarning: boolean;

    constructor(data?: CodeResponse) {
        this.Code = data.Code;
        this.Message = data.Message || "";
        this.IsSuccessful = this.Code === 0;
        this.IsFailed = !this.IsSuccessful;
        this.IsWarning = data.Code === Result.warningCode;
    }
}

class SiteInfrastructure {
    localStorage = {
        get: (key: string) => {
            const result = store.enabled ? store.get(key) : $.cookie(key);
            return result ? JSON.parse(result) : undefined;
        },

        set: (key: string, value: string | number | object) => {
            const data = JSON.stringify(value);
            store.enabled ? store.set(key, data) : $.cookie(key, data, this.localStorage.getCookieExpireOptions());
        },

        check: (key: string): boolean => {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },

        getCookieExpireOptions: () => {
            const cookieExpire = { expires: 365, path: window.location.pathname };

            return cookieExpire;
        },
        clear: () => {
            if (store.enabled) {
                const allKeys = store.getAll();
                for (const key in allKeys) {
                    if (key !== "LANGUAGE") {
                        store.remove(key);
                    }
                }
            }
        },
        remove: (key: string) => {
            store.enabled ? store.remove(key) : $.cookie(key, undefined, this.localStorage.getCookieExpireOptions());
        },

        shiftStorage: (prefixKey: string, value: object, maxIndex: number) => {
            for (let i = 1; i < maxIndex - 1; i++) {
                const oldValue = siteInfrastructure.localStorage.get(`${indexManager.elements.currentURLCookieName}_${(i + 1)}`);
                siteInfrastructure.localStorage.set(`${indexManager.elements.currentURLCookieName}_${i}`, oldValue);
            }

            siteInfrastructure.localStorage.set(`${indexManager.elements.currentURLCookieName}_${(maxIndex - 1)}`, value);
        }
    };

    loadInfo = ($: JQueryStatic, id: string, handleOnSuccessLoading: Function) => {
        const $component = $(id),
            url = $component.attr('action-url');

        if (url === '' || typeof url === 'undefined') {
            return;
        }

        $.ajax({
            method: 'GET',
            url: url,
            cache: false,
            beforeSend: () => {
                $component.append("<div class='block-icon-small'></div>");
            },
            success: (responseHtml) => {
                $component.html(responseHtml);
                handleOnSuccessLoading();
            }
        });
    };

    showMessage = ($target: JQuery, result: Result) => {
        $target.empty();

        if (result.IsSuccessful) {
            this.showSuccess($target, result.Message);
        } else if (result.IsFailed) {
            this.showError($target, result.Message);
        } else if (result.IsWarning) {
            this.showWarn($target, result.Message);
        }
    };

    showSuccess = ($target: JQuery, message: string) => {
        $target.html("<div class='msg msg-success'>" + message + "</div>");
    };

    showError = ($target: JQuery, error: string) => {
        $target.html("<div class='msg msg-error'>" + error + "</div>");
    };

    showWarn = ($target: JQuery, warning: string) => {
        $target.html("<div class='msg msg-warning'>" + warning + "</div>");
    };

    logout = () => {
        let index = window;
        while (index !== index.parent) {
            index = index.parent;
        }

        const mainRootPath = $("#mainRootPath").val();
        const isInternalElement = (top.document.getElementById("IsInternal") as HTMLInputElement);
        const isInternal = isInternalElement ? isInternalElement.value : "0";
        index.location.href = `${mainRootPath}Login/Logout`;
        siteInfrastructure.localStorage.clear();

        if (isInternal === "1") {
            index.close();
        }
    };

    returnCurrentPage = () => {
        const data = (window.top.siteInfrastructure.localStorage || siteInfrastructure.localStorage).get("reloadCN88CustomerListCallBackUrl");

        if (data === undefined || !data.currentPage) {
            return;
        }

        window.top.$("#main").attr("src", data.currentPage);
    };
}


class IndexManager {
    urls: {
        mainRootPath: string,
        changeLanguage: string,
        getTotalOnlineMemberUrl: string,
        onlineList: string,
        liveChat: string,
        contactUs: string,
        reCheckin: string,
        changePassword: string,
        changeSecurityCode: string,
        onlineMembersPath: string,
        oldReportsPath: string,
        wagerMonitoringConsolePath: string,
        troubleshootingPath: string
    };
    elements: {
        mainMenu: JQuery,
        subMenu: JQuery,
        currentURLCookieName: string,
        subMenuItems: JQuery,
        mainFrame: JQuery,
        menuBar: JQuery,
        onlineUserId: object,
        clearClientData: string,
        isNotHeadLine: number,
        intervalToGetTotalOnlineMember: number,
        header: JQuery,
        content: JQuery,
        listMenu: JQuery,
        hamburgerContainer: JQuery,
        hamburgerIcon: JQuery,
        isDevice: boolean,
        pageTitle: JQuery,
        announcementPath: string,
        initMainFrameLink: string,
        problemAccountPath: string
    };
    currentState: {
        mainMenuId: string,
        isOpenPopup: boolean,
        isMainMenuClick: boolean,
        isScrolling: boolean,
        isClearActiveMenu: boolean,
        wagerMonitoringWindow: Window
    };
    resources: {
        changePassword: string,
        changeSecurityCode: string,
        mobileHeaderName: "mobileHeaderName"
    };
    topWindow: Window;

    init: (topWindow: object) => void;

    cacheElements: () => void;

    bindEvents: () => void;

    initForMobile: () => void;

    changeLanguage: (lang: string) => void;

    GetTotalOnlineMember: () => void;

    reloadPage: () => void;

    removeBookmarksInUrl: (url: string) => string;

    viewOnlineList: () => void;

    viewOldReport: () => void;

    viewTroubleshooting: () => void;

    viewWagerMonitoringConsole: () => void;

    mainMenuClick: (id: string) => void;

    subMenuClick: (id: string) => void;

    addHistory: (newEvent: object) => void;

    restoreUrl: () => void;

    reCheckin: () => void;

    activeMainMenu: (id: string) => void;

    closePopupAndReloadMainIframe: () => void;

    activeSubMenu: (id: string, mainMenuId: string) => void;

    calculateMainFrameHeight: () => void;

    closeMenuBarOnMobile: () => void;

    calculateMenuBarHeight: (subMenu) => void;

    clearAllActiveMenu: () => void;

    isMySettingsPage: (currentUrl: string) => boolean;

    setMobileHeaderName: (text) => void;

    calculateUserMobileHeight: () => void;

    hideUserMobileDropdown: () => void;

    locationReload: () => void;

    selector: {
        firstItemRedirect: string,
        mobileMenuDropdown: string
    };
}

class PopupManager {
    popupOptions: { preUrl: string }[];
    init: (jquery: JQueryStatic, window: Window) => void;
    open: (element: object) => void;
    close: () => void;
    revertLatestPopupTitle: () => void;
    setLatestPopupTitle: (title: string) => void;
}

class CodeResponse {
    Code: number;
    Message: string;
}

class ResponseError {
    status: number;
}

class Cryptor {
    CfsEnCode: (value: string) => string;
    SHA256: (value: string) => string;
}

const siteInfrastructure = new SiteInfrastructure();

window.top.cn88SuccessfulUpdatingCallback = siteInfrastructure.returnCurrentPage;

window.top.cn88CancelCallback = siteInfrastructure.returnCurrentPage;

window.top.cn88SaveAndNewCallback = function () {
    const mainiFrame: JQuery = window.top.$("#main");
    mainiFrame.attr("src", mainiFrame.attr("src"));
};