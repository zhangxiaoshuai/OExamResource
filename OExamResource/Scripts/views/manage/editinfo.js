$(function () {
    var parameters = {};
    //下拉
    parameters = {};
    parameters.schoolid = $("#hdId").val();
    $.sync.ajax.post("/LanManage/GetSFD", parameters, function (data) {
        if (data.State) {
            $("#ddlSFD").setTemplateURL("/Scripts/template/manage/sfd.htm");
            $("#ddlSFD").processTemplate(data.Result);
        }
    });
    $("#ddlSFD").bind("change", function () {
        window.location = "/LanManage/EditInfo/" + $("#ddlSFD").val();
    });

    //修改名称
    var name = $("#txtName").val();
    $("#btnEditName").toggle(function () {
        $(this).val("确定");
        $("#hName").hide();
        $("#txtName").show();
    }, function () {
        if ($("#txtName").css("display") != "none") {
            parameters.schoolid = $("#hdId").val();
            parameters.name = $("#txtName").val();
            if ($("#txtName").val().length == 0) {
                $.dialog.tips("名称不能为空！");
            } else if ($("#txtName").val() == name) {
                $.dialog.tips("修改成功！");
                $(this).val("修改名称");
                $("#hName").show();
                $("#txtName").hide();
            } else {
                $.sync.ajax.post("/LanManage/EditName", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips(data.Msg);
                    }
                });
                $(this).val("修改名称");
                $("#hName").show();
                $("#txtName").hide();
            }
        } else {
            return false;
        }
    });

    //修改介绍
    var remark = $("#remark").val();
    $("#btnEditRemark").bind("click", function () {
        if ($("#remark").attr("disabled") != "disabled") {
            parameters.schoolid = $("#hdId").val();
            parameters.remark = $("#remark").val();
            $.sync.ajax.post("/LanManage/EditRemark", parameters, function (data) {
                if (data.State) {
                    $("#btnEdit").click();
                    $.dialog.tips(data.Msg);
                }
            });
        } else {
            return false;
        }
    });
    $("#btnEdit").toggle(function () {
        $(this).val("取消");
        $("#remark").removeAttr("disabled");
    }, function () {
        $(this).val("修改");
        $("#remark").val(remark);
        $("#remark").attr("disabled", "disabled");
    });

    //图片
    $("#divBannerImgs img").die().live({
        mouseover: function () {
            $(this).addClass("hover");
        },
        mouseleave: function () {
            $(this).removeClass("hover");
        }
    });

    //删除图片
    $(".delImg").die().live("click", function () {
        var id = $(this).attr("dataid");
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        parameters.img = $("#img" + id).attr("src");
        $.sync.ajax.post("/LanManage/DelBannerImg", parameters, function (data) {
            if (data.State) {
                $("#" + id).remove();
            }
        });
    });

    //logo
    $("#btn_show_logo").bind("click", function () {
        $.dialog({
            title: "更改学校Logo",
            content: document.getElementById('form_Logo'),
            id: 'EF8931'
        });
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                var picPath = data.Result.Pic;
                if (!data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.list['EF8931'].close();
                    $("#imgLogo").attr("src", picPath);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_Logo").ajaxForm(options);
    });

    $("#delLogoImg").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        $.sync.ajax.post("/LanManage/DelLogoImg", parameters, function (data) {
            if (!!data.State) {
                $("#imgLogo").remove();
                var img = '<img id=\"imgName\" style=\"width: 104px;height: 100px; border: 1px solid #ffffff\" />';
                $("#delLogoImg").before(img);
            }
        });
    });
    //name
    $("#btn_show_name").bind("click", function () {
        $.dialog({
            title: "更改学校名称图片",
            content: document.getElementById('form_name'),
            id: 'EF8930'
        });
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                var picPath = data.Result.Pic;
                if (!data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.list['EF8930'].close();
                    $("#imgName").attr("src", picPath);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_name").ajaxForm(options);
    });

    $("#delNameImg").bind("click", function () {
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        $.sync.ajax.post("/LanManage/DelNameImg", parameters, function (data) {
            if (!!data.State) {
                $("#imgName").remove();
                var img = '<img id=\"imgName\" style=\"width: 400px;height: 100px; border: 1px solid #ffffff\" />';
                $("#delNameImg").before(img);
            }
        });
    });
    //banner
    $("#btn_show_banner").bind("click", function () {
        $.dialog({
            title: "更改学校标语",
            content: document.getElementById('form_banner'),
            id: 'EF8920'
        });
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                var picPath = data.Result.Pic;
                if (!data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.list['EF8920'].close();
                    var html = "<li id='" + data.Result.Id + "'><img id='img" + data.Result.Id + "', src='" + picPath + "' alt='' /><a class='delImg' style='vertical-align:top' href='javascript:void(0);' title='删除' dataid='" + data.Result.Id + "'>×</a></li>";
                    $("#app").before(html);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_banner").ajaxForm(options);
    });
}); 
