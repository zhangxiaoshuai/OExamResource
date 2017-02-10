$(function () {
    var parameters = {};
    parameters.Id = $("#hdId").val();
    $.sync.ajax.post("/Lan/TopResource", parameters, function (data) {
        if (data.State) {
            $("#full").setTemplateURL("/Scripts/template/lan/full.htm");
            $("#full").processTemplate(data.Result);
        }
    });
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
    /*动画*/
    if (viewmodel.banners.length > 1) {
        $(".banner_00").setTemplateURL("/Scripts/template/lan/banner.htm");
        $(".banner_00").processTemplate(viewmodel.banners);

        var _currentbanner = $(".banner_01").length - 1;
        $(".banner_01").eq(_currentbanner).addClass("current_banner");
        $(".banner_00").css("background-image", "url(" + $(".banner_01").eq(_currentbanner).attr("dataurl") + ")");

        var _bannertimer = setInterval(function () {
            if (_currentbanner <= 0) {
                _currentbanner = $(".banner_01").length;
            }
            _currentbanner--;
            $(".banner_00").fadeTo('slow', 0.5, function () {
                $(".banner_00").css("background-image", "url(" + $(".banner_01").eq(_currentbanner).attr("dataurl") + ")").fadeTo('slow', 1);
            });
            $(".banner_01").removeClass('current_banner').eq(_currentbanner).addClass("current_banner");
        }, 5000);
    } else if (viewmodel.banners.length == 1) {
        $(".banner_00").css("background-image", "url(" + viewmodel.banners[0] + ")");
    }

    $(".banner_01").hover(function () {
        $(".banner_01").removeClass('current_banner')
        $(".banner_00").css("background-image", "url(" + $(this).attr("dataurl") + ")");
        $(this).addClass("current_banner");
        clearInterval(_bannertimer);
    }, function () {
        _bannertimer = setInterval(function () {
            if (_currentbanner <= 0) {
                _currentbanner = $(".banner_01").length;
            }
            _currentbanner--;
            $(".banner_00").fadeTo('slow', 0.5, function () {
                $(".banner_00").css("background-image", "url(" + $(".banner_01").eq(_currentbanner).attr("dataurl") + ")").fadeTo('slow', 1);
            });
            $(".banner_01").removeClass('current_banner').eq(_currentbanner).addClass("current_banner");
        }, 5000);
    });
    /*动画end*/
    /*预览*/
    $(".imgZoom").live("click", function () {
        var $that = $(this);
        parameters = {};
        parameters.ext = $(($that.attr("dataurl") || "jpg").split(".")).last().get(0).toLowerCase().trim();
        parameters.html = "";
        if ($that.attr("dataurl").trim().length <= 0) {
            layoutparm.html = "<img src='/Images/lan/default/resource.jpg' style='margin-bottom:-3px;width:650px;height:500px;'/>";
        } else {
            if (parameters.ext == "swf" || parameters.ext == "ppt" || parameters.ext == "doc") {
                parameters.html = "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0'";
                parameters.html += " width='650' height='500'>";
                parameters.html += " <param name='movie' value='" + $that.attr("dataurl") + "' />";
                parameters.html += " <param name='quality' value='high' />";
                parameters.html += " <embed src='" + $that.attr("dataurl") + "' quality='high' pluginspage='http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash'";
                parameters.html += " type='application/x-shockwave-flash' width='650px' height='500px'>";
                parameters.html += " </embed>";
                parameters.html += " </object>";
            } else if (parameters.ext == "flv") {
                parameters.html = '<embed type="application/x-shockwave-flash" src="/Images/source/flvplayer.swf" width="650" height="500" style="margin-bottom:-3px;" id="single" name="single" quality="high" allowfullscreen="true" flashvars="file=' + $that.attr("dataurl") + '&amp;image=' + $that.attr("dataurl") + '&amp;width=650&amp;height=500">';
            } else if (parameters.ext == "mp3") {
                parameters.html = "<object id='Player' width='650' height='500' classid='CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6'>";
                parameters.html += "<param name='URL' value='" + $that.attr("dataurl") + "'>";
                parameters.html += "<param name='autoStart' value='1'>";
                parameters.html += "<param name='balance' value='0'>";
                parameters.html += "<param name='baseURL' value>";
                parameters.html += "<param name='captioningID' value>";
                parameters.html += "<param name='currentPosition' value='0'>";
                parameters.html += "<param name='currentMarker' value='0'>";
                parameters.html += "<param name='defaultFrame' value>";
                parameters.html += "<param name='enabled' value='1'>";
                parameters.html += "<param name='enableErrorDialogs' value='0'>";
                parameters.html += " <param name='enableContextMenu' value='1'>";
                parameters.html += "<param name='fullScreen' value='0'>";
                parameters.html += "<param name='invokeURLs' value='1'>";
                parameters.html += "<param name='mute' value='0'>";
                parameters.html += "<param name='playCount' value='1'>";
                parameters.html += "<param name='rate' value='1'>";
                parameters.html += "<param name='SAMIStyle' value>";
                parameters.html += "<param name='SAMILang' value>";
                parameters.html += "<param name='SAMIFilename' value>";
                parameters.html += "<param name='stretchToFit' value='0'>";
                parameters.html += "<param name='uiMode' value='full'>";
                parameters.html += "<param name='volume' value='100'>";
                parameters.html += "<param name='VideoBorder3D' value='0'>";
                parameters.html += "<param name='windowlessVideo' value='5'>";
                parameters.html += "</object>";
            } else {
                parameters.html = "<img src='" + $that.attr("dataurl") + "' style='margin-bottom:-3px;width:650px;height:500px;'/>";
            }
        }
        $.dialog({
            opacity: 0.1,
            padding: 0,
            title: '预览',
            content: $(parameters.html).get(0),
            lock: true,
            close: function () {
                parameters = {};
            }
        });
    });
});