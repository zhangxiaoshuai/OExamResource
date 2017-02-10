$(function () {
    var parameters = {};
    parameters.Id = "367a325f-d476-499f-9f42-0639b2944d47";
    $("#367a325f-d476-499f-9f42-0639b2944d47").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });
    parameters = {};
    $("#Quality_Quality").click(function () {
        $.sync.ajax.post("/QualityManage/Quality_Quality", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Quality_Quality.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        $("#btnSearchCourse_quality").click();
    });


    var parameters = {};
    //分页数据
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 50,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.name = "";
        parameters.sort = "-1";
        parameters.year = "-1";
        parameters.conn = -1;
        $.sync.ajax.post("/DataManage/GetQualityList", parameters, function (data) {
            if (data.State) {
                $("#divCrouseList").setTemplateURL("/Scripts/template/manage/course.htm");
                $("#divCrouseList").processTemplate(data.Result);
            }
        });
    });

    //搜索按钮
    $("#btnSearchCourse_quality").die().live("click", function () {
        parameters = {};
        parameters.name = $("#txtCourseName").val();
        parameters.sort = $("#ddlSort_quality").val();
        parameters.year = $("#ddlYears_quality").val();
        parameters.conn = $("#ddlConn_quality").val();
        $.sync.ajax.post("/DataManage/GetQualityListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.name = $("#txtCourseName").val();
                    parameters.sort = $("#ddlSort_quality").val();
                    parameters.year = $("#ddlYears_quality").val();
                    parameters.conn = $("#ddlConn_quality").val();
                    $.sync.ajax.post("/DataManage/GetQualityList", parameters, function (data) {
                        if (data.State) {
                            $("#divCrouseList").setTemplateURL("/Scripts/template/manage/course.htm");
                            $("#divCrouseList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlSort_quality,#ddlYears_quality,#ddlConn_quality").die().live("change", function () {
        $("#btnSearchCourse_quality").click();
    });


    //全选、取消
    $("#ckAll_quality").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divCrouseList :checkbox").attr("checked", !!state);
    });
    ///修改 -----------------------------------------
    //修改
    $(".edit_quality").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/QualityManage/EditCourse", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/quality_editcource.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    var parameters = {};
    //一级改变二级
    $("#ddlSub1").die().live("change", function () {
        parameters = {};
        parameters.sub1 = $("#ddlSub1").val();
        $.sync.ajax.post("/DataManage/GetSub2", parameters, function (data) {
            if (data.State) {
                $("#ddlSub2").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSub2").processTemplate(data.Result);
            }
        });
    });
    //确认
    $("#btnSure").die().live("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtCourseName").val();
        parameters.author = $("#txtCourseAuthor").val();
        parameters.province = $("#txtCourseProvince").val();
        parameters.school = $("#txtCourseSchool").val();
        parameters.year = $("#ddlYears").val();
        parameters.sort = $("#ddlSort").val();
        parameters.sub1 = $("#ddlSub1").val();
        parameters.sub2 = $("#ddlSub2").val();
        parameters.hie = $("#ddlHie").val();
        parameters.connected = $("#ddlConn").val();
        parameters.address = $("#txtCourseAddress").val();
        if (parameters.name.length <= 0 || parameters.address.length <= 0) {
            $.dialog.tips("不能为空！");
            return false;
        }
        $.sync.ajax.post("/QualityManage/Edit", parameters, function (data) {
            if (data.State) {
                $.dialog.tips("修改成功！");
            }
        });
    });
    //返回
    $("#btnEsc").die().live("click", function () {
        $("#Quality_Quality").click();
    });

    //测试
    $("#btnTest").die().live("click", function () {
        var url = $("#txtCourseAddress").val();
        window.open(url, "_blank");
    });

    ///////////////////////////////////////////////
    //所有删除
    $(".del_quality").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($(this).attr("dataid"));
        if (parameters.guidlist.length <= 0) {
            return false;
        };
        $.sync.ajax.post("/QualityManage/DeleteCourse", parameters, function (data) {
            if (data.State) {
                $.dialog.tips("删除成功！");
                $("#btnSearchCourse_quality").click();
            }
        });
    });

    //删除选择
    $("#btnDelete_quality").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divCrouseList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length <= 0) {
            $.dialog.tips("没有选中课程！");
            return false;
        };
        $.sync.ajax.post("/QualityManage/DeleteCourse", parameters, function (data) {
            if (data.State) {
                $.dialog.tips("删除成功！");
                $("#btnSearchCourse_quality").click();
            }
        });
    });

    //变色
    $("#divCrouseList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });

    //////////课程更新开始////////////// /
    
    parameters = {};
    $("#Quality_Update").click(function () {
        $.sync.ajax.post("/QualityManage/Quality_Update", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Quality_Update.htm");
                $("#divdata").processTemplate(data.Result);
            }
        }); 
    });
    $("#btnImport_update").die().live("click", function () {
        if ($("#txtSheet").val().trim().length <= 0) {
            $.dialog.alert("请正确填写！");
            return false;
        }
        if ($("#upXls").val().trim().length <= 0) {
            $.dialog.alert("请正确选择文件！");
            return false;
        }
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                $.dialog.alert(data.Msg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_update").ajaxForm(options);
    });
});