/*
author:xiejiabin@163.com
*/
$.extend({
    sync: {}
});
/*ajax封装*/
$.sync.ajax = {
    timeout: 30000,
    get: function (sUrl, jData, func, async, dataType) {
        $.ajax({
            url: sUrl,
            data: JSON.stringify(jData),
            type: 'get',
            async: (async | false) == 0 ? false : true,
            cache: false,
            dataType: dataType || 'json',
            contentType: 'application/json; charset=utf-8',
            timeout: this.timeout,
            success: function (data) {
                func(data);
            },
            error: function (x, e, s) {
                alert(x.responseText);
            }
        })
    },
    post: function (sUrl, jData, func, async, dataType) {
        $.ajax({
            url: sUrl,
            data: JSON.stringify(jData),
            type: "post",
            async: (async | false) == 0 ? false : true,
            cache: false,
            dataType: dataType || 'json',
            contentType: "application/json;charset=utf-8",
            timeout: this.timeout,
            success: function (data) {
                func(data);
            },
            error: function (x, e, s) {
                document.write(x.responseText);
            },
            beforeSend: function () {
            },
            complete: function () {
            }
        })
    }
};