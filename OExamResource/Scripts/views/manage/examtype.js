$(function () {
    var parameters = {};
    //试卷分类 1、2级
    $.sync.ajax.post("/DataManage/GetExamTypeList", parameters, function (data) {
        if (data.State) {
            $("#tabExamType").setTemplateURL("/Scripts/template/manage/examtypelist.htm");
            $("#tabExamType").processTemplate(data.Result);
        }
    });
});