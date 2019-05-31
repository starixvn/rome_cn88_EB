"use strict";
var betTypes: any = betTypes;

class HouseWinLossReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $dateContainer: JQuery;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    ticketStatusSelectionComponent: SingleSelectionDropdownComponent;

    constructor($: JQueryStatic,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        ticketStatusSelectionComponent: SingleSelectionDropdownComponent,
        toggleHandler: ToggleHandler) {
        this.$ = $;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
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
    }

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
        this.ticketStatusSelectionComponent.updateSelectedValue();
    };

    renderComponents(): void {
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", () => { this.onchangeSportType(); }));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.ticketStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-ticketstatus", "UserSelectedTicketStatusId"));
    }
    

    onchangeSportType(): void {
        let sportType = this.sportGroupSelectionComponent.$dropdown.val();
        this.betTypeSelectionComponent.$dropdown.empty();
        for (var i = 0; i < betTypes.length; i++) {
            var betTypeGroup = betTypes[i];
            if (betTypeGroup[0].SportTypeGroup == 0) {
                this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[0].Value + '>' + betTypeGroup[0].Text + '</option>');

                if (sportType == 0)
                    break;
            }

            if (betTypeGroup[0].SportTypeGroup == sportType) {
                for (var j = 0; j < betTypeGroup.length; j++) {
                    this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[j].Value + '>' + betTypeGroup[j].Text + '</option>');
                }

                break;
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
}

$(function () {
    const houseWinLossReport = new HouseWinLossReport(
        $,
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ToggleHandler($));

    houseWinLossReport.init();
})
