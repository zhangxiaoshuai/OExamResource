$(function () {
    var parameters = {};

    //模块列表
    parameters = {};
    parameters.id = $("#hdId").val();
    $.sync.ajax.post("/DataManage/GetModules", parameters, function (data) {
        if (data.State) {
            $("#divModules").setTemplateURL("/Scripts/template/manage/module.htm");
            $("#divModules").processTemplate(data.Result);
        }
    });

    //学校IP段
    parameters = {};
    parameters.schoolid = $("#hdId").val();
    $.sync.ajax.post("/DataManage/GetIpList", parameters, function (data) {
        if (data.State) {
            $("#txtIP").val(data.Result);
        }
    });



    //确认
    $("#btnEditSchool").bind("click", function () {
        var parameters = {};
        if ($("#txtSchoolName").val().length == 0) {
            $.dialog.tips("学校名称不能为空！");
            return false;
        }
        if ($("#txtEndTime").val().length == 0) {
            $.dialog.tips("到期时间不能为空！");
            return false;
        }
        parameters.id = $("#hdId").val();
        parameters.schoolname = $("#txtSchoolName").val();
        parameters.ids = [];
        $("#divModules").find(":checkbox:checked").each(function () {
            parameters.ids.push($(this).val());
        });
        parameters.ips = $("#txtIP").val().replace("，", ",");
        parameters.end = $("#txtEndTime").val();
        parameters.remind = $("#txtRemind").val();
        parameters.sellid = $("#ddlSeller").val();
        parameters.trial = $("#ddlTrial").val();
        $.sync.ajax.post("/SystemManage/EditSchool", parameters, function (data) {
            if (!!data.State) {
                $.dialog.tips(data.Msg);
                $("#btnEsc").click();
            } else {
                parameters = {};
                parameters.schoolid = $("#hdId").val();
                $.sync.ajax.post("/DataManage/GetIpList", parameters, function (data) {
                    if (data.State) {
                        $("#txtIP").val(data.Result);
                    }
                });
                $.dialog.alert(data.Msg);
            }

        });
    });

    //取消
    $("#btnEsc").bind("click", function () {
        window.location.href = "/SystemManage/SchoolList";
    });
})