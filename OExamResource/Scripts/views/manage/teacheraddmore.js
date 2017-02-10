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
    //院系改变
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

    //生成
    $("#btnCreate").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.departmentid = $("#ddlDepartment").val();
        if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.departmentid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        var prefix = $("#txtPrefix").val().trim();
        var count = $("#txtCount").val().trim();
        var regP = /^[A-Za-z0-9_]+$/;
        var regC = /^[1-9][0-9]*$/;
        if (regP.test(prefix) && prefix.length > 0) {
            parameters.prefix = prefix;
            $("#lbPreMsg").text("");
        } else {
            $("#lbPreMsg").text("错误字符！");
        }
        if (regC.test(count) && count.length > 0) {
            if (count > 200) {
                $.dialog.alert("输入200以内数字！");
                return false;
            } else {
                parameters.count = count;
                $("#lbCouMsg").text("");
            }
        } else {
            $("#lbCouMsg").text("错误数字！");
            return false;
        }
        $.sync.ajax.post("/TeacherManage/AddMoreTeacher", parameters, function (data) {
            if (data.State) {
                art.dialog({
                    id: 'testID',
                    content: data.Msg,
                    button: [
        {
            name: '确定',
            callback: function () {
                window.location = "/TeacherManage/TeacherList";
            },
            focus: true
        }
    ]
                });
            };
        });
    });

});