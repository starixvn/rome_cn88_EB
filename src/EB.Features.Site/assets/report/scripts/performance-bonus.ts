"use strict";
var accounting: any;
var periods: any;
let NexDatePickerHelper: any;

class PerformanceBonusReport {
    $: JQueryStatic;
    $BonusRateOnGrid: JQuery;
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
        this.$BonusRateOnGrid = this.$("#BonusRateOnGrid");
        this.$fromDateContainer = this.$("#from-date-container");
        this.$toDateContainer = this.$("#to-date-container");
        this.$dateRangePicker = this.$("#" + this.dateRangePickerComponent.options.selectorId);
    };

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$BonusRateOnGrid.on("keyup", this.onRecaculate);
        new Cleave("#HeadlineStock", {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand'
        });
    };

    onRecaculate = () => {
        let totalBonusRate = 0;
        let totalBonusRate1035 = 0;
        let totalRoundDown = 0;
        this.$(".js-bonus-rate").each((index: number, element: Element) => {
            const $this = this.$(element);
            const headlineStock = Number($this.data("headlinestock"));
            const bonusRate = Number(this.$BonusRateOnGrid.val());
            const value = headlineStock * bonusRate
            totalBonusRate += value;
            $this.text(accounting.formatNumber(value,2,","));
        });

        this.$("#total-bonus-rate").text(accounting.formatNumber(totalBonusRate, 2, ","));

        this.$(".js-bonus-rate1035").each((index: number, element: Element) => {
            const $this = this.$(element);
            const headlineStock = Number($this.data("headlinestock"));
            const bonusValue = Number(this.$BonusRateOnGrid.val());
            const value = headlineStock * bonusValue * 1.035;
            totalBonusRate1035 += value;
            $this.text(accounting.formatNumber(value, 2, ","));
        });
        this.$("#total-bonus-rate1035").text(accounting.formatNumber(totalBonusRate1035, 2, ","));

        this.$(".js-bonus-rate-round-down").each((index: number, element: Element) => {
            const $this = this.$(element);
            const headlineStock = Number($this.data("headlinestock"));
            const bonusRate = Number(this.$BonusRateOnGrid.val());
            const value = Math.floor(Math.abs(headlineStock * bonusRate * 1.035 / 100)) * 100;
            totalRoundDown += value;
            $this.text(accounting.formatNumber(value, 2, ","));
        });
        this.$("#total-bonus-rate-round-down").text(accounting.formatNumber(totalRoundDown, 2, ","));
    };   

    onExportExcel = () => {
        const headlineStock = this.$("#HeadlineStock").val().replace(',', '');
        this.$("#HeadlineStock").val(headlineStock);
        this.$exportExcelForm.submit();
    };

    onReportSubmit = (e: any) => {
        this.$("#BonusRate").val(this.$BonusRateOnGrid.val());
        const headlineStock = this.$("#HeadlineStock").val().replace(',', '');
        this.$("#HeadlineStock").val(headlineStock);
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
