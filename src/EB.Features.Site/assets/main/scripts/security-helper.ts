"use strict";

interface String {
    encrypt(secretKey: string): string;
    hash(salt: string, token: string): string;
}

declare var Sha1: any;
declare var Sha256: any;
declare var CryptoJS: any;

String.prototype.encrypt = function (secretKey: string): string {
    let plainText: string = this.toString();
    const passPhrase = "a5s8d2e9c172";
    let key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
    let iv = key;

    let pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    return pwd.toString();
};

String.prototype.hash = function (salt: string, token: string): string {
    if (!salt || !token) {
        return "";
    }

    let plainText: string = this.toString();
    let sha1SC = Sha1.hash(plainText + salt);

    return Sha256.hash(sha1SC + token);
};
