$(function () {
    var par = {};
    $.sync.ajax.post("/QualityCourse/GetList", par, function (data) {
        $("#changequality").setTemplateURL("/Scripts/template/qualitycourse/index.htm");
        $("#changequality").processTemplate(data.Result);
    });
    $(".cateid").click(function () {
        var dataid = $(this).attr("dataid");
        var $that = $(".divChange[dataid='" + dataid + "']");
        $("#fudong").stop(true, true);
        $("#fudong>div").html($(".subcontent[dataid='" + dataid + "']").html());
        $("#fudong").hide().slideDown().offset({ top: $that.offset().top + $that.height() + 1, left: $(".divChange:eq(0)").offset().left })
        $(".divChange").removeClass("index_dl_click");
        $that.addClass("index_dl_click");
        return false;
    });
    $("body").click(function () {
        $("#fudong").slideUp();
    });

    //鼠标移动变色
    $(".qualitydiv").live({
        mouseenter: function () {
            $(this).css("background-color", "#708AD5");
        },
        mouseleave: function () {
            $(this).css("background-color", "#9BADE2");
        }
    });

    $("#searchname")
    .data("isInp", false)
    .focus(function () {
        if (!$(this).data("isInp")) {
            $(this).data("isInp", true).val("").css("color", "Black");
        }
    })
    .blur(function () {
        if ($(this).val().trim() == "") {
            $(this).data("isInp", false).val("请输入精品课程名称...").css("color", "Gray");
        }
    })
    .keyup(function (evt) {
        if (evt.keyCode == 13) {
            $("#btnsearch").click();
        }
    });
    $("#btnsearch").click(function () {
        var searchname = $("#searchname").val().trim();
        var jibie = $("#ddljibie").val().trim();
        var niandu = $("#ddlniandu").val().trim();

        if (searchname != "" && searchname != "请输入精品课程名称...") {
            window.location = "/QualityCourse/List?searchname=" + searchname + "&id=00000000-1111-0000-0000-000000000000&type=10&jibie="+jibie+"&niandu="+niandu;
        } else {
            window.location = "/QualityCourse/List?searchname=&id=00000000-1111-0000-0000-000000000000&type=10&jibie=" + jibie + "&niandu=" + niandu;
        }
    });

});