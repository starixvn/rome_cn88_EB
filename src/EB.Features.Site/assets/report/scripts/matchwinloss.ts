"use strict";
var betTypes: any = betTypes;

class MatchWinLossReport {
    $: JQueryStatic;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $dateContainer: JQuery;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    pagerComponent: ClientPagerComponent;

    constructor($: JQueryStatic,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: ClientPagerComponent,
        toggleHandler: ToggleHandler) {
        this.$ = $;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
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
        this.$reportForm.submit(this.onReportSubmit); debugger;
        this.$dateContainer.datepicker({
            minDate: this.$dateContainer.data("mindate"),
            defaultDate: this.$dateContainer.data("currentdate"),
            format: 'YYYY-MM-DD'
        }); 

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

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    };

    renderComponents(): void {
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", () => { this.onchangeSportType(); }));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
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
    const matchWinLossReport = new MatchWinLossReport(
        $,
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ClientPagerComponent($),
        new ToggleHandler($));

    matchWinLossReport.init();
})
