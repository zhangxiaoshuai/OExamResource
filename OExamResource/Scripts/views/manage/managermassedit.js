$(function () {
    var parameters = {};
    //选中的帐号ID
    parameters.guidlist = $("#hdGuidList").val();
    $.sync.ajax.post("/DataManage/Teachers", parameters, function (data) {
        if (data.State) {
            $("#listUser").setTemplateURL("/Scripts/template/manage/mass.htm");
            $("#listUser").processTemplate(data.Result);
        } else {
            $.dialog.tips("没有选中用户！");
            $("#btnSure").attr("disabled", "disabled");
            $("#btnInitialize").attr("disabled", "disabled");
            $("#btnPermission").attr("disabled", "disabled");
        }
    });

    parameters = {};
    parameters.facid = $("#ddlFaculty").val();
    $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
        if (data.State) {
            $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
            $("#ddlDepartment").processTemplate(data.Result);
        } else {
            $("#ddlDepartment").empty();
            $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
        }
    });

    //学校改变
    $("#ddlSchool").bind("change", function () {
        parameters.schoolid = $("#ddlSchool").val();
        $.sync.ajax.post("/DataManage/GetFaculty", parameters, function (data) {
            if (data.State) {
                $("#ddlFaculty").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFaculty").processTemplate(data.Result);
                parameters.facid = $("#ddlFaculty").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment").empty();
                        $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    }
                });
            } else {
                $("#ddlFaculty").empty();
                $("#ddlFaculty").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

        $.sync.ajax.post("/DataManage/GetRole", parameters, function (data) {
            if (data.State) {
                $("#ddlRole").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlRole").processTemplate(data.Result);
            } else {
                $("#ddlRole").empty();
                $("#ddlRole").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });

    //学院改变
    $("#ddlFaculty").bind("change", function () {
        parameters = {};
        parameters.facid = $("#ddlFaculty").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment").processTemplate(data.Result);
            } else {
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });
    //初始化密码
    $("#btnInitialize").bind("click", function () {
        parameters = {};
        parameters.guidlist = $("#hdGuidList").val();
        $.sync.ajax.post("/SystemManage/Initialize", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

    //修改权限


    //确定
    $("#btnSure").bind("click", function () {
        parameters = {};
        parameters.guidlist = $("#hdGuidList").val();
        parameters.school = $("#ddlSchool").val();
        parameters.faculty = $("#ddlFaculty").val();
        parameters.department = $("#ddlDepartment").val();
        if (parameters.faculty == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.department == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        parameters.role = $("#ddlRole").val();
        if (parameters.role == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加角色！");
            return false;
        }
        parameters.job = $("#ddlJob").val();
        parameters.pro = $("#ddlProfession").val();
        parameters.post = $("#ddlPost").val();
        $.sync.ajax.post("/SystemManage/MassEditTeacher", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                $("#btnEsc").click();
            }
        });
    });

    //取消
    $("#btnEsc").bind("click", function () {
        window.location.href = document.referrer;
    });
});