$(function () {
    var parameters = {};
    //试卷分类2级
    parameters.id = $("#hdId").val();
    $.sync.ajax.post("/DataManage/GetExamType2", parameters, function (data) {
        if (data.State) {
            $("#tdExamType1").setTemplateURL("/Scripts/template/manage/examtype1.htm");
            $("#tdExamType1").processTemplate(data.Result);
        }
    });

    $("#tdExamType1 a").each(function () {
        $("#" + $("#hdId").val()).css("color", "white");
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
    $(".typeA").bind("click", function () {
        var $a = $(this);
        $("#tdExamType2 a").removeAttr("style");
        $a.css("color", "white");
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
    $("#btnSearchExamPaper").bind("click", function () {
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
    $("#divPaperList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    /*删除按钮*/
    $(".del").die().live("click", function () {
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


});