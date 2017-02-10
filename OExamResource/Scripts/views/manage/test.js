$(function () {
    var parameters = {};
    parameters.Id = "d814321c-f673-4f7d-88cd-6bc83637fd36";
    $("#d814321c-f673-4f7d-88cd-6bc83637fd36").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });


    //---------试题库列表---开始--------------------------
    //显示——试题库列表
    $("#Test_List").click(function () {
        $.sync.ajax.post("/TestManage/Test_List", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/test_list.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters.id = null;
        $.sync.ajax.post("/DataManage/TestList", parameters, function (data) {
            if (data.State) {
                $("#ulTestList").setTemplateURL("/Scripts/template/manage/test.htm");
                $("#ulTestList").processTemplate(data.Result);
            }
        });
    });

    //大类全选
    $(".Test_checkAll").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("typeid");
        if (!$(this).attr("checked")) {
            $(this).attr("checked", false);
            parameters.isdeleted = 1;
        } else {
            $(this).attr("checked", true);
            parameters.isdeleted = 0;
        }
        parameters.state = 1;
        $.sync.ajax.post("/TestManage/DelTest", parameters, function (data) {
            if (data.State) {
                $("#Test_List").click();
            }
        });
    });

    //大类前台显示
    $(".Test_check").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("typeid");
        if (!$(this).attr("checked")) {
            $(this).attr("checked", false);
            parameters.isdeleted = 1;
        } else {
            $(this).attr("checked", true);
            parameters.isdeleted = 0;
        }
        parameters.state = 0;
        $.sync.ajax.post("/TestManage/DelTest", parameters, function (data) {
            if (data.State) {
                //                $("#Test_List").click();
            }
        });
    });
    //---------试题库列表----结束-----------------------
    //---------试题库更新----开始-----------------------
    $("#Test_Update").click(function () {
        $.sync.ajax.post("/TestManage/Test_Update", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Test_Update.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    $("#btnImport_test").die().live("click", function () {
        if ($("#txtSheet").val().trim().length <= 0) {
            $.dialog.alert("请正确填写！");
            return false;
        }
        if ($("#upXls").val().trim().length <= 0) {
            $.dialog.alert("请正确选择文件！");
            return false;
        }
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                $.dialog.alert(data.Msg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                $.dialog.alert(textStatus);
                $.dialog.alert(errorThrown);
            }
        };
        $("#form_update").ajaxForm(options);
    });
    //---------试题库更新----结束-----------------------
});