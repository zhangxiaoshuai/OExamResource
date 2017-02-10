$(function () {
    var parameters = {};
    //读取列表
    function readlist() {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.type = $("#ddlTypes").val();
        $.sync.ajax.post("/LanManage/GetLanTypes", parameters, function (data) {
            if (data.State) {
                if ($("#ddlTypes").val() == 2) {
                    if ($("#hdShow").val() == 1) {
                        $("#tdSchool").show();
                    } else {
                        $("#tdSchool").hide();
                    }
                    $("#divTypes").setTemplateURL("/Scripts/template/manage/lantype2.htm");
                }
                else {
                    $("#tdSchool").hide();
                    $("#divTypes").setTemplateURL("/Scripts/template/manage/lantype.htm");
                }
                $("#divTypes").processTemplate(data.Result);
            }
        });
    }
    //加载列表
    readlist();

    //学校改变，读取相应列表
    $("#ddlSchool").bind("change", function () {
        readlist();
    });

    //分类类型改变，读取相应列表
    $("#ddlTypes").bind("change", function () {
        readlist();
    });
    //添加
    $("#btnAddType").live("click", function () {
        parameters = {};
        if ($("#txtName").val().trim().length == 0) {
            $.dialog.tips("请输入名称");
            return false;
        }
        parameters.name = $("#txtName").val();
        parameters.type = $("#ddlTypes").val();
        parameters.schoolid = $("#ddlSchool").val();
        $.sync.ajax.post("/LanManage/AddType", parameters, function (data) {
            if (data.State) {
                readlist();
                $.dialog.tips(data.Msg);
            } else {
                $.dialog.alert(data.Msg);
            }
        });
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/LanManage/DeleteType", parameters, function (data) {
                if (data.State) {
                    readlist();
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });

    //所有修改
    $(".edit").die().live("click", function () {
        var $edit = $(this);
        var id = $edit.attr("dataid");
        $.dialog({
            title: "更改名称",
            content: document.getElementById('divEdit'),
            id: 'EF8930'
        });
        $("#hdTypeId").val(id);
        $("#txtNewName").val($("#p" + id).text().trim());
    });

    //确定修改
    $("#btnEditType").die().live("click", function () {
        parameters = {};
        parameters.id = $("#hdTypeId").val();
        parameters.name = $("#txtNewName").val().trim();
        $.sync.ajax.post("/LanManage/EditType", parameters, function (data) {
            if (data.State) {
                $.dialog["EF8930"].close();
                readlist();
                $.dialog.tips(data.Msg);
            } else {
                $.dialog.alert(data.Msg);
            }
        });
    });
    //变色
    $("#divTypes tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
});