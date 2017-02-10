$(function () {
    var parameters = {};

    if ($("#divResource").attr("flag") == 0) {
        //获取素材Ip段下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbRIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbRIp").processTemplate(data.Result);
            }
        });

        //获取素材分类下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetSubDetailsR", parameters, function (data) {
            if (data.State) {
                $("#lbRSub").setTemplateURL("/Scripts/template/manage/detailsR.htm");
                $("#lbRSub").processTemplate(data.Result);
            }
        });

        //获取素材月份下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbRMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbRMonthCount").processTemplate(data.Result);
            }
        });
    }

    if ($("#divMedicine").attr("flag") == 0) {
        //获取医学Ip段下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbMIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMIp").processTemplate(data.Result);
            }
        });

        //获取医学分类下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetSubDetailsM", parameters, function (data) {
            if (data.State) {
                $("#lbMSub").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMSub").processTemplate(data.Result);
            }
        });

        //获取医学月份下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbMMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMMonthCount").processTemplate(data.Result);
            }
        });
    }

    if ($("#divTest").attr("flag") == 0) {
        //获取试题库Ip段下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "3";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbTIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbTIp").processTemplate(data.Result);
            }
        });

        //获取试题月份下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "3";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbTMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbTMonthCount").processTemplate(data.Result);
            }
        });
    }


    if ($("#divExam").attr("flag") == 0) {
        //获取模考库Ip段下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbEIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbEIp").processTemplate(data.Result);
            }
        });

        //获取模考分类详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetSubDetailsE", parameters, function (data) {
            if (data.State) {
                $("#lbESub").setTemplateURL("/Scripts/template/manage/detailsE.htm");
                $("#lbESub").processTemplate(data.Result);
            }
        });

        //获取模考月份下载详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbEMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbEMonthCount").processTemplate(data.Result);
            }
        });
    }

    if ($("#divQualityCourse").attr("flag") == 0) {
        //获取精品课程库Ip段详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbQIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQIp").processTemplate(data.Result);
            }
        });

        //获取精品课程分类详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetSubDetailsQ", parameters, function (data) {
            if (data.State) {
                $("#lbQSub").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQSub").processTemplate(data.Result);
            }
        });

        //获取精品课程月份详情
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbQMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQMonthCount").processTemplate(data.Result);
            }
        });
    }
});