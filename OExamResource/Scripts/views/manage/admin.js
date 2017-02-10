$(function () {
    var parameters = {};
    var sex = $("#hdSex").val();
    $(":radio[name='rSex']").each(function () {
        if ($(this).next("label").text() == sex) {
            $(this).attr("checked", true);
        }
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

    //修改信息
    $("#btnInfoSure").bind("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $("#lb_name").text("真实姓名不能为空！");
            return false;
        }
        parameters.departmentid = $("#ddlDepartment").val();
        parameters.birthday = $("#txtBirthday").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.email = $("#txtEmail").val();
        parameters.phone = $("#txtPhone").val();
        parameters.sex = $(":radio[name='rSex']:checked").next("label").text().trim();
        parameters.job = $("#ddlJob").val();
        parameters.post = $("#ddlPost").val();
        parameters.profession = $("#ddlProfession").val();
        $.sync.ajax.post("/SystemManage/EditInfo", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            };
        });
    });

});