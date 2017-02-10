$(function () {
    /*注册参数变量  初始化页面*/
    var parameters = {};
    /*获取上级*/
    parameters.id = $("#hdId").val();
    $.sync.ajax.post("/Exam/SupList", parameters, function (data) {
        if (data.State) {
            $("#divNav").setTemplateURL("/Scripts/template/exam/nav.htm");
            $("#divNav").processTemplate(data.Result);
        }
    });
    /*获取下级*/
    parameters.tier = parseInt($("#hdTier").val()) + 1;
    $.sync.ajax.post("/Exam/SubList", parameters, function (data) {
        if (data.State) {
            $("#divSub").setTemplateURL("/Scripts/template/exam/sub.htm");
            $("#divSub").processTemplate(data.Result);
        }
    });
    /*分页列表初始化*/
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 12,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.pageIndex = pageindex;
        $.sync.ajax.post("/Exam/PaperList", parameters, function (data) {
            if (data.State) {
                $("#divPage").setTemplateURL("/Scripts/template/exam/page.htm");
                $("#divPage").processTemplate(data.Result);
            }
        });
    });
    /*全部按钮，跳转到第一页*/
    $("#lkAll").on("click", function () {
        $.cellpagebar.init({
            total: $("#hdCount").val(),
            pagesize: 12,
            container: $("#divBar")
        }, function (pageindex) {
            parameters = {};
            parameters.id = $("#hdId").val();
            parameters.pageIndex = pageindex;
            $.sync.ajax.post("/Exam/PaperList", parameters, function (data) {
                if (data.State) {
                    $("#divPage").setTemplateURL("/Scripts/template/exam/page.htm");
                    $("#divPage").processTemplate(data.Result);
                }
            });
        });
    });
    $("#lkOther").on("click", function () {
        $.cellpagebar.init({
            total: $(this).attr("dataCount"),
            pagesize: 12,
            container: $("#divBar")
        }, function (pageindex) {
            parameters = {};
            parameters.id = $("#hdId").val();
            parameters.pageIndex = pageindex;
            $.sync.ajax.post("/Exam/OtherPaperList", parameters, function (data) {
                if (data.State) {
                    $("#divPage").setTemplateURL("/Scripts/template/exam/page.htm");
                    $("#divPage").processTemplate(data.Result);
                }
            });
        });
    });

    $(".show").live("click", function () {
        parameters = {};
        $.sync.ajax.post("/Exam/show_type", parameters, function (data) {
            if (data.State) {
                $.dialog.alert("购买正式版,请联系: 高等学校教学资源网  400-810-4002");
            }
        });
    });
});