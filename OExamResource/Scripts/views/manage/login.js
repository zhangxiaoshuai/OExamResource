$(function () {
    $("#btnManagerLogin").bind("click", function () {
        var parameters = {};
        parameters.name = $("#txtManagerName").val();
        parameters.pwd = $("#txtManagerPwd").val();
        $.sync.ajax.post("/Admin/ManagerLogin", parameters, function (data) {
            if (!data.State) {
                $.dialog.tips(data.Msg);
            } else {
                window.location.href = data.Result;
            }
        });
    });

    $("body").keyup(function (evt) {
        evt.keyCode == 13 && $("#btnManagerLogin").click();
    });
});