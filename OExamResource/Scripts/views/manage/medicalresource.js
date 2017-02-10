$(function () {
    var parameters = {};
    //分类改变选择素材专业
    $("#ddlCate").bind("change", function () {
        parameters = {};
        parameters.cateid = $("#ddlCate").val();
        $.sync.ajax.post("/DataManage/GetMedicalSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlSpe").processTemplate(data.Result);
            } else {
                $("#ddlSpe").empty();
                $("#ddlSpe").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        $("#btnSearchResource").click();
    });

    $("#ddlCate,#ddlType,#ddlSpe").bind("change", function () {
        $("#btnSearchResource").click();
    });

    //分页数据
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 50,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.name = "";
        parameters.categoryid = $("#ddlCate").val();
        parameters.specialityid = $("#ddlSpe").val();
        parameters.typeid = $("#ddlType").val();
        $.sync.ajax.post("/DataManage/GetMedicalResourceList", parameters, function (data) {
            if (data.State) {
                $("#divResourceList").setTemplateURL("/Scripts/template/manage/medicalresource.htm");
                $("#divResourceList").processTemplate(data.Result);
            }
        });
    });

    //搜索按钮
    $("#btnSearchResource").bind("click", function () {
        parameters = {};
        parameters.categoryid = $("#ddlCate").val();
        parameters.specialityid = $("#ddlSpe").val();
        parameters.typeid = $("#ddlType").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetMedicalResourceListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.categoryid = $("#ddlCate").val();
                    parameters.specialityid = $("#ddlSpe").val();
                    parameters.typeid = $("#ddlType").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetMedicalResourceList", parameters, function (data) {
                        if (data.State) {
                            $("#divResourceList").setTemplateURL("/Scripts/template/manage/medicalresource.htm");
                            $("#divResourceList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //全选、取消
    $("#ckAll").click(function () {
        var state = $(this).attr("checked");
        $("#divResourceList :checkbox").attr("checked", !!state);
    });

    //反选
    $("#ckReverse").click(function () {
        $("#divResourceList :checkbox").each(function () {
            $(this).attr("checked", !$(this).attr("checked"));
        });
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($btnDel.attr("dataid"));
        $.dialog({
            id: 'deleteResource',
            content: '是否将资源放入资源审核？',
            button: [
        {
            name: '确定',
            callback: function () {
                parameters.flag = "softdelete";
                $.sync.ajax.post("/MedicalManage/DeleteResource", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经放入资源审核!");
                        $.dialog.list["deleteResource"].close();
                        $("#btnSearchResource").click();
                    }
                });
            },
            focus: true
        },
        {
            name: '直接删除',
            callback: function () {
                if (confirm("是否彻底删除资源?")) {
                    parameters.flag = "delete";
                    $.sync.ajax.post("/MedicalManage/DeleteResource", parameters, function (data) {
                        if (data.State) {
                            $.dialog.tips("资源已经被删除!");
                            $.dialog.list["deleteResource"].close();
                            $("#btnSearchResource").click();
                        }
                    });
                }
            }
        },
        {
            name: '取消'
        }
    ]
        });
    });

    //删除选择
    $("#btnDelete").bind("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divResourceList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length <= 0) {
            $.dialog.tips("没有选中素材！");
            return false;
        };
        $.dialog({
            id: 'deleteResources',
            content: '是否将资源放入资源审核？',
            button: [
        {
            name: '确定',
            callback: function () {
                parameters.flag = "softdelete";
                $.sync.ajax.post("/MedicalManage/DeleteResource", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经放入资源审核!");
                        $.dialog.list["deleteResources"].close();
                        $("#btnSearchResource").click();
                    }
                });
            },
            focus: true
        },
        {
            name: '直接删除',
            callback: function () {
                if (confirm("是否彻底删除资源?")) {
                    parameters.flag = "delete";
                    $.sync.ajax.post("/MedicalManage/DeleteResource", parameters, function (data) {
                        if (data.State) {
                            $.dialog.tips("资源已经被删除!");
                            $.dialog.list["deleteResource"].close();
                            $("#btnSearchResource").click();
                        }
                    });
                }
            }
        },
        {
            name: '取消'
        }
    ]
        });

    });

    //批量修改
    $("#btnEditSelected").bind("click", function () {
        var data = [];
        $("#divResourceList").find(":checkbox:checked").each(function () {
            data.push($(this).val());
        });
        if (data.length > 0) {
            $("#hdData").val(JSON.stringify(data));
            $("#form1").submit();
        } else {
            $.dialog.tips("没有选中素材！");
        }
    });

    //预览
    $(".preview").die().live("click", function () {
        var data = [];
        var id = $(this).attr("id");
        $("#divResourceList :checkbox[value='" + id + "']").attr("checked", "checked");
        $("#divResourceList").find(":checkbox:checked").each(function () {
            data.push($(this).val());
        });
        $("#hdData2").val(JSON.stringify(data));
        $("#form2").submit();

    });
    //变色
    $("#divResourceList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
});