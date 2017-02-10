function nofindImg(e, ico) {
    e.onerror = null;
    e.src = ico;
}
$(function () {
//    var par = {};
//    par.id = $("#hdId").val();
//    par.key = $("#hdKey").val();
//    $("a.listtype").click(function () {
//        var $that = $(this);
//        $("a.listtype.curr").removeClass("curr");
//        $that.addClass("curr");
//        par.baseType = $that.attr("basetype");
//        par.typeId = $that.attr("typeid") || null;
//        $.cellpagebar.init({
//            total: $that.attr("typecount"),
//            pagesize: 10,
//            container: $("#divBar")
//        }, function (pageindex) {
//            par.pageIndex = pageindex;
//            $.sync.ajax.post("/Medical/List", par, function (data) {
//                if (data.State) {
//                    $("#resourcelist").setTemplateURL("/Scripts/template/Medical/listpage.htm");
//                    $("#resourcelist").processTemplate(data.Result);
//                }
//            }, true);
//        });
//    }).eq(0).click();
//    $('#subject').load('/Medical/ListSub?id=@(ViewContext.RouteData.Values["Id"])&isJX=@(ViewBag.IsJX)&key=@Key');
//    var par = {};
//    var parameters = {};
//    parameters.typeid = "00000000-1234-0000-0000-000000000000";
//    if ($("#searchname").val().trim() == "") {
//        $("#searchname").attr("value", "请输入资源搜索关键字...");
//    }
//    //资源类型
//    par = {};
//    par.id = $("#hdId").val();
//    par.type = $("#hdType").val();
//    par.searchname = $("#hdsearchname").val();
//    $.sync.ajax.post("/Medical/Get_type", par, function (data) {
//        $("#listtype").setTemplateURL("/Scripts/template/Medical/listtype.htm");
//        $("#listtype").processTemplate(data.Result);
//    });

//    $(".listtype").click(function () {
//        parameters.typeid = $(this).attr("typeid");
//        $("#hdCount").val($(this).attr("typecount"));
//        $(".listtype.curr").removeClass("curr");
//        $(this).addClass("curr");
//        $.cellpagebar.init({
//            total: $("#hdCount").val(), //数据总条数
//            pagesize: 10, 			//每页的数据条数
//            container: $("#divBar")	//分页条在页面的位置
//        }, function (pageindex) { 		//pageindex是点击每个分页按钮返回的页码	//这个函数里面就是用这个页码来进行交互等处理的js代码
//            //这里结合ajax请求和jtemplates模板生成数据分页列表
//            parameters.id = $("#hdId").val();
//            parameters.type = $("#hdType").val();
//            parameters.searchname = $("#hdsearchname").val();
//            parameters.pageIndex = pageindex;
//            $.sync.ajax.post("/Medical/Get_list", parameters, function (data) {
//                if (data.State) {
//                    $("#resourcelist").setTemplateURL("/Scripts/template/Medical/listpage.htm");
//                    $("#resourcelist").processTemplate(data.Result);
//                }
//            });
//        });
//    });

//    //导航
//    par = {};
//    par.id = $("#hdId").val();
//    par.type = $("#hdType").val();
//    par.searchname = $("#hdsearchname").val();
//    $.sync.ajax.post("/Medical/Getnavigation", par, function (data) {
//        $("#navigation").setTemplateURL("/Scripts/template/Medical/navigation.htm");
//        $("#navigation").processTemplate(data.Result);
//    });
    //大类 学科 专业 
//    par = {};
//    par.id = $("#hdId").val();
//    par.type = $("#hdType").val();
//    par.searchname = $("#hdsearchname").val();
//    $.sync.ajax.post("/Medical/GetSubject", par, function (data) {
//        if (data.State) {
//            $("#subject").setTemplateURL("/Scripts/template/Medical/listsubject.htm");
//            $("#subject").processTemplate(data.Result);
//        }
//    });
    //分页
//    $.cellpagebar.init({
//        total: $("#hdCount").val(), //数据总条数
//        pagesize: 10, 			//每页的数据条数
//        container: $("#divBar")	//分页条在页面的位置
//    }, function (pageindex) { 		//pageindex是点击每个分页按钮返回的页码											//这个函数里面就是用这个页码来进行交互等处理的js代码
//        //这里结合ajax请求和jtemplates模板生成数据分页列表
//        parameters.id = $("#hdId").val();
//        parameters.type = $("#hdType").val();
//        parameters.searchname = $("#hdsearchname").val();
//        parameters.pageIndex = pageindex;
//        $.sync.ajax.post("/Medical/Get_list", parameters, function (data) {
//            if (data.State) {
//                $("#resourcelist").setTemplateURL("/Scripts/template/Medical/listpage.htm");
//                $("#resourcelist").processTemplate(data.Result);
//            }
//        });
//    });

    //预览
    //    $(".btnpreview").live("click", function () {
    //        var data = [];
    //        data.push($(this).parent().next().find(".ckip").attr("resourceid"));
    //        $("#hdData").val(JSON.stringify(data));
    //        window.open("/Medical/Preview?hdData=" + $("#hdData").val() + "&hdId=" + $("#hdId").val() + "&hdType=" + $("#hdType").val() + "&hdsearchname" + $("#hdsearchname").val());
    //    });

    //    //全选
    //    $("#CheckAll").click(function () {
    //        var flag = $(this).attr("checked");
    //        if (flag == undefined) {
    //            $("[name=fx]:checkbox").each(function () {
    //                $(this).removeAttr("checked", "checked");
    //            })
    //        }
    //        $("[name=fx]:checkbox").each(function () {
    //            if ($(this).attr("check")) {
    //                $(this).attr("checked", flag);
    //            }
    //        })
    //    });

    //    $(".ckip").live("click", function () {
    //        if (!$(this).attr("check")) {
    //            $(this).attr("checked", false);
    //        }
    //    });

    //    //图片边框鼠标移动变色
    //    $(".bgcolor").live({
    //        mouseenter: function () {
    //            $(this).css("border", "3px solid #F37E03");
    //        },
    //        mouseleave: function () {
    //            $(this).css("border", "3px solid #F2F2F2");
    //        }
    //    });

    //    //资源类型鼠标移动变色
    //    $(".licolor").mousemove(function () {
    //        $(this).attr("style", "border: 2px solid #F37E03");
    //        $("#left").attr("style", "");
    //    });
    //    $(".licolor").mouseout(function () {
    //        $(this).attr("style", " ");
    //        $("#left").attr("style", "border-top: 2px solid #ffffff; border-bottom:2px solid #ffffff");
    //    });

    /*搜索start*/
//    $("#searchname")
//    .data("isInp", false)
//    .focus(function () {
//        if ($(this).val() == "请输入资源搜索关键字...") {
//            if (!$(this).data("isInp")) {
//                $(this).data("isInp", true).val("").css("color", "Black");
//            }
//        }
//    })
//    .blur(function () {
//        if ($(this).val().trim() == "") {
//            $(this).data("isInp", false).val("请输入资源搜索关键字...").css("color", "Gray");
//        }
//    })
//    .keyup(function (evt) {
//        if (evt.keyCode == 13) {
//            $(".btnsearch").click();
//        }
//    });
//    //搜索
//    $("#btnsearch").click(function () {
//        var searchname = $("#searchname").val().trim();
//        if (searchname != "" && searchname != "请输入资源搜索关键字...") {
//            if ($('#hdId').val() != '' && $('#hdId').val() != null && $('#hdId').val() != '00000000-0000-0000-0000-000000000000') {
//                window.location = "/Medical/List/" + $('#hdId').val() + "?searchname=" + searchname + "&type=" + $('#hdType').val();
//            } else {
//                window.location = "/Medical/List?searchname=" + searchname + "&type=" + $('#hdType').val();
//            }
//        } else {
//            $.dialog.tips("请输入要搜索的关键字！");
//        }
//    });
})