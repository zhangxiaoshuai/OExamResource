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
        $.sync.ajax.post("/TeacherManage/Initialize", parameters, function (data) {
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
        parameters.job = $("#ddlJob").val();
        parameters.pro = $("#ddlProfession").val();
        parameters.post = $("#ddlPost").val();
        $.sync.ajax.post("/TeacherManage/MassEditTeacher", parameters, function (data) {
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