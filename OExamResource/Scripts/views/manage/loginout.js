$(function () {
    $("#aLoginOut").click(function () {
        parameters = {};
        $.sync.ajax.post("/Admin/LoginOut", parameters, function (data) {
            if (data.State) {
                window.location.href = "/Admin/Login";
            }
        });
    });
});