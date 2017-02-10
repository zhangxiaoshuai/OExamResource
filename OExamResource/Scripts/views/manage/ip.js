$(function () {

    var parameters = {};

    //刷新
    $("#btnRe").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#hdSchoolId").val();
        $.sync.ajax.post("/DataManage/GetIpList", parameters, function (data) {
            if (data.State) {
                $("#txtIP").val(data.Result);
            }
        });
    });


    $("#btnIpEdit").toggle(function () {
        $(this).val("取消");
        $("#txtIP").removeAttr("disabled");
    }, function () {
        $(this).val("修改");
        $("#btnRe").click();
        $("#txtIP").attr("disabled", "disabled");
    });

    $("#btnSure").bind("click", function () {
        if ($("#txtIP").attr("disabled") != "disabled") {
            parameters = {};
            parameters.id = $("#hdSchoolId").val();
            parameters.ips = $("#txtIP").val().replace("，", ",");
            $.sync.ajax.post("/SystemManage/EditIp", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#txtIP").attr("disabled", "disabled");
                    $("#btnIpEdit").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
                $("#btnRe").click();
            });
        } else {
            return false;
        }
    });
});