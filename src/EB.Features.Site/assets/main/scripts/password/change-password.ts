"use strict";

class ChangePassword {
    $: JQueryStatic;
    top: Window;
    resources: {
        pleaseEnterOldPassword: string,
        passwordCannotEmpty: string,
        confirmPasswordNotCorret: string,
        newPassMustbeDiffOldPass: string,
        validLengthPassword: string
    };
    elements: {
        currentPassword: JQuery,
        newPassword: JQuery,
        confirmPassword: JQuery,
        messsageContainer: JQuery,
        hashKey: string,
        isResetMemberPassword: boolean
    };

    minLenghtPassword = 2;
    maxLenghtPassword = 50;
    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        this.cacheElements();

        this.$("#formPassword").submit(() => {
            this.$.blockUI();

            const $currentPassword: JQuery = this.elements.currentPassword;
            const $newPassword: JQuery = this.elements.newPassword;
            const $confirmPassword: JQuery = this.elements.confirmPassword;

            if (!this.validatePasswords($currentPassword, $newPassword, $confirmPassword, this.elements.isResetMemberPassword)) {
                this.$.unblockUI();
                return false;
            }

            if ($currentPassword.length > 0) {
                const encryptedPwd = window.CryptoJS.CfsEnCode($currentPassword.val());
                const hashPwd = window.CryptoJS.SHA256(encryptedPwd);
                $currentPassword.val(hashPwd);
            }

            return true;
        });

        this.$("#buttonCancel").click(() => {
            if (self.$("#mainRootPath").length > 0) {
                siteInfrastructure.logout();
            } else {
                this.top.popupManager.close();
            }
        });

        const focusField: JQuery = this.elements.currentPassword.length > 0
            ? this.elements.currentPassword
            : this.elements.newPassword;
        focusField.focus();
    }

    cacheElements(): void {
        this.resources = {
            pleaseEnterOldPassword: this.$("#please-enter-oldPassword").val(),
            passwordCannotEmpty: this.$("#password-cannot-empty").val(),
            confirmPasswordNotCorret: this.$("#not-correct-confirm-password").val(),
            newPassMustbeDiffOldPass: this.$("#newPassword-must-be-diff-old").val(),
            validLengthPassword: this.$("#valid-length-password").val()
        };

        const $memberHiddenField: JQuery = this.$("#isResetMemberPassword");
        this.elements = {
            currentPassword: this.$("#currentPassword"),
            newPassword: this.$("#newPassword"),
            confirmPassword: this.$("#confirmPassword"),
            messsageContainer: this.$("#message-container"),
            hashKey: this.$("#hashKey").val(),
            isResetMemberPassword: $memberHiddenField.length > 0 && $memberHiddenField.val() === "1"
        };
    }

    validatePasswords(currentPassword: JQuery, newPassword: JQuery, confirmPassword: JQuery, isResetMemberPassword: boolean): boolean {
        const isChangePasswordPage: boolean = currentPassword.length > 0;
        const currentPasswordValue: string = isChangePasswordPage ? currentPassword.val() : "";
        const newPasswordValue: string = newPassword.val();
        const confirmPasswordValue: string = confirmPassword.val();

        if (isChangePasswordPage && currentPasswordValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.pleaseEnterOldPassword);
            currentPassword.focus();

            return false;
        }

        if (newPasswordValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.passwordCannotEmpty);
            newPassword.focus();

            return false;
        }

        if (newPasswordValue.length < this.minLenghtPassword || newPasswordValue.length > this.maxLenghtPassword) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.validLengthPassword);
            newPassword.focus();

            return false;
        }

        if (newPasswordValue !== confirmPasswordValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.confirmPasswordNotCorret);
            confirmPassword.focus();

            return false;
        }

        if (isChangePasswordPage && newPasswordValue === currentPasswordValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.newPassMustbeDiffOldPass);
            newPassword.focus();

            return false;
        }

        return true;
    }
}

$(function (): void {
    const changePassword: ChangePassword = new ChangePassword($, top);
    changePassword.init();
});
