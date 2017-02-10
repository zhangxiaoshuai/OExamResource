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
        $.sync.ajax.post("/DataManage/GetUserList", parameters, function (data) {
            if (data.State) {
                $("#divUserList").setTemplateURL("/Scripts/template/manage/user.htm");
                $("#divUserList").processTemplate(data.Result);
            }
        });
    });

    //搜索用户按钮
    $("#btnSearchUser").bind("click", function () {
        parameters = {};
        parameters.state = $("#ddlState").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetUserListCount", parameters, function (data) {
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
                    $.sync.ajax.post("/DataManage/GetUserList", parameters, function (data) {
                        if (data.State) {
                            $("#divUserList").setTemplateURL("/Scripts/template/manage/user.htm");
                            $("#divUserList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlState").bind("change", function () {
        $("#btnSearchUser").click();
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
            $.sync.ajax.post("/TeacherManage/StartTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchUser").click();
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
            $.sync.ajax.post("/TeacherManage/StopTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchUser").click();
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
            $.sync.ajax.post("/TeacherManage/DeleteTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchUser").click();
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
            $.sync.ajax.post("/TeacherManage/Affirm", parameters, function (data) {
                $("#ckAll").removeAttr("checked");
                $("#ckReverse").removeAttr("checked");
                $.dialog.tips(data.Msg);
                $("#btnSearchUser").click();
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

    //变色
    $("#divUserList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });

});