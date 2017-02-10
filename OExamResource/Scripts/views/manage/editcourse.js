$(function () {
    var parameters = {};
    //一级改变二级
    $("#ddlSub1").bind("change", function () {
        parameters = {};
        parameters.sub1 = $("#ddlSub1").val();
        $.sync.ajax.post("/DataManage/GetSub2", parameters, function (data) {
            if (data.State) {
                $("#ddlSub2").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSub2").processTemplate(data.Result);
            }
        });
    });
    //确认
    $("#btnSure").bind("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtCourseName").val();
        parameters.author = $("#txtCourseAuthor").val();
        parameters.province = $("#txtCourseProvince").val();
        parameters.school = $("#txtCourseSchool").val();
        parameters.year = $("#ddlYears").val();
        parameters.sort = $("#ddlSort").val();
        parameters.sub1 = $("#ddlSub1").val();
        parameters.sub2 = $("#ddlSub2").val();
        parameters.hie = $("#ddlHie").val();
        parameters.connected = $("#ddlConn").val();
        parameters.address = $("#txtCourseAddress").val();
        if (parameters.name.length <= 0 || parameters.address.length <= 0) {
            $.dialog.tips("不能为空！");
            return false;
        }
        $.sync.ajax.post("/QualityManage/Edit", parameters, function (data) {
            if (data.State) {
                $.dialog.tips("修改成功！");
            }
        });
    });
    //返回
    $("#btnEsc").bind("click", function () {
        window.location.href = "/QualityManage/Quality";
    });

    //测试
    $("#btnTest").bind("click", function () {
        var url = $("#txtCourseAddress").val();
        window.open(url, "_blank");
    });
});