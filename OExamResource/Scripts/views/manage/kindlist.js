$(function () {
    var parameters = {};
    //默认Job被选中，读取列表
    $.sync.ajax.post("/DataManage/GetJob", parameters, function (data) {
        if (data.State) {
            $("#divKind").setTemplateURL("/Scripts/template/manage/job.htm");
            $("#divKind").processTemplate(data.Result);
        }
    });

    //单选改变，读取相应列表
    $("input[name='rKind']").bind("click", function () {
        var kind = $("input[name='rKind']:checked").val();
        if (kind == "Job") {
            $.sync.ajax.post("/DataManage/GetJob", parameters, function (data) {
                if (data.State) {
                    $("#divKind").setTemplateURL("/Scripts/template/manage/job.htm");
                    $("#divKind").processTemplate(data.Result);
                }
            });
        } else if (kind == "Pro") {
            $.sync.ajax.post("/DataManage/GetPro", parameters, function (data) {
                if (data.State) {
                    $("#divKind").setTemplateURL("/Scripts/template/manage/pro.htm");
                    $("#divKind").processTemplate(data.Result);
                }
            });
        } else if (kind == "Post") {
            $.sync.ajax.post("/DataManage/GetPost", parameters, function (data) {
                if (data.State) {
                    $("#divKind").setTemplateURL("/Scripts/template/manage/post.htm");
                    $("#divKind").processTemplate(data.Result);
                }
            });
        }
    });

    //所有Job删除
    $(".delJob").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeleteJob", parameters, function (data) {
                if (data.State) {
                    $("#rJob").click();
                }
            });
        });
    });

    //所有Pro删除
    $(".delPro").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeletePro", parameters, function (data) {
                if (data.State) {
                    $("#rPro").click();
                }
            });
        });
    });

    //所有Post删除
    $(".delPost").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeletePost", parameters, function (data) {
                if (data.State) {
                    $("#rPost").click();
                }
            });
        });
    });

    //变色
    $("#divKind tr:gt(0)").live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
});