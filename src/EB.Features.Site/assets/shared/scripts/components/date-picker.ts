class DatePickerOption {
    constructor(
        public $: JQueryStatic = null,
        public pickerId = "date-container") {
    }
}

class DatePickerComponent {
    $: JQueryStatic;
    $dateContainer: JQuery;
    options = new DatePickerOption();

    render = (options: DatePickerOption = new DatePickerOption()) => {
        this.$ = options.$;
        this.options = this.$.extend({}, this.options, options) as DatePickerOption;

        this.$dateContainer = this.$("#" + this.options.pickerId);
        this.$dateContainer.datepicker({
            allowInputToggle: true,
            minDate: this.$dateContainer.data("mindate"),
            defaultDate: this.$dateContainer.data("currentdate"),
            format: 'YYYY-MM-DD',
            locale: moment.locale("en-us", { week: { dow: 1 } })
        });
    }
}