$(function () {
    var parameters = {};
    //素材分类 
    $.sync.ajax.post("/DataManage/GetCategorySubject", parameters, function (data) {
        if (data.State) {
            $("#ddlcs").setTemplateURL("/Scripts/template/manage/ddlcsc.htm");
            $("#ddlcs").processTemplate(data.Result);
        }
        $("#ddlcs").get(0).selectedIndex = 0;
    });
    //分类改变选择素材专业
    $("#ddlcs").bind("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                $("#ddlSpe").processTemplate(data.Result);
                $("#ddlSpe").get(0).selectedIndex = 0;
                parameters = {};
                parameters.speid = $("#ddlSpe").val();
                $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
                    if (data.State) {
                        $("#ddlKnow").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                        $("#ddlKnow").processTemplate(data.Result);
                        $("#ddlKnow").get(0).selectedIndex = 0;
                    } else {
                        $("#ddlKnow").empty();
                        $("#ddlKnow").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
                    }
                });
            } else {
                $("#ddlSpe").empty();
                $("#ddlSpe").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
            }
        });
    });

    //专业改变选择素材知识点
    $("#ddlSpe").bind("change", function () {
        parameters = {};
        parameters.speid = $("#ddlSpe").val();
        $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnow").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                $("#ddlKnow").processTemplate(data.Result);
                $("#ddlKnow").get(0).selectedIndex = 0;
            } else {
                $("#ddlKnow").empty();
                $("#ddlKnow").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
            }
        });
    });

    //保存按钮
    $("#btnSure").bind("click", function () {
        parameters = {};
        parameters.guidlist = JSON.parse($("#hdGuidList").val());
        if ($("#hdGuidList").val().length <= 0) {
            $.dialog.tips("没有选中素材！");
            return false;
        };
        parameters.cateid = $("#ddlcs option:selected").parent("optgroup").attr("id");
        if (parameters.cateid.length <= 0) {
            $.dialog.tips("没有选中分类！");
            return false;
        }
        parameters.subid = $("#ddlcs").val();
        parameters.speid = $("#ddlSpe").val();
        parameters.typeid = $("#ddlType").val();
        parameters.jp = $("#ddlBoutiqueGrade").val();
        parameters.fescribe = $("#txtFescribe").val();
        parameters.keys = $("#txtKey").val();
        $.sync.ajax.post("/ResourceManage/MassEditRes", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                $("#btnEsc").click();
            }
        });
    });

    //返回按钮
    $("#btnEsc").bind("click", function () {
        window.location.href = "/ResourceManage/Resource";
    });
});
