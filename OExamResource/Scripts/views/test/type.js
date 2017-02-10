$(function () {
    /*注册参数变量  初始化页面*/
    var parameters = {};
    /*获取上级*/
    parameters.id = $("#hdId").val();
    $.sync.ajax.post("/Test/SupList", parameters, function (data) {
        if (data.State) {
            $("#divNav").setTemplateURL("/Scripts/template/test/nav.htm");
            $("#divNav").processTemplate(data.Result);
        }
    });
    /*获取下级*/
    $.sync.ajax.post("/Test/SubList", parameters, function (data) {
        if (data.State) {
            $("#divSub").setTemplateURL("/Scripts/template/test/sub.htm");
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
        $.sync.ajax.post("/Test/PaperList", parameters, function (data) {
            if (data.State) {
                $("#divPage").setTemplateURL("/Scripts/template/test/page.htm");
                $("#divPage").processTemplate(data.Result);
            }
        });
    });

        $("#A1").live("click",function () {
        parameters = {};
        $.sync.ajax.post("/Test/Down_Prompt", parameters, function (data) {
            if (data.State) {
                $.dialog.alert("购买正式版,请联系: 高等学校教学资源网  400-810-4002");
            }
        });
    });
});