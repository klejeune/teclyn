
var teclyn = {
    $,
    init: function (jquery) {
        if (jquery) {
            this.$ = jquery;
        } else {
            this.$ = $;
        }

        this.initForms();
    },
    initForms: function () {
        var t = this;

        $('.command-form')
            .submit(function() {
                var form = this;
                var previousRequestData = form.teclynRequestData;
                var newRequestData = t.serializeForm(form);

                //if (previousRequestData !== newRequestData) {
                form.teclynRequestData = newRequestData;
                t.executeRemoteCommand(new FormData(form), form, $(form).data("reload"));
                //}

                return false;
            });
    },
    serializeForm: function (form) {
        return $(form).serialize();
    },
    executeRemoteCommand: function (data, anchor, reload, callback, onSuccess, onFailure) {
        $(':focus').blur();

        $.ajax({
            type: 'POST',
            url: '/teclyn/command/execute',
            data: data,
            contentType: false,
            processData: false,
            success: function(response) {
                if (response.Success) {
                    if (onSuccess) {
                        onSuccess();
                    }

                    if (callback) {
                        callback(response, anchor);
                    }

                    if (reload) {
                        location.reload();
                    }
                } else {
                    if (onFailure) {
                        onFailure();
                    }
                }
            }
        });
    }
};

function dump(data) {
    alert(dumpInner(data, 0));
}

function dumpInner(data, level) {
    var display = '';

    $.each(data, function (index, value) {
        if (level < 1 && value != null && typeof value === 'object') {
            value = dumpInner(value, level + 1);
        }

        var indent = '';

        if (level > 0) {
            indent = Array(level + 1).join(" ");
        }

        display += indent + "[" + index + "] = " + value + '\n';
    });

    return display;
}