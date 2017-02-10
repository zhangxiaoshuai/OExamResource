$(function () {
    var par = {};
    $.sync.ajax.post("/Medical/Getload", par, function (data) {
        if (data.State) {
            $("#upload").setTemplateURL("/Scripts/template/Medical/loading.htm");
            $("#upload").processTemplate(data.Result);
        }
    });
    $.sync.ajax.post("/Medical/Getcategory", par, function (data) {
        $("#slcategory").setTemplateURL("/Scripts/template/Medical/category.htm");
        $("#slcategory").processTemplate(data.Result);
    });
    $("#slcategory").change(function () {
        par = {};
        par.cateid = $("#slcategory").val();
        $.sync.ajax.post("/Medical/Getslsub", par, function (data) {
            if (data.State) {
                $("#slsubject").setTemplateURL("/Scripts/template/Medical/category.htm");
                $("#slsubject").processTemplate(data.Result);
            } else {
                $("#slsubject").attr("value", "00000000-0000-0000-0000-000000000000");
                $("#slspeciality").attr("value", "00000000-0000-0000-0000-000000000000");
            }
        });
    });
    $("#slsubject").change(function () {
        par = {};
        par.subid = $("#slsubject").val();
        $.sync.ajax.post("/Medical/Getspe", par, function (data) {
            $("#slspeciality").setTemplateURL("/Scripts/template/Medical/category.htm");
            $("#slspeciality").processTemplate(data.Result);
        });
    });
    $.sync.ajax.post("/Medical/Gettypelist", par, function (data) {
        $("#sltype").setTemplateURL("/Scripts/template/Medical/category.htm");
        $("#sltype").processTemplate(data.Result);
    });
    $("#btnshangchuan").click(function () {
        par = {};
        $("#uploadform").ajaxForm({
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                if (data.State) {
                    $.dialog.tips(data.Result);
                    $("#btnreset").click();
                } else {
                    $.dialog.tips(data.Result);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        });
    });

    //资源名称
    $("#source_name").blur(function () {
        if ($("#source_name").val().length == 0) {
            $("#name_m").text("请填写资源名称！")
        }
    })
    .focus(function () {
        $("#name_m").text("*")
    });
    //大类
    $("#slcategory").blur(function () {
        if ($("#slcategory").val() == "00000000-0000-0000-0000-000000000000") {
            $("#labcate").text("请选择资源大类！")
        }
    })
    .focus(function () {
        $("#labcate").text("*")
    });
    //学科
    $("#slsubject").blur(function () {
        if ($("#slsubject").val() == "00000000-0000-0000-0000-000000000000") {
            $("#labsub").text("请选择资源学科！")
        }
    })
    .focus(function () {
        $("#labsub").text("*")
    });
//    //专业
//    $("#slspeciality").blur(function () {
//        if ($("#slspeciality").val() == "00000000-0000-0000-0000-000000000000") {
//            $("#labspe").text("请选择资源专业！")
//        }
//    })
//    .focus(function () {
//        $("#labspe").text("*")
//    });
    //文件
    $("#uploadfile").blur(function () {
        if ($("#uploadfile").val().length == 0) {
            $("#labupl").text("请选择资源文件！")
        }
    })
    .focus(function () {
        $("#labupl").text("*")
    });
    //资源类型
    $("#sltype").blur(function () {
        if ($("#sltype").val() == "00000000-0000-0000-0000-000000000000") {
            $("#labty").text("请选择资源类型！")
        }
    })
    .focus(function () {
        $("#labty").text("*")
    })
    .change(function () {
        par = {};
        par.typeid = $("#sltype").val();
        $.sync.ajax.post("/Medical/Typepic", par, function (data) {
            $("#labty").text(data.Result);
        });
    });
    //资源简介
    $("#Fescribe").blur(function () {
        if ($("#Fescribe").val().length == 0) {
            $("#labfes").text("请填写资源简介！")
        }
    })
    .focus(function () {
        $("#labfes").text("*")
    });
    $("#btngoup").click(function () {
        window.location = "/Medical/Preview?hdId=" + $("#hdId").val() + "&hdType=" + $("#hdtype").val() + "&hdData=" + $("#Guidlist").val() + "&hdsearchname=" + $("#hdsearchname").val();
    });

});