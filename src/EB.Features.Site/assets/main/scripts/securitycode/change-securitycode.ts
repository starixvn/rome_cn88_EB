"use strict";
class ChangeSecurityCode {
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

        this.$("#formSecurityCode").submit(function (): boolean {
            self.$.blockUI();
            let $oldSecurityCode: JQuery = self.elements.oldSecurityCode;
            let $securityCode: JQuery = self.elements.securityCode;
            let $confirmSecurityCode: JQuery = self.elements.confirmSecurityCode;

            if (!self.validateSecurityCodes($oldSecurityCode, $securityCode, $confirmSecurityCode)) {
                self.$.unblockUI();
                return false;
            }

            let hashKey: string = self.elements.hashKey;

            if ($oldSecurityCode.length > 0) {
                let oldSecCode: string = $oldSecurityCode.val().hash(self.elements.salt, self.elements.token);
                $oldSecurityCode.val(oldSecCode);
            }

            let secCode: string = $securityCode.val().encrypt(hashKey);
            $securityCode.val(secCode);
            $confirmSecurityCode.val(secCode);

            return true;
        });

        this.$("#buttonCancel").click(function (): void {
            if (self.$("#mainRootPath").length > 0) {
                siteInfrastructure.logout();
            } else {
                self.top.popupManager.close();
            }
        });

        let focusField: JQuery = this.elements.oldSecurityCode.length > 0
            ? this.elements.oldSecurityCode
            : this.elements.securityCode;
        focusField.focus();
    }

    cacheElements(): void {
        this.resources = {
            oldSecurityCodeEmpty: this.$("#alert-old-securitycode-empty").val(),
            securityCodeEmpty: this.$("#alert-securitycode-empty").val(),
            securityCodeWrongRule: this.$("#alert-securitycode-wrong-rule").val(),
            errorSecurityCodeConfirm: this.$("#alert-error-securitycode-confirm").val(),
            sameSecurityCode: this.$("#alert-same-securitycode").val()
        };

        this.elements = {
            oldSecurityCode: this.$("#oldSecurityCode"),
            securityCode: this.$("#securityCode"),
            confirmSecurityCode: this.$("#confirmSecurityCode"),
            messsageContainer: this.$("#message-container"),
            hashKey: this.$("#hashKey").val(),
            salt: this.$("#securitySalt").val(),
            token: this.$("#securitycodeToken").val()
        };
    }

    validateSecurityCodes(oldSecurityCode: JQuery, securityCode: JQuery, confirmSecurityCode: JQuery): boolean {
        let isChangeSecurityCodePage: boolean = oldSecurityCode.length > 0;
        let oldSecurityCodeValue: string = isChangeSecurityCodePage ? oldSecurityCode.val() : "";
        let securityCodeValue: string = securityCode.val();
        let confirmSecurityCodeValue: string = confirmSecurityCode.val();

        if (isChangeSecurityCodePage && oldSecurityCodeValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.oldSecurityCodeEmpty);
            oldSecurityCode.focus();

            return false;
        }

        if (securityCodeValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.securityCodeEmpty);
            securityCode.focus();

            return false;
        }

        if (!this.isValidSecurityCode(securityCodeValue)) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.securityCodeWrongRule);
            securityCode.focus();

            return false;
        }

        if (securityCodeValue !== confirmSecurityCodeValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.errorSecurityCodeConfirm);
            confirmSecurityCode.focus();

            return false;
        }

        if (isChangeSecurityCodePage && securityCodeValue === oldSecurityCodeValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.sameSecurityCode);
            securityCode.focus();

            return false;
        }

        return true;
    }

    isValidSecurityCode(securityCode: string): boolean {
        let securityCodeLength: number = securityCode ? securityCode.length : 0;

        if (securityCodeLength !== 6) {
            return false;
        }

        let regex: RegExp = /^([0-9])(?!\1+$)[0-9]+$/;
        let result: boolean = securityCode.search(regex) !== -1;

        if (result) {
            let isForwardRun: boolean = true;
            let isBackwardRun: boolean = true;
            let forward: number = 0;
            let backward: number = 0;
            forward = backward = parseInt(securityCode.charAt(0));

            for (let i: number = 1; i < securityCodeLength; i++) {
                let current: number = parseInt(securityCode.charAt(i));

                if (++forward !== current) {
                    isForwardRun = false;
                }

                if (--backward !== current) {
                    isBackwardRun = false;
                }
            }

            result = !(isForwardRun || isBackwardRun);
        }

        return result;
    }
}

$(function (): void {
    const changeSecurityCode: ChangeSecurityCode = new ChangeSecurityCode($, top);
    changeSecurityCode.init();
});