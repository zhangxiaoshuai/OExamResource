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
        if ($("#hdGuidList").val().length <= 0) {
            $.dialog.tips("没有选中资源！");
            return false;
        };
        parameters.guidlist = JSON.parse($("#hdGuidList").val());
        parameters.cateid = $("#ddlCs").val();
        if (parameters.cateid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("没有选中分类！");
            return false;
        }
        parameters.speid = $("#ddlSpe").val();
        parameters.typeid = $("#ddlType").val();
        parameters.knowid = $("#ddlKnow").val();
        parameters.fescribe = $("#txtFescribe").val();
        parameters.keys = $("#txtKey").val();
        $.sync.ajax.post("/MedicalManage/MassEditRes", parameters, function (data) {
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
