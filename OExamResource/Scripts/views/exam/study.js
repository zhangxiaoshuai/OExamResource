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
    $.sync.ajax.post("/Exam/GetPaper", par, function (data) {
        if (data.State) {
            $("#divContent").setTemplateURL("/Scripts/template/exam/study.htm", null, { filter_data: false });
            $("#divContent").processTemplate(data.Result);
            $("#divNum").setTemplateURL("/Scripts/template/exam/number.htm");
            $("#divNum").processTemplate(data.Result);
            var _s = 0;
            for (var i = 0; i < data.Result.length; i++) {
                _s += data.Result[i].Part.ItemCount * data.Result[i].Part.ItemScore;
            }
            $("#lblScore").text(_s);
        }
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
    /*题号定位*/
    $("#divNum a").click(function () {
        $(document).scrollTop($($(this).attr("href").substr($(this).attr("href").lastIndexOf('#'))).offset().top);
        return false;
    });
    /*点击查看答案*/
    $(".divToggle").live("click", function () {
        $(this).parent().find(".answer_exam,.explain_exam,.icon_exam").slideDown();
        $(this).slideUp();
    });
    /*测试一下*/
    $("#btnTest").click(function () {
        if (($("dd[id][flag='1']").length || -1) <= 0) {
            review(1);
        } else {
            $.dialog.confirm("只复习不记得的题吗？", function () {
                review(0);
            }, function () {
                review(1);
            });
        }
    });
    var review = function (flag) {
        var $thats;
        if (!!flag) {
            $thats = $("dd[id]");
        } else {
            $thats = $("dd[id][flag='0']");
        }
        var _index = 0;
        var _right = 0;
        var $that = $thats.eq(_index);
        $("#divTest div").eq(0).html($that.html());
        $("#divTest .answer_exam,#divTest .explain_exam,#divTest .icon_exam").hide();
        $.dialog({
            fixed: false,
            content: $("#divTest").html(),
            title: "测试一下（总题数：" + $thats.length + "题,已经测试：" + _index + "题，记得答案：" + _right + "题）",
            close: function () {
                art.dialog.notice({
                    title: '测试完毕',
                    width: 220,
                    content: "总题数：" + $thats.length + "题，已经测试：" + _index + "题，记得答案：" + _right + "题。",
                    icon: 'succeed',
                    time: 8
                });
            },
            button: [{
                name: '记得',
                callback: function () {
                    $that.attr("flag", "1");
                    _index++;
                    _right++;
                    if (_index >= $thats.length) {
                        this.close();
                        return false;
                    }
                    $that = $thats.eq(_index);
                    $("#divTest div").eq(0).html($that.html());
                    $("#divTest .answer_exam,#divTest .explain_exam,#divTest .icon_exam").hide();
                    this.content($("#divTest").html()).title("测试一下（总题数：" + $thats.length + "题,已经测试：" + _index + "题，记得答案：" + _right + "题）");
                    return false;
                },
                focus: true
            }, {
                name: '不记得',
                callback: function () {
                    $that.attr("flag", "0");
                    _index++;
                    if (_index >= $thats.length) {
                        this.close();
                        return false;
                    }
                    $that = $thats.eq(_index);
                    $("#divTest div").eq(0).html($that.html());
                    $("#divTest .answer_exam,#divTest .explain_exam,#divTest .icon_exam").hide();
                    this.content($("#divTest").html()).title("测试一下（总题数：" + $thats.length + "题,已经测试：" + _index + "题，记得答案：" + _right + "题）");
                    return false;
                }
            }]
        });
    }
    /*模拟考试*/
    $("#btnExam").click(function () {
        location.href = "/Exam/Test/" + $("#hdId").val();
    });

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
    /*初始化计时器*/
    var _timer = 0;
    setInterval(function () {
        _timer++;
        $("#lblTime").html(Math.floor(_timer / 60) + ":" + (_timer % 60));
    }, 1000);

});