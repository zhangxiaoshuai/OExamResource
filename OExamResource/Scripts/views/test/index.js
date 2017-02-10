$(function () {
    var parameters = {};
    $.sync.ajax.post("/Test/GetData", parameters, function (data) {
        if (data.State) {
            $("#full").setTemplateURL("/Scripts/template/test/index.htm");
            $("#full").processTemplate(data.Result);
        }
    });
});