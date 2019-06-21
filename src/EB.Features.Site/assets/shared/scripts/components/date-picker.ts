class DatePickerOption {
    constructor(
        public $: JQueryStatic = null,
        public pickerId: string = "date-container") {
    }
}

class DatePickerComponent {
    $: JQueryStatic;
    $dateContainer: JQuery;
    options = new DatePickerOption();

    render = (options: DatePickerOption = new DatePickerOption()) => {
        this.$ = options.$;
        this.options = <DatePickerOption>this.$.extend({}, this.options, options);

        this.$dateContainer = this.$("#" + this.options.pickerId);
        this.$dateContainer.datepicker({
            allowInputToggle: true,
            minDate: this.$dateContainer.data("mindate"),
            defaultDate: this.$dateContainer.data("currentdate"),
            format: 'YYYY-MM-DD'
        });
    }
}