$(function () {
    var parameters = {};
    parameters.Id = "9cdc6d66-12be-4c63-b08f-506baf6b8ad8";
    $("#9cdc6d66-12be-4c63-b08f-506baf6b8ad8").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });

    //---------试题库列表---开始--------------------------
    //显示——试题库列表
    $("#Exam_List").click(function () {
        //试卷分类 1、2级
        $.sync.ajax.post("/DataManage/GetExamTypeList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/exam_list.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });

    $(".Exam_check").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("typeid");
        if (!$(this).attr("checked")) {
            $(this).attr("checked", false);
            parameters.isdeleted = true;
        } else {
            $(this).attr("checked", true);
            parameters.isdeleted = false;
        }
        $.sync.ajax.post("/ExamManage/DeleteCate", parameters, function (data) {
            if (data.State) {
                $("#Exam_List").click();
            }
        });
    });
    //点击分类
    $("#paperlist_exam").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("papid");
        $.sync.ajax.post("/ExamManage/paperlist_exam", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/exam_paperlist.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        var parameters = {};
        //试卷分类2级
        parameters.id = $("#hdId").val();
        $.sync.ajax.post("/DataManage/GetExamType2", parameters, function (data) {
            if (data.State) {
                $("#tdExamType1").setTemplateURL("/Scripts/template/manage/examtype1.htm");
                $("#tdExamType1").processTemplate(data.Result);
            }
        });
        //试卷分类3级
        parameters = {};
        parameters.id = $("#hdId").val();
        $.sync.ajax.post("/DataManage/GetExamType3", parameters, function (data) {
            if (data.State) {
                $("#tdExamType2").setTemplateURL("/Scripts/template/manage/examtype2.htm");
                $("#tdExamType2").processTemplate(data.Result);
            }
        });
        $("#btnSearchExamPaper").click();
    });
    $("#exam1").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("ids");
        $.sync.ajax.post("/ExamManage/paperlist_exam", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/exam_paperlist.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        var parameters = {};
        //试卷分类2级
        parameters.id = $("#hdId").val();
        $.sync.ajax.post("/DataManage/GetExamType2", parameters, function (data) {
            if (data.State) {
                $("#tdExamType1").setTemplateURL("/Scripts/template/manage/examtype1.htm");
                $("#tdExamType1").processTemplate(data.Result);
            }
        });
        //试卷分类3级
        parameters = {};
        parameters.id = $("#hdId").val();
        $.sync.ajax.post("/DataManage/GetExamType3", parameters, function (data) {
            if (data.State) {
                $("#tdExamType2").setTemplateURL("/Scripts/template/manage/examtype2.htm");
                $("#tdExamType2").processTemplate(data.Result);
            }
        });
        $("#btnSearchExamPaper").click();
    });


    //    $("#tdExamType1 a").each(function () {
    //        $("#" + $("#hdId").val()).css("color", "white");
    //    });



    //2级试卷列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 20,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtPaperName").val();
        $.sync.ajax.post("/DataManage/GetPaperList2", parameters, function (data) {
            if (data.State) {
                $("#divPaperList").setTemplateURL("/Scripts/template/manage/paperlist.htm");
                $("#divPaperList").processTemplate(data.Result);
            }
        });
    });
    //3级分类试卷
    $(".typeA").die().live("click", function () {
        var $a = $(this);
        $("#hdType3Id").val($a.attr("id"));
        parameters = {};
        parameters.id = $a.attr("id");
        parameters.name = $("#txtPaperName").val();
        $.sync.ajax.post("/DataManage/GetPaperListCount3", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 20,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.id = $a.attr("id");
                    parameters.name = $("#txtPaperName").val();
                    $.sync.ajax.post("/DataManage/GetPaperList3", parameters, function (data) {
                        if (data.State) {
                            $("#divPaperList").setTemplateURL("/Scripts/template/manage/paperlist.htm");
                            $("#divPaperList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //搜索
    $("#btnSearchExamPaper").die().live("click", function () {
        var countUrl = "GetPaperListCount2";
        var listUrl = "GetPaperList2";
        var id = $("#hdId").val();
        parameters = {};
        if ($("#hdType3Id").val().length == 0) {
            id = $("#hdId").val();
        } else {
            id = $("#hdType3Id").val();
            countUrl = "GetPaperListCount3";
            listUrl = "GetPaperList3";
        }
        parameters.id = id;
        parameters.name = $("#txtPaperName").val();
        $.sync.ajax.post("/DataManage/" + countUrl, parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 20,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.id = id;
                    parameters.name = $("#txtPaperName").val();
                    $.sync.ajax.post("/DataManage/" + listUrl, parameters, function (data) {
                        if (data.State) {
                            $("#divPaperList").setTemplateURL("/Scripts/template/manage/paperlist.htm");
                            $("#divPaperList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //变色
    $("#divPaperList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    /*删除按钮*/
    $(".del_exam").die().live("click", function () {
        parameters.id = $(this).attr("dataid");
        $.dialog({
            id: 'deletePaper',
            content: '是否确认删除，删除后无法恢复？',
            button: [
        {
            name: '确定',
            callback: function () {
                $.sync.ajax.post("/ExamManage/DeletePaper", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips(data.Msg);
                        $("#btnSearchExamPaper").click();
                    }
                });
            },
            focus: true
        },
        {
            name: '取消'
        }
    ]
        });
    });
    //---------试题库列表---结束--------------------------
    //---------试题库更新---开始--------------------------
    $("#Exam_Update").click(function () {
        //试卷分类 1、2级
        $.sync.ajax.post("/ExamManage/Exam_Update", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/exam_update.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    $("#btnImport_exam").die().live("click", function () {
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
    //---------试题库更新---结束--------------------------
});