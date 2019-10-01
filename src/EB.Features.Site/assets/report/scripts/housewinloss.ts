"use strict";
var betTypes: any = betTypes;
var allBetTypeText: any = allBetTypeText;

class HouseWinLossReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $dateRangeContainer: JQuery;

    sportGroupMultiSelectionComponent: SportGroupMultiSelectionComponent;
    viewerSelectionComponent: SingleSelectionDropdownComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
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
        this.$ = $;
        this.topWindow = topWindow;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sportGroupMultiSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
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

        this.$loadLegendURL = this.$("#LoadLegendURL");        
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.datePickerComponent.render(new DatePickerOption($, "date-container"));

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
        this.$("#icon-information").on("click", this.showLegend);
        this.switchCalendarType();
        var this$ = this.$;
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
    };

    switchCalendarType = () => {
        if (this.$("#IsHistoricReport").val() == "True") {
            this.$("#GroupViewHistory").css("display", "none");
            this.$("#GroupBackToCurrent").css("display", "block");
            this.$('.date-box').css('display', 'none');
            this.$dateRangeContainer.css('display', 'inline-block');
            this.$("#ticketStatusContainer").css("display", "none");
        }
        else {
            this.$("#ticketStatusContainer").css("display", "inline-block");
            this.$("#GroupViewHistory").css("display", "block");
            this.$("#GroupBackToCurrent").css("display", "none");
            this.$('.date-box').css('display', 'inline-block');
            this.$dateRangeContainer.css('display', 'none');
        }
    }
    
    UpdateFromDateToDate = () => {
        if (this.$("#IsHistoricReport").val() == "False") {
            this.$("#fromDate").val(this.$("#date").val());
            this.$("#toDate").val(this.$("#date").val());
        } 
    }

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    onReportSubmit = (e: any) => {
        this.$.blockUI();
        let sportGroups = this.sportGroupMultiSelectionComponent.getSelectedItems();
        this.viewerSelectionComponent.updateSelectedValue();
        if (sportGroups.length <= 0) {
            this.$buttonSportGroup.addClass("error");
            e.preventDefault();
            this.$.unblockUI();
            return;
        }
        var viewerId = this.viewerSelectionComponent.getSelectedItem();
        if (viewerId === "0") {
            this.$("#RoleId").val("0"); //Headline
        } else {
            this.$("#RoleId").val("4"); //SMA
        }
        
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
        this.ticketStatusSelectionComponent.updateSelectedValue();        
        this.UpdateFromDateToDate();
    };

    renderComponents(): void {
        this.dateRangePickerComponent.render();
        this.sportGroupMultiSelectionComponent.render(new SportGroupMultiSelectionOption("dropdown-sportgroup", true, "sportgroup-ids", () => { this.onchangeSportGroup(); }));
        this.viewerSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-viewer", "UserSelectedViewer", () => { this.onChangeUserSelectedViewer() }));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
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
        let value = this.viewerSelectionComponent.$dropdown.val();
        this.$custLevelId.val(value == 0 ? "0" : "4");
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
})
