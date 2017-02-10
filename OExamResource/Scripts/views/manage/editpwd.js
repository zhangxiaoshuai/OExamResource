$(function () {

    //修改密码
    $("#btnPwdSure").bind("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.oldpwd = $("#txtOldPwd").val();
        parameters.newpwd = $("#txtNewPwd").val();
        var newPwdSure = $("#txtNewPwdSure").val();
        if (parameters.oldpwd.length <= 0) {
            $("#lb_old").text("原密码不能为空！");
            return false;
        }
        if (parameters.newpwd.length <= 0) {
            $("#lb_new").text("新密码不能为空！");
            return false;
        }
        if (parameters.newpwd != newPwdSure) {
            $("#lb_sure").text("两次密码不一致！");
            return false;
        }
        $.sync.ajax.post("/SystemManage/EditPwd", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                window.location.href = "/SystemManage/Admin";
            } else {
                $.dialog.tips(data.Msg);
            }
        });

    });
});
