$(function () {
    var parameters = {};
    //页面数据
    parameters.id = null;
    $.sync.ajax.post("/DataManage/TestList", parameters, function (data) {
        if (data.State) {
            $("#ulTestList").setTemplateURL("/Scripts/template/manage/test.htm");
            $("#ulTestList").processTemplate(data.Result);
        }
    });
    
    //li
    $("#ulTestList li").die().live("click", function (event) {
        var id = $(this).attr("dataid");
        if ($("#li" + id).attr("flag") == "+") {
            parameters.id = id;
            $.sync.ajax.post("/DataManage/TestList", parameters, function (data) {
                if (data.State) {
                    $("#ul" + id).setTemplateURL("/Scripts/template/manage/test.htm");
                    $("#ul" + id).processTemplate(data.Result);
                }
                $("#li" + id).attr("flag", "-");
            });
        } else if ($("#li" + id).attr("flag") == "") {
            return false;
        }
        if ($("#ul" + id).css("display") == "none") {
            $("#ul" + id).css("display", "");
            $("#lli" + id).text("-");
        }
        else {
            $("#ul" + id).css("display", "none");
            $("#lli" + id).text("+");
        }
    });

    $(".uli").die().live({
        mouseover: function () {
            $(this).css("background", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).css("background", "");
        }
    });
    //删除
    $(".dela").bind("click", function () {
        parameters.id = $(this).attr("dataid");
        $.dialog({
            id: 'deleteTest',
            content: '是否确认删除，删除后无法恢复？',
            button: [
        {
            name: '确定',
            callback: function () {
                $.sync.ajax.post("/TestManage/DelTest", parameters, function (data) {
                    if (data.State) {
                        $("#uli" + parameters.id).remove();
                        $("#li" + parameters.id).remove();
                    }
                });
            },
            focus: true
        },
        {
            name: '取消'
        }
    ]
        });

    });
});