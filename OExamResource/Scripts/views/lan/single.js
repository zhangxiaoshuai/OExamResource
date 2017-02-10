$(function () {
    var parameters = {};
    /*#region 搜索*/
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
    /*#endregion search*/
    /*#region 资源分类*/
    $("#fulltype").setTemplateURL("/Scripts/template/lan/type.htm");
    $("#fulltype").processTemplate(viewmodel.jsontype.Result);
    $(".type_left_01,.type_left_03").live("mouseenter", function () {
        $(this).css("text-decoration", "underline").find(".imgtype").show();
    }).live("mouseleave", function () {
        $(this).css("text-decoration", "none").find(".imgtype").hide();
    }).live("click", function () {
        if ($(this).is(".type_left_04")) {
            return false;
        }
        $(this).siblings(".type_left_04").removeClass("type_left_04");
        $(this).addClass("type_left_04");
        $("#hdTid").val($(this).attr("dataid")).attr("domtype", $(this).is(".type_left_03_1") ? 0 : 1);
        $.cellpagebar.init({
            total: $(this).find("span").text().replace("(", "").replace(")", ""),
            pagesize: 8,
            container: $("#pagefooter")
        }, function (pageindex) {
            parameters = {};
            parameters.pageIndex = pageindex;
            parameters.pageCount = 8;
            parameters.cid = $("#hdCid").val();
            parameters.tid = $(".type_left_04:eq(0)").attr("dataid");
            $.sync.ajax.post("/Lan/SinglePage", parameters, function (data) {
                if (!!data.State) {
                    $("#fullpage").setTemplateURL("/Scripts/template/lan/singpage.htm");
                    $("#fullpage").processTemplate(data.Result);
                }
            });
        });
    });
    $(".type_left_01").click();
    /*#endregion */
    /*#region 导航*/
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
    /*#endregion */
    /*#region 动画*/
    if (viewmodel.jsonbanner.length > 1) {
        $("#fullbanner").setTemplateURL("/Scripts/template/lan/banner_c.htm");
        $("#fullbanner").processTemplate(viewmodel.jsonbanner);
        var _currentbanner = $(".banner_01").length - 1;
        $(".banner_01").eq(_currentbanner).addClass('current_banner');
        $("#fullbanner").css("background-image", "url(" + $(".banner_01").eq(_currentbanner).attr("dataurl") + ")");
        var _bannertimer = setInterval(function () {
            if (_currentbanner <= 0) {
                _currentbanner = $(".banner_01").length;
            }
            _currentbanner--;
            $("#fullbanner").fadeTo('slow', 0.5, function () {
                $("#fullbanner").css("background-image", "url(" + $(".banner_01").eq(_currentbanner).attr("dataurl") + ")").fadeTo('slow', 1);
            });
            $(".banner_01").removeClass('current_banner').eq(_currentbanner).addClass('current_banner');
        }, 5000);
    } else if (viewmodel.jsonbanner.length == 1) {
        $("#fullbanner").css("background-image", "url(" + viewmodel.jsonbanner[0] + ")");
    }
    $(".banner_01").hover(function () {
        $(".banner_01").removeClass('current_banner');
        $("#fullbanner").css("background-image", "url(" + $(this).attr("dataurl") + ")");
        $(this).addClass('current_banner');
        clearInterval(_bannertimer);
    }, function () {
        _bannertimer = setInterval(function () {
            if (_currentbanner <= 0) {
                _currentbanner = $(".banner_01").length;
            }
            _currentbanner--;
            $("#fullbanner").fadeTo('slow', 0.5, function () {
                $("#fullbanner").css("background-image", "url(" + $(".banner_01").eq(_currentbanner).attr("dataurl") + ")").fadeTo('slow', 1);
            });
            $(".banner_01").removeClass('current_banner').eq(_currentbanner).addClass('current_banner');
        }, 5000);
    });
    /*#endregion*/
    /*#region 分类编辑功能*/
    function refreshtype() {
        parameters = {};
        parameters.sid = $("#hdId").val();
        parameters.uid = "00000000-0000-0000-0000-000000000000";
        parameters.cid = $("#hdCid").val();
        parameters.key = "";
        $.sync.ajax.post("/Lan/Type", parameters, function (data) {
            if (data.State) {
                $("#fulltype").setTemplateURL("/Scripts/template/lan/type.htm");
                $("#fulltype").processTemplate(data.Result);
                if ($("#hdTid").val() == "00000000-0000-0000-0000-000000000000") {
                    $(".type_left_01").addClass("type_left_04");
                } else {
                    if ($("#hdTid").attr("domtype") == 1) {
                        $(".type_left_03[dataid='" + $("#hdTid").val() + "']:last").addClass("type_left_04");
                    } else {
                        $(".type_left_03[dataid='" + $("#hdTid").val() + "']:eq(0)").addClass("type_left_04");
                    }
                }
            }
        });
    }
    $(".imgtype").live("click", function () {
        if ($(this).hasClass("typetop")) {/*置顶*/
            parameters = {};
            parameters.typeId = $(this).parent().attr("dataid");
            $.sync.ajax.post("/Lan/TypeTop", parameters, function (data) {
                $.artDialog.tips(data.Msg);
                if (data.State) {
                    refreshtype();
                }
            });
        }
        else if ($(this).hasClass("typecal")) {/*取消置顶*/
            parameters = {};
            parameters.id = $(this).parent().attr("datakid");
            $.sync.ajax.post("/Lan/TypeCancel", parameters, function (data) {
                if (data.State) {
                    $.sync.ajax.post("/Lan/TypeCancel", parameters, function (data) {
                        if (data.State) {
                            $.artDialog.tips(data.Msg);
                            refreshtype();
                        }
                    });
                }
            });
        }
        else if ($(this).hasClass("typeup")) {/*调整位置*/
            if ($(".type_left_03_1").index($(this).parent()) == 0) {
                return false;
            } else {
                parameters = {};
                parameters.id1 = $(this).parent().prev().attr("datakid");
                parameters.id2 = $(this).parent().attr("datakid");
                $.sync.ajax.post("/Lan/TypeUp", parameters, function (data) {
                    if (data.State) {
                        refreshtype();
                    }
                });
            }
        } else if ($(this).hasClass("typedel")) {/*删除*/
            var $that = $(this);
            $.artDialog.confirm("确定要删除吗", function () {
                parameters = {};
                parameters.id = $that.parent().attr("dataid");
                $.sync.ajax.post("/Lan/TypeDel", parameters, function (data) {
                    $.artDialog.tips(data.Msg);
                    if (data.State) {
                        refreshtype();
                    }
                });
            }).title("删除自定义分类");
        } else if ($(this).hasClass("typeedit")) {/*update*/
            var $that = $(this);
            $.artDialog.prompt("请输入新名称", function (val) {
                parameters = {};
                parameters.id = $that.parent().attr("dataid");
                parameters.name = val;
                $.sync.ajax.post("/Lan/TypeEdit", parameters, function (data) {
                    $.artDialog.tips(data.Msg);
                    if (data.State) {
                        refreshtype();
                    }
                });
            }).title("修改自定义分类");
        } else if ($(this).hasClass("typeinfo")) {/*查看*/
            parameters = {};
            parameters.id = $(this).parent().attr("dataid");
            parameters.userId = $(this).attr("dataid");
            $.sync.ajax.post("/Lan/TypeInfo", parameters, function (data) {
                $.artDialog.tips(data.Msg);
                if (data.State) {
                    $.artDialog({ icon: 'face-smile', opacity: 0, title: data.Result.Title, content: "创建者：" + data.Result.User + "<br/>" + "创建时间：" + data.Result.Date.toDate("yyyy年MM月dd日"), cancelVal: '关闭', cancel: true });
                }
            });
        }
        return false;
    });
    $("#typeadd").live("click", function () {
        $.artDialog.prompt("请输入分类名称", function (val) {
            parameters = {};
            parameters.name = val;
            $.sync.ajax.post("/Lan/TypeAdd", parameters, function (data) {
                $.artDialog.tips(data.Msg);
                if (data.State) {
                    refreshtype();
                }
            });
        }).title("新建自定义分类");
        return false;
    });
    $(".type_left_02").live("click", function () {
        if ($(this).hasClass("type_left_02_1")) {
            $(this).removeClass("type_left_02_1").css("background-image", "url(/Content/image/lan/16-arrow-right.png) ").nextUntil(".type_left_02").slideUp();
        } else {
            $(this).addClass("type_left_02_1").css("background-image", "url(/Content/image/lan/16-arrow-down.png) ").nextUntil(".type_left_02").slideDown();
        }
    });
    /*#endregion */
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