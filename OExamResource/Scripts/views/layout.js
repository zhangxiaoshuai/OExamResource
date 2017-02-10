var datareginit;
function bindSchool(e) {
    var html = "<option value='00000000-0000-0000-0000-000000000000'>请选择</option>";
    switch ($(e).attr("id")) {
        case "ddlSchool0":
            var _school = datareginit[e.selectedIndex].List;
            for (var i = 0; i < _school.length; i++) {
                html += "<option value='" + _school[i].Id + "'>" + _school[i].Name + "</option>";
            }
            $("#ddlSchool1").html(html).change();
            break;
        case "ddlSchool1":
            if (e.selectedIndex > 0) {
                var _school = datareginit[$("#ddlSchool0").get(0).selectedIndex].List[e.selectedIndex - 1].List;
                for (var i = 0; i < _school.length; i++) {
                    html += "<option value='" + _school[i].Id + "'>" + _school[i].Name + "</option>";
                }
            }
            $("#ddlSchool2").html(html);
            break;
        default:
    }
}
$(function () {
    var layoutparm = {};
    function sync_login() {
        layoutparm = {};
        layoutparm.name = $("#txtLoginName").val();
        layoutparm.pwd = $("#txtLoginPwd").val();
        layoutparm.path = window.location.pathname.split("/")[1] || "";
        if (layoutparm.name && layoutparm.pwd) {
            $.sync.ajax.post("/Account/Login", layoutparm, function (data) {
                $("#txtLoginPwd").val("");
                $.dialog.tips(data.Msg);
                if (data.State == 1) {
                    location.reload();
                    art.dialog.list['Confirm'].close();
                } else if (data.State == 2) {
                    window.location.href = data.Result;
                }
            });
        } else {
            $.dialog.tips("账号和密码不可为空");
        }
    }
    $(".divChange").hover(function () {
        $(this).addClass("index_dl_mouseover");
    }, function () {
        $(this).removeClass("index_dl_mouseover");
    }).click(function () {
        $(".divChange").removeClass("index_dl_click");
        $(this).addClass("index_dl_click");
    });
    var strnav = (location.pathname.split("/")[1] || "Resource").toLowerCase();
    $("#nav a[flag='" + strnav + "']").css({ "background-color": "#708ad5", "height": "31px" });
    /*登录*/
    $("#btnLogin").click(function () {
        $.artDialog.confirm(document.getElementById('acc_txt'), function () {
            sync_login();
        }, null, "face-smile").title("登录");
        $("#txtLoginName").focus();
    });
    $("#txtLoginName,#txtLoginPwd").keyup(function (evt) {
        evt.keyCode == 13 && sync_login();
    });
    /*验证是否登录*/
    layoutparm = {};
    $.sync.ajax.post("/Account/GetIdentity", layoutparm, function (data) {
        if (!data.State) {
            $("#trLogin").show();
        } else {
            $("#trLogout").show();
            $("#lblUserName").text(data.Result.Name);
        }
    }, true);
    /*注销*/
    $("#btnLogout").click(function () {
        layoutparm = {};
        $.sync.ajax.post("/Account/Logout", layoutparm, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                setTimeout(function () { location.reload(); /* history.go(0);*/ }, 1000);
            }
        });
    });
    /*注册*/
    $("#btnReg").click(function () {
        $.sync.ajax.post("/Account/RegInit", layoutparm, function (data) {
            if (data.State) {
                $("#acc_reg").setTemplateURL("/Scripts/template/home/reginit.htm");
                $("#acc_reg").processTemplate(data.Result);
                datareginit = data.Result.School;
                $("#ddlSchool0").change();
                $.artDialog.confirm(document.getElementById('acc_reg'), function () {
                    layoutparm = {};
                    layoutparm.LoginName = $("#txtRegId").val().trim();
                    layoutparm.LoginPwd = $("#txtRegPwd").val().trim();
                    layoutparm.LoginPwd2 = $("#txtRegPwd2").val().trim();
                    layoutparm.Name = $("#txtRegName").val().trim();
                    layoutparm.Sex = $("#rbRegMan").prop("checked") ? "男" : "女";
                    layoutparm.Birthday = $("#txtRegBirthday").val().trim();
                    layoutparm.SchoolId = $("#ddlSchool0").val();
                    layoutparm.FacultyId = $("#ddlSchool1").val();
                    layoutparm.DepartmentId = $("#ddlSchool2").val();
                    layoutparm.ProfessionId = $("#ddlzy").val();
                    layoutparm.PostId = $("#ddlzw").val();
                    layoutparm.JobId = $("#ddlzc").val();
                    layoutparm.Email = $("#txtRegEmail").val().trim();
                    layoutparm.Phone = $("#txtRegPhone").val().trim();
                    layoutparm.Type = $("#rbRegDefault").prop("checked") ? 1 : 0;
                    if (layoutparm.LoginName == "") {
                        $.artDialog.tips("账号必须填写");
                        return false;
                    }
                    if (layoutparm.LoginPwd == "") {
                        $.artDialog.tips("密码必须填写");
                        return false;
                    }
                    if (layoutparm.LoginPwd != layoutparm.LoginPwd2) {
                        $.artDialog.tips("密码有误");
                        return false;
                    }
                    if (layoutparm.Name == "") {
                        $.artDialog.tips("姓名必须填写");
                        return false;
                    }
                    $.sync.ajax.post("/Account/RegPost", layoutparm, function (data) {
                        $.artDialog.tips(data.Msg);
                        if (!data.State) {
                            return false;
                        }
                    });
                }, null, "none").title("注册新用户");
            } else {
                $.artDialog.tips(data.Msg);
            }
        });
    });
});