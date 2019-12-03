class LinkBuilderOption {
    linkClasses: string[];

    constructor(linkClasses: string[] = ["a.breadcrumb-link,a.downline-link"]) {
        this.linkClasses = linkClasses;
    }
}

class LinkBuilderComponent {
    $: JQueryStatic;

    options = new LinkBuilderOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: LinkBuilderOption = new LinkBuilderOption) => {
        this.options = this.$.extend({}, this.options, options);
    };

    updateParam = (name: string, value: string) => {
        const this$ = this.$;

        this.options.linkClasses.forEach((className: string) => {
            const $linkClass = this.$(className);

            $linkClass.each((index: number, element: Element) => {
                const $link = this$(element);
                let currentUrl = $link.attr("href");

                currentUrl = cn88Features.replaceQueryStringValue(currentUrl, name, value);

                $link.attr("href", currentUrl);
            });
        });
    };
}