$(function () {
    $("#btnImport").bind("click", function () {
        if ($("#txtSheet").val().trim().length <= 0) {
            $.tips("请正确填写！");
            return false;
        }
        if ($("#upXls").val().trim().length <= 0) {
            $.tips("请正确选择文件！");
            return false;
        }
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                $.dialog.alert(data.Msg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_update").ajaxForm(options);
    });
});