"use strict";
class MatchWinLossReport extends ReportBase {
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    pagerComponent: ClientPagerComponent;
    datePickerComponent: DatePickerComponent;
    $buttonSportGroup: JQuery;

    constructor($: JQueryStatic,
        sportGroupSelectionComponent: SportGroupMultiSelectionComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: ClientPagerComponent,
        datePickerComponent: DatePickerComponent) {
        super($, sportGroupSelectionComponent, betTypeSelectionComponent);
        this.$ = $;
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
            const $this: JQuery = $(this);
            const icon: JQuery = $this.find(".group-name-wrapper").children().first();
            const children: JQuery = $this.siblings(`.childrow-${$this.data("leagueid")}`);

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
    }

    onReportSubmit = (e: JQueryEventObject) => {
        this.$.blockUI();
        const sportGroups = this.sportGroupMultiSelectionComponent.getSelectedItems();
        if (sportGroups.length <= 0) {
            this.$buttonSportGroup.addClass("error");
            e.preventDefault();
            this.$.unblockUI();

            return;
        }

        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    }

    renderComponents(): void {
        super.renderComponents();
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.$buttonSportGroup = this.$("#dropdown-sportgroup").parent().find(".multiselect.btn");
    }
}

$(function () {
    const matchWinLossReport = new MatchWinLossReport(
        $,
        new SportGroupMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ClientPagerComponent($),
        new DatePickerComponent());

    matchWinLossReport.init();
});
