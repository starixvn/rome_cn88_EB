"use strict";
var betTypes: any = betTypes;
var allBetTypeText: any = allBetTypeText;

class MatchWinLossReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    sportGroupMultiSelectionComponent: SportGroupMultiSelectionComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    pagerComponent: ClientPagerComponent;
    datePickerComponent: DatePickerComponent;
    $buttonSportGroup: JQuery;

    constructor($: JQueryStatic,
        sportGroupSelectionComponent: SportGroupMultiSelectionComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: ClientPagerComponent,
        toggleHandler: ToggleHandler,
        datePickerComponent: DatePickerComponent) {
        this.$ = $;
        this.sportGroupMultiSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
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
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.datePickerComponent.render(new DatePickerOption($, "date-container"));

        $(".group").click(function (): void {
            let $this: JQuery = $(this);
            let icon: JQuery = $this.find(".group-name-wrapper").children().first();
            let children: any = $this.siblings(`.childrow-${$this.data("leagueid")}`);

            if (icon.hasClass("icon-chevron-down")) {
                icon.removeClass().addClass("icon-chevron-right");
                children.addClass("hide");
            } else {
                icon.removeClass().addClass("icon-chevron-down");
                children.removeClass("hide");
            }
        });

        this.$("#icon-excel").on("click", this.onExportExcel);
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
    };

    renderComponents(): void {
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.sportGroupMultiSelectionComponent.render(new SportGroupMultiSelectionOption("dropdown-sportgroup", true, "sportgroup-ids", () => { this.onchangeSportGroup(); }));        
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
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
}

$(function () {
    const matchWinLossReport = new MatchWinLossReport(
        $,
        new SportGroupMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ClientPagerComponent($),
        new ToggleHandler($),
        new DatePickerComponent());

    matchWinLossReport.init();
})
