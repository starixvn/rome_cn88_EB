"use strict";

class MultiSelectionDropdownOption {
    constructor(
        public checkboxAllId = "checkbox-all",
        public items: any = [],
        public hiddenInputId = "HiddenInputId",
        public searchTextbox = "txt-search",
        public clearSearchId = "clear-search") {
    }
}

class MultiSelectionDropdownComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $checkboxAll: JQuery;
    $searchTextbox: JQuery;
    $leaves: any;

    options = new MultiSelectionDropdownOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: MultiSelectionDropdownOption = new MultiSelectionDropdownOption()) => {
        this.options = this.$.extend({}, this.options, options) as MultiSelectionDropdownOption;
        this.$leaves = [];

        this.$hiddenInput = this.$("#" + this.options.hiddenInputId);
        this.$checkboxAll = this.$("#" + this.options.checkboxAllId);
        const $searchTextbox = this.$("#" + this.options.searchTextbox);

        const this$ = this.$;
        const search = this.search;
        const leaves = this.$leaves;

        if (this.options.items.length) {
            this.bindParentChildEvent(this.options.items[0]);
        }

        $searchTextbox.on("change keyup", function () {
            const text = this$(this).val();
            search(text);
        });

        this.$("#" + this.options.clearSearchId).on("click touchend", () => {
            $searchTextbox.val("");
            leaves.removeClass("hide");
        });
    };

    bindParentChildEvent = (parent) => {
        const countChildCheck = this.countChildCheck;
        const $parent = this.$(parent.id);

        $parent.change(function () {
            for (const child of parent.childs) {
                const $child = this.$(child.id);
                if (this.checked) {
                    $child.attr("disabled", "disabled");
                } else {
                    $child.removeAttr("disabled");
                }
            }
        });

        if (parent.childs.length === 0) {
            this.$leaves.push($parent);
            return;
        }

        for (const child of parent.childs) {
            const $child = this.$(child.id);
            $child.click(function () {
                if (this.checked && countChildCheck(parent.childs) === parent.childs.length) {
                    $parent.prop("checked", true);
                    for (const newChild of parent.childs) {
                        this.$(newChild.id).attr("disabled", "disabled");
                    }
                }
            });

            this.bindParentChildEvent(child);
        }
    };
    countChildCheck = (childs) => {
        let count = 0;

        for (const child of childs) {
            const $child = this.$(child.id);
            if ($child.is(":checked")) {
                count++;
            }
        }

        return count;
    };

    getSelectedItems = () => {
        const selected = [];

        if (this.$(this.options.items[0].id).is(":checked")) {
            return selected;
        }

        const this$ = this.$;

        this.$leaves.each(function () {
            selected.push(this$(this).val());
        });

        return selected;
    };

    getSelectedItemTexts = () => {
        const selected = [];

        if (this.$(this.options.items[0].id).is(":checked")) {
            return selected;
        }

        const this$ = this.$;

        this.$leaves.each(function () {
            selected.push(this$(this).attr("data-text"));
        });

        return selected;
    };

    updateSelectedValues = () => {
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };
    search = (text) => {
        const lowerCaseText = text.toLowerCase();
        const this$ = this.$;
        this.$leaves.each(function () {
            const chk = this$(this);
            const chkValue = chk.attr("data-text").toLowerCase();
            if (chkValue.indexOf(lowerCaseText) === -1) {
                chk.parent().addClass("hide");
            } else {
                chk.parent().removeClass("hide");
            }
        });
    };
}