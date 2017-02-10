$(function () {
    var parameters = {};
    //分页数据
    //学校信息列表
    $.cellpagebar.init({
        total: $("#sCount").text(),
        pagesize: 30,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.pageIndex = pageindex;
        parameters.tag = $("#hdTag").val();
        if ($("#hdTag").val() == 0) {
            $("#lku").text("资源库下载");
        } else if ($("#hdTag").val() == 1) {
            $("#lku").text("医学库下载");
        } else if ($("#hdTag").val() == 2) {
            $("#lku").text("精品课程库浏览");
        } else if ($("#hdTag").val() == 3) {
            $("#lku").text("试题库下载组卷");
        } else if ($("#hdTag").val() == 4) {
            $("#lku").text("模考库模考");
        } else if ($("#hdTag").val() == 5) {

        }
        $.sync.ajax.post("/TeacherManage/GetDetailsList", parameters, function (data) {
            if (data.State) {
                $("#divLogList").setTemplateURL("/Scripts/template/manage/downT.htm");
                $("#divLogList").processTemplate(data.Result);
            }
        });
    });


    //变色
    $("#divLogList tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });

    $("#aLogTxt").click(function () {
        if ($("#hdTag").val() == 0 || $("#hdTag").val() == 1) {
            $.dialog({
                title: "导出详细记录",
                content: document.getElementById('getTxtRM'),
                id: 'EF8930'
            });
        } else if ($("#hdTag").val() == 3) {
            $.dialog({
                title: "导出详细记录",
                content: document.getElementById('getTxtT'),
                id: 'EF8933'
            });
        } else if ($("#hdTag").val() == 4) {
            $.dialog({
                title: "导出详细记录",
                content: document.getElementById('getTxtE'),
                id: 'EF8934'
            });
        } else if ($("#hdTag").val() == 2) {
            $.dialog({
                title: "导出详细记录",
                content: document.getElementById('getTxtQ'),
                id: 'EF8932'
            });

        }

    });

    //
    $("#btnGetTxtRM").click(function () {
        if ($("#RMstart").val().trim().length != 0 && $("#RMend").val().trim().length != 0) {
            window.location.href = "/TeacherManage/GetDetailsListTxt/" + $("#hdId").val() + "?tag=" + $("#hdTag").val() + "&status=" + $('input:radio[name="RMstatus"]:checked').val() + "&str=" + $('input:radio[name="RMstatus"]:checked').next("label").text() + "&start=" + $("#RMstart").val() + "&end=" + $("#RMend").val();
        } else {
            $.dialog.tips("请正确填写时间！");
        }
    });

    $("#btnGetTxtT").click(function () {
        if ($("#RMstart").val().trim().length != 0 && $("#RMend").val().trim().length != 0) {
            window.location.href = "/TeacherManage/GetDetailsListTxt/" + $("#hdId").val() + "?tag=" + $("#hdTag").val() + "&status=" + $('input:radio[name="Tstatus"]:checked').val() + "&str=" + $('input:radio[name="Tstatus"]:checked').next("label").text() + "&start=" + $("#Tstart").val() + "&end=" + $("#Tend").val();
        } else {
            $.dialog.tips("请正确填写时间！");
        }
    });

    $("#btnGetTxtE").click(function () {
        if ($("#RMstart").val().trim().length != 0 && $("#RMend").val().trim().length != 0) {
            window.location.href = "/TeacherManage/GetDetailsListTxt/" + $("#hdId").val() + "?tag=" + $("#hdTag").val() + "&status=" + $('input:radio[name="Estatus"]:checked').val() + "&str=" + $('input:radio[name="Estatus"]:checked').next("label").text() + "&start=" + $("#Estart").val() + "&end=" + $("#Eend").val();
        } else {
            $.dialog.tips("请正确填写时间！");
        }
    });

    $("#btnGetTxtQ").click(function () {
        if ($("#RMstart").val().trim().length != 0 && $("#RMend").val().trim().length != 0) {
            window.location.href = "/TeacherManage/GetDetailsListTxt/" + $("#hdId").val() + "?tag=2&status=2&str=浏览" + "&start=" + $("#Qstart").val() + "&end=" + $("#Qend").val();
        } else {
            $.dialog.tips("请正确填写时间！");
        }
    });
});