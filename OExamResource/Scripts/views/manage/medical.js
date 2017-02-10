$(function () {
    var parameters = {};
    parameters.Id = "bc10713f-58c7-4c91-9384-c29b26f6087d";
    $("#bc10713f-58c7-4c91-9384-c29b26f6087d").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });


});