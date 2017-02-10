$(function () {
    var parameters = {};
    //专业改变选择知识点
    $("#ddlSpe").bind("change", function () {
        parameters = {};
        parameters.speid = $("#ddlSpe").val();
        $.sync.ajax.post("/DataManage/GetMedicalKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnow").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlKnow").processTemplate(data.Result);
            } else {
                $("#ddlKnow").empty();
                $("#ddlKnow").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });
    //分类改变选择素材专业
    $("#ddlCs").bind("change", function () {
        parameters = {};
        parameters.cateid = $("#ddlCs").val();
        $.sync.ajax.post("/DataManage/GetMedicalSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlSpe").processTemplate(data.Result);
                $("#ddlSpe").get(0).selectedIndex = 0;
                parameters = {};
                parameters.speid = $("#ddlSpe").val();
                $.sync.ajax.post("/DataManage/GetMedicalKnowledge", parameters, function (data) {
                    if (data.State) {
                        $("#ddlKnow").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                        $("#ddlKnow").processTemplate(data.Result);
                        $("#ddlSpe").get(0).selectedIndex = 0;
                    } else {
                        $("#ddlKnow").empty();
                        $("#ddlKnow").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    }
                });
            } else {
                $("#ddlSpe").empty();
                $("#ddlSpe").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });


    //保存按钮
    $("#btnSure").bind("click", function () {
        parameters = {};
        parameters.resid = $("#hdId").val();
        parameters.name = $("#txtResourceName").val();
        parameters.cateid = $("#ddlCs").val();
        parameters.speid = $("#ddlSpe").val();
        parameters.knowid = $("#ddlKnow").val();
        parameters.typeid = $("#ddlType").val();
        parameters.fescribe = $("#txtFescribe").val();
        parameters.keys = $("#txtKey").val();
        if (parameters.name.trim().length <= 0) {
            $.dialog.tips("素材名称不能为空！");
            return false;
        }
        $.sync.ajax.post("/MedicalManage/EditRes", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                $("#btnEsc").click();
            }
        });
    });

    //返回按钮
    $("#btnEsc").bind("click", function () {
        window.location.href = "/MedicalManage/Resource";
    });
});
