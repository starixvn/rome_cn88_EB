"use strict";
interface JQuery {
    pagination(option: object): void;

}

class PagerOption {
    constructor(
        public onSelectPage: () => void = null,
        public pagerId = "report-pager",
        public formId = "report-form",
        public pageIndexId = "PageIndex",
        public pageSizeId = "PageSize",
        public pageList: number[] = []) {
        const item10 = 10;
        const item50 = 50;
        const item100 = 100;
        const item500 = 500;
        pageList = pageList || [item10, item50, item100, item500];
    }
}

class PagerComponent {
    $: JQueryStatic;
    $pageIndex: JQuery;
    $pageSize: JQuery;
    $reportForm: JQuery;

    totalRecords: number;
    pageSize: number;
    pageIndex: number;

    pageList: number[] = [];

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: PagerOption = this.options) => {
        const updateHiddenFields = this.updateHiddenFields;

        const pagerOption = this.$.extend({}, this.options, options) as PagerOption;
        const pager = this.$("#" + pagerOption.pagerId);

        this.$reportForm = this.$("#" + pagerOption.formId);
        this.totalRecords = pager.data("totalrecords");
        this.pageSize = pager.data("pagesize");
        this.pageIndex = pager.data("pageindex");
        this.$pageIndex =
            this.$(`<input type="hidden" id="${pagerOption.pageIndexId}" name="${pagerOption.pageIndexId}" value="${this.pageIndex}" />`);
        this.$pageSize =
            this.$(`<input type="hidden" id="${pagerOption.pageSizeId}" name="${pagerOption.pageSizeId}" value="${this.pageSize}" />`);
        this.pageList = pagerOption.pageList;

        this.$reportForm.append(this.$pageIndex).append(this.$pageSize);

        if (this.isShow()) {
            pager.pagination({
                pageList: this.pageList,
                total: this.totalRecords,
                pageSize: this.pageSize,
                pageNumber: this.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    updateHiddenFields(pageNumber, pageSize);
                    if (pagerOption.onSelectPage && typeof (pagerOption.onSelectPage) === "function") {
                        pagerOption.onSelectPage();
                    }
                },
                beforePageText: pager.data("labelpage"),
                afterPageText: `${pager.data("labelof")} {pages}`,
                displayMsg: pager.data("labeldisplayitems")
            });

            pager.removeClass("hide");
        } else {
            pager.addClass("hide");
        }

        return pager;
    };

    onChange = () => {
        this.$reportForm.trigger("submit");
    };

    updateHiddenFields = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);
    };

    isShow = () => {
        // total records less than min page size => do not need to show pager
        //return this.pageList[0] <= this.totalRecords;
        return true;
    };

    options = new PagerOption(this.onChange);
}