"use strict";
interface Window {
    betTypes: BetType[];
    allBetTypeText: string;
    periods: Period[];
    NexDatePickerHelper: NexDatePicker;
}
class NexDatePicker {
    correctDateInRange: (fromDate: Date, toDate: Date, maxDate: Date) => string;
}
class BetType {
    SportTypeGroup: number;
    Value: string;
    Text: string;
}
class Period {
    Id: string;
    FromDate: string;
    ToDate: string;
}
class CN88Features {
    $: JQueryStatic;
    store: StoreJSStatic;

    constructor($: JQueryStatic, store: StoreJSStatic) {
        this.$ = $;
        this.store = store;
    }

    format = {
        monthFormat: "MMMM - yyyy",
        shortDateWithSlash_DatePicker: "MM/DD/YYYY",
        shortDateWithSlash_MonthPicker: "MM/dd/yyyy",
        shortDate_DatePicker: "YYYY-MM-DD"
    };

    sort = {
        none: 0,
        ascending: 1,
        descending: -1
    };
    currencyFixed = 2;
    keyCode = {
        enter: 13
    };
    numberDisplayedProductsOnFilter = 1;
    maxTimesToShowTooltip = 5;
    screenSmallThan768 = () => {
        const currentWindowWidth = screen.width || this.$(window).width();
        const minWidth = 768;
        return currentWindowWidth < minWidth;
    };

    splitCell = (firstCell: string, cellClass = "", lastCellStyle = "bold") => {
        const this$ = this.$;

        const combinedCells = this.$("." + firstCell + "-combined");

        combinedCells.each(function (index: number) {
            const oldValues = this$(this).find("li");
            const cellsLength = oldValues.length;
            let oldCells = "";

            for (let i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                    (i === lastCellIndex ? " " + lastCellStyle : "") + "\">" +
                    this$(oldValues[i]).html() +
                    "</td>";
            }

            this$(combinedCells[index]).before(oldCells);
        });
    };
        
    localStorage = {
        get: (key: string) => {
            const result = this.store.enabled ? this.store.get(key) : this.$.cookie(key);
            return result ? JSON.parse(result) : undefined;
        },

        set: (key: string, value: object) => {
            const data = JSON.stringify(value);
            this.store.enabled ? this.store.set(key, data) : this.$.cookie(key, data, this.localStorage.getCookieExpireOptions());
        },

        getCookieExpireOptions: () => {
            if (this.isIe()) {
                return { expires: 365 };
            }

            return { expires: 365, path: window.location.pathname };
        }
    };
    isIe = () => {
        const ua = window.navigator.userAgent;
        const msie = ua.indexOf("MSIE ");

        return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
    };
    replaceQueryStringValue = (queryString: string, paramName: string, paramValue: string) => {
        if (typeof queryString === "undefined"
            || queryString === null) {
            return "";
        }

        const regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");

        const newQueryString = regex.test(queryString)
            ? queryString.replace(regex, `$1${paramName}=${paramValue}`)
            : `${queryString}${(queryString.indexOf("?") >= 0 ? "&" : "?")}${paramName}=${paramValue}`;

        return newQueryString;
    };

    correctEvenRowClass = ($tableReport: JQuery) => {
        const this$ = this.$;

        let isEvenRow = false;
        $tableReport.find("tbody > tr").each(function () {
            const $row = this$(this);
            if ($row.hasClass("even-row") && !isEvenRow) {
                $row.removeClass("even-row");
            } else if (!$row.hasClass("even-row") && isEvenRow) {
                $row.addClass("even-row");
            }

            isEvenRow = !isEvenRow;
        });
    };
    convertAttributeToBool = (attrValue: string) => {
        return !!attrValue && attrValue.toString().toLowerCase() === "true";
    };

    formatNumber = (value: number) => value.toLocaleString("en-US");

    formatCurrency = (value: number) => value.toFixed(this.currencyFixed).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}

const cn88Features = new CN88Features($, store);

interface JQuery {
    stickyTableHeaders(): void;
}