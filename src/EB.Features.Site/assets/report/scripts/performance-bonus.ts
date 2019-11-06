"use strict";

class PerformanceBonusReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $fromDateContainer: JQuery;
    $toDateContainer: JQuery;
    $dateRangePicker: JQuery;
    $userName: JQuery;
    $headlineStock: JQuery;
    $dividendRate: JQuery;

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
    }

    public cacheDOMElements() {
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDateContainer = this.$("#from-date-container");
        this.$toDateContainer = this.$("#to-date-container");
        this.$dateRangePicker = this.$("#" + this.dateRangePickerComponent.options.selectorId);
        this.$userName = this.$("#UserNames");
        this.$headlineStock = this.$("#HeadlineStock");
        this.$dividendRate = this.$("#DividendRate");
    }

    public registerEvents() {
        this.$userName.on("change", this.CorrectUserName);
        this.$reportForm.submit(this.onReportSubmit);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$headlineStock.on("keyup", this.CheckHeadlineStock);
        new Cleave((this.$dividendRate[0]), {
            numeral: true,
            numeralDecimalScale: 4
        });
        new Cleave((this.$headlineStock[0]), {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand'
        });
    }

    CheckHeadlineStock = (e: any) => {
        const headlineStock = this.$headlineStock.val().replace(/,/g, '');
        const newValue = Number(headlineStock);
        const maxStock = 999999999999.99;
        if (newValue > maxStock) {
            this.$headlineStock.val(this.$headlineStock.data("oldvalue"));
            new Cleave((this.$headlineStock[0]), {
                numeral: true,
                numeralThousandsGroupStyle: 'thousand'
            });
        } else {
            this.$headlineStock.data("oldvalue", newValue);
        }
    }

    CorrectUserName = () => {
        let userNames = this.$userName.val();
        userNames = userNames.trim().split(/\r\n|\r|\n|\s/g).filter(function (el) {
            return el !== "";
        }).join(",");
        this.$userName.val(userNames);
    }

    convertStringNumberToNumber = () => {
        const headlineStock = this.$headlineStock.val().replace(/,/g, '');
        this.$headlineStock.val(headlineStock);

        const bonusRate = this.$dividendRate.val().replace(/,/g, '');
        this.$dividendRate.val(bonusRate);
    }
    onExportExcel = () => {
        this.$exportExcelForm.submit();
    }

    onReportSubmit = (e: any) => {
        this.$.blockUI();
        this.convertStringNumberToNumber();
        this.periodSelectionComponent.updateSelectedValue();
        this.currencySelectionComponent.updateSelectedValue();
        this.conjunctionSelectionComponent.updateSelectedValue();
    }

    renderComponents(): void {
        this.periodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId", () => { this.onPeriodsChange(); }));
        this.currencySelectionComponent.render(new SingleSelectionDropdownOption("dropdown-currency", "UserSelectedCurrencyId"));
        this.conjunctionSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-conjunction", "UserSelectedConjunctionId"));
        this.dateRangePickerComponent.render();
    }

    onPeriodsChange(): void {
        const id = this.periodSelectionComponent.$dropdown.val();

        for (const period of window.periods) {
            if (period.Id === id) {
                this.changeDateRange(new Date(period.FromDate), new Date(period.ToDate));
                break;
            }
        }
    }

    changeDateRange(fromDate, toDate): void {
        const minDate = new Date(this.$dateRangePicker.data("mindate"));
        const maxDate = new Date(this.$dateRangePicker.data("maxdate"));
        const fDate = window.NexDatePickerHelper.correctDateInRange(fromDate, minDate, maxDate);
        const tDate = window.NexDatePickerHelper.correctDateInRange(toDate, minDate, maxDate);
                
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
});
