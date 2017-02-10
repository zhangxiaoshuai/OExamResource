$(function () {
    var parameters = {};
    //确定
    $("#btnAdd").bind("click", function () {
        parameters = {};
        parameters.kind = $("input[name='rKind']:checked").val();
        parameters.name = $("#txtName").val().trim();
        if (parameters.name.length <= 0) {
            return false;
        } else {
            $.sync.ajax.post("/TeacherManage/TeaKindAdd", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $("#lbError").text(data.Msg);
                }
            });
        }
    });
    //取消
    $("#btnEsc").bind("click", function () {
        window.location = "/TeacherManage/KindList";
    });

});