$(function () {
    /*注册参数变量  初始化页面*/
    var parameters = {};
    /*获取上级*/
    parameters.id = $("#hdId").val();
    parameters.key = $("#txtSearch").val().trim();
    parameters.year = $("#ddlYear").val();
    parameters.mean = $("#ddlMean").val();
    $.sync.ajax.post("/Exam/SearchSup", parameters, function (data) {
        if (data.State) {
            $("#divNav").setTemplateURL("/Scripts/template/exam/searchnav.htm");
            $("#divNav").processTemplate(data.Result);
        }
    });
    /*获取下级*/
    parameters.tier = parseInt($("#hdTier").val()) + 1;
    $.sync.ajax.post("/Exam/SearchSub", parameters, function (data) {
        if (data.State) {
            $("#divSub").setTemplateURL("/Scripts/template/exam/searchsub.htm");
            $("#divSub").processTemplate(data.Result);
            delete parameters.tier;
        }
    });
    /*分页列表初始化*/
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 12,
        container: $("#divBar")
    }, function (pageindex) {
        parameters.pageIndex = pageindex;
        $.sync.ajax.post("/Exam/SearchPage", parameters, function (data) {
            if (data.State) {
                $("#divPage").setTemplateURL("/Scripts/template/exam/page.htm");
                $("#divPage").processTemplate(data.Result);
            }
        });
    });

    $(".show").live("click", function () {
        //parameters = {};
        $.sync.ajax.post("/Exam/show_type", parameters, function (data) {
            if (data.State) {
                $.dialog.alert("购买正式版,请联系: 高等学校教学资源网  400-810-4002");
            }
        });
    });

});