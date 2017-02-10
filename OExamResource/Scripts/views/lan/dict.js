$(function () {
    var parameters = {};
    if (viewmodel.key.trim() == "") {
        $("#txtSearch").data("isInp", false).css("color", "gray").val("请输入课程名称/资源名称/教师名称");
    } else {
        $("#txtSearch").data("isInp", true).val(viewmodel.key.trim());
    }
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
    /*资源分类*/
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
            parameters.sid = $("#hdId").val();
            parameters.uid = $("#hdUid").val();
            parameters.tid = $("#hdTid").val();
            parameters.cid = $(".ctype").filter(".type_left_04").eq(0).attr("dataid");
            parameters.key = viewmodel.key;
            $.sync.ajax.post("/Lan/ResourcePage", parameters, function (data) {
                if (!!data.State) {
                    $("#fullpage").setTemplateURL("/Scripts/template/lan/dicpage.htm");
                    $("#fullpage").processTemplate(data.Result);
                }
            });
        });
    });
    /*课程分类*/
    $(".ctype").hover(function () {
        $(this).css("text-decoration", "underline");
    }, function () {
        $(this).css("text-decoration", "none");
    }).click(function () {
        if ($(this).is(".type_left_04")) {
            return false;
        }
        $(this).addClass("type_left_04").siblings(".type_left_04").removeClass("type_left_04");
        parameters = {};
        parameters.sid = $("#hdId").val();
        parameters.uid = $("#hdUid").val();
        parameters.cid = $(this).attr("dataid");
        parameters.key = viewmodel.key;
        $.sync.ajax.post("/Lan/Type", parameters, function (data) {
            if (data.State) {
                $("#fulltype").setTemplateURL("/Scripts/template/lan/type.htm");
                $("#fulltype").processTemplate(data.Result);
                if ($("#hdTid").val() == "00000000-0000-0000-0000-000000000000") {
                    $(".type_left_01").click();
                } else {
                    if ($("#hdTid").attr("domtype") == 1) {
                        $(".type_left_03[dataid='" + $("#hdTid").val() + "']:last").click();
                    } else {
                        $(".type_left_03[dataid='" + $("#hdTid").val() + "']:eq(0)").click();
                    }
                }
            }
        });
    });
    $(".ctype:eq(0)").click();
    /*课程分类导航*/
    $(".ddlCtype").data("flag", "0").click(function () {
        if ($(this).data("flag") == "0") {/*open*/
            $(".ddlCtype").attr("src", "/Content/image/lan/xxjm_png_08.png").data("flag", "0");
            $(this).attr("src", "/Content/image/lan/aui_close.png").data("flag", "1");
            $("#divCourseNav_00").show().offset({ left: $(this).parent().offset().left, top: $(this).parent().offset().top + $(this).parent().height() });
            $("#divCourseNav_03").html($(this).parent().parent().siblings(".xxjm_jsjm_right003").children().clone(true));
        } else {/*close*/
            $("#divCourseNav_00").hide();
            $(".divCourseNav_02").css("color", "Blue");
            $(this).attr("src", "/Content/image/lan/xxjm_png_08.png").data("flag", "0");
        }
    });
    $(".divCourseNav_02").click(function () {
        $(this).css("color", "Red").siblings().css("color", "Blue");
        $("#divCourseNav_03 .xxjm_jsjm_right005").hide();
        var _flag = $(this).text().trim();
        if (_flag == "#") {
            $("#divCourseNav_03 .xxjm_jsjm_right005[flag='_other']").show();
        } else if (_flag == "*") {
            $("#divCourseNav_03 .xxjm_jsjm_right005").show();
        } else {
            $("#divCourseNav_03 .xxjm_jsjm_right005[flag='" + _flag + "']").show();
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
    /*#region 分类编辑功能*/
    function refreshtype() {
        parameters = {};
        parameters.sid = $("#hdId").val();
        parameters.uid = $("#hdUid").val();
        parameters.cid = $(".type_left_04").filter(".ctype").attr("dataid");
        parameters.key = viewmodel.key;
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

    /*弹窗预览*/
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