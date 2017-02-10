$(function () {
    var parameters = {};
    $.sync.ajax.post("/Exam/CategoryList", parameters, function (data) {
        if (data.State) {
            $("#full").setTemplateURL("/Scripts/template/exam/index.htm");
            $("#full").processTemplate(data.Result);
        }
    });
    $(".btnOpen").on("click", function () {
        var dataid = $(this).attr("dataid");
        var $that = $(".divChange[dataid='" + dataid + "']");
        $(".divChange").removeClass("index_dl_click");
        $that.addClass("index_dl_click");
        $("#fudong").stop(true, true);
        $("#fudong div").html($(".subcontent[dataid='" + dataid + "']").html());
        $("#fudong").hide().slideDown("fast").offset({ top: $that.offset().top + $that.height(), left: $(".divChange:eq(0)").offset().left });
        return false;
    });
    $("body").click(function () {
        $("#fudong").slideUp();
    });
});