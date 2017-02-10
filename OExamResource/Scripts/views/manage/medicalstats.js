$(function () {
    var parameters = {};
    $.sync.ajax.post("/DataManage/GetMedicalStatistics", parameters, function (data) {
        if (data.State) {
            $("#stats").setTemplateURL("/Scripts/template/manage/medicalstats.htm");
            $("#stats").processTemplate(data.Result);
        }
    });
});