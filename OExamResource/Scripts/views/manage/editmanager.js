$(function () {
    var parameters = {};
    var sex = $("#hdSex").val();
    $(":radio[name='rSex']").each(function () {
        if ($(this).next("label").text() == sex) {
            $(this).attr("checked", true);
        }
    });

    //学校改变
    $("#ddlSchool").bind("change", function () {
        parameters = {};
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
        var list = [];
        list.push($("#hdId").val());
        parameters.guidlist = JSON.stringify(list);
        $.sync.ajax.post("/TeacherManage/Initialize", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

    //修改信息
    $("#btnInfoSure").bind("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $("#lb_name").text("真实姓名不能为空！");
            return false;
        }
        parameters.schoolid = $("#ddlSchool").val();
        parameters.birthday = $("#txtBirthday").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.departmentid = $("#ddlDepartment").val();
        if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.departmentid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        parameters.email = $("#txtEmail").val();
        parameters.phone = $("#txtPhone").val();
        parameters.sex = $(":radio[name='rSex']:checked").next("label").text().trim();
        parameters.role = $("#ddlRole").val();
        if (parameters.role == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加角色！");
            return false;
        }
        parameters.job = $("#ddlJob").val();
        parameters.post = $("#ddlPost").val();
        parameters.profession = $("#ddlProfession").val();
        $.sync.ajax.post("/SystemManage/EditManagerInfo", parameters, function (data) {
            if (data.State) {
                $("#lb_name").text("");
                $.dialog.tips(data.Msg);
                $("#btnEsc").click();
            };
        });
    });

    //取消
    $("#btnEsc").bind("click", function () {
        window.location.href = document.referrer;
    });
});