"use strict";
class HouseWinLossReport extends ReportBase{
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $dateRangeContainer: JQuery;

    viewerSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    ticketStatusSelectionComponent: SingleSelectionDropdownComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    $buttonSportGroup: JQuery;
    sortComponent: SortComponent;
    datePickerComponent: DatePickerComponent;
    linkBuilderComponent: LinkBuilderComponent;
    $custLevelId: JQuery;
    $tableReport: JQuery;

    $loadLegendURL: JQuery;
    topWindow: JQuery;
    $isHistoryReport: JQuery;
    $internalLoginForm: JQuery;

    constructor($: JQueryStatic,
        topWindow: any,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SportGroupMultiSelectionComponent,
        viewerSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        ticketStatusSelectionComponent: SingleSelectionDropdownComponent,
        toggleHandler: ToggleHandler,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        datePickerComponent: DatePickerComponent) {
        super($, sportGroupSelectionComponent, betTypeSelectionComponent);
        this.$ = $;
        this.topWindow = topWindow;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
        this.ticketStatusSelectionComponent = ticketStatusSelectionComponent;
        this.viewerSelectionComponent = viewerSelectionComponent;
        this.sortComponent = sortComponent;
        this.linkBuilderComponent = linkBuilderComponent;
        this.datePickerComponent = datePickerComponent;
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();
        this.$.unblockUI();
    }

    public cacheDOMElements() {
        this.$custLevelId = this.$("#RoleId");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$dateRangeContainer = this.$("#dateRangeContainer");

        this.$tableReport = this.$("#tbl-report");
        this.$isHistoryReport = this.$("#IsHistoricReport");
        this.$loadLegendURL = this.$("#LoadLegendURL");        
        this.$internalLoginForm = this.$("#internal-login-form");
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.datePickerComponent.render(new DatePickerOption($, "date-container"));

        this.$("#icon-excel").on("click", this.onExportExcel);
        
        this.$("#ViewHistoryReport").click(() => {
            this.changeReportMode();
        });
        this.$("#BackToCurrentReport").click(() => {
            this.changeReportMode();
        });
        this.$("#icon-information").on("click", this.showLegend);
        this.switchCalendarType();
        const viewBetlist = this.viewbetlist;
        const viewReport = this.viewReport;
        const this$ = this.$;
        this.$(".lnk-betlist").on("click touchend", function () {
            const elm = this$(this);
            viewBetlist({
                query: elm.attr("data-query")
            });
        });

        this.$(".lnk-report").on("click touchend", function () {
            const elm = this$(this);
            viewReport({
                query: elm.attr("data-query"),
                custId: elm.attr("data-cust-id"),
                custName: elm.attr("data-cust-name"),
                roleId: elm.attr("data-role-id")
            });
        });
    }
    changeReportMode = () => {
        this.$isHistoryReport.val(this.$isHistoryReport.val() === "True" ? "False" : "True");
        this.$("#tab-main-content").remove();
        this.switchCalendarType();
    }

    viewbetlist = (options) => {        
        this.$("#AfterLogInQuery").val(options.query);
        const url = this.$internalLoginForm.attr("action") + "?" + this.$internalLoginForm.serialize();
        window.open(url, "internallogin", "scrollbars=1,fullscreen=yes,top=290");
    }

    viewReport = (options) => {
        this.$("#AfterLogInQuery").val(options.query);
        this.$internalLoginForm.find("#custId").val(options.custId);
        this.$internalLoginForm.find("#custName").val(options.custName);
        this.$internalLoginForm.find("#roleId").val(options.roleId);
        const url = this.$internalLoginForm.attr("action") + "?" + this.$internalLoginForm.serialize();
        window.open(url, "scrollbars=1,fullscreen=yes,top=290");
    }

    showLegend = () => {
        this.topWindow.popupManager.init(this.$, top);
        this.topWindow.popupManager.open({
            title: "",
            url: this.$loadLegendURL.val(),
            top: 300,
            width: 600,
            height: 100
        });
    }

    switchCalendarType = () => {
        const inlineBlock = "inline-block";
        if (this.$isHistoryReport.val() === "True") {
            this.$("#GroupViewHistory").css("display", "none");
            this.$("#GroupBackToCurrent").css("display", "block");
            this.$('.date-box').css('display', 'none');
            this.$dateRangeContainer.css('display', inlineBlock);
            this.$("#ticketStatusContainer").css("display", "none");
        }
        else {
            this.$("#ticketStatusContainer").css("display", inlineBlock);
            this.$("#GroupViewHistory").css("display", "block");
            this.$("#GroupBackToCurrent").css("display", "none");
            this.$('.date-box').css('display', inlineBlock);
            this.$dateRangeContainer.css('display', 'none');
        }
    }
    
    UpdateFromDateToDate = () => {
        if (this.$isHistoryReport.val() === "False") {
            this.$("#fromDate").val(this.$("#date").val());
            this.$("#toDate").val(this.$("#date").val());
        } 
    }

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    }

    onReportSubmit = (e: any) => {
        this.$.blockUI();
        const sportGroups = this.sportGroupMultiSelectionComponent.getSelectedItems();
        this.viewerSelectionComponent.updateSelectedValue();
        if (sportGroups.length <= 0) {
            this.$buttonSportGroup.addClass("error");
            e.preventDefault();
            this.$.unblockUI();
            return;
        }
        const viewerId = this.viewerSelectionComponent.getSelectedItem();
        if (viewerId === "0") {
            this.$("#RoleId").val("0"); //Headline
        } else {
            this.$("#RoleId").val("4"); //SMA
        }
        
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
        this.ticketStatusSelectionComponent.updateSelectedValue();        
        this.UpdateFromDateToDate();
    }

    renderComponents(): void {
        super.renderComponents();
        this.dateRangePickerComponent.render();
        this.viewerSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-viewer", "UserSelectedViewer", () => { this.onChangeUserSelectedViewer(); }));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.ticketStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-ticketstatus", "UserSelectedTicketStatusId"));
        this.$buttonSportGroup = this.$("#dropdown-sportgroup").parent().find(".multiselect.btn");
        const headers = this.$custLevelId.val() === "4"
            ? { 0: { sorter: "text" }, 1: { sorter: "text" } }
            : { 0: { sorter: "text" } };
            this.sortComponent.render(
                new SortOption(
                    this.$tableReport,
                    parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
                    true,
                    this.sortComponent.afterSort,
                    headers,
                    this.$reportForm));

        this.linkBuilderComponent.render();
    }

    onChangeUserSelectedViewer(): void {
        const value = this.viewerSelectionComponent.$dropdown.val();
        this.$custLevelId.val(value === "0" ? "0" : "4");
    }
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const houseWinLossReport = new HouseWinLossReport(
        $,
        window.top,
        new DateRangePickerComponent($),
        new SportGroupMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ToggleHandler($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent,
        new DatePickerComponent());

    houseWinLossReport.init();
});
