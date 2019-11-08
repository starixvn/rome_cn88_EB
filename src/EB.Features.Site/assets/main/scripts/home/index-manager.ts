class HomeIndex {
    protected $: JQueryStatic;
    protected topWindow: Window;

    markMessageAsReadPath = null;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.markMessageAsReadPath = this.$("#MarkMessageAsReadPath").val();
        siteInfrastructure.loadInfo(this.$, "#announcements", this.initAnnouncements);
        this.$("#generalinformation a").on("click touchend", (event) => {
            this.topWindow.indexManager.activeMainMenu("menu-ACCOUNTMANAGEMENT");
            this.topWindow.indexManager.activeSubMenu("sub-menu-DOWNLINELIST", "menu-ACCOUNTMANAGEMENT");
            const url = event.target.attributes["data-url"].nodeValue;
            window.location.href = url;
        });
    };

    initAnnouncements = () => {
        this.onToggleMessage(this.markMessageAsRead);
        this.$("#view-announcements").on("click touchend", (event) => {
            this.topWindow.indexManager.activeMainMenu("menu-ANNOUNCEMENT");
            const target = event.target.attributes["data-target"].nodeValue;
            this.topWindow.$("#main").attr("src", target);
        });
    };

    onToggleMessage = (markMessageAsRead: (message: JQuery) => void) => {
        this.$(".message").each((index: number, element: Element) => {
            const $this = this.$(element),
                shortContent = $this.children(".short-content"),
                fullContent = $this.children(".full-content"),
                icon = $this.find(".icon");

            $this.on("click", () => {
                if (!shortContent.is(":hidden")) {
                    shortContent.hide();
                    fullContent.show();
                    icon.toggleClass("icon-add icon-minus");
                    markMessageAsRead($this);
                }
                else {
                    shortContent.show();
                    fullContent.hide();
                    icon.toggleClass("icon-minus icon-add");
                }
            });
        });
    }
    markMessageAsRead = (message: JQuery) => {
        const isUnRead: boolean = message.hasClass("bold");

        if (isUnRead) {
            const onSuccess = (isMarkedSuccess: boolean) => {
                if (isMarkedSuccess) {
                    message.removeClass("bold");
                }
            };

            this.$.ajax({
                url: this.markMessageAsReadPath,
                data: {
                    messageId: message.attr("id")
                },
                method: "POST",
                success: onSuccess
            });
        }
    }
}


$(function () {
    const homeIndex = new HomeIndex($, top);
    homeIndex.init();
});