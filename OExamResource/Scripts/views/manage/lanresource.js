$(function () {
    var parameters = {};

    //学校院系
    parameters.schoolid = $("#hdSchoolId").val();
    $.sync.ajax.post("/LanManage/GetSFD", parameters, function (data) {
        if (data.State) {
            $("#ddlSFD").setTemplateURL("/Scripts/template/manage/sfd.htm");
            $("#ddlSFD").processTemplate(data.Result);
        }
    });

    //基本资源分类
    parameters = {};
    $.sync.ajax.post("/LanManage/GetBase", parameters, function (data) {
        if (data.State) {
            $("#opt0").setTemplateURL("/Scripts/template/manage/ddl.htm");
            $("#opt0").processTemplate(data.Result);
        }
    });
    //扩展资源分类
    parameters = {};
    $.sync.ajax.post("/LanManage/GetExt", parameters, function (data) {
        if (data.State) {
            $("#opt1").setTemplateURL("/Scripts/template/manage/ddl.htm");
            $("#opt1").processTemplate(data.Result);
        }
    });
    //学校自定义资源分类 
    parameters = {};
    parameters.schoolid = $("#ddlSFD").val();
    $.sync.ajax.post("/LanManage/GetCustom", parameters, function (data) {
        if (data.State) {
            $("#opt2").setTemplateURL("/Scripts/template/manage/ddl.htm");
            $("#opt2").processTemplate(data.Result);
        }
    });
    $("#ddlSFD").bind("change", function () {
        parameters = {};
        parameters.sfd = $("#ddlSFD").val();
        $.sync.ajax.post("/LanManage/GetCourse", parameters, function (data) {
            if (data.State) {
                $("#ddlCourse").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlCourse").processTemplate(data.Result);
            }
        });
        parameters = {};
        parameters.schoolid = $("#ddlSFD").val();
        $.sync.ajax.post("/LanManage/GetCustom", parameters, function (data) {
            if (data.State) {
                $("#opt2").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#opt2").processTemplate(data.Result);
            }
        });
    });

    $("#ddlSFD,#ddlCourse,#ddlType").bind("change", function () {
        $("#btnSearchLanResource").click();
    });

    //分页数据
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 50,
        container: $("#divBar")
    }, function (pageindex) {
        //schoolId, courseId, typeId, key
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.key = "";
        parameters.schoolId = $("#ddlSFD").val();
        parameters.courseId = $("#ddlCourse").val();
        parameters.typeId = $("#ddlType").val();
        $.sync.ajax.post("/DataManage/GetLanResourceList", parameters, function (data) {
            if (data.State) {
                $("#divLanResourceList").setTemplateURL("/Scripts/template/manage/lanresource.htm");
                $("#divLanResourceList").processTemplate(data.Result);
            }
        });
    });

    //搜索按钮
    $("#btnSearchLanResource").bind("click", function () {
        parameters = {};
        parameters.key = $("#txtLanResource").val();
        parameters.schoolId = $("#ddlSFD").val();
        parameters.courseId = $("#ddlCourse").val();
        parameters.typeId = $("#ddlType").val();
        $.sync.ajax.post("/DataManage/GetLanResourceListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.key = $("#txtLanResource").val();
                    parameters.schoolId = $("#ddlSFD").val();
                    parameters.courseId = $("#ddlCourse").val();
                    parameters.typeId = $("#ddlType").val();
                    $.sync.ajax.post("/DataManage/GetLanResourceList", parameters, function (data) {
                        if (data.State) {
                            $("#divLanResourceList").setTemplateURL("/Scripts/template/manage/lanresource.htm");
                            $("#divLanResourceList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //全选、取消
    $("#ckAll").click(function () {
        var state = $(this).attr("checked");
        $("#divLanResourceList :checkbox").attr("checked", !!state);
    });

    //反选
    $("#ckReverse").click(function () {
        $("#divLanResourceList :checkbox").each(function () {
            $(this).attr("checked", !$(this).attr("checked"));
        });
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog({
            id: 'deleteResource',
            title: '删除资源',
            content: '是否确认删除？',
            button: [{
                name: '确定',
                callback: function () {
                    parameters = {};
                    parameters.guidlist = [];
                    parameters.guidlist.push($btnDel.attr("dataid"));
                    $.sync.ajax.post("/LanManage/DeleteResource", parameters, function (data) {
                        if (data.State) {
                            $.dialog.list["deleteResource"].close();
                            $("#btnSearchLanResource").click();
                            $.dialog.tips(data.Msg);
                        }
                    });
                    return false;
                },
                focus: true
            },
        {
            name: '取消'
        }]
        });
    });

    //删除
    $("#btnDelete").bind("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divLanResourceList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length <= 0) {
            $.dialog.tips("没有选中资源！");
            return false;
        };
        $.dialog({
            id: 'deleteResource',
            title: '删除资源',
            content: '是否确认删除？',
            button: [{
                name: '确定',
                callback: function () {
                    $.sync.ajax.post("/LanManage/DeleteResource", parameters, function (data) {
                        if (data.State) {
                            $.dialog.list["deleteResource"].close();
                            $("#btnSearchLanResource").click();
                            $.dialog.tips(data.Msg);
                        }
                    });
                    return false;
                },
                focus: true
            },
        {
            name: '取消'
        }]
        });
    });

    //变色
    $("#divLanResourceList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
});