$(function () {
    var par = {};
    $.sync.ajax.post("/Resource/GetList", par, function (data) {
        $("#indexlist").setTemplateURL("/Scripts/template/resource/index.htm");
        $("#indexlist").processTemplate(data.Result);
    });

    $(".cateid").on("click", function () {
        var dataid = $(this).attr("dataid");
        var $that = $(".divChange[dataid='" + dataid + "']");
        $("#fudong").stop(true, true);
        $("#fudong div").html($(".subcontent[dataid='" + dataid + "']").html());
        $("#fudong").hide().slideDown("fast", function () {
            $("#fudong").offset({ left: $(".divChange:eq(0)").offset().left });
        }).offset({ top: $that.offset().top + $that.height(), left: $(".divChange:eq(0)").offset().left });
        $(".divChange").removeClass("index_dl_click");
        $that.addClass("index_dl_click");
        return false;
    });

    $("body").click(function () {
        $("#fudong").slideUp();
    });
});