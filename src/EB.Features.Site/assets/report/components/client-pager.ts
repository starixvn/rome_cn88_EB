"use strict";

interface JQuery {
    pagination(option: any): void;
}

class ClientPagerOption {
    constructor(
        public onSelectPage: () => void = null,
        public pagerId = "report-pager",
        public formId = "report-form",
        public pageIndexId = "PageIndex",
        public pageSizeId = "PageSize",
        public tableReportId = "tbl-report") {
    }
}

class ClientPagerComponent {
    $: JQueryStatic;
    $pageIndex: JQuery;
    $pageSize: JQuery;
    $reportForm: JQuery;

    $bodyRows: JQuery;

    totalRecords: number;
    pageSize: number;
    pageIndex: number;
    row1000 = 1000;
    row2000 = 2000;
    row3000 = 3000;
    row5000 = 5000;
    row10 = 10;
    row50 = 50;
    row100 = 100;
    row500 = 500;
    pageList: number[] = [this.row1000, this.row2000, this.row3000, this.row5000];
    smallPageList: number[] = [this.row10, this.row50, this.row100, this.row500];

    options = new ClientPagerOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: ClientPagerOption = new ClientPagerOption()): JQuery => {
        this.options = this.$.extend({}, this.options, options) as ClientPagerOption;
        const pager = this.$("#" + this.options.pagerId);

        if (pager.length > 0) {
            const tableId = "#" + this.options.tableReportId;
            const tableStackId = tableId + "-stack";
            const isMobile = this.$(tableStackId).data("ismobile");
            this.$reportForm = this.$("#" + this.options.formId);
            this.$bodyRows = isMobile
                ? this.$(tableStackId + " > tbody > tr")
                : this.$(tableId + " > tbody > tr");

            this.totalRecords = parseInt(pager.data("totalrecords"), 0);
            this.pageSize = parseInt(pager.data("pagesize"), 0);


            if (!isMobile || (isMobile && this.totalRecords > this.pageList[0])) {
                this.pageIndex = parseInt(pager.data("pageindex"), 0);
                this.$pageIndex =
                    this.$(`<input type="hidden" id="${this.options.pageIndexId}" name="${this.options.pageIndexId}" value="${this.pageIndex}" />`);
                this.$pageSize =
                    this.$(`<input type="hidden" id="${this.options.pageSizeId}" name="${this.options.pageSizeId}" value="${this.pageSize}" />`);

                this.$reportForm.append(this.$pageIndex).append(this.$pageSize);

                pager.pagination({
                    pageList: this.pageList,
                    total: this.totalRecords,
                    pageSize: this.pageSize,
                    pageNumber: this.pageIndex,

                    showRefresh: false,
                    onSelectPage: this.onSelectPage,
                    beforePageText: pager.data("labelpage"),
                    afterPageText: `${pager.data("labelof")} {pages}`,
                    displayMsg: pager.data("labeldisplayitems")
                });

                this.showBodyRows();
                pager.removeClass("hide");
            }
        }

        return pager;
    };

    onSelectPage = (pageNumber: number, pageSize: number) => {
        this.updateHiddenFields(pageNumber, pageSize);
        this.showBodyRows();

        if (this.options.onSelectPage) {
            this.options.onSelectPage();
        }
    };

    updateHiddenFields = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);

        this.pageSize = pageSize;
        this.pageIndex = pageNumber;
    };

    showBodyRows = () => {
        if (!this.totalRecords) {
            return;
        }

        const startIndex = this.pageSize * (this.pageIndex - 1);
        const endIndex = startIndex + this.pageSize;

        this.$bodyRows.addClass("hide");
        for (let i = startIndex; i < endIndex && i < this.totalRecords; i++) {
            this.$(this.$bodyRows[i]).removeClass("hide");
        }
    };
}