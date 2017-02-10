$(function () {
    var par = {};
    $.sync.ajax.post("/Medical/GetList", par, function (data) {
        $("#indexlist").setTemplateURL("/Scripts/template/Medical/index.htm");
        $("#indexlist").processTemplate(data.Result);
    }); 

//    $("#searchname")
//    .data("isInp", false)
//    .focus(function () {
//        if (!$(this).data("isInp")) {
//            $(this).data("isInp", true).val("").css("color", "Black");
//        }
//    })
//    .blur(function () {
//        if ($(this).val().trim() == "") {
//            $(this).data("isInp", false).val("请输入资源搜索关键字...").css("color", "Gray");
//        }
//    })
//    .keyup(function (evt) {
//        if (evt.keyCode == 13) {
//            $("#btnsearch").click();
//        }
//    });
//    $("#btnsearch").click(function () {
//        var searchname = $("#searchname").val().trim();
//        if (searchname != "" && searchname != "请输入资源搜索关键字...") {
//            window.location = "/Medical/List?searchname=" + searchname;
//        } else {
//            $.dialog.tips("请输入要搜索的关键字！");
//        }
//    });

});