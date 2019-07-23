"use strict";
class SubAccount {
    $: JQueryStatic;
    elements = {
        message: null,
        formUpdate: null,
        name: null,
        smaUsername: null,
        subAccountUsername: null,
        totalbets: null,
        acViewOnly: null,
        acEditView: null,
        reports: null
    };
    resources = {
        invalidSMASubAccount: null,
        noAuthorized: null
    };
    updateData = {
        custId: 0,
        subAccId: 0,
        username: "",
        pageAccess: ""
    };
    message: Message;
    forbiddenStatusCode = 403;

    constructor($: JQueryStatic) {
        this.$ = $;
        this.message = new Message();
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
    }

    cacheElements(): void {
        this.elements.message = this.$("#message");
        this.elements.formUpdate = this.$("#formUpdate");
        this.elements.name = this.$("#name").focus();
        this.elements.smaUsername = this.$("#smaUsername");
        this.elements.subAccountUsername = this.$("#subAccountUsername");
        this.elements.totalbets = this.$("#totalbets");
        this.elements.acViewOnly = this.$("#acViewOnly");
        this.elements.acEditView = this.$("#acEditView");
        this.elements.reports = this.$("#reports");

        this.resources.invalidSMASubAccount = this.$("#InvalidSMASubAccount").val();
        this.resources.noAuthorized = this.$("#NoAuthorized").val();
    }

    bindEvents(): void {
        this.elements.acViewOnly.change(
            (e: JQueryEventObject) => this.acViewOnlyChanged(this.$(e.target))
        );

        this.elements.acEditView.change(
            (e: JQueryEventObject) => this.acEditViewChanged(this.$(e.target))
        );

        const formSearch = this.$("#formSearch");
        formSearch.submit((e: JQueryEventObject) => {
            e.preventDefault();
            this.formSearchSubmitted(formSearch);
        });

        this.elements.formUpdate.submit((e: JQueryEventObject) => {
            e.preventDefault();
            this.isInvalidUpdateData() || this.updateSubAccountPermissions();
        });
    }

    acViewOnlyChanged(checkbox: JQuery): void {
        if (!checkbox.is(":checked")) {
            this.elements.acEditView.prop("checked", false);
        }
    }

    acEditViewChanged(checkbox: JQuery): void {
        if (checkbox.is(":checked")) {
            this.elements.acViewOnly.prop("checked", true);
        }
    }

    formSearchSubmitted(formSearch: JQuery): void {
        const name = this.$.trim(this.elements.name.val());
        if (name.length === 0) {
            this.showInvalidSMASubAccountMessage();
            return;
        }

        this.$.blockUI();
        const url = `${formSearch.attr("action")}?name=${name}`;
        $.ajax({
            type: formSearch.attr("method"),
            url: url,
            dataType: "json",
            success: (data: any) => this.showSubAccountInfoOnUI(data),
            complete: this.$.unblockUI
        });
    }

    showInvalidSMASubAccountMessage(): void {
        this.message.showError(this.elements.message, this.resources.invalidSMASubAccount);
        this.elements.formUpdate.addClass("hide");
        this.elements.name.focus();
    }

    showSubAccountInfoOnUI(data: any): void {
        this.updateData.custId = data.CustId;
        this.updateData.subAccId = data.SubAccId;
        this.updateData.username = data.UserName;

        if (this.isInvalidUpdateData()) {
            this.showInvalidSMASubAccountMessage();
        } else {
            this.message.hideMessage(this.elements.message);
            this.elements.formUpdate.removeClass("hide");

            this.elements.smaUsername.html(`${data.SMAUsername} (${data.SMANickname || ""})`);
            this.elements.subAccountUsername.html(`${data.UserName} (${data.NickName || ""})`);
            this.elements.totalbets.prop("checked", data.Permissions.ViewTotalBets);
            this.elements.acViewOnly.prop("checked", data.Permissions.ViewMemberInfo);
            this.elements.acEditView.prop("checked", data.Permissions.UpdateAccount);
            this.elements.reports.prop("checked", data.Permissions.ViewReports);
        }
    }

    isInvalidUpdateData(): boolean {
        return this.updateData.custId === 0 || this.updateData.subAccId === 0;
    }

    updateSubAccountPermissions(): void {
        this.$.blockUI();
        this.updateData.pageAccess = this.getPageAccess();
        this.$.ajax({
            type: this.elements.formUpdate.attr("method"),
            url: this.elements.formUpdate.attr("action"),
            headers: this.setupAntiXSRF(),
            data: this.updateData,
            dataType: "json",
            success: (data) => {
                const result = new Result(data);
                const messageHandler = result.IsSuccessful ? this.message.showSuccess : this.message.showError;
                messageHandler(this.elements.message, result.Message);
            },
            error: (jqXHR: JQueryXHR) => {
                if (jqXHR.status === this.forbiddenStatusCode) {
                    this.message.showError(this.elements.message, this.resources.noAuthorized);
                }
            },
            complete: this.$.unblockUI
        });
    }

    getPageAccess(): string {
        const permissions: string[] = [];
        const pageAccesses: JQuery[] = [
            this.elements.totalbets,
            this.elements.acViewOnly,
            this.elements.acEditView,
            this.elements.reports];

        for (const item of pageAccesses) {
            if (item.is(":checked")) {
                permissions.push(item.val());
            }
        }

        return permissions.join(",");
    }

    setupAntiXSRF() {
        const token = this.$("input[name=__RequestVerificationToken]").val();
        const headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}

$(() => {
    new SubAccount($).init();
});