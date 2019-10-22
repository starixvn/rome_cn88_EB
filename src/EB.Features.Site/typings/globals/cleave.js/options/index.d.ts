type CreditCardType =
    | "amex"
    | "dankort"
    | "diners"
    | "discover"
    | "instapayment"
    | "jcb"
    | "maestro"
    | "mastercard"
    | "uatp"
    | "unknown"
    | "unionPay"
    | "mir"
    | "visa";

type CreditCardTypeChangeHandler = (this: Cleave, type: CreditCardType) => void;

interface CleaveOptions {
    creditCard?: boolean;
    creditCardStrictMode?: boolean;
    creditCardType?: string;
    onCreditCardTypeChanged?: CreditCardTypeChangeHandler;
}

// Phone Options
interface CleaveOptions {
    phone?: boolean;
    phoneRegionCode?: string;
}

// Date Options
interface CleaveOptions {
    date?: boolean;
    datePattern?: ReadonlyArray<string>;
}

// Time Options
interface CleaveOptions {
    time?: boolean;
    timePattern?: ReadonlyArray<string>;
}

// Numeral Options
type NumeralThousandsGroupStyleType = "lakh" | "thousand" | "wan" | "none";

interface CleaveOptions {
    numeral?: boolean;
    numeralDecimalMark?: string;
    numeralDecimalScale?: number;
    numeralIntegerScale?: number;
    numeralPositiveOnly?: boolean;
    numeralThousandsGroupStyle?: NumeralThousandsGroupStyleType;
    stripLeadingZeroes?: boolean;
}

// Extra Options
interface CleaveOptions {
    blocks?: ReadonlyArray<number>;
    copyDelimiter?: boolean;
    delimiter?: string;
    delimiters?: ReadonlyArray<string>;
    initValue?: any;
    lowercase?: boolean;
    numericOnly?: boolean;
    prefix?: string;
    noImmediatePrefix?: boolean;
    rawValueTrimPrefix?: boolean;
    uppercase?: boolean;
    onValueChanged?(event: any): void;
}
