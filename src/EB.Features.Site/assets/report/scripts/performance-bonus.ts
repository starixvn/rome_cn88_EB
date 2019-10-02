"use strict";
var periods: any;
let NexDatePickerHelper: any;

class PerformanceBonusReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $fromDateContainer: JQuery;
    $toDateContainer: JQuery;
    $dateRangePicker: JQuery;

    periodSelectionComponent: SingleSelectionDropdownComponent;
    currencySelectionComponent: SingleSelectionDropdownComponent;
    conjunctionSelectionComponent: SingleSelectionDropdownComponent;
    dateRangePickerComponent: DateRangePickerComponent;

    constructor($: JQueryStatic,
        periodSelectionComponent: SingleSelectionDropdownComponent,
        currencySelectionComponent: SingleSelectionDropdownComponent,
        conjunctionSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent) {
        this.$ = $;
        this.periodSelectionComponent = periodSelectionComponent;
        this.currencySelectionComponent = currencySelectionComponent;
        this.conjunctionSelectionComponent = conjunctionSelectionComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();
        this.$.unblockUI();
    };

    public cacheDOMElements() {
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDateContainer = this.$("#from-date-container");
        this.$toDateContainer = this.$("#to-date-container");
        this.$dateRangePicker = this.$("#" + this.dateRangePickerComponent.options.selectorId);
    };

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$("#icon-excel").on("click", this.onExportExcel);
        new Cleave("#BonusRate", {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand'
        });
        new Cleave("#HeadlineStock", {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand'
        });
    };

    convertStringNumberToNumber = () => {
        const headlineStock = this.$("#HeadlineStock").val().replace(',', '');
        this.$("#HeadlineStock").val(headlineStock);

        const bonusRate = this.$("#BonusRate").val().replace(',', '');
        this.$("#BonusRate").val(bonusRate);
    }
    onExportExcel = () => {
        this.convertStringNumberToNumber();
        this.$exportExcelForm.submit();
    };

    onReportSubmit = (e: any) => {
        this.convertStringNumberToNumber();
        this.periodSelectionComponent.updateSelectedValue();
        this.currencySelectionComponent.updateSelectedValue();
        this.conjunctionSelectionComponent.updateSelectedValue();
    };

    renderComponents(): void {
        this.periodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId", () => { this.onPeriodsChange(); }));
        this.currencySelectionComponent.render(new SingleSelectionDropdownOption("dropdown-currency", "UserSelectedCurrencyId"));
        this.conjunctionSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-conjunction", "UserSelectedConjunctionId"));
        this.dateRangePickerComponent.render();
    };

    onPeriodsChange(): void {
        const id = this.periodSelectionComponent.$dropdown.val();

        for (var i = 0; i < periods.length; i++) {
            if (periods[i].Id == id) {
                var period = periods[i];
                this.changeDateRange(period.FromDate, period.ToDate);
                break;
            }
        }        
    };

    changeDateRange(fromDate, toDate): void {
        const minDate = new Date(this.$dateRangePicker.data("mindate"));
        const maxDate = new Date(this.$dateRangePicker.data("maxdate"));
        const fDate = NexDatePickerHelper.correctDateInRange(fromDate, minDate, maxDate);
        const tDate = NexDatePickerHelper.correctDateInRange(toDate, minDate, maxDate);

        this.$fromDateContainer.data("DateTimePicker").maxDate(tDate);
        this.$fromDateContainer.data("DateTimePicker").date(fDate);

        this.$toDateContainer.data("DateTimePicker").minDate(fDate);
        this.$toDateContainer.data("DateTimePicker").date(tDate);
    }
}

$(function () {
    const performanceBonusReport = new PerformanceBonusReport(
        $,
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($));

    performanceBonusReport.init();
})
