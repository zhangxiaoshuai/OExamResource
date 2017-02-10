$(function () {
    var par = {};
    par.paperId = $("#hdId").val();
    /*初始化试题*/
    par = {};
    par.id = $("#hdId").val();
    $.sync.ajax.post("/DataManage/GetPaper", par, function (data) {
        if (data.State) {
            $("#divContent").setTemplateURL("/Scripts/template/manage/paper.htm", null, { filter_data: false });
            $("#divContent").processTemplate(data.Result);
            $("#divNum").setTemplateURL("/Scripts/template/manage/number.htm");
            $("#divNum").processTemplate(data.Result);
            var _s = 0;
            for (var i = 0; i < data.Result.length; i++) {
                _s += data.Result[i].Part.ItemCount * data.Result[i].Part.ItemScore;
            }
            $("#lblScore").text(_s);
        }
    });
    /*显示隐藏题号导航*/
    $("#r_sub").toggle(function () {
        var _w = 0;
        var _hideleft = setInterval(function () {
            _w -= 4;
            $("#r_main").css("right", _w);
            if (_w <= -152) {
                $("#r_sub").html("&lt;");
                clearInterval(_hideleft);
            }
        }, 1);
    }, function () {
        var _w = -152;
        var _showleft = setInterval(function () {
            _w += 4;
            $("#r_main").css("right", _w);
            if (_w >= 0) {
                $("#r_sub").html("&gt;");
                clearInterval(_showleft);
            }
        }, 1);
    });
    /*题号定位*/
    $("#divNum a").click(function () {
        $(document).scrollTop($($(this).attr("href")).offset().top);
        return false;
    });
    /*修改按钮*/
    $(".btnEditItem").toggle(function () {
        var $btn = $(this);
        $btn.val("确认");
        var id = $btn.attr("dataid");
        $("#q" + id).removeAttr("disabled");
        $("#an" + id).removeAttr("disabled");
        $("#exp" + id).removeAttr("disabled");
    }, function () {
        var $btn = $(this);
        $btn.val("修改");
        var id = $btn.attr("dataid");
        $("#q" + id).attr("disabled", "disabled");
        $("#an" + id).attr("disabled", "disabled");
        $("#exp" + id).attr("disabled", "disabled");
        par = {};
        par.itemid = $btn.attr("dataid");
        par.question = $("#q" + id).val();
        par.answer = $("#an" + id).val();
        par.explain = $("#exp" + id).val();
        $.sync.ajax.post("/ExamManage/EditItem", par, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

});