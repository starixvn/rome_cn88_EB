"use strict";
interface JQuery {
    toggleHandler(option: any): void;
}

class ToggleHandlerOption {
    constructor(
        public toggleIconId: string = "toggleIconId",
        public sourceId: string = "sourceId",
        public targetId: string = "targetId",
        public hideSourceElement: boolean = true) {
    }
}

class ToggleHandler {
    $: JQueryStatic;
    $filterToggleIcon: JQuery;
    $sourceElement: JQuery;
    $targetElement: JQuery;
    options = new ToggleHandlerOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: ToggleHandlerOption) => {
        this.options = <ToggleHandlerOption>this.$.extend({}, this.options, options);

        this.$filterToggleIcon = this.$("#" + this.options.toggleIconId);
        this.$sourceElement = this.$("#" + this.options.sourceId);
        this.$targetElement = this.$("#" + this.options.targetId);

        if (this.options.hideSourceElement) {
            this.$sourceElement.hide();
        }

        this.$filterToggleIcon.on("click touchend", (e) => {
            e.preventDefault();
            let isCollapsed = this.$filterToggleIcon.hasClass("icon-filter-down");

            if (isCollapsed) {
                this.$sourceElement
                    .slideDown(200);
                this.$targetElement
                    .slideUp(210);
                this.$filterToggleIcon
                    .removeClass("icon-filter-down")
                    .addClass("icon-filter-up");
            }
            else {
                this.$sourceElement.slideUp(200);
                this.$targetElement
                    .slideDown(210);
                this.$filterToggleIcon
                    .removeClass("icon-filter-up")
                    .addClass("icon-filter-down");
            }
        });
    };

    hideFilter = () => {
        let isCollapsed = this.$filterToggleIcon.hasClass("icon-filter-down");
        if (isCollapsed) {
            return;
        }

        this.$sourceElement.slideUp(200);
        this.$targetElement
            .slideDown(210);
        this.$filterToggleIcon
            .removeClass("icon-filter-up")
            .addClass("icon-filter-down");
    };
}
