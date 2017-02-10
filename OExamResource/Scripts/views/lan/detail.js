$(function () {
    var parameters = {};
    $("#txtSearch").keyup(function (evt) {
        evt.keyCode == 13 && $("#btnSearch").click();
    });
    $("#btnSearch").click(function () {
        if ($("#txtSearch").val().trim() == "") {
            $.dialog.tips("请输入要搜索的关键字");
        } else {
            parameters = {};
            parameters.key = $("#txtSearch").val().trim();
            parameters.id = $("#hdId").val();
            location.href = "/Lan/Dict?" + $.param(parameters);
        }
    });
    /*导航*/
    $("#fullNav").setTemplateURL("/Scripts/template/lan/nav.htm");
    $("#fullNav").processTemplate(viewmodel.jsonnav);
    /*第一层*/
    $(".xxjm_nav_01").click(function () {
        $(".nav_choose03").removeClass("nav_choose03");
        if ($(this).is(".xxjm_nav_02")) {
            $(".nav_choose").hide();
            $(this).removeClass("xxjm_nav_02").find("img").attr("src", "/Content/image/lan/xxjm_png_08.png");
            return false;
        }
        $(".xxjm_nav_02").removeClass("xxjm_nav_02").find("img").attr("src", "/Content/image/lan/xxjm_png_08.png");
        $(this).addClass("xxjm_nav_02").find("img").attr("src", "/Content/image/lan/aui_close.png");
        if ($(this).attr("flag") == "nav_A" || $(this).attr("flag") == "nav_D") {
            $(".nav_choose01").hide();
        } else {
            $(".nav_choose01").show();
        }
        $(".nav_choose").css("margin-left", $(".xxjm_head").offset().left).show();
        $("#fullNav .nav_choose04").hide();
        var $that = $("#fullNav .nav_choose04[flag='" + $(this).attr("flag") + "']");
        $that.show();
        $that.find(".nav_choose05").show();
    });
    /*字母导航*/
    $(".nav_choose02").click(function () {
        $(".nav_choose03").removeClass("nav_choose03");
        $(this).addClass("nav_choose03");
        var $that = $("#fullNav .nav_choose04:visible");
        $that.find(".nav_choose05").hide();
        var _flag = $(this).text().trim();
        if (_flag == "#") {
            _flag = "_other";
        }
        if (_flag == "*") {
            _flag = "nav_choose05";
        }
        $that.find("." + _flag).show();
    });
    /*导航end*/
    $("#btnCollect").click(function () {
        parameters = {};
        parameters.id = $(this).val();
        $.sync.ajax.post("/Lan/Collect", parameters, function (data) {
            $.dialog.tips(data.Msg);
        });
    });
});