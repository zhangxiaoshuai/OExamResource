$(function () {
    var parameters = {};
    $.sync.ajax.post("/DataManage/GetStatistics", parameters, function (data) {
        if (data.State) {
            $("#stats").setTemplateURL("/Scripts/template/manage/stats.htm");
            $("#stats").processTemplate(data.Result);
        }
    });
});