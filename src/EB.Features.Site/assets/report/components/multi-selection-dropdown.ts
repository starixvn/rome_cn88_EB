"use strict";
class MultiSelectionDropdownOption {
    constructor(
        public checkboxAllId: string = "checkbox-all",
        public items: any = [],
        public hiddenInputId: string = "HiddenInputId",
        public searchTextbox: string = "txt-search",
        public clearSearchId: string = "clear-search") {
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
        this.options = <MultiSelectionDropdownOption>this.$.extend({}, this.options, options);
        this.$leaves = [];

        this.$hiddenInput = this.$("#" + this.options.hiddenInputId);
        this.$checkboxAll = this.$("#" + this.options.checkboxAllId);
        let $searchTextbox = this.$("#" + this.options.searchTextbox);

        let self = this;
        if (this.options.items.length) {
            this.bindParentChildEvent(this.options.items[0]);
        }

        $searchTextbox.on("change keyup", function () {
            var text = self.$(this).val();
            self.search(text);
        });

        this.$("#" + this.options.clearSearchId).on("click touchend", () => {
            $searchTextbox.val("");
            self.$leaves.removeClass("hide");
        });
    };

    bindParentChildEvent = (parent) => {
        var self = this;
        var $parent = this.$(parent.id);
        var $childs = [];

        $parent.change(function () {
            for (var i = 0; i < parent.childs.length; i++) {
                var $child = this.$(parent.childs[i].id);
                if (this.checked) {
                    $child.attr("disabled", "disabled");
                } else {
                    $child.removeAttr("disabled");
                }
            }
        });

        if (parent.childs.length == 0) {
            this.$leaves.push($parent);
            return;
        }

        for (var i = 0; i < parent.childs.length; i++) {
            var child = parent.childs[i];
            var $child = this.$(parent.childs[i].id);
            $child.click(function () {
                if (this.checked && self.countChildCheck(parent.childs) == parent.childs.length) {
                    $parent.prop("checked", true);

                    for (var i = 0; i < parent.childs.length; i++) {
                        var $child = this.$(parent.childs[i].id);
                        $child.attr("disabled", "disabled");
                    }
                }
            });

            this.bindParentChildEvent(child);
        }
    };
    countChildCheck = (childs) => {
        var count = 0;

        for (var i = 0; i < childs.length; i++) {
            var $child = this.$(childs[i].id);
            if ($child.is(":checked")) {
                count++;
            }
        }

        return count;
    };

    getSelectedItems = () => {
        let selected = [];

        if (this.$(this.options.items[0].id).is(":checked")) {
            return selected;
        }

        let this$ = this.$;

        this.$leaves.each(function () {
            selected.push(this$(this).val());
        });

        return selected;
    };

    getSelectedItemTexts = () => {
        let selected = [];

        if (this.$(this.options.items[0].id).is(":checked")) {
            return selected;
        }

        let this$ = this.$;

        this.$leaves.each(function () {
            selected.push(this$(this).attr("data-text"));
        });

        return selected;
    };

    updateSelectedValues = () => {
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };
    search = (text) => {
        var lowerCaseText = text.toLowerCase();
        var this$ = this.$;
        this.$leaves.each(function () {
            var chk = this$(this);
            var chkValue = chk.attr("data-text").toLowerCase();
            if (chkValue.indexOf(lowerCaseText) == -1) {
                chk.parent().addClass("hide");
            } else {
                chk.parent().removeClass("hide");
            }
        });
    };
}