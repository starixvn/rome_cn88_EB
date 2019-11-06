/// <reference path="../scripts/core.ts" />

"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class SportGroupMultiSelectionOption {
    constructor(
        public dropdownId = "dropdown-sportgroup",
        public validateBeforeSubmit = true,
        public productsInputId = "sportgroup-ids",
        public onChange: Function = null) {
    }
}

class SportGroupMultiSelectionComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdown: JQuery;
    $multiSelectButton: JQuery;

    options = new SportGroupMultiSelectionOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: SportGroupMultiSelectionOption = new SportGroupMultiSelectionOption()) => {
        this.options = this.$.extend({}, this.options, options) as SportGroupMultiSelectionOption;

        this.$dropdown = this.$("#" + this.options.dropdownId);
        const hasAnyItems = cn88Features.convertAttributeToBool(this.$dropdown.data("has-any-sport"));
        this.$hiddenInput = this.$("#" + this.options.productsInputId);

        this.$dropdown.multiselect({
            maxHeight: 300,
            numberDisplayed: cn88Features.numberDisplayedProductsOnFilter,
            nonSelectedText: hasAnyItems
                ? this.$dropdown.data("label-select-sport")
                : this.$dropdown.data("label-there-is-no-sport"),
            nSelectedText: ` ${this.$dropdown.data("label-selected")}`,
            allSelectedText: this.$dropdown.data("label-all-sport"),
            selectAllText: this.$dropdown.data("label-select-all"),
            includeSelectAllOption: hasAnyItems,
            onChange: this.onChange,
            onSelectAll: this.onChange,
            language: this.$dropdown.data("language")
        });

        this.$multiSelectButton = this.$dropdown.parent().find(".multiselect.btn");

        return this.$dropdown;
    };

    onChange = (option: any, checked: boolean) => {
        this.validate();
        this.$hiddenInput.val(this.getSelectedItems().join(","));

        if (this.options.onChange) {
            this.options.onChange();
        }
    };

    validate = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.$dropdown.find("option:selected").length
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
        }
    };

    getSelectedItems = () => {
        const selected = [];
        const selectedOptions = this.$dropdown.find("option:selected");
        const this$ = this.$;

        selectedOptions.each(function () {
            selected.push(this$(this).val());
        });

        return selected;
    };   
}