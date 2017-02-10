$(function () {
    var par = {};
    //导航
    par = {};
    par.id = $("#hdId").val();
    par.type = $("#hdType").val();
    par.jibie = $("#hdjibie").val();
    par.niandu = $("#hdniandu").val();
    par.searchname = $("#hdsearchname").val();
    $.sync.ajax.post("/QualityCourse/Getnavigation", par, function (data) {
        $("#navigation").setTemplateURL("/Scripts/template/qualitycourse/navigation.htm");
        $("#navigation").processTemplate(data.Result);
    });
    //大类 学科 专业 
    par = {};
    par.id = $("#hdId").val();
    par.type = $("#hdType").val();
    par.jibie = $("#hdjibie").val();
    par.niandu = $("#hdniandu").val();
    par.searchname = $("#hdsearchname").val();
    $.sync.ajax.post("/QualityCourse/GetSubject", par, function (data) {
        if (data.State) {
            $("#subject").setTemplateURL("/Scripts/template/qualitycourse/listsubject.htm");
            $("#subject").processTemplate(data.Result);
        } else {
            $("#subject").attr("display", "none");
        }

    });
    //分页
    $.cellpagebar.init({
        total: $("#hdCount").val(), //数据总条数
        pagesize: 10, 			//每页的数据条数
        container: $("#divBar")	//分页条在页面的位置
    }, function (pageindex) { 		//pageindex是点击每个分页按钮返回的页码											//这个函数里面就是用这个页码来进行交互等处理的js代码
        //这里结合ajax请求和jtemplates模板生成数据分页列表
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.type = $("#hdType").val();
        parameters.searchname = $("#hdsearchname").val();
        parameters.jibie = $("#hdjibie").val();
        parameters.niandu = $("#hdniandu").val();
        parameters.pageIndex = pageindex;
        $.sync.ajax.post("/QualityCourse/Get_list", parameters, function (data) {
            if (data.State) {
                $("#resourcelist").setTemplateURL("/Scripts/template/qualitycourse/listpage.htm");
                $("#resourcelist").processTemplate(data.Result);
            }
        });
    });
    //图片边框鼠标移动变色
    $(".bgcolor").live({
        mouseenter: function () {//#778899  #87CEEB
            $(this).css("border", "3px solid #A9A9A9");
        },
        mouseleave: function () {
            $(this).css("border", "3px solid #F2F2F2");
        }
    });
    /*搜索start*/
    $("#searchname")
    .data("isInp", false)
    .focus(function () {
        if ($(this).val() == "请输入精品课程名称...") {
            if (!$(this).data("isInp")) {
                $(this).data("isInp", true).val("").css("color", "Black");
            }
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

    if ($("#searchname").val() == "") {
        $("#searchname").val("请输入精品课程名称...").css("color", "Gray");
    }

    //搜索
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
    $(".btnpreview").click(function () {
        // $.dialog.tips("正在验证访问是否正常，请稍等...");      
    });

    if ($("#hdjibie").val() != null) {
        $("#ddljibie").attr("Value", $("#hdjibie").val());
    }

    if ($("#hdniandu").val() != null) {
        $("#ddlniandu").attr("Value", $("#hdniandu").val());
    }
})