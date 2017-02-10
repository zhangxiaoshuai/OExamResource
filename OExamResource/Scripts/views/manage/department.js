$(function () {
    var parameters = {};

    //分页数据
    //用户信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 10,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.schoolid = $("#hdSchoolId").val();
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetDepList", parameters, function (data) {
            if (data.State) {
                $("#divDepList").setTemplateURL("/Scripts/template/manage/dep.htm");
                $("#divDepList").processTemplate(data.Result);
            }
        });
    });

    //搜索院系按钮
    $("#btnSearchDep").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetDepListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 10,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.schoolid = $("#ddlSchool").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetDepList", parameters, function (data) {
                        if (data.State) {
                            $("#divDepList").setTemplateURL("/Scripts/template/manage/dep.htm");
                            $("#divDepList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlSchool").bind("change", function () {
        $("#btnSearchDep").click();
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/DeleteFacDep", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                }
                $.dialog.tips(data.Msg);

            });
        });
    });

    //所有修改
    $(".edit").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divEditDepFac'),
            id: 'EF89EE',
            title: '修改院系名称'
        });
        $("#txtEditDepFacName").val($(this).parent("td").prev("td").text().trim());
        $("#hdDepFacId").val($(this).attr("dataid"));
        $("#hdDepFacName").val($("#txtEditName").val());
    });

    //确认修改
    $("#btnEditDepFac").bind("click", function () {
        parameters = {};
        parameters.id = $("#hdDepFacId").val();
        parameters.name = $("#txtEditDepFacName").val();
        if (parameters.name == $("#hdDepName").val()) {
            $.dialog.tips("院系名称没有更改！");
            return false;
        }
        if (parameters.name.length != 0) {
            $.sync.ajax.post("/SystemManage/EditFacDep", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                }
                $.dialog.tips(data.Msg);
                var list = art.dialog.list;
                for (var i in list) {
                    list[i].close();
                };
            });
        } else {
            $.dialog.tips("院系名称不能为空！");
        };

    });

    //添加学院按钮
    $("#btnAddFac").bind("click", function () {
        $.dialog({
            content: document.getElementById('divAddFac'),
            id: 'EF8931',
            title: $("#ddlSchool :selected").text()
        });
    });
    //添加系别按钮
    $("#btnAddDep").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        $.sync.ajax.post("/DataManage/GetFacDep", parameters, function (data) {
            if (data.State) {
                $("#ddlFac").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFac").processTemplate(data.Result);
            } else {
                $("#ddlFac").empty();
                $("#ddlFac").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        $.dialog({
            content: document.getElementById('divAddDep'),
            id: 'EF893L',
            title: $("#ddlSchool :selected").text()
        });
    });

    //确认添加学院
    $("#btnSureFac").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.name = $("#txtNewFacName").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("学院名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/AddFac", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                    $.dialog.list["EF8931"].close();
                }
                $.dialog.tips(data.Msg);

            });
        }
    });

    //确认添加系别
    $("#btnSureDep").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.facultyid = $("#ddlFac").val();
        parameters.name = $("#txtNewDepName").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("学院名称不能为空！");
            return false;
        } else if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/AddDep", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                    $.dialog.list["EF893L"].close();
                }
                $.dialog.tips(data.Msg);

            });
        }
    });
    //变色
    $("#divDepList tr:gt(0),").die().live({
        mouseover: function () {
            $(this).css("background", "#D6DFF7");
            $("#divDepList td[class='rows']").css("background", "#799AE1");
        },
        mouseleave: function () {
            $(this).css("background", "");
            $("#divDepList td[class='rows']").css("background", "#799AE1");
        }
    });
});