 $(function () {
    var parameters = {};

    //ȷ��
    $("#btnSure").bind("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtName").val().trim();
        if (parameters.name.length <= 0) {
            return false;
        } else {
            $.sync.ajax.post("/TeacherManage/EditProfession", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#btnEsc").click();
                }
            });
        }
    });

    //ȡ��
    $("#btnEsc").bind("click", function () {
        window.location = "/TeacherManage/KindList";
    });

});
