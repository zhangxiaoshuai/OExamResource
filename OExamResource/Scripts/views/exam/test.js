$(function () {
    var par = {};
    par.paperId = $("#hdId").val();
    $.sync.ajax.post("/Exam/IsCollect", par, function (data) {
        if (data.State) {
            $("#btnLike").attr({ "iscol": "1" }).val("已收藏");
        }
    });
    /*初始化试题*/
    par = {};
    par.id = $("#hdId").val();
    $.sync.ajax.post("/Exam/GetTestPaper", par, function (data) {
        if (data.State) {
            $("#divContent").setTemplateURL("/Scripts/template/exam/test.htm", null, { filter_data: false });
            $("#divContent").processTemplate(data.Result);
            $("#divNum").setTemplateURL("/Scripts/template/exam/r_sub.htm");
            $("#divNum").processTemplate(data.Result);
            var _s = 0;
            for (var i = 0; i < data.Result.length; i++) {
                _s += data.Result[i].Part.ItemCount * data.Result[i].Part.ItemScore;
            }
            $("#lblScore").text(_s);
        }
    });
    /*题号定位*/
    $("#divNum a").click(function () {
        $(document).scrollTop($($(this).attr("href").substr($(this).attr("href").lastIndexOf('#'))).offset().top);
        return false;
    });
    /*单选 判断*/
    $(".a_rd").change(function () {
        var $that = $(this).parents("dd");
        $that.find(".a_my:eq(0)").html($(this).next("label:eq(0)").text());
        $that.find(".a_my:eq(0)").attr("datav2", $(this).val());
    });
    /*多选*/
    $(".a_ck").change(function () {
        var $that = $(this).parents("dd");
        var _v2 = "";
        var _htm = "";
        $(this).parent().children(".a_ck:checked").each(function () {
            _v2 += $(this).val();
            _htm += $(this).next("label:eq(0)").text();
        });
        $that.find(".a_my:eq(0)").html(_htm);
        $that.find(".a_my:eq(0)").attr("datav2", _v2);
    });
    /*下拉*/
    $(".a_ddl").change(function () {
        var $that = $(this).parents("dd");
        $that.find(".a_my:eq(0)").html($(this).val());
        $that.find(".a_my:eq(0)").attr("datav2", $(this).val());
    });
    /*主观*/
    $(".a_txt").change(function () {
        var $that = $(this).parents("dd");
        $that.find(".a_my:eq(0)").html($(this).val());
    });
    /*题号着色*/
    $(".a :input").change(function () {
        $("#divNum .num_exam[href='#" + $(this).parents("dd").attr("id") + "']").addClass("num_exam_01");
    });

    /*显示隐藏题号导航*/
    if (!!window.ActiveXObject && !window.XMLHttpRequest) {
        $("#r_sub").toggle(function () {
            $("#divNum").hide();
            $(this).css("right", 1);
        }, function () {
            $("#divNum").show();
            $(this).css("right", 151);
        });
    } else {
        $("#r_sub").toggle(function () {
            var _w = 0;
            var _hideleft = setInterval(function () {
                _w -= 4;
                $("#r_main").css("right", _w);
                if (_w <= -152) {
                    clearInterval(_hideleft);
                }
            }, 1);
        }, function () {
            var _w = -152;
            var _showleft = setInterval(function () {
                _w += 4;
                $("#r_main").css("right", _w);
                if (_w >= 0) {
                    clearInterval(_showleft);
                }
            }, 1);
        });
    }

    /*收藏*/
    $("#btnLike").hover(function () {
        $(this).val($(this).attr("count"));
    }, function () {
        if ($(this).attr("iscol") == "1") {
            $(this).val("已收藏");
        } else {
            $(this).val("收藏");
        }
    }).click(function () {
        par = {};
        par.paperId = $("#hdId").val();
        if ($(this).attr("iscol") == "1") {
            return false;
        } else {
            $.sync.ajax.post("/Exam/AddCollectPaper", par, function (data) {
                $.dialog.tips(data.Msg);
                if (data.State) {
                    $("#btnLike").attr("iscol", "1").val("已收藏").attr("count", data.Result);
                }
            });
        }
    });
    /*交卷评分*/
    $("#btnOver").click(function () {
        $.dialog.confirm("确定交卷吗？", function () {
            _over();
        });
    });

    var _over = function () {
        var _data = {};
        _data.Total = $("#lblScore").html(); /*总分*/
        _data.Score = 0; /*得分*/
        _data.List = [];
        clearInterval(_timer);
        $("dl").each(function () {/*大题循环*/
            var _item = {};
            _item.Title = $(this).children("dt:first").text();
            //截取Title长度
            if (_item.Title.length >= 30) {
                _item.Title = _item.Title.substring(0, 30) + "...";
            }
            _item.Num = 0; /*步长*/
            _item.Single = parseFloat($(this).children("dt:first").attr("datas")) || 0; /*单题*/
            var $thats = $(this).children("dd:even");
            for (var i = 0; i < $thats.length; i++) {/*小题*/
                $thats.eq(i).find("table tr:not(:first,:last)").show();
                $thats.eq(i).find("table tr:last").hide();
                var $that = $thats.eq(i).find("table tr .a_my:eq(0)");
                if (!!$that.attr("dataeq")) {
                    if ($that.attr("datav1").trim() == $that.attr("datav2").trim()) {
                        $("#divNum .num_exam[href='#" + $thats.eq(i).attr("id") + "']").addClass("num_exam_02");
                        $that.parent().addClass("a_my_01");
                        _item.Num++;
                    } else if ($that.attr("datav2").trim() != "") {
                        $("#divNum .num_exam[href='#" + $thats.eq(i).attr("id") + "']").addClass("num_exam_03");
                        $that.parent().addClass("a_my_02");
                    }
                }
            }
            _data.Score += _item.Num * _item.Single;
            _data.List.push(_item);
        });
        $("#lblMark").text(_data.Score + "分");
        $("#trMark").show();
        $("#btnOver").unbind();
        $("#card").setTemplateURL("/Scripts/template/exam/card.htm");
        $("#card").processTemplate(_data);
        $.dialog({ content: document.getElementById("card"), ok: true, title: "您的客观题得分：" });
        par = {};
        par.paperId = $("#hdId").val();
        par.score = _data.Score;
        $.sync.ajax.post("/Exam/PostScore", par, function (data) {
            if (data.State) {
                if ($("#tbLastLog").length > 0) {
                    par = {};
                    par.paperId = $("#hdId").val();
                    $.sync.ajax.post("/Exam/GetLastTests", par, function (data) {
                        $("#tbLastLog").setTemplateURL("/Scripts/template/exam/log.htm");
                        $("#tbLastLog").processTemplate(data.Result);
                    });
                }
            }
        });
    };

    /*初始化计时器*/
    var _time = $("#lblTime").attr("data") * 60;
    var _timer = setInterval(function () {
        if (_time > 0) {
            _time--;
            $("#lblTime").html(Math.floor(_time / 60) + ":" + (_time % 60));
        } else {
            clearInterval(_timer);
            var _arttimer;
            art.dialog({
                content: '时间到',
                init: function () {
                    var that = this, i = 3;
                    var fn = function () {
                        that.content("时间到，自动交卷：" + i);
                        !i && that.close();
                        i--;
                    };
                    _arttimer = setInterval(fn, 1000);
                    fn();
                },
                close: function () {
                    clearInterval(_arttimer);
                    _over();
                }
            }).show();
        }
    }, 1000);

    if ($("#tbLastLog").length > 0) {
        par = {};
        par.paperId = $("#hdId").val();
        $.sync.ajax.post("/Exam/GetLastTests", par, function (data) {
            $("#tbLastLog").setTemplateURL("/Scripts/template/exam/log.htm");
            $("#tbLastLog").processTemplate(data.Result);
        });
    }



    $("#btnPassPaper").click(function () {
        par = {};
        par.paperId = $("#hdId").val();
        $.sync.ajax.post("/Exam/PassPaper", par, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg + ' 3秒后关闭');
                timename = setTimeout("window.close();", 3000)
            }
        });
    });


    $("#btnDownPaper").click(function () {
        par = {};
        par.paperId = $("#hdId").val();
        $.sync.ajax.post("/Exam/DownPaper", par, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg + ' 3秒后关闭');
                timename = setTimeout("window.close();", 3000)
            }
        });
    });


    $("#btnDelPaper").click(function () {
        par = {};
        par.paperId = $("#hdId").val();
        $.dialog({
            id: 'delPaper',
            content: '是否删除？',
            button: [
        {
            name: '确定',
            callback: function () {
                $.sync.ajax.post("/Exam/SoftDelPaper", par, function (data) {
                    if (data.State) {
                        var timer;
                        $.dialog({
                            content: data.Msg,
                            init: function () {
                                var that = this, i = 3;
                                var fn = function () {
                                    that.title(i + '秒后窗口关闭');
                                    !i && that.close();
                                    i--;
                                };
                                timer = setInterval(fn, 1000);
                                fn();
                            },
                            close: function () {
                                clearInterval(timer);
                                window.close();
                            }
                        }).show();
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