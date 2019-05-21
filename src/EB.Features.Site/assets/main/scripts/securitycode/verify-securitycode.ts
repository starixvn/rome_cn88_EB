"use strict";
declare var Sha1: any;
declare var Sha256: any;

class VerifySecurityCode {
    $: JQueryStatic;
    top: Window;
    resources: any;
    elements: any;

    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        let self: any = this;
        this.cacheElements();

        this.$("#formSecurityCode").submit(function (e: any): void {
            e.preventDefault();
            self.verifySecurityCode(self.$(this));
        });

        this.elements.securityCode.focus();
        self.top.popupManager.setLatestPopupTitle(this.$("#securityCodeTitle").val());
    }

    cacheElements(): void {
        this.resources = {
            securityCodeEmpty: this.$("#alert-securitycode-empty").val()
        };

        this.elements = {
            securityCode: this.$("#securityCode"),
            messsageContainer: this.$("#message-container"),
            salt: this.$("#securitySalt").val(),
            token: this.$("#securitycodeToken").val()
        };
    }

    verifySecurityCode(form: JQuery): void {
        this.$.blockUI();

        let $securityCode: JQuery = this.elements.securityCode;
        let securityCodeValue: string = $securityCode.val();

        if (securityCodeValue.length === 0) {
            this.$.unblockUI();
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.securityCodeEmpty);
            $securityCode.focus();

            return;
        }

        securityCodeValue = securityCodeValue.hash(this.elements.salt, this.elements.token);
        this.submitForm(form.attr("method"), form.attr("action"), securityCodeValue);
    }

    submitForm(method, url, securityCode: string): void {
        let self: any = this;

        this.$.ajax({
            type: method,
            url: url,
            headers: this.setupAntiXSRF(),
            data: { securityCode: securityCode },
            dataType: "json",
            success: function (data: any) {
                if (data.Code === 0) {
                    self.elements.securityCode.attr("disabled", "disabled");
                    self.$("#btnSubmit").remove();
                    self.top.popupManager.revertLatestPopupTitle();
                    self.afterVerifed();
                }
                else if (data.Code === 2) { // account is closed 
                    alert(data.Message);
                    siteInfrastructure.logout();
                }
                else {
                    siteInfrastructure.showError(self.elements.messsageContainer, data.Message);
                    self.elements.securityCode.focus();
                }
            },
            error: function (xhr: any) {
                if (xhr.status === 401) {
                    alert((<HTMLInputElement>document.getElementById("errorMessage")).value);
                    siteInfrastructure.logout();
                }
            },
            complete: self.$.unblockUI
        });
    }

    setupAntiXSRF(): any {
        let token = this.$("input[name=__RequestVerificationToken]").val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }

    afterVerifed(): void {
        let redirectUrl = this.getUrlVars()["redirectUrl"];

        if (redirectUrl) {
            redirectUrl = decodeURIComponent(redirectUrl);
        } else {
            redirectUrl = this.$("#mainRootPath").val();
        }

        window.location.href = redirectUrl;
    }

    getUrlVars() {
        let hash;
        let vars = [];
        let url = window.location.href;
        let hashes = url.slice(url.indexOf("?") + 1).split("&");

        for (let i = 0, max = hashes.length; i < max; i++) {
            hash = hashes[i].split("=");
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }

        return vars;
    }
}

$(function (): void {
    const verifySecurityCode: VerifySecurityCode = new VerifySecurityCode($, top);
    verifySecurityCode.init();
});