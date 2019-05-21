"use strict";
class Kickout {
    init() {
        const messageElement = document.getElementById("message");
        const message = messageElement != null
            ? (messageElement as HTMLInputElement).value
            : (document.getElementById("errorMessage") as HTMLInputElement).value;

        alert(message);
        siteInfrastructure.logout();
    }
}

$(() => {
    const kickout: Kickout = new Kickout();
    kickout.init();
});