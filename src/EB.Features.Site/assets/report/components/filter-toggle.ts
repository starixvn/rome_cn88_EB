"use strict";
interface JQuery {
    toggleHandler(option: any): void;
}

class ToggleHandlerOption {
    constructor(
        public toggleIconId = "toggleIconId",
        public sourceId = "sourceId",
        public targetId = "targetId",
        public hideSourceElement = true) {
    }
}

class ToggleHandler {
    $: JQueryStatic;
    $filterToggleIcon: JQuery;
    $sourceElement: JQuery;
    $targetElement: JQuery;
    options = new ToggleHandlerOption();
    slideUpTime = 210;
    slideDownTime = 200;
    eventName: {
        IconFilterDown: "icon-filter-down",
        IconFilterUp: "icon-filter-up",
    };
    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: ToggleHandlerOption) => {
        this.options = this.$.extend({}, this.options, options) as ToggleHandlerOption;

        this.$filterToggleIcon = this.$("#" + this.options.toggleIconId);
        this.$sourceElement = this.$("#" + this.options.sourceId);
        this.$targetElement = this.$("#" + this.options.targetId);

        if (this.options.hideSourceElement) {
            this.$sourceElement.hide();
        }

        this.$filterToggleIcon.on("click touchend", (e) => {
            e.preventDefault();
            const isCollapsed = this.$filterToggleIcon.hasClass(this.eventName.IconFilterDown);

            if (isCollapsed) {
                this.$sourceElement
                    .slideDown(this.slideDownTime);
                this.$targetElement
                    .slideUp(this.slideUpTime);
                this.$filterToggleIcon
                    .removeClass(this.eventName.IconFilterDown)
                    .addClass(this.eventName.IconFilterUp);
            }
            else {
                this.$sourceElement.slideUp(this.slideDownTime);
                this.$targetElement
                    .slideDown(this.slideUpTime);
                this.$filterToggleIcon
                    .removeClass(this.eventName.IconFilterUp)
                    .addClass(this.eventName.IconFilterDown);
            }
        });
    };

    hideFilter = () => {
        const isCollapsed = this.$filterToggleIcon.hasClass(this.eventName.IconFilterDown);
        if (isCollapsed) {
            return;
        }

        this.$sourceElement.slideUp(this.slideDownTime);
        this.$targetElement
            .slideDown(this.slideUpTime);
        this.$filterToggleIcon
            .removeClass(this.eventName.IconFilterUp)
            .addClass(this.eventName.IconFilterDown);
    };
}
