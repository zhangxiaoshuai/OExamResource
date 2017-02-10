//var _isShare = 0;
$(function () {
    //    var par = {};
    //    $("#btn1").bind("click", function () {
    //        par = {};
    //        par.teacherid = $("#teacherid").val();
    //        par.resourceid = $("#resourceid").val();
    //        par.folderid = $("#folderid").val();
    //        $.sync.ajax.post("/Resource/Collect_resource", par, function (data) {
    //            $.dialog.tips(JSON.stringify(data.Result));
    //        });
    //    });
    //    par = {};
    //    par.searchname = $("#hdsearchname").val();
    //    par.id = $("#hdId").val();
    //    par.type = $("#hdtype").val();
    //    $.sync.ajax.post("/Resource/Getnavigation", par, function (data) {
    //        $("#previewdaohang").setTemplateURL("/Scripts/template/resource/previewdaohang.htm");
    //        $("#previewdaohang").processTemplate(data.Result);

    //    });
    //    //绑定图片列表
    //    par = {};
    //    par.guidlist = $("#Guidlist").val();
    //    par.IsTop = $("#IsTop").val();
    //    $.sync.ajax.post("/Resource/GetPreviewList", par, function (data) {
    //        if (data.State) {
    //            _isShare = data.Result.IsShare;
    //            //document.getElementById("bdshell_js").src = "http://bdimg.share.baidu.com/static/js/shell_v2.js?cdnversion=" + Math.ceil(new Date() / 3600000)
    //            //中间大图片
    //            //            $("#previewbigpic").setTemplateURL("/Scripts/template/resource/previewbigpic.htm");
    //            //            $("#previewbigpic").processTemplate(data.Result.List);

    //            //下边图片
    //            $("#preview").setTemplateURL("/Scripts/template/resource/preview.htm");
    //            $("#preview").processTemplate(data.Result.List);
    //            //右边详细信息
    //            //            $("#previewdetall").setTemplateURL("/Scripts/template/resource/previewdetall.htm");
    //            //            $("#previewdetall").processTemplate(data.Result.List);

    //            if (data.Result.List.Count > 1) {
    //                $("#leftclick").attr("click", "false");
    //                $("#rightclick").attr("src", "javasrcipt:viod(0);");
    //            }
    //            clickpic($("#sid").val());
    //            //            var previewfilepath = $("#viewerPlaceHolder").attr("previewfilepath");
    //            //            var fp = new FlexPaperViewer(
    //            //            						 '/Scripts/common/FlexPaperViewer',
    //            //            						 'viewerPlaceHolder',
    //            //                                     { config: {
    //            //                                         SwfFile: escape(previewfilepath),
    //            //                                         Scale: 2.0,
    //            //                                         ZoomTransition: 'easeout',
    //            //                                         ZoomTime: 0.5,
    //            //                                         ZoomInterval: 0.2,
    //            //                                         FitPageOnLoad: true,
    //            //                                         FitWidthOnLoad: true,
    //            //                                         PrintEnabled: true,
    //            //                                         FullScreenAsMaxWindow: true,
    //            //                                         ProgressiveLoading: true,
    //            //                                         MinZoomSize: 0.2,
    //            //                                         MaxZoomSize: 5,
    //            //                                         SearchMatchAll: true,
    //            //                                         InitViewMode: 'Portrait',

    //            //                                         ViewModeToolsVisible: false,
    //            //                                         ZoomToolsVisible: true,
    //            //                                         NavToolsVisible: true,
    //            //                                         localeChain: 'zh_CN'
    //            //                                     }
    //            //                                     });

    //        }
    //    });
    //    $("#lkShare").die().live("click", function () {
    //        $.dialog.confirm(document.getElementById("divShare"), function () {
    //            par = {};
    //            par.resourceId = $("#jiucuo_big_a").attr("resourceid");
    //            par.courseId = $("#ddlCourse").val();
    //            par.typeId = $("#ddlResourceType").val();
    //            if (!par.resourceId || !par.courseId || !par.typeId) {
    //                $.dialog.alert("请完善相关资料");
    //                return false;
    //            }
    //            $.sync.ajax.post("/Resource/ShareToLan", par, function (data) {
    //                $.dialog.alert(data.Msg);
    //            });
    //        }).title("共享到校内资源库");
    //    });

    //    function clickpic(id) {
    //        par = {};
    //        par.resourceid = id;
    //        par.istop = $("#IsTop").val();
    //        $.sync.ajax.post("/Resource/GetSourceDetall", par, function (data) {
    //            if (data.State) {
    //                // document.getElementById("bdshell_js").src = "http://bdimg.share.baidu.com/static/js/shell_v2.js?cdnversion=" + Math.ceil(new Date() / 3600000)
    //                $("#previewbigpic").setTemplateURL("/Scripts/template/resource/previewbigpic.htm");
    //                $("#previewbigpic").processTemplate(data.Result);
    //                $("#previewdetall").setTemplateURL("/Scripts/template/resource/previewdetall.htm");
    //                $("#previewdetall").processTemplate(data.Result);

    //                var previewfilepath = $("#viewerPlaceHolder").attr("previewfilepath");
    //                var fp = new FlexPaperViewer(
    //                						 '/Scripts/common/FlexPaperViewer',
    //                						 'viewerPlaceHolder',
    //                                         { config: {
    //                                             SwfFile: escape(previewfilepath),
    //                                             Scale: 2.0,
    //                                             ZoomTransition: 'easeout',
    //                                             ZoomTime: 0.5,
    //                                             ZoomInterval: 0.2,
    //                                             FitPageOnLoad: true,
    //                                             FitWidthOnLoad: true,
    //                                             PrintEnabled: true,
    //                                             FullScreenAsMaxWindow: true,
    //                                             ProgressiveLoading: true,
    //                                             MinZoomSize: 0.2,
    //                                             MaxZoomSize: 5,
    //                                             SearchMatchAll: true,
    //                                             InitViewMode: 'Portrait',

    //                                             ViewModeToolsVisible: false,
    //                                             ZoomToolsVisible: true,
    //                                             NavToolsVisible: true,
    //                                             localeChain: 'zh_CN'
    //                                         }
    //                                         });
    //            }
    //        });
    //    }

    //    //单机图片 事件
    //    $(".clickpic").die().bind("click", function () {
    //        $("#bigpic").attr("src", $(this).attr("value"));
    //        clickpic($(this).attr("resourceid"));

    //    });
    //    var _currentImg = 0;
    //    //左箭头
    //    $("#leftclick").click(function () {

    //        if (_currentImg < 1) {
    //            return false;
    //        }
    //        $("#rightNav").attr("src", "../../../Images/source/youhei.png");
    //        var $thats = $("#tdImg a");

    //        if (_currentImg < 2) {
    //            $("#leftNav").attr("src", "../../../images/source/zuohui.png");
    //        }

    //        $thats.hide();
    //        for (var i = 1; i > -3; i--) {
    //            $thats.eq(_currentImg - i).show();
    //        }
    //        _currentImg--;
    //    });

    //    //右箭头
    //    $("#rightclick").click(function () {
    //        $("#leftNav").attr("src", "../../../images/source/zuohei.png");
    //        var $thats = $("#tdImg a");
    //        if ($thats.length - 5 <= _currentImg) {
    //            $("#rightNav").attr("src", "../../../Images/source/youhui.png");
    //        }
    //        if ($thats.length - 4 <= _currentImg) {
    //            return false;
    //        }
    //        $thats.hide();
    //        for (var i = 1; i < 5; i++) {
    //            $thats.eq(_currentImg + i).show();
    //        }
    //        _currentImg++;
    //    });

    //    //图片边框鼠标移动变色
    //    $(".bgcolor").mousemove(function () {
    //        $(this).attr("style", "border: 3px solid #F37E03");
    //    });
    //    $(".bgcolor").mouseout(function () {
    //        $(this).attr("style", "border: 3px solid #F2F2F2");
    //    });
    //    $(".bgcolor").click(function () {
    //        $(this).attr("style", "border: 3px solid #F37E03");
    //    });
    //    //纠错
    //    $("#jiucuo_big_a").live("click", function () {
    //        par = {};
    //        par.id = $(this).attr("resourceid");
    //        $.sync.ajax.post("/Resource/jiucuo", par, function (data) {
    //            if (data.State) {
    //                $.dialog.alert(data.Result);
    //            }
    //        });
    //    });
    //    //收藏
    //    $("#shoucang_big_a").live("click", function () {
    //        par = {};
    //        par.id = $(this).attr("resourceid");
    //        $.sync.ajax.post("/Resource/shoucang", par, function (data) {
    //            if (data.State) {
    //                $.dialog.alert(data.Result);
    //            }
    //        });
    //    });
    //    $("#shangchuan").live("click", function () {
    //        par = {};
    //        par.guidlist = $("#Guidlist").val();
    //        par.type = $("#hdtype").val();
    //        par.Id = $("#hdId").val();
    //        par.searchname = $("#hdsearchname").val();
    //        $.sync.ajax.post("/Resource/shangchuan", par, function (data) {
    //            if (data.Result.loginbool) {
    //                window.location = "/Resource/Uploading?Id=" + data.Result.Id + "&type=" + data.Result.type + "&Guidlist=" + data.Result.guidlist + "&searchname=" + data.Result.searchname;
    //            } else {
    //                $.dialog.alert("请登录！");
    //            }
    //        });
    //    });
    //    $("#img_CourseName").live("click", function () {
    //        par = {};
    //        par.Id = $("#hdId").val();
    //        par.CourseName = $("#txtCourseName").val();
    //        if (par.CourseName != "") {
    //            $.sync.ajax.post("/Resource/GetCourceName", par, function (data) {
    //                $.dialog.alert(data.Msg);
    //            });
    //        }
    //    });
    //    $("#txtCourseName").live("keyup", function (evt) {
    //        par = {};
    //        par.CourseName = $("#txtCourseName").val();
    //        if (par.CourseName != "" & (par.CourseName.length > 1)) {
    //            $("#img_CourseName").attr("style", "height: 20px; vertical-align: bottom; width: 20px;")
    //        } else {
    //            $("#img_CourseName").attr("style", "height: 20px; vertical-align: bottom; width: 20px;display:none")
    //        }
    //    });
    //    $("#txtCourseName").live("blur", function () {
    //        par = {};
    //        par.CourseName = $("#txtCourseName").val();
    //        if (par.CourseName != "" & (par.CourseName.length > 2)) {
    //            $("#img_CourseName").attr("style", "height: 20px; vertical-align: bottom; width: 20px;")
    //        } else {
    //            $("#img_CourseName").attr("style", "height: 20px; vertical-align: bottom; width: 20px;display:none")
    //        }
    //    });

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
            $.sync.ajax.post("/Resource/SetError", par, function (data) {
                if (data.State == 1) {
                    $.dialog.tips(data.Msg);
                }
            });
        }, "此资源有错误");
    });

    $("#ckShare").change(function () {
        if ($(this).prop("checked")) {
            $("#tbShare").show();
        } else {
            $("#tbShare").hide();
        }
    });

    $("#btnLog").click(function () {
        if ($("#divShare").size() <= 0) {
            $.sync.ajax.post("/Resource/shoucang", { id: $("#btnLog").attr("dataid") }, function (data) {
                $.dialog.tips(data.Result);
            });
        } else {
            $.dialog.confirm(document.getElementById("divShare"), function () {
                if ($("#ckShare").prop("checked")) {
                    par = {};
                    par.resourceId = $("#btnLog").attr("dataid");
                    par.courseId = $("#ddlCourse").val();
                    par.typeId = $("#ddlResourceType").val();
                    if (!par.resourceId || !par.courseId || !par.typeId) {
                        $.dialog.alert("请完善相关资料");
                        return false;
                    }
                    $.sync.ajax.post("/Resource/ShareToLan", par, function (data) {
                        $.dialog.alert(data.Msg);
                    });
                } else {
                    $.sync.ajax.post("/Resource/shoucang", { id: $("#btnLog").attr("dataid") }, function (data) {
                        $.dialog.tips(data.Msg);
                    });
                }
            }).title("共享到校内资源库");
        }
    });
});  