declare var numeral: any;


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
    main: any;
    siteInfrastructure: any;
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

    constructor(data?: any) {
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

        set: (key: string, value: any) => {
            const data = JSON.stringify(value);
            store.enabled ? store.set(key, data) : $.cookie(key, data, this.localStorage.getCookieExpireOptions());
        },

        check: (key: string): boolean => {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },

        getCookieExpireOptions: () => {
            const cookieExpire: any = { expires: 365, path: window.location.pathname };

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

        shiftStorage: (prefixKey: string, value: any, maxIndex: number) => {
            for (let i = 1; i < maxIndex - 1; i++) {
                const oldValue = siteInfrastructure.localStorage.get(`${indexManager.elements.currentURLCookieName}_${(i + 1)}`);
                siteInfrastructure.localStorage.set(`${indexManager.elements.currentURLCookieName}_${i}`, oldValue);
            }

            siteInfrastructure.localStorage.set(`${indexManager.elements.currentURLCookieName}_${(maxIndex - 1)}`, value);
        }
    };

    loadInfo = ($: any, id: string, handleOnSuccessLoading: any) => {
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

    showMessage = ($target: any, result: Result) => {
        $target.empty();

        if (result.IsSuccessful) {
            this.showSuccess($target, result.Message);
        } else if (result.IsFailed) {
            this.showError($target, result.Message);
        } else if (result.IsWarning) {
            this.showWarn($target, result.Message);
        }
    };

    showSuccess = ($target: any, message: string) => {
        $target.html("<div class='msg msg-success'>" + message + "</div>");
    };

    showError = ($target: any, error: string) => {
        $target.html("<div class='msg msg-error'>" + error + "</div>");
    };

    showWarn = ($target: any, warning: string) => {
        $target.html("<div class='msg msg-warning'>" + warning + "</div>");
    };

    logout = () => {
        let index: any = window;
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

const siteInfrastructure = new SiteInfrastructure();

window.top.cn88SuccessfulUpdatingCallback = siteInfrastructure.returnCurrentPage;

window.top.cn88CancelCallback = siteInfrastructure.returnCurrentPage;

window.top.cn88SaveAndNewCallback = function () {
    const mainiFrame: JQuery = window.top.$("#main");
    mainiFrame.attr("src", mainiFrame.attr("src"));
};