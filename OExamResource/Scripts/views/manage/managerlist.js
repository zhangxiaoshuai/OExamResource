$(function () {
    var parameters = {};

    //分页数据
    //用户信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 30,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.state = -1;
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetManagerList", parameters, function (data) {
            if (data.State) {
                $("#divUserList").setTemplateURL("/Scripts/template/manage/manager.htm");
                $("#divUserList").processTemplate(data.Result);
            }
        });
    });

    //搜索用户按钮
    $("#btnSearchManager").on("click", function () {
        parameters = {};
        parameters.state = $("#ddlState").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetManagerListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 30,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.state = $("#ddlState").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetManagerList", parameters, function (data) {
                        if (data.State) {
                            $("#divUserList").setTemplateURL("/Scripts/template/manage/manager.htm");
                            $("#divUserList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlState").bind("change", function () {
        $("#btnSearchManager").click();
    });

    //变色
    $("#divUserList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });

    //全选、取消
    $("#ckAll").click(function () {
        var state = $(this).attr("checked");
        $("#divUserList :checkbox").attr("checked", !!state);
    });

    //反选
    $("#ckReverse").click(function () {
        $("#divUserList :checkbox").each(function () {
            $(this).attr("checked", !$(this).attr("checked"));
        });
    });

    //所有启用
    $(".start").die().live("click", function () {
        if ($(this).attr("state") == "已启用") {
            return false;
        }
        var $btnStart = $(this);
        $.dialog.confirm("是否确认启用？", function () {
            parameters = {};
            parameters.id = $btnStart.attr("dataid");
            $.sync.ajax.post("/SystemManage/StartTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                }
            });
        });
    });

    //所有停用
    $(".stop").die().live("click", function () {
        if ($(this).attr("state") == "已停用") {
            return false;
        }
        var $btnStop = $(this);
        $.dialog.confirm("是否确认停用？", function () {
            parameters = {};
            parameters.id = $btnStop.attr("dataid");
            $.sync.ajax.post("/SystemManage/StopTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/DeleteTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });

    //单选操作
    $("#btnAffirm").bind("click", function () {
        parameters = {};
        parameters.ids = [];
        $("#divUserList").find(":checkbox:checked").each(function () {
            parameters.ids.push($(this).val());
        });
        parameters.aff = $("input[name='groupAff']:checked").val();
        if (parameters.ids.length > 0) {
            $.sync.ajax.post("/SystemManage/Affirm", parameters, function (data) {
                $("#ckAll").removeAttr("checked");
                $("#ckReverse").removeAttr("checked");
                $.dialog.tips(data.Msg);
                $("#btnSearchManager").click();
            });
        };
    });

    //批量修改
    $("#btnEditSelected").bind("click", function () {
        var data = [];
        $("#divUserList").find(":checkbox:checked").each(function () {
            data.push($(this).val());
        });
        if (data.length > 0) {
            $("#hdData").val(JSON.stringify(data));
            $("form").submit();
        }
    });

    //添加按钮
    $("#btnAddManager").bind("click", function () {
        $.dialog({
            content: document.getElementById('divAddManager'),
            id: 'EF893L'
        });
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

    //确认添加
    $("#btnAdd").bind("click", function () {
        parameters = {};
        var pwd = $("#txtSurePwd").val();
        parameters.name = $("#txtTrueName").val();
        parameters.loginName = $("#txtLoginName").val();
        parameters.loginPwd = $("#txtLoginPwd").val();
        parameters.schoolid = $("#ddlSchool").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.departmentid = $("#ddlDepartment").val();
        parameters.role = $("#ddlRole").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("真实姓名不能为空！");
            return false;
        } else if (parameters.loginPwd == 0) {
            $.dialog.tips("登录密码不能为空！");
            return false;
        } else if (parameters.loginName == 0) {
            $.dialog.tips("登录名称不能为空！");
            return false;
        } else if (parameters.loginPwd != pwd) {
            $.dialog.tips("两次密码不一致！");
            return false;
        } else if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.role == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加角色！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/AddManager", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                    $.dialog.list["EF893L"].close();
                    $.dialog.tips(data.Msg);
                };
            });
        }


    });

});