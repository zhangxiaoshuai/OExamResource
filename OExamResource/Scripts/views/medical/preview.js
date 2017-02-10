$(function () {
    var par = {};

    $("#lkList").scrollTop($("#hdST").val() || 0);

    $("#lkList a").click(function () {
        $(this).attr("href", $(this).attr("href") + "&st=" + $("#lkList").scrollTop());
    });

    $("#btnError").click(function () {
        par = {};
        par.id = $(this).attr("dataid");
        par.code = $(this).attr("datacode");
        $.dialog.prompt("感谢您提出宝贵的意见", function (s) {
            par.remark = s;
            $.sync.ajax.post("/Medical/SetError", par, function (data) {
                if (data.State == 1) {
                    $.dialog.tips(data.Msg);
                }
            });
        }, "此资源有错误");
    });

//    $("#ckShare").change(function () {
//        if ($(this).prop("checked")) {
//            $("#tbShare").show();
//        } else {
//            $("#tbShare").hide();
//        }
//    });
    $("#btnLog").click(function () {
        $.sync.ajax.post("/Medical/shoucang", { id: $("#btnLog").attr("dataid") }, function (data) {
                $.dialog.tips(data.Result);
            });
    });
});  