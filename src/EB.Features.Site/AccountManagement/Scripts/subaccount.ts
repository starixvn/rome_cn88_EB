"use strict";
class SubAccount {
    $: JQueryStatic;
    elements: any = {
        message: null,
        formSearch: null,
        formUpdate: null,
        name: null,
        smaUsername: null,
        subAccountUsername: null,
        totalbets: null,
        acViewOnly: null,
        acEditView: null,
        reports: null,
        buttonSave: null
    };
    resources: any = {
        invalidSMASubAccount: null,
        noAuthorized: null
    };
    updateData: any = {
        custId: 0,
        subAccId: 0,
        username: "",
        pageAccess: ""
    };
    message: Message;

    constructor($: JQueryStatic) {
        this.$ = $;
        this.message = new Message();
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
    }

    private cacheElements(): void {
        this.elements.message = this.$("#message");
        this.elements.formSearch = this.$("#formSearch");
        this.elements.formUpdate = this.$("#formUpdate");
        this.elements.name = this.$("#name").focus();
        this.elements.smaUsername = this.$("#smaUsername");
        this.elements.subAccountUsername = this.$("#subAccountUsername");
        this.elements.totalbets = this.$("#totalbets");
        this.elements.acViewOnly = this.$("#acViewOnly");
        this.elements.acEditView = this.$("#acEditView");
        this.elements.reports = this.$("#reports");
        this.elements.buttonSave = this.$("#buttonSave");

        this.resources.invalidSMASubAccount = this.$("#InvalidSMASubAccount").val();
        this.resources.noAuthorized = this.$("#NoAuthorized").val();
    }

    private bindEvents(): void {
        this.elements.acViewOnly.change((e: JQueryEventObject) => {
            const checkbox = this.$(e.target);
            if (!checkbox.is(":checked")) {
                this.elements.acEditView.prop("checked", false);
            }
        });

        this.elements.acEditView.change((e: JQueryEventObject) => {
            const checkbox = this.$(e.target);
            if (checkbox.is(":checked")) {
                this.elements.acViewOnly.prop("checked", true);
            }
        });

        this.elements.formSearch.submit((e: JQueryEventObject) => {
            e.preventDefault();

            const name = this.$.trim(this.elements.name.val());
            if (name.length === 0) {
                this.message.showError(this.elements.message, this.resources.invalidSMASubAccount);
                return;
            }

            this.$.blockUI();
            const url: string = `${this.elements.formSearch.attr("action")}?name=${name}`;
            $.ajax({
                type: this.elements.formSearch.attr("method"),
                url: url,
                dataType: "json",
                success: (data: any) => {
                    this.updateData = { custId: data.CustId, subAccId: data.SubAccId, username: data.UserName };
                    if (this.isInvalidUpdateData()) {
                        this.message.showError(this.elements.message, this.resources.invalidSMASubAccount);
                        this.elements.formUpdate.addClass("hide");
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
                },
                complete: this.$.unblockUI
            });
        });

        this.elements.formUpdate.submit((e: JQueryEventObject) => {
            e.preventDefault();

            if (this.isInvalidUpdateData()) {
                return;
            }

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
                error: _ => this.message.showError(this.elements.message, this.resources.noAuthorized),
                complete: this.$.unblockUI
            });
        });
    }

    private isInvalidUpdateData(): boolean {
        return this.updateData.custId === 0 || this.updateData.subAccId === 0;
    }

    private getPageAccess(): string {
        let permissions: string[] = [];
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

    private setupAntiXSRF() {
        const token = this.$("input[name=__RequestVerificationToken]").val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}

$(() => {
    new SubAccount($).init();
});