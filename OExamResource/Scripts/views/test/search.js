$(function () {
    /*注册参数变量  初始化页面*/
    var parameters = {};
    /*获取上级*/
    parameters.id = $("#hdId").val();
    parameters.key = $("#txtSearch").val();
    $.sync.ajax.post("/Test/SearchSup", parameters, function (data) {
        if (data.State) {
            $("#divNav").setTemplateURL("/Scripts/template/test/searchsup.htm");
            $("#divNav").processTemplate(data.Result);
        }
    });
    /*获取下级*/
    parameters.tier = $("#hdTier").val();
    $.sync.ajax.post("/Test/SearchSub", parameters, function (data) {
        if (data.State) {
            $("#divSub").setTemplateURL("/Scripts/template/test/searchsub.htm");
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
        parameters.key = $("#txtSearch").val();
        parameters.pageIndex = pageindex;
        $.sync.ajax.post("/Test/SearchPage", parameters, function (data) {
            if (data.State) {
                $("#divPage").setTemplateURL("/Scripts/template/test/page.htm");
                $("#divPage").processTemplate(data.Result);
            }
        });
    });

    $("#A1").live("click", function () {
        parameters = {};
        $.sync.ajax.post("/Test/Down_Prompt", parameters, function (data) {
            if (data.State) {
                $.dialog.alert("购买正式版,请联系: 高等学校教学资源网  400-810-4002");
            }
        });
    });
});