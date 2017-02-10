$(function () {
    var parameters = {};

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
                parameters = {};
                parameters.facid = $("#ddlFaculty").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment").empty();
                        $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    };
                });
            } else {
                $("#ddlFaculty").empty();
                $("#ddlFaculty").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            };
        });
    });

    //学院改变
    $("#ddlFaculty").bind("change", function () {
        parameters.facid = $("#ddlFaculty").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment").processTemplate(data.Result);
            } else {
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            };
        });
    });

    //添加教师
    $("#btnAdd").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.loginname = $("#txtLoginName").val();
        if (parameters.loginname.trim().length <= 0) {
            $.dialog.tips("登录名不能为空！");
            return false;
        }
        if ($("#txtLoginPwd").val() != $("#txtSurePwd").val()) {
            $.dialog.tips("两次密码不一致！");
            return false;
        }
        parameters.pwd = $("#txtLoginPwd").val();
        if (parameters.pwd.trim().length <= 0) {
            $.dialog.tips("密码不能为空！");
            return false;
        }
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $.dialog.tips("真实姓名不能为空！");
            return false;
        }
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
        parameters.sex = $("input[name='rSex']:checked").next("label").text().trim();
        parameters.jobid = $("#ddlJob").val();
        parameters.postid = $("#ddlPost").val();
        parameters.professionid = $("#ddlProfession").val();
        $.sync.ajax.post("/TeacherManage/AddTeacher", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                window.location.href = "/TeacherManage/TeacherList";
            };
        });
    });

});