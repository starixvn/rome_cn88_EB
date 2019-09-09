"use strict";
var betTypes: any = betTypes;
var allBetTypeText: any = allBetTypeText;

class HouseWinLossReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $dateContainer: JQuery;
    $dateRangeContainer: JQuery;

    $internalLoginForm: JQuery;
    $internalCustId: JQuery;
    $internalLanguageKey: JQuery;
    $internalCustName: JQuery;
    $internalRoleId: JQuery;

    sportGroupMultiSelectionComponent: SportGroupMultiSelectionComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    ticketStatusSelectionComponent: SingleSelectionDropdownComponent;
    $buttonSportGroup: JQuery;

    constructor($: JQueryStatic,
        sportGroupSelectionComponent: SportGroupMultiSelectionComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        ticketStatusSelectionComponent: SingleSelectionDropdownComponent,
        toggleHandler: ToggleHandler) {
        this.$ = $;
        this.sportGroupMultiSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
        this.ticketStatusSelectionComponent = ticketStatusSelectionComponent;
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();
        this.$.unblockUI();
    }

    public cacheDOMElements() {
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$dateContainer = this.$("#date-container");
        this.$dateRangeContainer = this.$("#dateRangeContainer");

        this.$internalLoginForm = this.$("#internal-login-form");
        this.$internalCustId = this.$internalLoginForm.find("#custId");
        this.$internalLanguageKey = this.$internalLoginForm.find("#languageKey");
        this.$internalCustName = this.$internalLoginForm.find("#custName");
        this.$internalRoleId = this.$internalLoginForm.find("#roleId");
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$dateContainer.datepicker({
            allowInputToggle: true,
            minDate: this.$dateContainer.data("mindate"),
            defaultDate: this.$dateContainer.data("currentdate"),
            format: 'YYYY-MM-DD'
        });

        this.$("#icon-excel").on("click", this.onExportExcel);

        this.$("#ViewHistoryReport").click(() => {
            this.$("#IsHistoricReport").val(this.$("#IsHistoricReport").val() == "True" ? "False" : "True");
            this.$("#tab-main-content").remove();
            this.switchCalendarType();
        });
        this.$("#BackToCurrentReport").click(() => {
            this.$("#IsHistoricReport").val(this.$("#IsHistoricReport").val() == "True" ? "False" : "True");
            this.$("#tab-main-content").remove();
            this.switchCalendarType();
        });
        this.initMonthlyCalendar();

        var this$ = this.$;
        var thisInternalLogin = this.internalLogin;
        this.$(".lnk-internal").on("click touchend", function () {
            var elm = this$(this);
            thisInternalLogin({
                custId: elm.attr("data-cust-id"),
                languageKey: elm.attr("data-languageKey"),
                custName: elm.attr("data-cust-name"),
                roleId: elm.attr("data-role-id")
            });
        });
    }

    switchCalendarType = () => {
        if (this.$("#IsHistoricReport").val() == "True") {
            this.$("#GroupViewHistory").css("display", "none");
            this.$("#GroupBackToCurrent").css("display", "block");
            this.$('.from-date-box').css('display', 'none');
            this.$('.js-month-group').css('display', 'inline-block');
            this.$("#ticketStatusContainer").css("display", "none");
        }
        else {
            this.$("#ticketStatusContainer").css("display", "inline-block");
            this.$("#GroupViewHistory").css("display", "block");
            this.$("#GroupBackToCurrent").css("display", "none");
            this.$('.from-date-box').css('display', 'inline-block');
            this.$('.js-month-group').css('display', 'none');
        }
    }

    initMonthlyCalendar = () => {
        var maxDate = new Date(this.$dateRangeContainer.data("maxdate"));
        var fromDate = new Date(this.$dateRangeContainer.data("fromdate"));
        var toDate = new Date(this.$dateRangeContainer.data("todate"));
        this.$("#monPickr1").monPickr({
            today: fromDate,
            maxDate: maxDate,
            showDateRange: true,
            fromDayId: "fromDay1",
            toDayId: "toDay1",
            shortMonthsInYear: this.$dateRangeContainer.data("longmonths-inyear").split(","),
            longMonthsInYear: this.$dateRangeContainer.data("longmonths-inyear").split(","),
        });


        this.$("#monPickr1").monPickr("setMonth", fromDate);
        this.$("#monPickr1").monPickr("setFromDay", fromDate.getDate());
        this.$("#monPickr1").monPickr("setToDay", toDate.getDate());

        this.switchCalendarType();
    }

    UpdateFromDateToDate = () => {
        if (this.$("#IsHistoricReport").val() == "True") {
            this.$("#fromDate").val(this.$("#monPickr1").monPickr("getFromDate", "MM/dd/yyyy"));
            this.$("#toDate").val(this.$("#monPickr1").monPickr("getToDate", "MM/dd/yyyy"));
        } else {
            this.$("#fromDate").val(this.$("#date").val());
            this.$("#toDate").val(this.$("#date").val());
        }
    }

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    onReportSubmit = (e: any) => {
        let sportGroups = this.sportGroupMultiSelectionComponent.getSelectedItems();
        if (sportGroups.length <= 0) {
            this.$buttonSportGroup.addClass("error");
            e.preventDefault();

            return;
        }

        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
        this.ticketStatusSelectionComponent.updateSelectedValue();

        this.UpdateFromDateToDate();
    };

    renderComponents(): void {        
        this.sportGroupMultiSelectionComponent.render(new SportGroupMultiSelectionOption("dropdown-sportgroup", true, "sportgroup-ids", () => { this.onchangeSportGroup(); }));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.ticketStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-ticketstatus", "UserSelectedTicketStatusId"));
        this.$buttonSportGroup = this.$("#dropdown-sportgroup").parent().find(".multiselect.btn");
    }

    onchangeSportGroup(): void {
        let sportGroups = this.sportGroupMultiSelectionComponent.getSelectedItems();
        if (this.betTypeSelectionComponent.$dropdown != null) {
            this.betTypeSelectionComponent.$dropdown.empty();
            this.betTypeSelectionComponent.$dropdown.append('<option value = 0>' + allBetTypeText + '</option>');

            const exists = [];
            for (const betType of betTypes) {
                if (sportGroups.indexOf(betType.SportTypeGroup.toString()) != -1) {
                    var key = betType.Value;

                    if (exists[key]) {
                        continue;
                    }

                    exists[key] = true;
                    this.betTypeSelectionComponent.$dropdown.append(`<option value="${betType.Value}">${betType.Text}</option>`);
                }
            }

            this.betTypeSelectionComponent.$dropdown.multiselect('destroy')
            this.betTypeSelectionComponent.$dropdown.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false
            });
        }
    };

    internalLogin = (options) => {
        this.$internalCustId.val(options.custId);
        this.$internalLanguageKey.val(options.languageKey);
        this.$internalCustName.val(options.custName);
        this.$internalRoleId.val(options.roleId);
        this.$internalLoginForm.submit();
    };
}

$(function () {
    const houseWinLossReport = new HouseWinLossReport(
        $,
        new SportGroupMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ToggleHandler($));

    houseWinLossReport.init();
})
