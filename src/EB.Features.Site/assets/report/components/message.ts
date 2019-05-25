"use strict";

class Message {
    showMessage($target: JQuery, result: Result) {
        $target.empty();

        if (result.IsSuccessful) {
            this.showSuccess($target, result.Message);
        } else if (result.IsFailed) {
            this.showError($target, result.Message);
        } else if (result.IsWarning) {
            this.showWarn($target, result.Message);
        }
    }

    showSuccess($target: JQuery, message: string) {
        $target.html("<div class='msg msg-success'>" + message + "</div>");
    }

    showError($target: JQuery, error: string) {
        $target.html("<div class='msg msg-error'>" + error + "</div>");
    }

    showWarn($target: JQuery, warning: string) {
        $target.html("<div class='msg msg-warning'>" + warning + "</div>");
    }

    hideMessage($target: JQuery) {
        $target.empty();
    }
}