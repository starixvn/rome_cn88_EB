"use strict";
class ReportBase {
    $: JQueryStatic;

    sportGroupMultiSelectionComponent: SportGroupMultiSelectionComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;

    constructor($: JQueryStatic,
        sportGroupSelectionComponent: SportGroupMultiSelectionComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.sportGroupMultiSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
    }

    renderComponents(): void {
        this.sportGroupMultiSelectionComponent.render(new SportGroupMultiSelectionOption("dropdown-sportgroup", true, "sportgroup-ids", () => { this.onchangeSportGroup(); }));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
    }

    onchangeSportGroup(): void {
        const sportGroups = this.sportGroupMultiSelectionComponent.getSelectedItems();
        if (this.betTypeSelectionComponent.$dropdown != null) {
            this.betTypeSelectionComponent.$dropdown.empty();
            this.betTypeSelectionComponent.$dropdown.append(`<option value = 0>${window.allBetTypeText}</option>`);

            const exists = [];
            for (const betType of window.betTypes) {
                if (sportGroups.indexOf(betType.SportTypeGroup.toString()) !== -1) {
                    const key = betType.Value;

                    if (exists[key]) {
                        continue;
                    }

                    exists[key] = true;
                    this.betTypeSelectionComponent.$dropdown.append(`<option value="${betType.Value}">${betType.Text}</option>`);
                }
            }

            this.betTypeSelectionComponent.$dropdown.multiselect('destroy');
            this.betTypeSelectionComponent.$dropdown.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false
            });
        }
    }
}