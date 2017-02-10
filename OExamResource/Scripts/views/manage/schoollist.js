$(function () {
    var parameters = {};

    //模块列表
    $.sync.ajax.post("/DataManage/GetModuleList", parameters, function (data) {
        if (data.State) {
            $("#divModules").setTemplateURL("/Scripts/template/manage/module.htm");
            $("#divModules").processTemplate(data.Result);
        }
    });

    //分页数据
    //学校信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 10,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.sellerid = "00000000-0000-0000-0000-000000000000";
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetSchoolInfoList", parameters, function (data) {
            if (data.State) {
                $("#divSchoolList").setTemplateURL("/Scripts/template/manage/school.htm");
                $("#divSchoolList").processTemplate(data.Result);
            }
        });
    });

    $("#ddlSeller").bind("change", function () {
        $("#btnSearchSchool").click();
    });

    //添加按钮
    $("#btnAdd").bind("click", function () {
        art.dialog({
            title: "添加试用学校",
            content: document.getElementById('divAddSchool'),
            id: 'EF893L'
        });
    });



    //确认添加学校按钮
    $("#btnAddSchool").bind("click", function () {
        var parameters = {};
        if ($("#txtSchoolName").val().length == 0) {
            $.dialog.tips("学校名称不能为空！");
            return false;
        }
        if ($("#txtEndTime").val().length == 0) {
            $.dialog.tips("到期时间不能为空！");
            return false;
        }
        parameters.schoolname = $("#txtSchoolName").val();
        parameters.ids = [];
        $("#divModules").find(":checkbox:checked").each(function () {
            parameters.ids.push($(this).val());
        });
        parameters.ips = $("#txtIP").val().replace("，", ",");
        parameters.end = $("#txtEndTime").val();
        parameters.remind = $("#txtRemind").val();
        parameters.tier = $("#ddlTier").val();
        parameters.sell = $("#ddlSell").val();
        parameters.trial = $("#ddlTrial").val();
        if (parameters.schoolname.length > 0) {
            $.sync.ajax.post("/SystemManage/AddSchool", parameters, function (data) {
                if (!!data.State) {
                    $.dialog.tips(data.Msg);
                    $("#res").click();
                    $.dialog.list["EF893L"].close();
                    $("#btnSearchSchool").click();
                    $("#sSchoolName").text("").removeCss("color");
                } else {
                    $("#txtIP").val(data.Result);
                    $.dialog.alert(data.Msg);
                }
            });

        } else {
            $("#sSchoolName").text("不能为空！").css("color", "red");
        };
    });


    //删除学校按钮
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/DeleteSchool", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#btnSearchSchool").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });


    //搜索学校按钮
    $("#btnSearchSchool").bind("click", function () {
        var parameters = {};
        parameters.sellerid = $("#ddlSeller option:selected").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetSchoolInfoListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 10,
                    container: $("#divBar")
                }, function (pageindex) {
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.sellerid = $("#ddlSeller").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetSchoolInfoList", parameters, function (data) {
                        if (data.State) {
                            $("#divSchoolList").setTemplateURL("/Scripts/template/manage/school.htm");
                            $("#divSchoolList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //变色
    $("#divSchoolList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
})